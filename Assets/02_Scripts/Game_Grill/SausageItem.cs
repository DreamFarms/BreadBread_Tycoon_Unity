using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Doneness { Raw = 0, Seared = 1, Medium = 2, Done = 3, Burnt = 4 }

public class SausageItem : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private Image img;
    [Tooltip("Doneness ������� 5��: Raw, Seared, Medium, Done, Burnt")]

    [SerializeField] private Sprite[] frontSprites = new Sprite[5]; // A��(��)
    [SerializeField] private Sprite[] backSprites = new Sprite[5];  // B��(��)

    [Header("Timing")]
    [SerializeField] private float stepSec = 5f;   // 5�ʸ��� ���� �ܰ�

    // �� �� ����/Ÿ�̸�
    private Doneness sideA = Doneness.Raw;
    private Doneness sideB = Doneness.Raw;
    private float sideATimer = 0f;
    private float sideBTimer = 0f;

    // ���� ������ A����? (true=A ��, false=B ��)
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
            // A�� ���� B�� �Ʒ� �� B�� ����
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
            // B�� ���� A�� �Ʒ� �� A�� ����
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
            OwnerSpawner = null; // ���Ŀ��� �׸��� ����
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
        // ����/�Ʒ��� ������
        faceUpIsA = !faceUpIsA;

        // ���� "�Ʒ��� �� ��"�� ������ �����ϹǷ� �� ���� Ÿ�̸Ӹ� 0����
        if (faceUpIsA)
            sideBTimer = 0f; // B�� �Ʒ��� ��
        else
            sideATimer = 0f; // A�� �Ʒ��� ��

        UpdateVisual();
    }

    // ���� ��Ģ: ��� Done(3) �̻�, Burnt(4) ����
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

        // �¿� ���� ������ ����
        var s = transform.localScale;
        s.x = faceUpIsA ? Mathf.Abs(s.x) : -Mathf.Abs(s.x);
        transform.localScale = s;
    }
}
