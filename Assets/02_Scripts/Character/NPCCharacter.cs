using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ExcelReader2;

public class NPCCharacter : Character
{
    public override void SetCharacterPrefer(string name)
    {
        print("Ȯ���� �� �̸�" + name);
        CheckPrefer(name);
    }
}
