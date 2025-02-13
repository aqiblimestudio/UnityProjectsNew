////using GameAnalyticsSDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
using GoogleMobileAds.Common;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class AdmobAdsManager : MonoBehaviour
{
    public static AdmobAdsManager Instance;

    public UnityEvent OnCallAdmobInitilized;
    public bool Internet;
    public bool Check_Firebase;

    public String SdkKey;
    public String InterID;
    public String RewardedID;
    public static String RemoveAdsKey = "RemoveAds";
    public static event Action onInitializeEvent;
    public string[] TestDevices;
    public Text _log,NewLogsForconsent,Track;
    public Action RewardHandle;
    public Action NotRewardHandle;
    public bool isRewarded = false;
    [HideInInspector]
    public bool InitSucceded;
    public bool AdShown = false;
    private static DateTime Time1ForAds;
    private static DateTime TimeForInterAds;
    public bool IsInterstitialAdReady = false;
    public bool HardLevelCheck;

    // bool  IsAppOpenAdAvailable=false;
    bool  trytoInitializeOnceMore=false;
    public bool isAdmobInitialized, ConsentPanelAlreadyShow;
    public GameObject Canves, RewardLoadingPanel, AppOpenLoading, NoInternetPanelLandscap;
    public bool isEditorCheck;
    public Text AdmobText,FirbaseText,TopBannerText, BottomBannerText, RectBannerText, AppopenText, InterText, RewardText;
    //public HideGameObject hideGame;

    void Awake()
    {
        if (Application.isEditor)
        {
            Debug.unityLogger.logEnabled = true;
        }
        else
        {
            Debug.unityLogger.logEnabled = false;
        }

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        StartCoroutine(Btn_Internet());

        if(PlayerPrefs.GetInt("GDPRConsentAd")!=0)
        {
            OnClose_consentForm();
            
        }
        else
        {
            GetGDPR_Consent();
            
        }
        
        isMediumBannerLoaded = false;

        isMRecShowing = false;

    }


    // Start is called before the first frame update
    void Start()
    {
        
        MRec_ShowEvent = null;

        MRec_ShowEvent += ShowAdmobMediumBanner;

        SM_ShowEvent_Top = null;
        SM_ShowEvent_Top += ShowAdmobTopBanner;

        SM_ShowEvent_Bottom = null;
        SM_ShowEvent_Bottom += ShowAdmobBottomBanner;

        isBannerLoaded_Top = false;
        isTopBannerShowing = false;

        isBannerLoaded_Bottom = false;
        isBottomBannerShowing = false;

        if (Test_Ads)
        {
            ADMOB_ID = TestAdmob_ID;
        }
        else
        {
#if UNITY_ANDROID
            ADMOB_ID = AndroidAdmob_ID;

#endif
        }
        setTimeForInterAds();
    }

    
    




    #region Admob 
    public bool Test_Ads;
    public AdmobId AndroidAdmob_ID = new AdmobId();
    public AdmobId TestAdmob_ID = new AdmobId();
    [HideInInspector]
    public AdmobId ADMOB_ID = new AdmobId();
    private void AdmobInit()
    {
        // Track.text = "Start AdmobInit call " + Time.timeScale;
        AdmobText.text = "Start_Initializing";
        Debug.Log("GG >> Admob:Start_Initializing");
        MobileAds.Initialize((initStatus) =>
        {
            Debug.Log("GG >> Admob:Initialized");



            Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
            foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
            {
                string className = keyValuePair.Key;
                AdapterStatus status = keyValuePair.Value;
                switch (status.InitializationState)
                {
                    case AdapterState.NotReady:
                        // The adapter initialization did not complete.

                        Debug.Log("GG >> Adapter: " + status.Description + " not ready.Name=" + className);

                        //      Logging.Log("Adapert is :: "+(AdmobGAEvents.AdaptersNotInitialized) + className);

                        break;
                    case AdapterState.Ready:
                        // The adapter was successfully initialized.
                        Debug.Log("GG >> Adapter: " + className + " is initialized.");
                       // Track.text = " AdmobInit initialized " + Time.timeScale;
#if UNITY_ANDROID

                        MediationAdapterConsent(className);
#endif


                        break;
                }
            }
            if (isAdmobInitialized)
            {
                //OnInitializeMax();

                LoadAllAds();

            }
            else
            {
                isAdmobInitialized = true;
                //OnInitializeMax();

                LoadAllAds();

            }


            isAdmobInitialized = true;
            //LoadAppOpenAd();


        });
    }
   
    void LoadAllAds()
    {
        Invoke(nameof(LoadAdmob_TopBanner), .5f);
        Invoke(nameof(LoadAdmob_BottomBanner), 1f);
        Invoke(nameof(LoadAdmobMediumBanner), 2f);
        Invoke(nameof(LoadAdMobAppOpenAd), 3f);

    }
    public void BothAdmobBannerCall()
    {
        LoadAdmob_TopBanner();
        //LoadAdmob_BottomBanner();
    }
    void MediationAdapterConsent(string AdapterClassname)
    {
        if (AdapterClassname.Contains("MobileAds"))
        {
            isAdmobInitialized = true;
            Debug.Log("GG >> Admob consent is send" + isAdmobInitialized);
            AdmobText.text = "Admob Initialized";
            // load ad
        }

    }
    void OnClose_consentForm()
    {
       // Track.text = "OnClose_consentForm call " + Time.timeScale;

        Time.timeScale = 1;
        //if (loadingScreen)
        //    loadingScreen.ShowLoading();
        if (!ConsentPanelAlreadyShow)
        {
            ConsentPanelAlreadyShow = true;
            AdmobInit();
        }
    }
    
    #region Get GDRP Consent


    ConsentForm _consentForm;

    void GetGDPR_Consent()
    {
        var debugSettings = new ConsentDebugSettings
        {
            // Geography appears as in EEA for debug devices.
            DebugGeography = DebugGeography.EEA,
            TestDeviceHashedIds = new List<string>
            {
                "7748b46d-458d-4c8b-bfb1-10ca1ad1abe1"
            }
        };

        // Here false means users are not under age.
        ConsentRequestParameters request = new ConsentRequestParameters
        {
            TagForUnderAgeOfConsent = false,
            ConsentDebugSettings = debugSettings,
        };

        // Check the current consent information status.
        Debug.Log("Call ConsentInfoUpdate");
        ConsentInformation.Update(request, OnConsentInfoUpdated);
    }
    int consentcount;
    public void OnConsentInfoUpdated(FormError error)
    {
        consentcount += 1;
        //NewLogsForconsent.text = "consentcount " + consentcount;
        if (error != null || ConsentPanelAlreadyShow)
        {
            OnClose_consentForm();

            // Handle the error.
            Debug.Log("Error" + error);
            //UnityEngine.Debug.LogError(error);
            return;
        }
        Debug.Log("OnConsentInfoUpdated run");
        if (ConsentInformation.IsConsentFormAvailable())
        {
            LoadConsentForm();
        }
        else
        {
            OnClose_consentForm();
        }

        // If the error is null, the consent information state was updated.
        // You are now ready to check if a form is available.
    }

    public void LoadConsentForm()
    {
        // Loads a consent form.
        Debug.Log("OnLoading call ConsentForm");
        ConsentForm.Load(OnLoadConsentForm);
    }
    public void OnLoadConsentForm(ConsentForm consentForm, FormError error)
    {
        if (error != null || ConsentPanelAlreadyShow)
        {
            OnClose_consentForm();
            // Handle the error.
            Debug.Log("Error On OnLoadConsentForm");
            //UnityEngine.Debug.LogError(error);
            return;
        }
        Debug.Log("OnLoadConsentForm run");
        // The consent form was loaded.
        // Save the consent form for future requests.
        _consentForm = consentForm;
        if(ConsentInformation.ConsentStatus == ConsentStatus.Required)
        {
            _consentForm.Show(OnShowForm);
        }
        
        if (ConsentInformation.ConsentStatus == ConsentStatus.Obtained)
        {
            PlayerPrefs.SetInt("GDPRConsentAd", 1);
            Debug.Log(" Consent Obtained ");
            OnClose_consentForm();
        }
        if (ConsentInformation.ConsentStatus == ConsentStatus.NotRequired)
        {
            PlayerPrefs.SetInt("GDPRConsentAd", 1);
            OnClose_consentForm();
        }
        if (ConsentInformation.ConsentStatus == ConsentStatus.Unknown)
        {
            OnClose_consentForm();
        }
        
    }


    public void OnShowForm(FormError error)
    {
        if (error != null)
        {
            OnClose_consentForm();
            // Handle the error.
            Debug.Log("Error On OnShowForm");
            UnityEngine.Debug.LogError(error);

            return;
        }
        Debug.Log("OnShowForm run");

        // Handle dismissal by reloading form.
        LoadConsentForm();
    }

    #endregion

    #region Banner Doth
    public void TopBannerIncrease()
    {
        if (indexTopBanner < 2)
            indexTopBanner = indexTopBanner + 1;
    }
    public void BottomBannerIncrease()
    {
        if(indexBottomBanner<2)
            indexBottomBanner = indexBottomBanner + 1;
    }
    public void RectBannerIncrease()
    {
        if (indexRectBanner < 2)
            indexRectBanner = indexRectBanner + 1;
    }
    #region Banner Variables


    BannerView BannerTop;
    BannerView BannerBottom;

    public delegate void Top_Showdelegate();
    public static event Top_Showdelegate SM_ShowEvent_Top;
    public delegate void Bottom_Showdelegate();
    public static event Bottom_Showdelegate SM_ShowEvent_Bottom;

    public static bool isBannerLoaded_Top = false;
    public static bool isTopBannerShowing = false;

    public  bool isBannerLoaded_Bottom = false; 
    public  bool isBottomBannerShowing = false;


    #endregion

    #region Bind  BannerEvents


    private void Bind_TopBannerEvents()
    {
        // Add similar event handlers for other events

        BannerTop.OnBannerAdLoaded += () => TopBanner_HandleOnAdLoaded();
        BannerTop.OnBannerAdLoadFailed += (LoadAdError error) => TopBanner_HandleOnAdLoadedFailed(error);
    }
    private void Bind_BottomBannerEvents()
    {
        // Add similar event handlers for other events

        BannerBottom.OnBannerAdLoaded += () => BottomBanner_HandleOnAdLoaded();
        BannerBottom.OnBannerAdLoadFailed += (LoadAdError error) => BottomBanner_HandleOnAdLoad_Failed(error);

    }
    
    private void TopBanner_HandleOnAdLoaded()
    {
        if (isBannerLoaded_Top)
        {
            Debug.Log("TopBanner:Refreshed Success");

        }
        else
        {
            isBannerLoaded_Top = true;
            TopBannerText.text = " Try: " + topBannerCounter + " Loaded Banner Id number " + (indexTopBanner);
            if (SM_ShowEvent_Top != null)
                SM_ShowEvent_Top.Invoke();
            Debug.Log("GG >> Admob:TopBanner:Loaded");

        }
        //MobileAdsEventExecutor.ExecuteInUpdate(() =>
        //{
        //    // Handle the loaded ad based on ecpmStatus
        //    // ...

        //    if (isBannerLoaded_Top)
        //    {
        //        Debug.Log("TopBanner:Refreshed Success");

        //    }
        //    else
        //    {
        //        isBannerLoaded_Top = true;
        //        TopBannerText.text = " Try: " + topBannerCounter + " Loaded Banner Id number " + (indexTopBanner);
        //        if (SM_ShowEvent_Top != null)
        //            SM_ShowEvent_Top.Invoke();
        //        Debug.Log("GG >> Admob:TopBanner:Loaded");

        //    }


        //});
    }

    private void TopBanner_HandleOnAdLoadedFailed(LoadAdError error)
    {
        if (isBannerLoaded_Top)
        {
            Debug.Log("TopBanner:Refresh_Failed");
        }
        else
        {
            Debug.Log("GG >> Admob:TopBanner:Failed to Load");
            ///////////////////////////////////////////
            
            TopBannerText.text = " Try: " + topBannerCounter + " Failed Banner Id number " + (indexTopBanner);
            indexTopBanner = (indexTopBanner + 1) % ADMOB_ID.ADMOB_TopBANNER_AD_ID.Length;
            Invoke(nameof(LoadAdmob_TopBanner), 5);
            //LoadAdmob_TopBanner();
        }
        //MobileAdsEventExecutor.ExecuteInUpdate(() =>
        //{
        //    // Handle the failed to load ad based on ecpmStatus
        //    // ...
        //    //  Debug.LogError("Small Banner view failed to load with error : " + error);

        //    if (isBannerLoaded_Top)
        //    {
        //        Debug.Log("TopBanner:Refresh_Failed");
        //    }
        //    else
        //    {
        //        Debug.Log("GG >> Admob:TopBanner:Failed to Load");
        //        ///////////////////////////////////////////
        //        indexTopBanner = indexTopBanner + 1;
        //        TopBannerText.text = " Try: " + topBannerCounter + " Failed Banner Id number " + (indexTopBanner);
        //        Invoke(nameof(LoadAdmob_TopBanner), 2);
        //        //LoadAdmob_TopBanner();
        //    }
        //});
    }
    private void BottomBanner_HandleOnAdLoaded()
    {
        if (isBannerLoaded_Bottom)
        {
            Debug.Log("BottomBanner:Refreshed Success");

        }
        else
        {
            isBannerLoaded_Bottom = true;
            BottomBannerText.text = " Try: " + bottomBannerCounter + " load Banner Id number " + (indexBottomBanner);
            Debug.Log("GG >> Admob:BottomBanner:Loaded");

        }
        //MobileAdsEventExecutor.ExecuteInUpdate(() =>
        //{
        //    // Handle the loaded ad based on ecpmStatus
        //    // ...
            
        //    if (isBannerLoaded_Bottom)
        //    {
        //        Debug.Log("BottomBanner:Refreshed Success");

        //    }
        //    else
        //    {
        //        isBannerLoaded_Bottom = true;
        //        BottomBannerText.text = " Try: " + bottomBannerCounter + " load Banner Id number " + (indexBottomBanner);
        //        Debug.Log("GG >> Admob:BottomBanner:Loaded");

        //    }
            
        //});
    }

    private void BottomBanner_HandleOnAdLoad_Failed(LoadAdError error)
    {
        if (isBannerLoaded_Bottom)
        {
            Debug.Log("BottomBanner:Refresh_Failed");
        }
        else
        {
            Debug.Log("GG >> Admob:BottomBanner:Failed to Load");
            ///////////////////////////////////////////
            
            BottomBannerText.text = " Try: " + bottomBannerCounter + " Failed Banner Id number " + (indexBottomBanner);
            indexBottomBanner = (indexBottomBanner + 1) % ADMOB_ID.ADMOB_BottomBANNER_AD_ID.Length;
            Invoke(nameof(LoadAdmob_BottomBanner), 5);
            //LoadAdmob_BottomBanner();
        }
        //MobileAdsEventExecutor.ExecuteInUpdate(() =>
        //{
        //    // Handle the failed to load ad based on ecpmStatus
        //    // ...
        //    //  Debug.LogError("Small Banner view failed to load with error : " + error);
            
        //    if (isBannerLoaded_Bottom)
        //    {
        //        Debug.Log("BottomBanner:Refresh_Failed");
        //    }
        //    else
        //    {
        //        Debug.Log("GG >> Admob:BottomBanner:Failed to Load");
        //        ///////////////////////////////////////////
        //        indexBottomBanner = indexBottomBanner + 1;
        //        BottomBannerText.text = " Try: " + bottomBannerCounter + " Failed Banner Id number " + (indexBottomBanner);
        //        Invoke(nameof(LoadAdmob_BottomBanner), 2);
        //        //LoadAdmob_BottomBanner();
        //    }
        //});
    }

    #endregion


    #region Top Banner 
    //TopBannerIds
    int topBannerCounter;
    int indexTopBanner=0;
    public void LoadAdmob_TopBanner()
    {
        if (ADMOB_ID.ADMOB_TopBANNER_AD_ID.Length < indexTopBanner)
        {
            Create_TOPBannerBanner(ADMOB_ID.ADMOB_TopBANNER_AD_ID[indexTopBanner]);
        }
            
        else
        {
            indexTopBanner = 0;
            Create_TOPBannerBanner(ADMOB_ID.ADMOB_TopBANNER_AD_ID[indexTopBanner]);
        }
        topBannerCounter += 1;
        
    }
    public void Create_TOPBannerBanner(string currentId)
    {
        

        if (!checkInternet())
        {
            return;
        }

        if (!isBannerLoaded_Top)
        {
            Debug.Log("Top Banner Loading Request Send");
            if (BannerTop == null)
            {
                Create_TOPBannerView(currentId);
            }
            // Create an empty ad request.
            //   AdRequest request = new AdRequest.Builder().Build();
            AdRequest request = new AdRequest();

            // Load the banner with the request.
            BannerTop.LoadAd(request);

            //BannerTop.Hide();
        }
        else
        {
            TopBannerText.text = " Try: " + topBannerCounter + "Already Loaded Banner Id number " + (indexTopBanner);
        }


    }

    public void Create_TOPBannerView(string currentId)
    {
        Debug.Log("Creating Top banner view.");

        // If we already have a banner, destroy the old one.
        if (BannerTop != null)
        {
            Debug.Log("Destroying Top banner view.");
            BannerTop.Destroy();
            BannerTop = null;
        }

        // Create a 320x50 banner at top of the screen.

        AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        //AdSize bannersize = new AdSize(320,50);
        this.BannerTop = new BannerView(currentId, adaptiveSize, AdPosition.Top);

        // Listen to events the banner may raise.
        Bind_TopBannerEvents();

        Debug.Log("Top Banner view created.");
    }

    public void ShowAdmobTopBanner()
    {
        if (isBannerLoaded_Top)
        {
            if (!isTopBannerShowing)
            {
                isTopBannerShowing = true;
                this.BannerTop.Show();

                TopBannerText.text = " Try: " +topBannerCounter + " Show Banner Id number " + (indexTopBanner);
            }
        }
    }


    #endregion

    #region Bottom Banner
    int bottomBannerCounter;
    int indexBottomBanner = 0;
    public void LoadAdmob_BottomBanner()
    {
        if (ADMOB_ID.ADMOB_BottomBANNER_AD_ID.Length < indexBottomBanner)
        {
            
            
            Create_BottomBanner(ADMOB_ID.ADMOB_BottomBANNER_AD_ID[indexBottomBanner], AdPosition.BottomRight);
        }

        else
        {
            indexBottomBanner = 0;
            Create_BottomBanner(ADMOB_ID.ADMOB_BottomBANNER_AD_ID[indexBottomBanner], AdPosition.BottomRight);
        }
        
        
        BottomBannerText.text = " Try: " + bottomBannerCounter + " create Banner Id number " + (indexBottomBanner);
    }
    public void Create_BottomBanner(string currentID, AdPosition adPosition)
    {


        if (!checkInternet())
        {
            return;
        }

        if (!isBannerLoaded_Bottom)
        {
            
            Debug.Log("Bottom Banner Loading Request Send");
            if (BannerBottom == null)
            {
                Create_BottomBannerView(currentID, adPosition);
            }
            // Create an empty ad request.
            //   AdRequest request = new AdRequest.Builder().Build();
            AdRequest request = new AdRequest();

            // Load the banner with the request.
            BannerBottom.LoadAd(request);

            BannerBottom.Hide();
        }
        else
        {
            BottomBannerText.text = " Try: " + bottomBannerCounter + " Already load Banner Id number " + (indexBottomBanner);
        }
        
    }
    public void Create_BottomBannerView(string currentID, AdPosition adPosition)
    {
        Debug.Log("Creating Bottom banner view.");

        // If we already have a banner, destroy the old one.
        if (BannerBottom != null)
        {
            Debug.Log("Destroying Bottom banner view.");
            BannerBottom.Destroy();
            BannerBottom = null;
        }

        // Create a 320x50 banner at top of the screen.

        //AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        AdSize bannersize = new AdSize(320,50);
        this.BannerBottom = new BannerView(currentID, bannersize, adPosition);

        // Listen to events the banner may raise.
        Bind_BottomBannerEvents();

        Debug.Log("bottom Banner view created.");
    }
    public void ShowAdmobBottomBanner()
    {
        if (isBannerLoaded_Bottom)
        {
            if (!isBottomBannerShowing)
            {
                
                isBottomBannerShowing = true;
                this.BannerBottom.Show();
                BottomBannerText.text = " Try: " + bottomBannerCounter + " Show Banner Id number " + (indexBottomBanner);
            }
        }
    }
    public void HideAdmobBottomBanner()
    {
        if (isBottomBannerShowing)
        {
            isBottomBannerShowing = false;
            this.BannerBottom.Hide();
            BottomBannerText.text = " Try: " + bottomBannerCounter + " hide Banner Id number " + (indexBottomBanner);
        }
    }
    #endregion

    #endregion
    #region Mrec Banner
    #region Mrec Banner Variables

    [HideInInspector]
    public BannerView MediumBanner;

    public delegate void MRec_Showdelegate();
    public static event MRec_Showdelegate MRec_ShowEvent;


    public static bool isMediumBannerLoaded = false;
    public static bool isMRecShowing = false;
    #endregion
    int RectBannerCounter;
    int indexRectBanner = 0;
    public void LoadAdmobMediumBanner()
    {
        if (ADMOB_ID.ADMOB_MEDIUMBANNER_AD_ID.Length < indexRectBanner)
        {
            
            
            CreateAdmobMediumBanner(ADMOB_ID.ADMOB_MEDIUMBANNER_AD_ID[indexRectBanner]);
        }

        else
        {
            indexRectBanner = 0;
            CreateAdmobMediumBanner(ADMOB_ID.ADMOB_MEDIUMBANNER_AD_ID[indexRectBanner]);
        }
        RectBannerCounter += 1;
        
        RectBannerText.text = " Try: " + RectBannerCounter + " Show Banner Id number " + (indexRectBanner);
    }
    public void CreateAdmobMediumBanner(string currentMRecId)
    {

        if (checkInternet())
        {

            if (!isMediumBannerLoaded)
            {

                if (MediumBanner == null)
                {
                    Create_MediumBannerView(currentMRecId);
                }

                // Create an empty ad request.
                AdRequest request = new AdRequest();

                // Load the banner with the request.
                this.MediumBanner.LoadAd(request);

                this.MediumBanner.Hide();
            }
            else
            {

                Debug.Log($"Loading Call Skip: MRec is Already Loaded");
            }

        }
    }


    private void Create_MediumBannerView(string currentMRecId)
    {
        
        Debug.Log("bottom Banner view created.");

        Debug.Log("Creating Rect banner view.");

        if (MediumBanner != null)
        {
            Debug.Log("Destroying Rect banner view.");
            MediumBanner.Destroy();
            MediumBanner = null;
        }

        MediumBanner = new BannerView(currentMRecId, AdSize.MediumRectangle, AdPosition.BottomLeft);
        
        BindMediumBannerEvents();
        Debug.Log("Rect Banner view created.");
    }




    public void ShowAdmobMediumBanner()
    {
        if (isMediumBannerLoaded)
        {
            if (!isMRecShowing)
            {
               // Track.text = "MediumBanner show call " ;
                isMRecShowing = true;
                MediumBanner.Show();
            }
                
        }


        

    }
    public void HideAdmobRectBanner()
    {
        if (isMediumBannerLoaded)
        {
            isMRecShowing = false;
            MediumBanner.Hide();
        }
    }
    private void BindMediumBannerEvents()
    {

        MediumBanner.OnBannerAdLoaded += () => MediumBanner_HandleOnAdLoaded();
        MediumBanner.OnBannerAdLoadFailed += (LoadAdError error) => MediumBanner_HandleOnAdFailedToLoad(error);


        // Add similar event handlers for other events
    }
    #region MediumBannerCallBack Handlers



    private void MediumBanner_HandleOnAdLoaded()
    {
        if (isMediumBannerLoaded)
        {

            Debug.Log("New MRec Loaded ,Previous replaced");
        }
        else
        {
            Debug.Log("MRec:Loaded_");
            isMediumBannerLoaded = true;
            RectBannerText.text = " Try: " + RectBannerCounter + " Load Banner Id number " + (indexRectBanner);
        }
        
    }


    private void MediumBanner_HandleOnAdFailedToLoad(LoadAdError error)
    {
        if (isMediumBannerLoaded)
        {
            Debug.Log("MRec:Refresh_Failed");
        }
        else
        {
            Debug.Log("MRec:Failed to Load");
            //isMediumBannerLoaded = false;
            
            RectBannerText.text = " Try: " + RectBannerCounter + " Failed Banner Id number " + (indexRectBanner);
            indexRectBanner = (indexRectBanner + 1) % ADMOB_ID.ADMOB_MEDIUMBANNER_AD_ID.Length;
            Invoke(nameof(LoadAdmobMediumBanner), 5);
        }
        
    }



    #endregion


    #endregion

    #region AppOpen 
    private void OnApplicationPause(bool isPaused)
    {
        if (!isPaused)
        {

            if (SceneManager.GetActiveScene().buildIndex == 0) //Active in Live Game
            {
                return;
            }
            if (!ForeGroundedAD)
            {
                AppopenText.text = "LoadAdMobAppOpenAd call Pause";
                ShowAppOpenAd();
            }
            else
            {
                ForeGroundedAD = false;
            }
        }
    }
    #region appOpenAds Data
    [HideInInspector]
    public AppOpenAd appOpenAd;

    public static bool isAppopenLoaded;
    public static bool AppopenShowed;
    public bool IsDeviceLowMemory = false;
    int IndexAppOpen = 0;
    public static bool ForeGroundedAD =false;
    
    #endregion
    public static bool onloadAppOpenCall = false;
    IEnumerator delayStopAppOpenCall()
    {
        yield return new WaitForSecondsRealtime(2.5f);
        onloadAppOpenCall = false;
    }
    public void LoadAdMobAppOpenAd()
    {
        if (!isAdmobInitialized || !checkInternet())
        {
            return;
        }
        if (!isAppopenLoaded)
        {
            if (appOpenAd != null)
            {
                appOpenAd.Destroy();
            }
            AdRequest adRequest = new AdRequest();
            // Create and load the AdMob App Open Ad with the current ID
            AppOpenAd.Load(ADMOB_ID.ADMOB_APPOPEN_AD_ID[IndexAppOpen], adRequest, HandleAdMobAppOpenAdLoad);
        }
        else
        {
            //if(this.appOpenAd!= null)
            //{
            //    ShowAppOpenAd();
            //    RegisterAppOpenEvents(appOpenAd);
            //}

        }

    }
    // Handle AdMob App Open Ad load success or failure
    private void HandleAdMobAppOpenAdLoad(AppOpenAd _appOpenAd, LoadAdError loadAdError)
    {
        if (loadAdError == null)
        {
            // AdMob ad is loaded successfully
            this.appOpenAd = _appOpenAd;
            isAppopenLoaded = true;
            //if (isMainMenu != 1)
            //{
            //    ShowAppOpenAd();
            //    RegisterAppOpenEvents(appOpenAd);
            //}
            //else
            //    isMainMenu = 0;



        }
        else
        {
            // If AdMob ad fails to load, try the next ad ID
            Debug.Log("AdMob App Open Ad failed to load: " + loadAdError);
            IndexAppOpen = (IndexAppOpen + 1) % ADMOB_ID.ADMOB_APPOPEN_AD_ID.Length;  // Move to the next ad ID
            AppopenText.text = "show AppOpen Ad call: " + IndexAppOpen;
            Invoke(nameof(LoadAdMobAppOpenAd), .2f);// LoadAdMobAppOpenAd();
        }


    }
    private void RegisterAppOpenEvents(AppOpenAd Openad)
    {
        // Raised when the ad is estimated to have earned money.
        Openad.OnAdPaid += (AdValue adValue) =>
        {
            
        };
        // Raised when an impression is recorded for an ad.
        Openad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("App open ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        Openad.OnAdClicked += () =>
        {
            Debug.Log("App open ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        Openad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("App open ad full screen content opened.");
            isAppopenLoaded = false;
            // Inform the UI that the ad is consumed and not ready./
            //isShowingAppOpenAd = true;
        };
        // Raised when the ad closed full screen content.
        Openad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("App open ad full screen content closed.");
            isAppopenLoaded = false;
            // It may be useful to load a new ad when the current one is complete.
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                appOpenAd = null;
                // isShowingAppOpenAd = false;
                LoadAdMobAppOpenAd();
                Debug.Log("GG >> Admob:aoad:Closed_");

            });
        };
        // Raised when the ad failed to open full screen content.
        Openad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                appOpenAd = null;
                isAppopenLoaded = false;
                Debug.Log("GG >> Admob:aoad:FailedToShow");

            });
            Debug.LogError("App open ad failed to open full screen content with error : "
                            + error);

        };
    }
    // Show the loaded App Open Ad (either AdMob or Unity)
    public void ShowAppOpenAd()
    {
        if (isAppopenLoaded)
        {
            if (appOpenAd != null && appOpenAd.CanShowAd())
            {
                AppopenText.text = "show AppOpen Ad call " + IndexAppOpen;
                ForeGroundedAD = true;
                appOpenAd.Show();
                Debug.Log("App Open Ad is showing.");
                RegisterAppOpenEvents(appOpenAd);
            }
        }
        else
        {
            LoadAdMobAppOpenAd();
        }


    }
    #endregion
    #endregion
    bool checkInternet()
    {
        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork
            || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            return true;
        else
            return false;
    }
    


    public void Log(string text)
    {
        Debug.Log(text);
        if (_log)
        {
            _log.text = text;
        }
    }
    

    public bool isDealayCompleteForAds()
    {
        double secondsPassed = (DateTime.UtcNow - Time1ForAds).TotalSeconds;

        if (secondsPassed > 5)
        {
            setTime1ForAds();
            return true;
        }
        return false;
    }

    public void setTime1ForAds()
    {
        Time1ForAds = DateTime.UtcNow;
    }
    public void setTimeForInterAds()
    {
        Time1ForAds = DateTime.UtcNow;
    }
    public void RemoveAds()
    {
        PlayerPrefs.SetInt(RemoveAdsKey, 1);
        //HideMaxBanner();

    }
    
    IEnumerator Btn_Internet()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable
         || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork
         || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            // 1st = Debug.Log("! ( No ) Yes internet connection");
            // 2nd = Debug.Log("Internet is available via Wi-Fi");
            // 3rd = Debug.Log("Internet is available via mobile data");
            Internet = true;
            print("Internet == Yes");
        }
        else
        {

            NoInterNetPanelActive();
            Internet = false;
            print("Internet == No");
        }
        yield return new WaitForSecondsRealtime(1);
        StartCoroutine(Btn_Internet());
        //Invoke(nameof(Btn_Internet), 1f);
    }

    
    void NoInterNetPanelActive()
    {
        

        NoInternetPanelLandscap.SetActive(true);

        Canves.SetActive(true);
        Time.timeScale = 0;
    }
    public void CloseNoInternetPanel()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable
            || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork
            || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            NoInternetPanelLandscap.SetActive(false);
            Canves.SetActive(false);
            Time.timeScale = 1;
        }

    }
    
    


    /// <summary>
    /// Create Ads objects.
    /// </summary>


    public void ShowRewardPanel(bool value)
    {
        Canves.SetActive(value);
        RewardLoadingPanel.SetActive(value);
    }
    
    

    public void TryingtoInitializedAgain()
    {
        if (!isAdmobInitialized)
        {
            if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                if (trytoInitializeOnceMore)
                {
                    trytoInitializeOnceMore = false;

                    //initialize(true); 
                    Invoke(nameof(CanTryAgain), 20f);

                }
            }
        }
    }

    public void CanTryAgain()
    {
        trytoInitializeOnceMore = true;
    }

}
[Serializable]
public class AdmobId
{
    public string ADMOB_APP_ID;
    
    public string[]
         ADMOB_APPOPEN_AD_ID,
         ADMOB_TopBANNER_AD_ID,
        ADMOB_BottomBANNER_AD_ID,
         //ADMOB_INTERTITIAL_AD_ID,
         ADMOB_MEDIUMBANNER_AD_ID;
         //ADMOB_REWARDED_AD_ID;
}
