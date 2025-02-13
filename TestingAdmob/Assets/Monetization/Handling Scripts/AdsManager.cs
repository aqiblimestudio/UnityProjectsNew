using GoogleMobileAds.Api;

abstract class AdsManager
{
    //#region Interstitial
    //public abstract void LoadInterstitial();
    //public abstract bool IsInterstitialLoaded();
    //public abstract void ShowInterstitial();
    //public abstract void RequestInterstitial();
    //#endregion

    //#region Rewarded
    //public abstract void LoadRewarded();
    //public abstract bool IsRewardedLoaded();
    //public abstract void ShowRewarded();
    //public abstract void RequestRewarded();
    //#endregion

    #region Top Banner
    public abstract void AssignTopBannerPoition(AdPosition adPosition);
    public abstract void LoadTopBanner();
    public abstract bool IsLoadTopBanner();
    public abstract bool IsAlreadyLoadTopBanner();
    public abstract void ShowTopBanner();
    public abstract void HideTopBanner(); 
    public abstract void ChangePositionBanner();
    #endregion
    #region Bottom Banner
    public abstract void AssignBottomBannerPoition(AdPosition adPosition);
    public abstract void LoadBottomBanner();
    public abstract bool IsLoadBottomBanner();
    public abstract bool IsAlreadyLoadBottomBanner();
    public abstract void ShowBottomBanner();
    public abstract void HideBottomBanner();
    #endregion

    #region Big Bottom Banner
    public abstract void AssignBigBannerPoition(AdPosition adPosition);
    public abstract void LoadBigBanner();
    public abstract bool IsLoadBigBanner();
    public abstract bool IsAlreadyLoadBigBanner();
    public abstract void ShowBigBanner();
    public abstract void HideBigBanner();
    #endregion
    #region AppOpen
    
    public abstract void LoadAppOpenAd();
    public abstract bool IsAppOpenLoaded();
    public abstract void ShowAppOpen();
    public abstract void RequestAppOpen();
    #endregion
}
