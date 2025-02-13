using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxMediationManager : MonoBehaviour
{
    private static DateTime Time1ForAds;
    private static DateTime TimeForInterAds;
    public bool IsInterstitialAdReady = false;
    public bool isRewarded = false;
    public Action RewardHandle;
    public Action NotRewardHandle;
    public String SdkKey;
    public String InterID;
    public String RewardedID;
    public String AppOpenID;
    public static String RemoveAdsKey = "RemoveAds";
    public string[] TestDevices;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //#region Max

    //public void OnInitializeMax()
    //{
    //    MaxSdk.SetHasUserConsent(true);
    //    FirbaseText.text = "Max initializing";
    //    MaxSdk.SetSdkKey(SdkKey);
    //    MaxSdk.SetTestDeviceAdvertisingIdentifiers(TestDevices);
    //    MaxSdk.InitializeSdk();

    //    Debug.Log("InitCalled");
    //    //Track.text = "OnInitialize Max call " + Time.timeScale;
    //    MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
    //    {

    //        InitSucceded = true;
    //        //Debug.Log("InitSuccess");
    //        FirbaseText.text = "Max initialized";
    //        LoadAllAds();
    //        //MaxSdk.LoadAppOpenAd(AppOpenID);
    //        //Invoke(nameof(OnLoadAdaptiveBanner), 1);
    //        //Invoke(nameof(OnLoadSmallBanner), 2);
    //        // Track.text = "Ads  Max call " + Time.timeScale;
    //        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += MaxOnInterstitialHiddenEvent;
    //        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += MaxOnInterstitialLoadedHiddenEvent;
    //        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;



    //        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
    //        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnAdDisplayed;
    //        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
    //        MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
    //        // AppLovin SDK is initialized, start loading ads

    //    };


    //}
    //private void OnAdDisplayed(string arg1, MaxSdkBase.AdInfo arg2)
    //{
    //    Debug.Log("Rewarded ad displayed.");
    //    isRewarded = false;  // Reset flag when the ad is displayed
    //}

    //#region Max Events Call
    //private void OnRewardedAdHiddenEvent(string arg1, MaxSdkBase.AdInfo arg2)
    //{
    //    if (!isRewarded)
    //    {
    //        if (NotRewardHandle != null)
    //            NotRewardHandle.Invoke();
    //    }

    //    ShowRewardPanel(false);
    //    MaxSdk.LoadRewardedAd(RewardedID);
    //}
    //private void MaxOnInterstitialLoadedHiddenEvent(string arg1, MaxSdkBase.AdInfo arg2)
    //{
    //    IsInterstitialAdReady = true;
    //}
    //private void MaxOnInterstitialHiddenEvent(string arg1, MaxSdkBase.AdInfo arg2)
    //{
    //    ShowRewardPanel(false);
    //    IsInterstitialAdReady = false;
    //    MaxSdk.LoadInterstitial(InterID);
    //}

    //private void OnAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo impressionData)
    //{
    //    //double revenue = impressionData.Revenue;
    //    //var impressionParameters = new[]
    //    //{
    //    //        new Firebase.Analytics.Parameter("ad_platform", "AppLovin"),
    //    //        new Firebase.Analytics.Parameter("ad_source", impressionData.NetworkName),
    //    //        new Firebase.Analytics.Parameter("ad_unit_name", impressionData.AdUnitIdentifier),
    //    //        new Firebase.Analytics.Parameter("ad_format", impressionData.AdFormat),
    //    //        new Firebase.Analytics.Parameter("value", revenue),
    //    //        new Firebase.Analytics.Parameter("currency", "USD"), // All Applovin revenue is sent in USD
    //    //};

    //    //if (FbAnalytics.Instance.firebaseInitialized)
    //    //{
    //    //    Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
    //    //}
    //}
    //private void OnRewardedAdReceivedRewardEvent(string arg1, MaxSdkBase.Reward arg2, MaxSdkBase.AdInfo arg3)
    //{

    //    isRewarded = true;
    //}
    //#endregion
    //#region Max Interstitial Call
    //public void LoadInterstitial()
    //{
    //    MaxSdk.LoadInterstitial(InterID);
    //}
    //public void InterstitialLoad_Show()
    //{
    //    StartCoroutine(ShowInterstetialDelay());
    //}
    //WaitForSecondsRealtime secondsRealtime = new WaitForSecondsRealtime(.5f);
    //IEnumerator ShowInterstetialDelay()
    //{
    //    LoadInterstitial();
    //    yield return secondsRealtime;
    //    ShowInterstitial();
    //}
    //public void ShowInterstitial()
    //{
    //    if (!InitSucceded || PlayerPrefs.GetInt(RemoveAdsKey, 0) == 1) return;


    //    if (MaxSdk.IsInterstitialReady(InterID))
    //    {
    //        MaxSdk.ShowInterstitial(InterID);
    //        setTimeForInterAds();
    //    }
    //    else
    //    {
    //        MaxSdk.LoadInterstitial(InterID);
    //    }
    //}
    //#endregion
    //#region Max Reward call
    //public void LoadRewardedVideo()
    //{
    //    if (!MaxSdk.IsRewardedAdReady(RewardedID))
    //        MaxSdk.LoadRewardedAd(RewardedID);
    //}
    //public bool CanShowReward()
    //{
    //    return MaxSdk.IsRewardedAdReady(RewardedID);
    //}

    //public void ShowRewardVideo()
    //{
    //    ShowRewardedVideo(RewardHandle, NotRewardHandle);
    //}

    //public void ShowRewardedVideo(Action _Reward, Action _NotReward)
    //{

    //    if (!InitSucceded) return;

    //    MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
    //    if (MaxSdk.IsRewardedAdReady(RewardedID))
    //    {
    //        RewardHandle = _Reward;
    //        NotRewardHandle = _NotReward;
    //        MaxSdk.ShowRewardedAd(RewardedID);
    //    }
    //    else
    //    {
    //        MaxSdk.LoadRewardedAd(RewardedID);
    //    }
    //}
    //public void ShowRewardedVideo(Action _Reward)
    //{

    //    if (!InitSucceded) return;

    //    MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
    //    if (MaxSdk.IsRewardedAdReady(RewardedID))
    //    {
    //        RewardHandle = _Reward;
    //        MaxSdk.ShowRewardedAd(RewardedID);
    //    }
    //    else
    //    {
    //        MaxSdk.LoadRewardedAd(RewardedID);
    //    }
    //}
    //public bool IsRewardedVideo_Available()
    //{
    //    if (!InitSucceded) return false;

    //    return MaxSdk.IsRewardedAdReady(RewardedID);
    //}
    //#endregion

    //#region Max AppOpen call
    //public void LoadAppOpen_Ad()
    //{
    //    if (!MaxSdk.IsAppOpenAdReady(AppOpenID))
    //        MaxSdk.LoadAppOpenAd(AppOpenID);

    //}
    //public void LoadShowAppOpenAd()
    //{
    //    LoadAppOpen_Ad();
    //    ShowAppOpen();
    //}
    //public void ShowAppOpen()
    //{
    //    if (!InitSucceded || PlayerPrefs.GetInt(RemoveAdsKey, 0) == 1) return;


    //    if (MaxSdk.IsAppOpenAdReady(AppOpenID))
    //    {
    //        MaxSdk.ShowAppOpenAd(AppOpenID);
    //    }
    //    MaxSdk.LoadAppOpenAd(AppOpenID);
    //}

    //#endregion


    //public void Update()
    //{
    //    if (isRewarded)
    //    {
    //        isRewarded = false;
    //        if (RewardHandle != null)
    //        {
    //            RewardHandle.Invoke();
    //            NotRewardHandle = null;
    //        }
    //    }
    //}


    //#endregion
}
