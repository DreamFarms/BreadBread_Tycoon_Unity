using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleADTest : MonoBehaviour
{
    private RewardedAd rewardedAd;

    public int gold;
    public GameObject messageBox;


    public void Start()
    {
        InitAds();
    }

    //���� �ʱ�ȭ �Լ�
    public void InitAds()
    {
        string adUnitId;

#if UNITY_ANDROID
        adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
            adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            adUnitId = "unexpected_platform";
#endif

        RewardedAd.Load(adUnitId, new AdRequest(), LoadCallback);
    }

    //�ε� �ݹ� �Լ�
    public void LoadCallback(RewardedAd rewardedAd, LoadAdError loadAdError)
    {
        if (rewardedAd != null)
        {
            this.rewardedAd = rewardedAd;
            Debug.Log("�ε强��");
        }
        else
        {
            Debug.Log(loadAdError.GetMessage());
        }

    }

    //���� �����ִ� �Լ�
    public void ShowAds()
    {
        if (rewardedAd.CanShowAd())
        {
            // rewardedAd.Show(GetReward);
        }
        else
        {
            Debug.Log("���� ��� ����");
        }
    }

    //���� �Լ�
    public void GetReward(Reward reward)
    {
        Debug.Log("���� ȹ��!");
        InitAds();
    }
}
