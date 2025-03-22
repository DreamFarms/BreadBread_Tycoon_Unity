using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestExample : MonoBehaviour
{
    void Start()
    {
        // ����Ʈ ����
        SaleQuest breadQuest = new SaleQuest(
            "quest_sell_bread",                  // ����Ʈ ID
            "�� �Ǹ� ������",                     // ����Ʈ ����
            "���� �Ǹ��ϼ���!",               // ����Ʈ ����
            "item_bread",                        // Ÿ�� ������ ID
            5                                    // �ʿ� ����
        );

        // ���� �߰�
        breadQuest.AddReward(new ItemReward(
            "reward_gold",                       // ���� ID
            "��ȭ",                               // ���� �̸�
            "����Ʈ �Ϸ� �������� ���� ��ȭ�Դϴ�.", // ���� ����
            "item_gold",                         // ������ ID
            100                                  // ����
        ));

        // ����Ʈ ����
        SaleQuest cookieQuest = new SaleQuest(
            "quest_sell_cookie",                  // ����Ʈ ID
            "��Ű �Ǹ� ������",                     // ����Ʈ ����
            "��Ű�� �Ǹ��ϼ���!",               // ����Ʈ ����
            "item_bread",                        // Ÿ�� ������ ID
            5                                    // �ʿ� ����
        );

        // ���� �߰�
        breadQuest.AddReward(new ItemReward(
            "reward_gold",                       // ���� ID
            "��ȭ",                               // ���� �̸�
            "����Ʈ �Ϸ� �������� ���� ��ȭ�Դϴ�.", // ���� ����
            "item_gold",                         // ������ ID
            100                                  // ����
        ));

        // ����Ʈ �Ŵ����� ��� �� ����
        QuestManager.Instance.RegisterQuest(breadQuest);
        QuestManager.Instance.StartQuest("quest_sell_bread");

        QuestManager.Instance.RegisterQuest(cookieQuest);
        QuestManager.Instance.StartQuest("quest_sell_cookie");

        // ����: �� �Ǹ� �̺�Ʈ �߻� �� ȣ��
        // QuestManager.Instance.UpdateQuestProgress("quest_sell_bread", "item_bread", 1);
    }
}
