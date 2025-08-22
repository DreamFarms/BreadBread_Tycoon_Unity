using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutGameManager : MonoBehaviour
{
    public static CutGameManager instance;

    private bool isTouchEnabled;

    [Header("Timer")]
    [SerializeField] private TimerUI timer;
    public float duration = 30f;

    [Header("KeyImage")]
    [SerializeField] private Image[] keyImages;          // UI 이미지 배열 (위, 아래, 좌, 우 순서 등)
    [SerializeField] private Sprite[] keySprites;        // 방향 스프라이트 (예: ↑, ↓, ←, → 이미지)
    private int[] spriteIndex = new int[7];              // 각 위치에 설정된 방향 인덱스

    [Header("CuttingObject")]
    [SerializeField] private List<CuttingObject> cuttingObjects = new List<CuttingObject>();
    [SerializeField] private Transform[] cuttingPositions;
    [SerializeField] private GameObject cuttingPrefab;

    private int currentInputIndex = 0;
    private int cuttingStage = 0;
    private CuttingObject currentCuttingObject => cuttingObjects[1]; // 항상 중앙

    [Header("Character")]
    [SerializeField] private RectTransform chracterTransform;
    [SerializeField] private Image chracter;
    [SerializeField] private Sprite[] face;
    [SerializeField] private Image[] resultImage;
    [SerializeField] private GameObject[] hands;

    [Header("Rail")]
    [SerializeField] private GameObject moveGroup;
    [SerializeField] private GameObject bottomRail;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveDistance;

    [Header("Reward")]
    [SerializeField] private Dictionary<string, int> cutRewardDic = new Dictionary<string, int>();
    private Dictionary<string, Sprite> rewardSpriteDic;   // 캐싱용 딕셔너리
    [SerializeField] private List<Sprite> rewardSprites;  // 모든 보상 스프라이트

    [SerializeField] private GameObject rewardPrefab;
    [SerializeField] private GameObject rewardSuccessPrefab;
    [SerializeField] private GameObject rewardFailPrefab;
    [SerializeField] private Transform rewardGroupTr;

    [Header("Connection")]
    [SerializeField] private RewardConnection cutConnection;

    private float lastInputTime = 0f;
    private bool isSmiling = false;

    private void Awake()
    {
        instance = this;
        CacheRewardSprites();
    }

    private void Update()
    {
        if (isSmiling && Time.time - lastInputTime > 1f)
        {
            chracter.sprite = face[0];
            isSmiling = false;
        }
    }

    private void Start()
    {
        isTouchEnabled = true;
        spriteIndex = new int[keyImages.Length];
        for (int i = 1; i < hands.Length; i++)
        {
            hands[i].SetActive(false);
        }
        ResetChat();
        timer.StartTimer(duration);
        InitCuttingSequence();
    }

    private void CacheRewardSprites()
    {
        rewardSpriteDic = new Dictionary<string, Sprite>();

        foreach (var sprite in rewardSprites)
        {
            if (!rewardSpriteDic.ContainsKey(sprite.name))
            {
                rewardSpriteDic.Add(sprite.name, sprite);
            }
            else
            {
                Debug.LogWarning($"중복된 스프라이트 이름이 있습니다: {sprite.name}");
            }
        }
    }

    private void SetKeyPad()
    {
        for (int i = 0; i < keyImages.Length; i++)
        {
            int index = UnityEngine.Random.Range(0, keySprites.Length);
            ChangeAlpha(i, 1);
            keyImages[i].sprite = keySprites[index];
            spriteIndex[i] = index;
        }

        currentInputIndex = 0;
        cuttingStage = 0;
        currentCuttingObject.Reset(); // 중앙 애만 리셋
        SetHand(false);
        hands[0].SetActive(true);
    }

    public void OnArrowKeyPressed(int inputIndex)
    {
        if (!isTouchEnabled) return;

        if (inputIndex == spriteIndex[currentInputIndex])
        {
            currentInputIndex++;
            if (hands[0].activeSelf == true)
            {
                hands[0].SetActive(false);
            }
            SetHand(true);

            ChangeAlpha(currentInputIndex - 1, 0.5f);

            // 입력이 발생하면 활짝 웃기
            chracter.sprite = face[1];
            lastInputTime = Time.time;
            isSmiling = true;

            if (currentInputIndex == 2 || currentInputIndex == 4 || currentInputIndex == 6)
            {
                cuttingStage++;
                currentCuttingObject.ApplyCuttingStage(cuttingStage);
            }

            if (currentInputIndex >= spriteIndex.Length)
            {
                Debug.Log("전체 성공!");
                lastInputTime = Time.time;
                AddReward(currentCuttingObject.fruitName, 1);
                SetResultChat(true);
                ProceedToNextCutting();  // ← 오브젝트 회전
                SetKeyPad();             // ← 새로운 퍼즐 재설정
            }
        }
        else
        {
            lastInputTime = Time.time;
            chracter.sprite = face[2];
            Debug.Log("틀렸습니다. 초기화됩니다.");
            Shake();
            SetResultChat(false);
            ProceedToNextCutting();
            SetKeyPad();
        }
    }
    
    private void SetHand(bool value)
    {
        if (value == true)
        {
            if (hands[1].activeSelf)
            {
                hands[1].SetActive(false);
                hands[2].SetActive(true);
            }
            else
            {
                hands[1].SetActive(true);
                hands[2].SetActive(false);
            }
        }
        else
        {
            hands[1].SetActive(value);
            hands[2].SetActive(value);
        }
    }

    private void SetResultChat(bool isSuccess)
    {
        if (isSuccess)
        {
            resultImage[0].enabled = true;
        }
        else
        {
            resultImage[1].enabled = true;
        }
    }

    private void ResetChat()
    {
        for (int i = 0; i < resultImage.Length; i++)
        {
            if (resultImage[i].enabled)
            {
                resultImage[i].enabled = false;
            }
        }
    }

    private Tween Shake()
    {
        return chracterTransform.DOShakeAnchorPos(
               duration: 0.5f,
               strength: new Vector2(30f, 0f),
               vibrato: 30,
               randomness: 10f,
               snapping: false,
               fadeOut: true
           );
    }

    private void ProceedToNextCutting()
    {
        isTouchEnabled = false;
        
        // 1. 오른쪽 오브젝트 제거
        var rightMost = cuttingObjects[2];
        cuttingObjects.RemoveAt(2);
        rightMost.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
        {
            Destroy(rightMost.gameObject);
        });

        // 2. 오브젝트 이동 + 레일 이동 동시에 실행
        Sequence seq = DOTween.Sequence();

        // 오브젝트 이동
        seq.Join(cuttingObjects[1].transform.DOMove(cuttingPositions[2].position, 0.5f));
        seq.Join(cuttingObjects[0].transform.DOMove(cuttingPositions[1].position, 0.5f));

        // 레일 이동
        seq.Join(moveGroup.transform.DOMoveX(moveGroup.transform.position.x + moveDistance, 0.5f));
        seq.Join(bottomRail.transform.DOMoveX(bottomRail.transform.position.x - moveDistance, 0.5f));

        // 3. 새 오브젝트 생성 후 왼쪽에 삽입
        var newObj = Instantiate(cuttingPrefab, cuttingPositions[0].position, Quaternion.identity, transform).GetComponent<CuttingObject>();
        int random = UnityEngine.Random.Range(0, 5);
        newObj.SetFruit(random);
        newObj.transform.localScale = Vector3.zero;
        newObj.transform.DOScale(Vector3.one, 0.3f);
        cuttingObjects.Insert(0, newObj);

        seq.OnComplete(() =>
        {
            // OnComplete 안에서 바로 기다릴 수 없으므로 AppendInterval 사용
        }).AppendInterval(0.5f)
          .OnComplete(() =>
          {
              isSmiling = false;
              isTouchEnabled = true;
              chracter.sprite = face[0];
              ResetChat();
          });
    }

    #region TMPRAIL
    private void MoveRail()
    {
        StartCoroutine(CoMoveRail());
    }

    IEnumerator CoMoveRail()
    {
        // 목표 위치를 잡고
        // 그 위치에 도달하면 반복문 탈출
        // 그리고 우유 생성
        float targetTopRailPosition = moveGroup.transform.position.x + moveDistance; // 목표 x 위치

        while (moveGroup.transform.position.x >= targetTopRailPosition) // 목표 위치에 도달하면 반복문 탈출
        {
            Vector2 currentTopPosition = moveGroup.transform.position;
            currentTopPosition.x -= moveSpeed * Time.deltaTime;
            moveGroup.transform.position = currentTopPosition;

            Vector2 currentBottomPosition = bottomRail.transform.position;
            currentBottomPosition.x += moveSpeed * Time.deltaTime;
            bottomRail.transform.position = currentBottomPosition;

            yield return null;
        }
        isTouchEnabled = true;
    }
    #endregion

    private void ChangeAlpha(int inputIndex, float value)
    {
        Color color = keyImages[inputIndex].color;
        color.a = value;
        keyImages[inputIndex].color = color;
    }

    private void ResetInput()
    {
        currentInputIndex = 0;
        cuttingStage = 0;

        // 중앙 컷팅 오브젝트만 리셋
        cuttingObjects[1].Reset();

        // 키패드 다시 설정
        Invoke("SetKeyPad", 1f);
    }

    private void InitCuttingSequence()
    {
        cuttingObjects.Clear();
        for (int i = 0; i < 3; i++)
        {
            var obj = Instantiate(cuttingPrefab, cuttingPositions[i].position, Quaternion.identity, transform).GetComponent<CuttingObject>();
            int random = UnityEngine.Random.Range(0, 5);
            obj.SetFruit(random);
            obj.gameObject.SetActive(i < 2); // 처음엔 2개만 보여줌
            cuttingObjects.Add(obj);
        }

        SetKeyPad(); // 첫 퍼즐 설정
    }

    public void OnCuttingSuccess()
    {
        if (cuttingObjects.Count == 0) return;

        // 1. 오른쪽 오브젝트 제거
        var rightMost = cuttingObjects[2];
        cuttingObjects.RemoveAt(2);
        rightMost.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
        {
            Destroy(rightMost.gameObject);
        });

        // 2. Center → Right, Left → Center 이동
        cuttingObjects[1].transform.DOMove(cuttingPositions[2].position, 0.3f);
        cuttingObjects[0].transform.DOMove(cuttingPositions[1].position, 0.3f);

        // 3. 새 오브젝트 생성 후 Left 위치에 삽입
        var newObj = Instantiate(cuttingPrefab, cuttingPositions[0].position, Quaternion.identity, transform).GetComponent<CuttingObject>();
        newObj.transform.localScale = Vector3.zero;
        int random = UnityEngine.Random.Range(0, 5);
        newObj.SetFruit(random);
        newObj.gameObject.SetActive(true);
        newObj.transform.DOScale(Vector3.one, 0.3f);

        // 4. 리스트 업데이트 (왼쪽에 추가)
        cuttingObjects.Insert(0, newObj);
    }

    private void AddReward(string fruitName, int amount)
    {
        if (cutRewardDic.ContainsKey(fruitName))
            cutRewardDic[fruitName] += amount;
        else
            cutRewardDic[fruitName] = amount;

        Debug.Log($"{fruitName} +{amount}, 총 {cutRewardDic[fruitName]}개");
    }

    public void ShowResult()
    {
        foreach (var reward in cutRewardDic)
        {
            Debug.Log($"{reward.Key} : {reward.Value}개 획득");
        }
    }

    public void RequestConnection()
    {
        cutConnection.RewardSaveRequest(cutRewardDic);
    }

    public void ShowRewardUI()
    {
        foreach (Transform child in rewardGroupTr)
            Destroy(child.gameObject);

        if (cutRewardDic.Keys.Count <= 0)
        {
            rewardFailPrefab.SetActive(true);
            return;
        }

        foreach (var reward in cutRewardDic)
        {
            GameObject rewardGo = Instantiate(rewardPrefab, rewardGroupTr);
            rewardGo.SetActive(true);
            RewardUI rewardUi = rewardGo.GetComponent<RewardUI>();

            if (rewardSpriteDic.TryGetValue(reward.Key, out Sprite sprite))
            {
                rewardUi.itemImage.sprite = sprite;
            }
            else
            {
                Debug.LogWarning($"스프라이트를 찾을 수 없습니다: {reward.Key}");
            }

            rewardUi.countText.text = reward.Value.ToString() + "개";
        }
        //ShowResult();
        rewardSuccessPrefab.SetActive(true);
        RequestConnection();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game_Cut");
    }

    public void Exit()
    {
        SceneManager.LoadScene("Map");
    }
}
