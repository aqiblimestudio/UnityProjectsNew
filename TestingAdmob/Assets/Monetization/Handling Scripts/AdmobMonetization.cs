using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using MicroLabGamesMonetization;
using System;
using UnityEngine;

class AdmobMonetization : AdsManager
{
    #region Top Banner Variables

    string topBannerAdId = "";
    bool is_LoadtopBanner;
    bool is_AlreadyShowtopBanner;                                    //Admob Rewarded Ad Object
    private BannerView topbannerAd;
    public int topBannerPositionIndex;
    private AdPosition topBannerAdPositions = AdPosition.Top;
    #endregion

    #region Bottom Banner Variables

    string bottomBannerAdId = "";
    bool is_LoadbottomBanner;
    bool is_AlreadyShowbottomBanner;                                    //Admob Rewarded Ad Object
    private BannerView bottombannerAd;
    public int bottomBannerPositionIndex;
    private AdPosition bottomBannerAdPositions = AdPosition.BottomRight;

    #endregion
    #region Big Banner Variables

    string BigBannerAdId = "";
    bool is_LoadBigBanner;
    bool is_AlreadyShowBigBanner;                                    //Admob Rewarded Ad Object
    private BannerView BigbannerAd;
    public int BigBannerPositionIndex;
    private AdPosition BigBannerAdPositions = AdPosition.BottomLeft;

    #endregion

    #region AppOpen AD Variables

    string AppOpenAdId = "";
    bool is_Loadappopen;
    bool is_AlreadyShowappopen;                                    //Admob Rewarded Ad Object
    private AppOpenAd appopenAd;
    public int appopenPositionIndex;

    #endregion


    public AdmobMonetization(string BanId)
    {
        this.topBannerAdId = BanId;
        this.topBannerAdPositions = AdPosition.Top;
    }
    public AdmobMonetization(string BanId, AdPosition adPosition)
    {
        this.bottomBannerAdId = BanId;
        this.bottomBannerAdPositions = adPosition;
    }
    public AdmobMonetization(string BanId, AdPosition adPosition, int big)
    {
        this.BigBannerAdId = BanId;
        this.BigBannerAdPositions = adPosition;
    }
    public AdmobMonetization(string appopenId,int appopen)
    {
        this.AppOpenAdId = appopenId;
    }

    #region Top Banner

    public override void AssignTopBannerPoition(AdPosition adPosition)
    {
        this.topBannerAdPositions = adPosition;
    }
    public override void LoadTopBanner()
    {

        if (this.topbannerAd ==null )
        {
            string adID = this.topBannerAdId;


            AdSize adSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
            
            this.topbannerAd = new BannerView(adID, adSize, this.topBannerAdPositions);

            this.topbannerAd.OnBannerAdLoaded += this.HandleOntopBannerAdLoaded;
            this.topbannerAd.OnBannerAdLoadFailed += this.HandleOntopBannerAdFailedToLoad;
            

            AdRequest request = new AdRequest();
            this.topbannerAd.LoadAd(request);
            this.topbannerAd.Hide();

        }
        else
        {
            if(this.topbannerAd != null)
            {
                this.topbannerAd.Destroy();

                string adID = this.topBannerAdId;


                AdSize adSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

                this.topbannerAd = new BannerView(adID, adSize, this.topBannerAdPositions);

                this.topbannerAd.OnBannerAdLoaded += this.HandleOntopBannerAdLoaded;
                this.topbannerAd.OnBannerAdLoadFailed += this.HandleOntopBannerAdFailedToLoad;


                AdRequest request = new AdRequest();
                this.topbannerAd.LoadAd(request);
                this.topbannerAd.Hide();
            }
        }
    }
    public override void ShowTopBanner()
    {
        if (this.topbannerAd != null && this.is_AlreadyShowtopBanner==false)
        {
            this.is_AlreadyShowtopBanner = true;
            this.topbannerAd.Show();
        }
    }
    public override void HideTopBanner()
    {
        if (this.topbannerAd != null && this.is_AlreadyShowtopBanner)
        {
            this.is_AlreadyShowtopBanner = false;
            this.topbannerAd.Hide();
        }
    }
    public override void ChangePositionBanner()
    {
        if (this.topbannerAd != null)
        {
            //AssignTopBannerPoition();
            this.topbannerAd.SetPosition(this.topBannerAdPositions);
            this.topbannerAd.Show();
        }
    }
    public void HandleOntopBannerAdLoaded()
    {
        this.is_LoadtopBanner = true;
    }

    public void HandleOntopBannerAdFailedToLoad(LoadAdError loadAdError)
    {
        this.is_LoadtopBanner = false;
    }

    
    public override bool IsLoadTopBanner()
    {
        if (this.is_LoadtopBanner)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public override bool IsAlreadyLoadTopBanner()
    {
        if (this.is_AlreadyShowtopBanner)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion
    #region Bottom Banner

    public override void AssignBottomBannerPoition(AdPosition adPosition)
    {
        this.bottomBannerAdPositions = adPosition;
    }
    public override void LoadBottomBanner()
    {

        if (this.bottombannerAd == null)
        {
            string adID = this.bottomBannerAdId;

            //AdSize adSize = new AdSize(320, 50);
            this.bottombannerAd = new BannerView(adID, new AdSize(320, 50), this.bottomBannerAdPositions); //AdSize.SmartBanner

            this.bottombannerAd.OnBannerAdLoaded += this.HandleOnBottomBannerAdLoaded;
            this.bottombannerAd.OnBannerAdLoadFailed += this.HandleOnBottomBannerAdFailedToLoad;


            AdRequest request = new AdRequest();
            this.bottombannerAd.LoadAd(request);
            this.bottombannerAd.Hide();

        }
        else
        {
            if (this.bottombannerAd != null)
            {
                this.bottombannerAd.Destroy();
                string adID = this.bottomBannerAdId;

                //AdSize adSize = new AdSize(320,50);
                this.bottombannerAd = new BannerView(adID, new AdSize(320, 50), this.bottomBannerAdPositions); //AdSize.SmartBanner

                this.bottombannerAd.OnBannerAdLoaded += this.HandleOnBottomBannerAdLoaded;
                this.bottombannerAd.OnBannerAdLoadFailed += this.HandleOnBottomBannerAdFailedToLoad;


                AdRequest request = new AdRequest();
                this.bottombannerAd.LoadAd(request);
                this.bottombannerAd.Hide();

            }
        }
    }
    public override void ShowBottomBanner()
    {
        if (this.bottombannerAd != null && this.is_AlreadyShowbottomBanner == false)
        {
            this.is_AlreadyShowbottomBanner = true;
            this.bottombannerAd.Show();
        }
    }
    public override void HideBottomBanner()
    {
        if (this.bottombannerAd != null && this.is_AlreadyShowbottomBanner)
        {
            this.is_AlreadyShowbottomBanner = false;
            this.bottombannerAd.Hide();
        }
    }
    
    public void HandleOnBottomBannerAdLoaded()
    {
        this.is_LoadbottomBanner = true;
    }

    public void HandleOnBottomBannerAdFailedToLoad(LoadAdError loadAdError)
    {
        this.is_LoadbottomBanner = false;
    }


    public override bool IsLoadBottomBanner()
    {
        if (this.is_LoadbottomBanner)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public override bool IsAlreadyLoadBottomBanner()
    {
        if (this.is_AlreadyShowbottomBanner)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion
    #region Big Banner

    public override void AssignBigBannerPoition(AdPosition adPosition)
    {
        this.BigBannerAdPositions = adPosition;
    }
    public override void LoadBigBanner()
    {

        if (this.BigbannerAd == null)
        {
            string adID = this.BigBannerAdId;
            AdSize adSize = new AdSize(300, 250);
            this.BigbannerAd = new BannerView(adID, adSize, this.BigBannerAdPositions);

            this.BigbannerAd.OnBannerAdLoaded += this.HandleOnBigBannerAdLoaded;
            this.BigbannerAd.OnBannerAdLoadFailed += this.HandleOnBigBannerAdFailedToLoad;


            AdRequest request = new AdRequest();
            this.BigbannerAd.LoadAd(request);
            this.BigbannerAd.Hide();

        }
        else
        {
            if (this.BigbannerAd != null)
            {
                this.BigbannerAd.Destroy();
                string adID = this.BigBannerAdId;
                AdSize adSize = new AdSize(300, 250);
                this.BigbannerAd = new BannerView(adID, adSize, this.BigBannerAdPositions);

                this.BigbannerAd.OnBannerAdLoaded += this.HandleOnBigBannerAdLoaded;
                this.BigbannerAd.OnBannerAdLoadFailed += this.HandleOnBigBannerAdFailedToLoad;


                AdRequest request = new AdRequest();
                this.BigbannerAd.LoadAd(request);
                this.BigbannerAd.Hide();

            }
        }
    }
    public override void ShowBigBanner()
    {
        if (this.BigbannerAd != null && this.is_AlreadyShowBigBanner == false)
        {
            this.is_AlreadyShowBigBanner = true;
            this.BigbannerAd.Show();
        }
    }
    public override void HideBigBanner()
    {
        if (this.BigbannerAd != null && this.is_AlreadyShowBigBanner)
        {
            this.is_AlreadyShowBigBanner = false;
            this.BigbannerAd.Hide();
        }
    }

    public void HandleOnBigBannerAdLoaded()
    {
        this.is_LoadBigBanner = true;
    }

    public void HandleOnBigBannerAdFailedToLoad(LoadAdError loadAdError)
    {
        this.is_LoadBigBanner = false;
    }


    public override bool IsLoadBigBanner()
    {
        if (this.is_LoadBigBanner)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public override bool IsAlreadyLoadBigBanner()
    {
        if (this.is_AlreadyShowBigBanner)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion
    

    #region AppOpen AD
    public override void LoadAppOpenAd()
    {
        RequestAppOpen();
    }

    public override bool IsAppOpenLoaded()
    {
        if (this.appopenAd.CanShowAd())
        {
            return true;
        }
        else
        {
            LoadAppOpenAd();
            return false;
        }
    }

    public override void ShowAppOpen()
    {
        if (this.appopenAd != null )
        {
            this.appopenAd.OnAdFullScreenContentClosed += HandleOnIntAdClosed;
            this.appopenAd.Show();
            
        }
        else
        {
            Debug.LogWarning("App Open Ad not ready.");
        }
    }

    public override void RequestAppOpen()
    {
        string adID = this.AppOpenAdId;
        AdRequest request = new AdRequest();
        AppOpenAd.Load(adID, request, (AppOpenAd loadedAd, LoadAdError loadAdError) =>
        {
            if (loadAdError != null)
            {
                // Handle the load error
                Debug.LogError("Failed to load App Open Ad: " + loadAdError.GetMessage());
                return;
            }

            // If ad is loaded successfully, store the ad reference
            this.appopenAd = loadedAd;
            Debug.Log("App Open Ad loaded successfully.");
            // Optionally, you can show the ad immediately

        });
        // Initialize an AppOpenAd.

    }

    

    public void HandleOnIntAdClosed()
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            LoadAppOpenAd();
        });
    }
    #endregion
}
