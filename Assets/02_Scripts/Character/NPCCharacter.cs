using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ExcelReader2;

public class NPCCharacter : Character
{
    public override void SetCharacterPrefer(string name, int money)
    {
        print("확인한 빵 이름" + name);
        CheckPrefer(name, money);
    }
}
