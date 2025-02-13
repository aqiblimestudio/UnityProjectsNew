
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;

public class AdNewManager : MonoBehaviour
{
    public bool isTest;
    private BannerView _bannerView;
    private InterstitialAd _interstitialAd;
    public string[] AllBanner_ID;
    public string Banner_ID,Interstitial_ID,Reward_ID;
    public string Test_Banner_ID, Test_Interstitial_ID, Test_Reward_ID;
    public bool isAdmobInitialized = false;
    public bool isBannerLoaded=false;
    public bool isInterstitialLoaded = false;
    public bool isRewardLoaded = false;
    public bool RewardAlloat = false;
    private string testDeviceID = "2C5BEBBF0612C3B79284DA27BDC16B9E";
    public Text _info;
    void Start()
    {
        // Initialize Google Mobile Ads SDK
        MobileAds.Initialize((initStatus) =>
        {
            Banner_ID = AllBanner_ID[currentbanner];
            Debug.Log("Admob Initialized.");
           // LoadBannerAd();
        });
    }

    public void LoadBannerAd()
    {
        if (_bannerView == null)
        {
            Debug.Log("Creating banner view");

            // Create the banner at the top of the screen
            
        }

        _bannerView = new BannerView(Banner_ID, AdSize.Banner, AdPosition.Top);
        ShowData("Creating Banner ad ");
        // Set up event handlers for loading and failure
        _bannerView.OnBannerAdLoaded += OnBannerAdLoaded;
        _bannerView.OnBannerAdLoadFailed += BannerFailedLoad;

        // Create an ad request (use a simple one for now)
        AdRequest adRequest = new AdRequest();

        // Load the banner ad
        Debug.Log("Loading banner ad.");
        _bannerView.LoadAd(adRequest);
    }
    private void OnBannerAdLoaded()
    {
        ShowData("Banner ad loaded successfully ");
        Debug.Log("Banner ad loaded successfully.");
        if (_bannerView != null)
            _bannerView.Show();
    }
    private void BannerFailedLoad(LoadAdError loadAdError)
    {
        ShowData("Banner ad failed to load: " + loadAdError.GetMessage());
        // Log the failure reason in detail
        Debug.LogError("Banner ad failed to load: " + loadAdError.GetMessage());
        setBannerID();
        Invoke(nameof(LoadBannerAd), 5);
    }

    //private void OnBannerAdLoaded(object sender, EventArgs args)
    //{
    //    ShowData("Banner ad loaded successfully ");
    //    Debug.Log("Banner ad loaded successfully.");
    //    if (_bannerView != null)
    //        _bannerView.Show();
    //}

    //private void BannerFailedLoad(object sender, AdFailedToLoadEventArgs args)
    //{
    //    ShowData("Banner ad failed to load: " + args.LoadAdError);
    //    // Log the failure reason in detail
    //    Debug.LogError("Banner ad failed to load: " + args.LoadAdError);
    //    setBannerID();
    //    Invoke(nameof(LoadBannerAd),5);
    //}
    int currentbanner =0;
        void setBannerID()
        {
            if (AllBanner_ID.Length-1 > currentbanner) {
                currentbanner += 1;
            }
            else
            {
                currentbanner = 0;
            }
            Banner_ID = AllBanner_ID[currentbanner];
        }
   
    //private void OnEnable()
    //{
    //    if (isTest)
    //    {
    //        Banner_ID = Test_Banner_ID;
    //        Interstitial_ID = Test_Interstitial_ID;
    //        Reward_ID = Test_Reward_ID;
    //    }
    //    MobileAds.Initialize((initStatus) =>
    //    {
    //        Debug.Log("GG >> Admob:Initialized");



    //    });
    //}
    //// Start is called before the first frame update
    //void Start()
    //{

    //    // Initialize the Google Mobile Ads SDK.



    //}
    //void MediationAdapterConsent(string AdapterClassname)
    //{
    //    if (AdapterClassname.Contains("MobileAds"))
    //    {
    //        isAdmobInitialized = true;
    //        ShowData("initialized admob Ready");
    //    }

    //}
    //void loadAllAds()
    //{
    //    this.RequestBanner();
    //    this.RequestInterstitial();
    //    this.RequestRewarded();
    //} 
    void ShowData(string value)
    {
        _info.text = value;
    }
    ////Banner ads

    //private void RequestBanner()
    //{

    //    Debug.Log("Creating banner view");

    //    // If we already have a banner, destroy the old one.
    //    if (_bannerView != null)
    //    {
    //        DestroyAd();
    //    }

    //    // Create a 320x50 banner at top of the screen
    //    _bannerView = new BannerView(Banner_ID, AdSize.Banner, AdPosition.Top);

    //}

    ///// <summary>
    ///// Destroys the banner view.
    ///// </summary>
    //public void DestroyAd()
    //{
    //    if (_bannerView != null)
    //    {
    //        Debug.Log("Destroying banner view.");
    //        _bannerView.Destroy();
    //        _bannerView = null;
    //    }
    //}

    //public void LoadBannerAd()
    //{
    //    // create an instance of a banner view first.
    //    if (_bannerView == null)
    //    {
    //        Debug.Log("Creating banner view");

    //        // If we already have a banner, destroy the old one.
    //        if (_bannerView != null)
    //        {
    //            DestroyAd();
    //        }

    //        // Create a 320x50 banner at top of the screen
    //        _bannerView = new BannerView(Banner_ID, AdSize.Banner, AdPosition.Top);
    //    }
    //    _bannerView.OnBannerAdLoaded += BannerLoaded;
    //    _bannerView.OnBannerAdLoadFailed += BannerFailedLoad;
    //    // create our request used to load the ad.
    //    var adRequest = new AdRequest();

    //    //bool isCollapsed = adRequest.Extras.ContainsKey("IsCollapsed") && adRequest.Extras["IsCollapsed"] == "true";
    //    //adRequest.Extras["IsCollapsed"] = isCollapsed ? "false" : "true";
    //    // send the request to load the ad.
    //    Debug.Log("Loading banner ad.");
    //    _bannerView.LoadAd(adRequest);
    //}
    //public void ShowBanner()
    //{
    //    if(_bannerView != null)
    //    {
    //        if (isBannerLoaded)
    //            _bannerView.Show();
    //    }
    //}
    //void BannerLoaded()
    //{
    //    ShowData("Banner Ready");
    //    isBannerLoaded = true;
    //}
    //void BannerFailedLoad(LoadAdError loadAdError)
    //{
    //    isBannerLoaded = false;
    //    ShowData("Banner not Ready");
    //}
    ////InterstitialAds


    //private void RequestInterstitial()
    //{


    //    // Clean up the old ad before loading a new one.
    //    if (_interstitialAd != null)
    //    {
    //        _interstitialAd.Destroy();
    //        _interstitialAd = null;
    //    }

    //    Debug.Log("Loading the interstitial ad.");

    //    // create our request used to load the ad.
    //    var adRequest = new AdRequest();

    //    // send the request to load the ad.
    //    InterstitialAd.Load(Interstitial_ID, adRequest,
    //        (InterstitialAd ad, LoadAdError error) =>
    //        {
    //            // if error is not null, the load request failed.
    //            if (error != null || ad == null)
    //            {
    //                Debug.LogError("interstitial ad failed to load an ad " +
    //                               "with error : " + error);
    //                return;
    //            }

    //            Debug.Log("Interstitial ad loaded with response : "
    //                      + ad.GetResponseInfo());

    //            _interstitialAd = ad;
    //        });

    //}


    ///// <summary>
    ///// Loads the interstitial ad.
    ///// </summary>
    //public void LoadInterstitialAd()
    //{
    //    // Clean up the old ad before loading a new one.
    //    if (!_interstitialAd.CanShowAd())
    //    {
    //        if (_interstitialAd != null)
    //        {
    //            _interstitialAd.Destroy();
    //            _interstitialAd = null;
    //        }

    //        Debug.Log("Loading the interstitial ad.");

    //        // create our request used to load the ad.
    //        var adRequest = new AdRequest();

    //        // send the request to load the ad.
    //        InterstitialAd.Load(Interstitial_ID, adRequest,
    //            (InterstitialAd ad, LoadAdError error) =>
    //            {
    //            // if error is not null, the load request failed.
    //            if (error != null || ad == null)
    //                {
    //                    isInterstitialLoaded = false;
    //                    Debug.LogError("interstitial ad failed to load an ad " +
    //                                   "with error : " + error);
    //                    return;
    //                }

    //                Debug.Log("Interstitial ad loaded with response : "
    //                          + ad.GetResponseInfo());
    //                isInterstitialLoaded = true;
    //                _interstitialAd = ad;
    //            });
    //    }else
    //        isInterstitialLoaded = true;

    //}


    //public void ShowInterstitialAd()
    //{
    //    if (_interstitialAd != null && _interstitialAd.CanShowAd())
    //    {
    //        Debug.Log("Showing interstitial ad.");

    //        _interstitialAd.Show();
    //        isInterstitialLoaded = false;
    //    }
    //    else
    //    {
    //        Debug.LogError("Interstitial ad is not ready yet.");
    //    }
    //}


    ////Rewarded ads

    //private void RequestRewarded()
    //{

    //    // Clean up the old ad before loading a new one.
    //    if (_rewardedAd != null)
    //    {
    //        _rewardedAd.Destroy();
    //        _rewardedAd = null;
    //    }

    //    Debug.Log("Loading the rewarded ad.");

    //    // create our request used to load the ad.
    //    var adRequest = new AdRequest();

    //    // send the request to load the ad.
    //    RewardedAd.Load(Reward_ID, adRequest,
    //        (RewardedAd ad, LoadAdError error) =>
    //        {
    //            // if error is not null, the load request failed.
    //            if (error != null || ad == null)
    //            {
    //                Debug.LogError("Rewarded ad failed to load an ad " +
    //                               "with error : " + error);
    //                return;
    //            }

    //            Debug.Log("Rewarded ad loaded with response : "
    //                      + ad.GetResponseInfo());

    //            _rewardedAd = ad;
    //        });
    //}

    //RewardedAd _rewardedAd;

    //public void LoadRewardedAd()
    //{
    //    // Clean up the old ad before loading a new one.
    //    if (_rewardedAd != null)
    //    {
    //        _rewardedAd.Destroy();
    //        _rewardedAd = null;
    //    }

    //    Debug.Log("Loading the rewarded ad.");

    //    // create our request used to load the ad.
    //    var adRequest = new AdRequest();

    //    // send the request to load the ad.
    //    RewardedAd.Load(Reward_ID, adRequest,
    //        (RewardedAd ad, LoadAdError error) =>
    //        {
    //            // if error is not null, the load request failed.
    //            if (error != null || ad == null)
    //            {
    //                isRewardLoaded = false;
    //                Debug.LogError("Rewarded ad failed to load an ad " +
    //                               "with error : " + error);
    //                return;
    //            }

    //            Debug.Log("Rewarded ad loaded with response : "
    //                      + ad.GetResponseInfo());
    //            isRewardLoaded = true;
    //            _rewardedAd = ad;
    //        });
    //}

    //public void ShowRewardedAd()
    //{
    //    const string rewardMsg =
    //        "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

    //    if (_rewardedAd != null && _rewardedAd.CanShowAd())
    //    {
    //        _rewardedAd.Show((Reward reward) =>
    //        {
    //            RewardAlloat = true;
    //            isRewardLoaded = false;
    //            // TODO: Reward the user.
    //            Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
    //        });

    //    }
    //}

}