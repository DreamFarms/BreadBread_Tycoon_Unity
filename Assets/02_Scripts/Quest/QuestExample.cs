using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestExample : MonoBehaviour
{
    void Start()
    {
        // 퀘스트 생성
        SaleQuest breadQuest = new SaleQuest(
            "quest_sell_bread",                  // 퀘스트 ID
            "빵 판매 마스터",                     // 퀘스트 제목
            "빵을 판매하세요!",               // 퀘스트 설명
            "item_bread",                        // 타겟 아이템 ID
            5                                    // 필요 개수
        );

        // 보상 추가
        breadQuest.AddReward(new ItemReward(
            "reward_gold",                       // 보상 ID
            "금화",                               // 보상 이름
            "퀘스트 완료 보상으로 받은 금화입니다.", // 보상 설명
            "item_gold",                         // 아이템 ID
            100                                  // 개수
        ));

        // 퀘스트 생성
        SaleQuest cookieQuest = new SaleQuest(
            "quest_sell_cookie",                  // 퀘스트 ID
            "쿠키 판매 마스터",                     // 퀘스트 제목
            "쿠키를 판매하세요!",               // 퀘스트 설명
            "item_bread",                        // 타겟 아이템 ID
            5                                    // 필요 개수
        );

        // 보상 추가
        breadQuest.AddReward(new ItemReward(
            "reward_gold",                       // 보상 ID
            "금화",                               // 보상 이름
            "퀘스트 완료 보상으로 받은 금화입니다.", // 보상 설명
            "item_gold",                         // 아이템 ID
            100                                  // 개수
        ));

        // 퀘스트 매니저에 등록 및 시작
        QuestManager.Instance.RegisterQuest(breadQuest);
        QuestManager.Instance.StartQuest("quest_sell_bread");

        QuestManager.Instance.RegisterQuest(cookieQuest);
        QuestManager.Instance.StartQuest("quest_sell_cookie");

        // 예시: 빵 판매 이벤트 발생 시 호출
        // QuestManager.Instance.UpdateQuestProgress("quest_sell_bread", "item_bread", 1);
    }
}
