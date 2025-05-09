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
    Cake_Strawberry, // "Strawberr"�� �ƴ� "Strawberry"�� ����
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

    // ��ȣ�ϴ� ��/������� Ȯ���ϴ� �޼���
    public void CheckPrefer(string breadName, int money)
    {
        foreach(var bread in preferBreads)
        {
            print("��ȣ�ϴ� �� " + preferBreads[0].ToString());
            if(breadName.Equals(bread.ToString()))
            {
                print("�� ���� �Ϸ�");
                myBread myBread = new myBread();
                myBread.breadName = breadName;
                myBread.money = money;

                PickBread(myBread);
                return;
            }
        }
        print("breadName" + breadName);
        Debug.Log($"{gameObject.name}�� �Ŵ뿡 ��ȣ�ϴ� ���� �����Ƿ� �ƹ��͵� ���� �ʾҽ��ϴ�.");
    }

    // ���� �ϳ��� ���� �޼���
    public void PickBread(myBread bread)
    {
        // todo : Ȯ��
        pickedBreadStack.Push(bread);
    }

    public void BuyBreadCounter()
    {
        connection.StartBreadSellConnection(pickedBreadStack);

    }

    // ĳ���� Ư�� �����ϴ� �޼���
    // ��ȣ�ϴ� ��, ��ȣ�ϴ� ��Ḧ ����
    public abstract void SetCharacterPrefer(string name, int money);
}
