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

    //광고 초기화 함수
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

    //로드 콜백 함수
    public void LoadCallback(RewardedAd rewardedAd, LoadAdError loadAdError)
    {
        if (rewardedAd != null)
        {
            this.rewardedAd = rewardedAd;
            Debug.Log("로드성공");
        }
        else
        {
            Debug.Log(loadAdError.GetMessage());
        }

    }

    //광고 보여주는 함수
    public void ShowAds()
    {
        if (rewardedAd.CanShowAd())
        {
            // rewardedAd.Show(GetReward);
        }
        else
        {
            Debug.Log("광고 재생 실패");
        }
    }

    //보상 함수
    public void GetReward(Reward reward)
    {
        Debug.Log("보상 획득!");
        InitAds();
    }
}
