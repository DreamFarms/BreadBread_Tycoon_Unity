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
    [SerializeField] private Image[] keyImages;          // UI �̹��� �迭 (��, �Ʒ�, ��, �� ���� ��)
    [SerializeField] private Sprite[] keySprites;        // ���� ��������Ʈ (��: ��, ��, ��, �� �̹���)
    private int[] spriteIndex = new int[7];              // �� ��ġ�� ������ ���� �ε���

    [Header("CuttingObject")]
    [SerializeField] private List<CuttingObject> cuttingObjects = new List<CuttingObject>();
    [SerializeField] private Transform[] cuttingPositions;
    [SerializeField] private GameObject cuttingPrefab;

    private int currentInputIndex = 0;
    private int cuttingStage = 0;
    private CuttingObject currentCuttingObject => cuttingObjects[1]; // �׻� �߾�

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
    private Dictionary<string, Sprite> rewardSpriteDic;   // ĳ�̿� ��ųʸ�
    [SerializeField] private List<Sprite> rewardSprites;  // ��� ���� ��������Ʈ

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
                Debug.LogWarning($"�ߺ��� ��������Ʈ �̸��� �ֽ��ϴ�: {sprite.name}");
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
        currentCuttingObject.Reset(); // �߾� �ָ� ����
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

            // �Է��� �߻��ϸ� Ȱ¦ ����
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
                Debug.Log("��ü ����!");
                lastInputTime = Time.time;
                AddReward(currentCuttingObject.fruitName, 1);
                SetResultChat(true);
                ProceedToNextCutting();  // �� ������Ʈ ȸ��
                SetKeyPad();             // �� ���ο� ���� �缳��
            }
        }
        else
        {
            lastInputTime = Time.time;
            chracter.sprite = face[2];
            Debug.Log("Ʋ�Ƚ��ϴ�. �ʱ�ȭ�˴ϴ�.");
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
        
        // 1. ������ ������Ʈ ����
        var rightMost = cuttingObjects[2];
        cuttingObjects.RemoveAt(2);
        rightMost.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
        {
            Destroy(rightMost.gameObject);
        });

        // 2. ������Ʈ �̵� + ���� �̵� ���ÿ� ����
        Sequence seq = DOTween.Sequence();

        // ������Ʈ �̵�
        seq.Join(cuttingObjects[1].transform.DOMove(cuttingPositions[2].position, 0.5f));
        seq.Join(cuttingObjects[0].transform.DOMove(cuttingPositions[1].position, 0.5f));

        // ���� �̵�
        seq.Join(moveGroup.transform.DOMoveX(moveGroup.transform.position.x + moveDistance, 0.5f));
        seq.Join(bottomRail.transform.DOMoveX(bottomRail.transform.position.x - moveDistance, 0.5f));

        // 3. �� ������Ʈ ���� �� ���ʿ� ����
        var newObj = Instantiate(cuttingPrefab, cuttingPositions[0].position, Quaternion.identity, transform).GetComponent<CuttingObject>();
        int random = UnityEngine.Random.Range(0, 5);
        newObj.SetFruit(random);
        newObj.transform.localScale = Vector3.zero;
        newObj.transform.DOScale(Vector3.one, 0.3f);
        cuttingObjects.Insert(0, newObj);

        seq.OnComplete(() =>
        {
            // OnComplete �ȿ��� �ٷ� ��ٸ� �� �����Ƿ� AppendInterval ���
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
        // ��ǥ ��ġ�� ���
        // �� ��ġ�� �����ϸ� �ݺ��� Ż��
        // �׸��� ���� ����
        float targetTopRailPosition = moveGroup.transform.position.x + moveDistance; // ��ǥ x ��ġ

        while (moveGroup.transform.position.x >= targetTopRailPosition) // ��ǥ ��ġ�� �����ϸ� �ݺ��� Ż��
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

        // �߾� ���� ������Ʈ�� ����
        cuttingObjects[1].Reset();

        // Ű�е� �ٽ� ����
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
            obj.gameObject.SetActive(i < 2); // ó���� 2���� ������
            cuttingObjects.Add(obj);
        }

        SetKeyPad(); // ù ���� ����
    }

    public void OnCuttingSuccess()
    {
        if (cuttingObjects.Count == 0) return;

        // 1. ������ ������Ʈ ����
        var rightMost = cuttingObjects[2];
        cuttingObjects.RemoveAt(2);
        rightMost.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
        {
            Destroy(rightMost.gameObject);
        });

        // 2. Center �� Right, Left �� Center �̵�
        cuttingObjects[1].transform.DOMove(cuttingPositions[2].position, 0.3f);
        cuttingObjects[0].transform.DOMove(cuttingPositions[1].position, 0.3f);

        // 3. �� ������Ʈ ���� �� Left ��ġ�� ����
        var newObj = Instantiate(cuttingPrefab, cuttingPositions[0].position, Quaternion.identity, transform).GetComponent<CuttingObject>();
        newObj.transform.localScale = Vector3.zero;
        int random = UnityEngine.Random.Range(0, 5);
        newObj.SetFruit(random);
        newObj.gameObject.SetActive(true);
        newObj.transform.DOScale(Vector3.one, 0.3f);

        // 4. ����Ʈ ������Ʈ (���ʿ� �߰�)
        cuttingObjects.Insert(0, newObj);
    }

    private void AddReward(string fruitName, int amount)
    {
        if (cutRewardDic.ContainsKey(fruitName))
            cutRewardDic[fruitName] += amount;
        else
            cutRewardDic[fruitName] = amount;

        Debug.Log($"{fruitName} +{amount}, �� {cutRewardDic[fruitName]}��");
    }

    public void ShowResult()
    {
        foreach (var reward in cutRewardDic)
        {
            Debug.Log($"{reward.Key} : {reward.Value}�� ȹ��");
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
                Debug.LogWarning($"��������Ʈ�� ã�� �� �����ϴ�: {reward.Key}");
            }

            rewardUi.countText.text = reward.Value.ToString() + "��";
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
