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



public abstract class Character : MonoBehaviour
{
    private Stack pickedBreadStack = new Stack();
    private List<Bread> preferBreads = new List<Bread>();
    private List<Ingredient> preferIngredients = new List<Ingredient>();

    // ��ȣ�ϴ� ��/������� Ȯ���ϴ� �޼���
    public void CheckPrefer(string breadName)
    {
        foreach(var bread in preferBreads)
        {
            if(breadName.Equals(bread))
            {
                PickBread(breadName);
                return;
            }
        }
        Debug.Log($"{gameObject.name}�� �Ŵ뿡 ��ȣ�ϴ� ���� �����Ƿ� �ƹ��͵� ���� �ʾҽ��ϴ�.");
    }

    // ���� �ϳ��� ���� �޼���
    public void PickBread(string breadName)
    {
        // todo : Ȯ��
        pickedBreadStack.Push(breadName);
    }

    // ĳ���� Ư�� �����ϴ� �޼���
    // ��ȣ�ϴ� ��, ��ȣ�ϴ� ��Ḧ ����
    public abstract void SetCharacterPrefer();
}
