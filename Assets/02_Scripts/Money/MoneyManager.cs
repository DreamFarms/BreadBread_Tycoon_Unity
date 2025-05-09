using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    public int money { get; private set; }
    
    public int cash { get; private set; }

    private int minMoney;
    private int maxMoney;

    [SerializeField] private MoneyUI moneyUI;

    public enum CalculateState
    {
        Plus,
        Minus
    }

    public enum Reward
    {
        Money,
        Cash
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(this);
        minMoney = 0;
        maxMoney = 99999999;
        money = 100;
        money = Mathf.Clamp(money, minMoney, maxMoney);
    }

    public CalculateState state { get; private set; }

    public void SetMoney(int value, CalculateState state) // µ∑ »Æ¿Œ
    {
        if (CheckMoney(value, state))
        {
            //µ∑ «•Ω√ UI
            if (state == CalculateState.Minus)
            {
                money -= value;
            }
            else
            {
                money += value;
                print("plus");
            }

            moneyUI.ChangeMoneyUI(money);
        }
        else
        {
            // µ∑¿Ã æ¯¥Ÿ¥¬ UI
            moneyUI.NoMoneyUI();
        }
    }

    public bool CheckMoney(int value, CalculateState state) // µ∑ »Æ¿Œ
    {
        if (((money - value < minMoney) && state == CalculateState.Minus) || ((money + value > maxMoney) && state == CalculateState.Plus))
        {
            return false;
        }
        return true;
    }

    public void CalculateMoney(int value, CalculateState state)
    {
        if (state == CalculateState.Plus)
        {
            money += value;
        }
        else
        {
            money -= value;
        }
        SetMoney(money, state);
    }

    public void SetCash(int value, CalculateState state) // µ∑ »Æ¿Œ
    {
        //cash UI
        moneyUI.ChangeCashUI(cash);
    }

    public void CalculateCash(int value, CalculateState state)
    {
        if (CheckCash(value, state))
        {
            if (state == CalculateState.Plus)
            {
                cash += value;
            }
            else
            {
                cash -= value;
            }
        }
        SetMoney(cash, state);
    }

    public bool CheckCash(int value, CalculateState state) // µ∑ »Æ¿Œ
    {
        if (((money - value < minMoney) && state == CalculateState.Minus))
        {
            return false;
        }
        return true;
    }

    public void GetReward(int value, Reward reward)
    {
        if (reward == Reward.Money)
        {
            SetMoney(value, CalculateState.Plus);
        }
        else
        {
            SetCash(value, CalculateState.Plus);
        }
    }
}
