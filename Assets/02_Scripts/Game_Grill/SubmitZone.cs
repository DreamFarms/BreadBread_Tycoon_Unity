using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubmitZone : MonoBehaviour, IDropHandler
{
    [Header("Stack UI")]
    [SerializeField] private RectTransform stackParent;
    [SerializeField] private Vector2 startPos = Vector2.zero;
    [SerializeField] private Vector2 step = new Vector2(100f, 0f);
    [SerializeField] private int maxPerRow = 3;

    private RectTransform Parent => stackParent ? stackParent : (RectTransform)transform;

    [Header("Counts")]
    [SerializeField] private int count = 0;          // ��ü ��ǰ ����
    [SerializeField] private int plateCapacity = 3;  // �� �׸��� �� �� ������(�䱸: 3)
    private int plateCount = 0;                      // ���� �׸��� ���� ����(0~capacity)

    [Header("Plate Animation")]
    [Tooltip("�ִϸ��̼� �� �̵���ų �׸� RectTransform (���� stackParent ���)")]
    [SerializeField] private RectTransform plateRectOverride;
    [SerializeField] private float dropOffsetY = -200f;   // �Ʒ��� �̵� �Ÿ�(����=�Ʒ�)
    [SerializeField] private float dropTime = 0.20f;    // �������� �ð�
    [SerializeField] private float returnTime = 0.25f;    // �ö���� �ð�
    [SerializeField] private Ease dropEase = Ease.InCubic;
    [SerializeField] private Ease returnEase = Ease.OutBack;

    private bool plateAnimating = false;

    [Header("Reject FX (Hand)")]
    [SerializeField] private RectTransform hand;     // �� �̹���(���� Canvas/�θ� ����)
    [SerializeField] private float yStart = -1000f;  // ���� Y(����)
    [SerializeField] private float yHit = -700f;   // Ÿ�� Y(����)
    [SerializeField] private float tIn = 0.15f;   // -1000 -> -700
    [SerializeField] private float tPunch = 0.10f;   // Ÿ�� ��ġ
    [SerializeField] private float tOut = 0.15f;   // -700 -> -1000
    [SerializeField] private AudioSource sfx;        // (�ɼ�) ȿ���� �ҽ�
    [SerializeField] private AudioClip sfxHit;       // (�ɼ�) Ÿ����

    [Header("Reject FX (Sausage)")]
    [SerializeField] private Vector2 flyDir = new Vector2(420f, 240f); // ������ ����
    [SerializeField] private float flyTime = 0.5f;
    [SerializeField] private float spinZ = 360f;
    [SerializeField] private float fadeTime = 0.25f;

    // (�ɼ�) �׸� ȸ��/��ü ���� ��
    public System.Action<int> OnPlateSubmitted;  // ���ڷ� �̹� �׸��� ���� ����(=capacity)
    public event System.Action OnSausageSubmittedSuccess;

    public void OnDrop(PointerEventData e)
    {
        var drag = e.pointerDrag ? e.pointerDrag.GetComponent<IngrdientController>() : null;
        if (drag == null) return;

        var sausage = drag.GetComponent<SausageItem>();
        if (sausage == null) return;

        drag.MarkDropped();

        // �׸��� �ִ� �Ÿ� ���� ���� ����
        if (sausage.CurrentSlot != null)
        {
            sausage.CurrentSlot.Vacate();
            sausage.DetachFromGrill();
        }

        if (sausage.IsExactlyBothDone)
        {
            if (plateAnimating) 
            { return; }

            AddToPlate(drag);
            count++;
            OnSausageSubmittedSuccess?.Invoke();

            if (plateCount >= plateCapacity)
            {
                CollectPlateAndReset(); // ���⼭ ���� �������ٰ� �ö���� ����
            }
        }
        else
        {
            // ���д� ���(���ϸ� ���⼭ ��/������ ���� ȣ��)
            PlayRejectFx(drag.GetComponent<RectTransform>(), sausage);
            Debug.Log("���� ����: Done �ƴ� �� ����");
        }
    }

    private void AddToPlate(IngrdientController drag)
    {
        var rect = drag.GetComponent<RectTransform>();
        rect.SetParent(Parent, worldPositionStays: false);

        Vector2 pos = startPos;
        pos += new Vector2(0f, -step.y * plateCount);   // �Ʒ��� �������� �ױ�
        rect.anchoredPosition = pos;

        plateCount++;
    }

    private void CollectPlateAndReset()
    {
        plateAnimating = true;

        var plate = plateRectOverride ? plateRectOverride : Parent;
        plate.DOKill(); // ���� Ʈ�� ����

        var start = plate.anchoredPosition;
        var down = start + new Vector2(0f, dropOffsetY);

        // �ܺ� �̺�Ʈ(������� ��) ����
        OnPlateSubmitted?.Invoke(plateCount);

        DOTween.Sequence()
            // 1) �Ʒ��� �������� ���
            .Append(plate.DOAnchorPos(down, dropTime).SetEase(dropEase))
            // 2) �Ʒ� ���� �� ���� ���� plateCount ����
            .AppendCallback(() =>
            {
                ClearPlateChildren(); // SausageItem�� ����
                plateCount = 0;
                // �ʿ��ϸ� �� ���� ��������Ʈ ����/���� ��� �� ó��
            })
            // 3) ���� �ڸ��� ����
            .Append(plate.DOAnchorPos(start, returnTime).SetEase(returnEase))
            .OnComplete(() =>
            {
                plateAnimating = false;
            });
    }

    private void ClearPlateChildren()
    {
        for (int i = Parent.childCount - 1; i >= 0; --i)
        {
            var child = Parent.GetChild(i).gameObject;
            if (child.GetComponent<SausageItem>() != null)
                Destroy(child);
        }
    }

    // ====== ����: ���� -1000 -> -700 ������ ġ�� �ٽ� ���� ======
    private void PlayRejectFx(RectTransform sausageRect, SausageItem sausage)
    {
        if (hand == null)
        {
            // �� ���� �� �Ǿ� ������ �׳� ��� ����
            sausage.Consume();
            return;
        }

        // �巡�� ���� + CanvasGroup �غ�
        var cg = sausageRect.GetComponent<CanvasGroup>();
        if (cg == null) cg = sausageRect.gameObject.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = false;

        // �� ���� ��ġ ����(���� X ����, Y�� yStart)
        var hp = hand.anchoredPosition;
        hand.anchoredPosition = new Vector2(hp.x, yStart);
        hand.gameObject.SetActive(true);

        // ������ ����
        DOTween.Sequence()
            // �� ��������
            .Append(hand.DOAnchorPosY(yHit, tIn).SetEase(Ease.OutCubic))
            // ��Ʈ Ÿ�̹�
            .AppendCallback(() =>
            {
                if (sfx && sfxHit) sfx.PlayOneShot(sfxHit);
                // �Ҽ��� ������ ����
                FlyAwayAndDestroy(sausageRect, cg, sausage);
            })
            // ��ġ ����(���ϰ�)
            .Append(hand.DOPunchScale(new Vector3(0.08f, -0.08f, 0f), tPunch, 8, 0.5f))
            // �� ����
            .Append(hand.DOAnchorPosY(yStart, tOut).SetEase(Ease.InCubic))
            .OnComplete(() => hand.gameObject.SetActive(false));
    }

    // ====== �Ҽ��� ���������� ���� ======
    private void FlyAwayAndDestroy(RectTransform sausageRect, CanvasGroup cg, SausageItem sausage)
    {
        // �ڿ�������� �ణ ����
        Vector2 jitter = new Vector2(Random.Range(-80f, 80f), Random.Range(0f, 120f));
        Vector2 endPos = sausageRect.anchoredPosition + flyDir + jitter;

        var move = sausageRect.DOAnchorPos(endPos, flyTime).SetEase(Ease.OutCubic);
        var rotate = sausageRect.DOLocalRotate(new Vector3(0, 0, spinZ), flyTime, RotateMode.FastBeyond360)
                                .SetEase(Ease.OutCubic);
        var fade = cg.DOFade(0f, Mathf.Min(flyTime, fadeTime)).SetEase(Ease.Linear);

        DOTween.Sequence()
               .Join(move)
               .Join(rotate)
               .Join(fade)
               .OnComplete(() => sausage.Consume());
    }
}
