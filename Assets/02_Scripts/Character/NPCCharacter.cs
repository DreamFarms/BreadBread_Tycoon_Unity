using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ExcelReader2;

public class NPCCharacter : Character
{
    public override void SetCharacterPrefer(string name, int money)
    {
        print("Ȯ���� �� �̸�" + name);
        CheckPrefer(name, money);
    }
}
