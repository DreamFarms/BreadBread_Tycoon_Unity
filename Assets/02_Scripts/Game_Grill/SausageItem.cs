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
    [SerializeField] private Sprite[] sprites = new Sprite[5];

    [Header("Timing")]
    [SerializeField] private float stepSec = 5f;   // 5초마다 다음 단계

    // 각 면 상태/타이머
    private Doneness sideA = Doneness.Raw;
    private Doneness sideB = Doneness.Raw;
    private float sideATimer = 0f;
    private float sideBTimer = 0f;

    // 현재 굽는 면(true=A, false=B)
    private bool sideAActive = true;

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

        if (sideAActive)
        {
            sideATimer += Time.deltaTime;
            if (sideATimer >= stepSec && sideA != Doneness.Burnt)
            {
                sideATimer = 0f;
                sideA = (Doneness)Mathf.Min((int)sideA + 1, (int)Doneness.Burnt);
                UpdateVisual();
            }
        }
        else
        {
            sideBTimer += Time.deltaTime;
            if (sideBTimer >= stepSec && sideB != Doneness.Burnt)
            {
                sideBTimer = 0f;
                sideB = (Doneness)Mathf.Min((int)sideB + 1, (int)Doneness.Burnt);
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
            if (sprites == null || sprites.Length < 5) return null;
            var maxSide = (Doneness)Mathf.Max((int)sideA, (int)sideB);
            return sprites[(int)maxSide];
        }
    }

    public void AttachToGrill(GrillSlot slot)
    {
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
        sideAActive = !sideAActive;
        if (sideAActive) sideATimer = 0f; else sideBTimer = 0f;
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
        if (sprites == null || sprites.Length < 5 || img == null) return;

        Doneness activeSide = sideAActive ? sideA : sideB;
        img.sprite = sprites[(int)activeSide];

        // 뒤집기 연출(좌우 반전)
        var s = transform.localScale;
        s.x = sideAActive ? Mathf.Abs(s.x) : -Mathf.Abs(s.x);
        transform.localScale = s;
    }
}
