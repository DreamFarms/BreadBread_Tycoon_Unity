using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Bread
{
    Sandwich,
    Icebox_Strawberry,
    Donut_Chocolate,
    ButterBar_Plane,
    RollCake_Chocolate,
    RollCake_Strawberry,
    ButterRoll_Salted,
    Bread_Melon,
    Cake_Strawberry, // "Strawberr"가 아닌 "Strawberry"로 수정
    Cookie_Strawberry,
    DinnerRoll
}

public enum Ingredient
{
    Flour,
    Flour_Green,
    Flour_Red,
    Salt,
    Sugar,
    Butter,
    Egg,
    Milk,
    FreshApple,
    FreshBanana,
    FreshBlueberry,
    FreshPeach,
    FreshStrawberry,
    Chocolate,
    MelonSyrup,
    Vanilla
}

public class myBread
{
    public string breadName;
    public int money;
}

public abstract class Character : MonoBehaviour
{
    public string Name;
    private Stack pickedBreadStack = new Stack();
    public List<Bread> preferBreads = new List<Bread>();
    public List<Ingredient> preferIngredients = new List<Ingredient>();
    [SerializeField] BreadSellConnection connection;

    // 선호하는 빵/재료인지 확인하는 메서드
    public void CheckPrefer(string breadName, int money)
    {
        foreach(var bread in preferBreads)
        {
            print("선호하는 빵 " + preferBreads[0].ToString());
            if(breadName.Equals(bread.ToString()))
            {
                print("빵 구매 완료");
                myBread myBread = new myBread();
                myBread.breadName = breadName;
                myBread.money = money;

                PickBread(myBread);
                return;
            }
        }
        print("breadName" + breadName);
        Debug.Log($"{gameObject.name}은 매대에 선호하는 빵이 없으므로 아무것도 사지 않았습니다.");
    }

    // 빵을 하나씩 고르는 메서드
    public void PickBread(myBread bread)
    {
        // todo : 확률
        pickedBreadStack.Push(bread);
    }

    public void BuyBreadCounter()
    {
        connection.StartBreadSellConnection(pickedBreadStack);

    }

    // 캐릭터 특성 설정하는 메서드
    // 선호하는 빵, 선호하는 재료를 설정
    public abstract void SetCharacterPrefer(string name, int money);
}
