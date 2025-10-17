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
    [SerializeField] private int count = 0;          // 전체 납품 누적
    [SerializeField] private int plateCapacity = 3;  // 한 그릇에 몇 개 쌓을지(요구: 3)
    private int plateCount = 0;                      // 현재 그릇에 쌓인 개수(0~capacity)

    [Header("Plate Animation")]
    [Tooltip("애니메이션 시 이동시킬 그릇 RectTransform (비우면 stackParent 사용)")]
    [SerializeField] private RectTransform plateRectOverride;
    [SerializeField] private float dropOffsetY = -200f;   // 아래로 이동 거리(음수=아래)
    [SerializeField] private float dropTime = 0.20f;    // 내려가는 시간
    [SerializeField] private float returnTime = 0.25f;    // 올라오는 시간
    [SerializeField] private Ease dropEase = Ease.InCubic;
    [SerializeField] private Ease returnEase = Ease.OutBack;

    private bool plateAnimating = false;

    [Header("Reject FX (Hand)")]
    [SerializeField] private RectTransform hand;     // 손 이미지(같은 Canvas/부모 권장)
    [SerializeField] private float yStart = -1000f;  // 시작 Y(숨김)
    [SerializeField] private float yHit = -700f;   // 타격 Y(보임)
    [SerializeField] private float tIn = 0.15f;   // -1000 -> -700
    [SerializeField] private float tPunch = 0.10f;   // 타격 펀치
    [SerializeField] private float tOut = 0.15f;   // -700 -> -1000
    [SerializeField] private AudioSource sfx;        // (옵션) 효과음 소스
    [SerializeField] private AudioClip sfxHit;       // (옵션) 타격음

    [Header("Reject FX (Sausage)")]
    [SerializeField] private Vector2 flyDir = new Vector2(420f, 240f); // 오른쪽 위로
    [SerializeField] private float flyTime = 0.5f;
    [SerializeField] private float spinZ = 360f;
    [SerializeField] private float fadeTime = 0.25f;

    // (옵션) 그릇 회수/교체 연출 훅
    public System.Action<int> OnPlateSubmitted;  // 인자로 이번 그릇에 쌓인 개수(=capacity)
    public event System.Action OnSausageSubmittedSuccess;

    public void OnDrop(PointerEventData e)
    {
        var drag = e.pointerDrag ? e.pointerDrag.GetComponent<IngrdientController>() : null;
        if (drag == null) return;

        var sausage = drag.GetComponent<SausageItem>();
        if (sausage == null) return;

        drag.MarkDropped();

        // 그릴에 있던 거면 비우고 상태 정리
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
                CollectPlateAndReset(); // 여기서 접시 내려갔다가 올라오는 연출
            }
        }
        else
        {
            // 실패는 폐기(원하면 여기서 손/날리기 연출 호출)
            PlayRejectFx(drag.GetComponent<RectTransform>(), sausage);
            Debug.Log("제출 실패: Done 아님 → 삭제");
        }
    }

    private void AddToPlate(IngrdientController drag)
    {
        var rect = drag.GetComponent<RectTransform>();
        rect.SetParent(Parent, worldPositionStays: false);

        Vector2 pos = startPos;
        pos += new Vector2(0f, -step.y * plateCount);   // 아래로 내려가며 쌓기
        rect.anchoredPosition = pos;

        plateCount++;
    }

    private void CollectPlateAndReset()
    {
        plateAnimating = true;

        var plate = plateRectOverride ? plateRectOverride : Parent;
        plate.DOKill(); // 기존 트윈 종료

        var start = plate.anchoredPosition;
        var down = start + new Vector2(0f, dropOffsetY);

        // 외부 이벤트(점수계산 등) 통지
        OnPlateSubmitted?.Invoke(plateCount);

        DOTween.Sequence()
            // 1) 아래로 떨어지는 모션
            .Append(plate.DOAnchorPos(down, dropTime).SetEase(dropEase))
            // 2) 아래 도착 시 내용 비우고 plateCount 리셋
            .AppendCallback(() =>
            {
                ClearPlateChildren(); // SausageItem들 제거
                plateCount = 0;
                // 필요하면 빈 접시 스프라이트 변경/사운드 재생 등 처리
            })
            // 3) 원래 자리로 복귀
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

    // ====== 연출: 손이 -1000 -> -700 내려와 치고 다시 복귀 ======
    private void PlayRejectFx(RectTransform sausageRect, SausageItem sausage)
    {
        if (hand == null)
        {
            // 손 세팅 안 되어 있으면 그냥 즉시 삭제
            sausage.Consume();
            return;
        }

        // 드래그 차단 + CanvasGroup 준비
        var cg = sausageRect.GetComponent<CanvasGroup>();
        if (cg == null) cg = sausageRect.gameObject.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = false;

        // 손 시작 위치 세팅(현재 X 유지, Y만 yStart)
        var hp = hand.anchoredPosition;
        hand.anchoredPosition = new Vector2(hp.x, yStart);
        hand.gameObject.SetActive(true);

        // 시퀀스 구성
        DOTween.Sequence()
            // 손 내려오기
            .Append(hand.DOAnchorPosY(yHit, tIn).SetEase(Ease.OutCubic))
            // 히트 타이밍
            .AppendCallback(() =>
            {
                if (sfx && sfxHit) sfx.PlayOneShot(sfxHit);
                // 소세지 날리기 시작
                FlyAwayAndDestroy(sausageRect, cg, sausage);
            })
            // 펀치 연출(약하게)
            .Append(hand.DOPunchScale(new Vector3(0.08f, -0.08f, 0f), tPunch, 8, 0.5f))
            // 손 복귀
            .Append(hand.DOAnchorPosY(yStart, tOut).SetEase(Ease.InCubic))
            .OnComplete(() => hand.gameObject.SetActive(false));
    }

    // ====== 소세지 날려버리고 삭제 ======
    private void FlyAwayAndDestroy(RectTransform sausageRect, CanvasGroup cg, SausageItem sausage)
    {
        // 자연스러움용 약간 랜덤
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
