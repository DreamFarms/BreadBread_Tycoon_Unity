using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] GameObject breadUI;
    [SerializeField] Image breadImage;

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
        ShowImage();
    }

    public void BuyBreadCounter()
    {
        connection.StartBreadSellConnection(pickedBreadStack);
    }

    public void ShowImage()
    {
        StopCoroutine(FadeOutImage());
        Color c = breadImage.color;
        breadImage.color = new Color(c.r, c.g, c.b, 1f);
        breadUI.gameObject.SetActive(true);
        StartCoroutine(FadeOutImage());
    }

    private IEnumerator FadeOutImage()
    {
        yield return new WaitForSeconds(2f); // 1�� ���

        float duration = 1f;
        float time = 0f;
        Color startColor = breadImage.color;

        while (time < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, time / duration);
            breadImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            time += Time.deltaTime;
            yield return null;
        }

        breadImage.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        breadImage.gameObject.SetActive(false);
    }

    // ĳ���� Ư�� �����ϴ� �޼���
    // ��ȣ�ϴ� ��, ��ȣ�ϴ� ��Ḧ ����
    public abstract void SetCharacterPrefer(string name, int money);
}
