using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Doneness { Raw = 0, Seared = 1, Medium = 2, Done = 3, Burnt = 4 }

public class SausageItem : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private Image img;
    [Tooltip("Doneness 순서대로 5장: Raw, Seared, Medium, Done, Burnt")]

    [SerializeField] private Sprite[] frontSprites = new Sprite[5]; // A면(앞)
    [SerializeField] private Sprite[] backSprites = new Sprite[5];  // B면(뒤)

    [Header("Timing")]
    [SerializeField] private float stepSec = 5f;   // 5초마다 다음 단계

    // 각 면 상태/타이머
    private Doneness sideA = Doneness.Raw;
    private Doneness sideB = Doneness.Raw;
    private float sideATimer = 0f;
    private float sideBTimer = 0f;

    // 현재 윗면이 A인지? (true=A 위, false=B 위)
    private bool faceUpIsA = true;

    public bool IsOnGrill { get; private set; } = false;
    public GrillSlot CurrentSlot { get; private set; }
    public BowlSpawner OwnerSpawner { get; set; }

    RectTransform rectTransform;

    private void Reset()
    {
        img = GetComponent<Image>();
    }

    private void Awake()
    {
        if (!img) img = GetComponent<Image>();
        UpdateVisual();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (!IsOnGrill) return;

        if (faceUpIsA)
        {
            // A가 위면 B가 아래 → B만 익힘
            sideBTimer += Time.deltaTime;
            if (sideBTimer >= stepSec && sideB != Doneness.Burnt)
            {
                sideBTimer = 0f;
                sideB = (Doneness)Mathf.Min((int)sideB + 1, (int)Doneness.Burnt);
                UpdateVisual();
            }
        }
        else
        {
            // B가 위면 A가 아래 → A만 익힘
            sideATimer += Time.deltaTime;
            if (sideATimer >= stepSec && sideA != Doneness.Burnt)
            {
                sideATimer = 0f;
                sideA = (Doneness)Mathf.Min((int)sideA + 1, (int)Doneness.Burnt);
                UpdateVisual();
            }
        }
    }
    public bool IsBurnt =>
        sideA == Doneness.Burnt || sideB == Doneness.Burnt;

    public bool IsExactlyBothDone =>
        sideA == Doneness.Done && sideB == Doneness.Done;

    public Sprite CurrentSprite
    {
        get
        {
            if (frontSprites == null || backSprites == null) return null;
            if (frontSprites.Length < 5 || backSprites.Length < 5) return null;

            if (faceUpIsA)
            {
                return frontSprites[(int)sideA];
            }
            else
            {
                return backSprites[(int)sideB];
            }
        }
    }

    public void AttachToGrill(GrillSlot slot)
    {
        if (OwnerSpawner != null)
        {
            OwnerSpawner.NotifyTaken(this);
            OwnerSpawner = null; // 이후에는 그릇과 무관
        }

        IsOnGrill = true;
        CurrentSlot = slot;
        sideATimer = 0f;
        sideBTimer = 0f;

        var rot = rectTransform.localEulerAngles;
        rot.z = 90f;
        rectTransform.localEulerAngles = rot;

        UpdateVisual();
    }

    public void DetachFromGrill()
    {
        IsOnGrill = false;
        CurrentSlot = null;

        var rot = rectTransform.localEulerAngles;
        rot.z = 0f;
        rectTransform.localEulerAngles = rot;

        UpdateVisual();
    }

    public void Flip()
    {
        // 윗면/아랫면 뒤집기
        faceUpIsA = !faceUpIsA;

        // 새로 "아래로 간 면"이 굽히기 시작하므로 그 면의 타이머만 0으로
        if (faceUpIsA)
            sideBTimer = 0f; // B가 아래가 됨
        else
            sideATimer = 0f; // A가 아래가 됨

        UpdateVisual();
    }

    // 제출 규칙: 양면 Done(3) 이상, Burnt(4) 금지
    public bool CanSubmit()
    {
        bool bothGood = sideA >= Doneness.Done && sideB >= Doneness.Done;
        bool anyBurnt = sideA == Doneness.Burnt || sideB == Doneness.Burnt;
        return bothGood && !anyBurnt;
    }

    public void Consume()
    {
        if (OwnerSpawner) OwnerSpawner.NotifyConsumed(this);
        Destroy(gameObject);
    }

    private void UpdateVisual()
    {
        if (img == null) return;

        Doneness upSide = faceUpIsA ? sideA : sideB;
        Sprite[] arr = faceUpIsA ? frontSprites : backSprites;

        if (arr != null && arr.Length >= 5)
            img.sprite = arr[(int)upSide];

        // 좌우 반전 연출은 선택
        var s = transform.localScale;
        s.x = faceUpIsA ? Mathf.Abs(s.x) : -Mathf.Abs(s.x);
        transform.localScale = s;
    }
}
