using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCharacter : Character
{
    public override void SetCharacterPrefer(string name, int money)
    {
        print("Ȯ���� �� �̸�" + name);
        CheckPrefer(name, money);
    }
}
