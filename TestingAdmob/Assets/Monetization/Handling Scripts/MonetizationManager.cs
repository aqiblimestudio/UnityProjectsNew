using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MicroLabGamesMonetization
{
    public class MonetizationManager : MonoBehaviour
    {
        public static MonetizationManager instance;
        public static bool ForeGroundedAD =false;
        public static bool ConsentPanelAlreadyShow = false;
        
        public bool isTesting,isEditor;
        public bool RemoveAds;
        public int CurrentTopBannerShow;
        public int CurrentBottomBannerShow;
        public int CurrentBigBannerShow;
        #region Variables


        #region Admob Variables


        [Header("Admob Top Banner Ids")]
        [Space(10)]
        [SerializeField] string AdmobHighTopBannerId;
        [SerializeField] string AdmobMedTopBannerId;
        [SerializeField] string AdmobLowTopBannerId;

        [Header("Admob Bottom Banner Ids")]
        [Space(10)]
        [SerializeField] string AdmobHighBottomBannerId;
        [SerializeField] string AdmobMedBottomBannerId;
        [SerializeField] string AdmobLowBottomBannerId;
        #endregion

        #region BigBanner Ids
        [Header("Admob BigBanner Ids")]
        [Space(10)]
        [SerializeField] string AdmobHighBigBannerId;
        [SerializeField] string AdmobMedBigBannerId;
        [SerializeField] string AdmobLowBigBannerId;
        #endregion

        #region AppOpen Ids
        [Header("Admob BigBanner Ids")]
        [Space(10)]
        [SerializeField] string AdmobHighAppOpenId;
        [SerializeField] string AdmobMedAppOpenId;
        [SerializeField] string AdmobLowAppOpenId;
        #endregion

        #region TestAds

        string BannerTestID = "ca-app-pub-3940256099942544/6300978111";
        string BigBannerTestID = "ca-app-pub-3940256099942544/6300978111";
        string AppOpenTestID = "ca-app-pub-3940256099942544/9257395921";
        #endregion

        #region Handlers


        AdmobMonetization HighTopBanner;
        AdmobMonetization MedTopBanner;
        AdmobMonetization LowTopBanner;

        AdmobMonetization HighBottomBanner;
        AdmobMonetization MedBottomBanner;
        AdmobMonetization LowBottomBanner;

        AdmobMonetization HighBigBanner;
        AdmobMonetization MedBigBanner;
        AdmobMonetization LowBigBanner;

        AdmobMonetization HighAppOpen;
        AdmobMonetization MedAppOpen;
        AdmobMonetization LowAppOpen;
        #endregion

        #region TempVariables
        #endregion
        public GameObject AppOpenBGScreen;
        public Text LHigh_Info, LMed_Info, LLow_Info;
        public Text High_Info, Med_Info, Low_Info;
        public Text High_HInfo, Med_HInfo, Low_HInfo;
        public int LHigh_InfoValue, LMed_InfoValue, LLow_InfoValue;
        public int High_InfoValue, Med_InfoValue, Low_InfoValue;
        public int High_HInfoValue, Med_HInfoValue, Low_HInfoValue;
        
        #endregion
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        void Start()
        {

            if (isTesting)
            {
                HighTopBanner = new AdmobMonetization(BannerTestID);
                MedTopBanner = new AdmobMonetization(BannerTestID);
                LowTopBanner = new AdmobMonetization(BannerTestID);

                HighBottomBanner = new AdmobMonetization(BannerTestID, AdPosition.BottomRight);
                MedBottomBanner = new AdmobMonetization(BannerTestID, AdPosition.BottomRight);
                LowBottomBanner = new AdmobMonetization(BannerTestID, AdPosition.BottomRight);

                HighBigBanner = new AdmobMonetization(BannerTestID, AdPosition.BottomLeft, 0);
                MedBigBanner = new AdmobMonetization(BannerTestID, AdPosition.BottomLeft, 0);
                LowBigBanner = new AdmobMonetization(BannerTestID, AdPosition.BottomLeft, 0);

                HighAppOpen = new AdmobMonetization(AppOpenTestID, 0);
                MedAppOpen = new AdmobMonetization(AppOpenTestID, 1);
                LowAppOpen = new AdmobMonetization(AppOpenTestID, 2);
            }
            else
            {
                HighTopBanner = new AdmobMonetization(AdmobHighTopBannerId);
                MedTopBanner = new AdmobMonetization(AdmobMedTopBannerId);
                LowTopBanner = new AdmobMonetization(AdmobLowTopBannerId);

                HighBottomBanner = new AdmobMonetization(AdmobHighBottomBannerId, AdPosition.BottomRight);
                MedBottomBanner = new AdmobMonetization(AdmobMedBottomBannerId, AdPosition.BottomRight);
                LowBottomBanner = new AdmobMonetization(AdmobLowBottomBannerId, AdPosition.BottomRight);

                HighBigBanner = new AdmobMonetization(AdmobHighBigBannerId, AdPosition.BottomLeft,0);
                MedBigBanner = new AdmobMonetization(AdmobMedBigBannerId, AdPosition.BottomLeft, 0);
                LowBigBanner = new AdmobMonetization(AdmobLowBigBannerId, AdPosition.BottomLeft, 0);

                HighAppOpen = new AdmobMonetization(AdmobHighAppOpenId,0);
                MedAppOpen = new AdmobMonetization(AdmobMedAppOpenId,1);
                LowAppOpen = new AdmobMonetization(AdmobLowAppOpenId,2);
            }

            MobileAds.Initialize(initStatus => { });

            if (PlayerPrefs.GetInt("GDPRConsentAd") != 0)
            {

                OnClose_consentForm();

            }
            else
            {
                GetGDPR_Consent();

            }
        }
        public void AdmobInitComplete()
        {
            Invoke(nameof(LoadTopBanner),3);
            Invoke(nameof(LoadBottomBanner), 4);
            Invoke(nameof(LoadBigBanner), 5);
            //LoadTopBanner();
            //LoadBottomBanner();
            //LoadBigBanner();
            LoadAppOpenAd();
        }
        bool RemoveAdsChecker()
        {
            if (RemoveAds)
                return true;
            else
                return false;
        }

        #region Get GDRP Consent

        void OnClose_consentForm()
        {
            GameResume();
            if (!ConsentPanelAlreadyShow)
            {
                ConsentPanelAlreadyShow = true;
                AdmobInitComplete();
            }
            
        }
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
            if (error != null)
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
            if (error != null)
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
            if (ConsentInformation.ConsentStatus == ConsentStatus.Required)
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

        #region  Top Banner
        public void LoadTopBanner()
        {
            if (RemoveAdsChecker())
                return;

            //Admob
           // Banners.AssignBannerPoition();

            HighTopBanner.AssignTopBannerPoition(AdPosition.Top);
            MedTopBanner.AssignTopBannerPoition(AdPosition.Top);
            LowTopBanner.AssignTopBannerPoition(AdPosition.Top);

            LHigh_InfoValue += 1;
            LMed_InfoValue += 1;
            LLow_InfoValue += 1;

            LHigh_Info.text = "HighBanner LoadBanner " + LHigh_InfoValue;
            HighTopBanner.LoadTopBanner();
            LMed_Info.text = "MedBanner LoadBanner " + LMed_InfoValue;
            MedTopBanner.LoadTopBanner();
            LLow_Info.text = "LowBanner LoadBanner " + LLow_InfoValue;
            LowTopBanner.LoadTopBanner();

        }
        
        public void ShowTopBanner()
        {
            if (RemoveAdsChecker())
                return;

            PriorityCheckTopBannerLoad();
        }
        
        void PriorityCheckTopBannerLoad()
        {
            if (!SwitchNextTopBannerLoadCheck(0))
            {
                if (!SwitchNextTopBannerLoadCheck(1))
                {
                    if (!SwitchNextTopBannerLoadCheck(2))
                    {
                    }
                }
            }
        }
        bool SwitchNextTopBannerLoadCheck(int Priority)
        {
            switch (Priority)
            {
                case 0:
                    if (HighTopBanner.IsLoadTopBanner())
                    {
                        
                        if (!HighTopBanner.IsAlreadyLoadTopBanner())
                        {
                            High_InfoValue += 1;
                            High_Info.text =  " HighBanner Show " + High_InfoValue;
                            CurrentTopBannerShow = 0;

                            HideTopBannerLoadCheck(1);
                            HideTopBannerLoadCheck(2);
                            HighTopBanner.ShowTopBanner();
                        }
                        else
                        {
                            High_InfoValue += 1;
                            High_Info.text = "Already HighBanner Show " + High_InfoValue;
                        }

                        return true;
                    }
                    else
                    {
                        LHigh_InfoValue += 1;
                        LHigh_Info.text = "HighBanner LoadBanner " + LHigh_InfoValue;
                        HighTopBanner.LoadTopBanner();
                        return false;
                    }
                        
                case 1:
                    if (MedTopBanner.IsLoadTopBanner())
                    {
                        CurrentTopBannerShow = 1;
                        if (!MedTopBanner.IsAlreadyLoadTopBanner())
                        {
                            Med_InfoValue +=1;
                            Med_Info.text = "MedBanner Show " + Med_InfoValue;
                            CurrentTopBannerShow = 0;

                            HideTopBannerLoadCheck(0);
                            HideTopBannerLoadCheck(2);
                            MedTopBanner.ShowTopBanner();
                        }
                        else
                        {
                            Med_InfoValue += 1;
                            Med_Info.text = "Already MedBanner Show" + Med_InfoValue;
                        }
                        return true;
                    }
                    else
                    {
                        LMed_InfoValue += 1;
                        LMed_Info.text = "MedBanner LoadBanner "+ LMed_InfoValue;
                        MedTopBanner.LoadTopBanner();
                        
                        return false;
                    }
                case 2:
                    if (LowTopBanner.IsLoadTopBanner())
                    {
                        CurrentTopBannerShow = 2;
                        if (!LowTopBanner.IsAlreadyLoadTopBanner())
                        {
                            Low_InfoValue += 1;
                            Low_Info.text = "LowBanner Show "+ Low_InfoValue;
                            CurrentTopBannerShow = 0;

                            HideTopBannerLoadCheck(0);
                            HideTopBannerLoadCheck(1);
                            LowTopBanner.ShowTopBanner();
                        }
                        else
                        {
                            Low_InfoValue += 1;
                            Low_Info.text = "Already LowBanner Show " + Low_InfoValue;
                        }
                        return true;
                    }
                    else
                    {
                        LLow_InfoValue += 1;
                        LLow_Info.text = "LowBanner LoadBanner "+ LLow_InfoValue;
                        
                        LowTopBanner.LoadTopBanner();
                        return false;
                    }
                case 99:
                    return false;
                default:
                    return false;
            }
        }
        //void PriorityBanner(int priority, int Position)
        //{
        //    switch (priority)
        //    {
        //        case 0:
                    
        //            break;
        //        case 1:
        //            if (!bannerAlreadyShown)
        //            {
        //                bannerAlreadyShown = true;
        //                Banners.BannerPositionIndex = Position;
        //                Banners.AssignBannerPoition();
        //                Banners.ShowBanner();
        //                //Banners.ChangePositionBanner();
        //            }
        //            else
        //            {
        //                Banners.BannerPositionIndex = Position;
        //                Banners.ChangePositionBanner();
        //            }
        //            break;
        //    }
        //}
        public void HideTopBanner()
        {
            if (!HideTopBannerLoadCheck(CurrentTopBannerShow))
            {
                if (!HideTopBannerLoadCheck(0))
                {
                    if (!HideTopBannerLoadCheck(1))
                    {
                        if (!HideTopBannerLoadCheck(2))
                        {
                        }
                    }
                }
            }
        }

        bool HideTopBannerLoadCheck(int Priority)
        {
            
            switch (Priority)
            {
                case 0:
                    
                    if (HighTopBanner.IsLoadTopBanner() )
                    {
                        if (HighTopBanner.IsAlreadyLoadTopBanner())
                        {
                            
                            High_HInfoValue += 1;
                            High_HInfo.text = "HighBanner HideBanner " + High_HInfoValue;
                            HighTopBanner.HideTopBanner();
                        }
                        else
                        {
                            High_HInfoValue += 1;
                            High_HInfo.text = "already HighBanner HideBanner " + High_HInfoValue;
                        }
                        

                        return true;
                    }
                    else
                        return false;
                case 1:
                    if (MedTopBanner.IsLoadTopBanner() )
                    {
                        if (MedTopBanner.IsAlreadyLoadTopBanner())
                        {
                            Med_HInfoValue += 1;
                            //&& MedBanner.IsAlreadyShowBanner()
                            Med_HInfo.text = "MedBanner HideBanner " + Med_HInfoValue;
                            MedTopBanner.HideTopBanner();
                        }
                        else
                        {
                            Med_HInfoValue += 1;
                            Med_HInfo.text = "Already MedBanner HideBanner " + Med_HInfoValue;
                        }
                        

                        return true;
                    }
                    else
                        return false;
                case 2:
                    if (LowTopBanner.IsLoadTopBanner() )
                    {
                        if (LowTopBanner.IsAlreadyLoadTopBanner())
                        {
                            Low_HInfoValue += 1;
                            //&& LowBanner.IsAlreadyShowBanner()
                            Low_HInfo.text = "LowBanner HideBanner " + Low_HInfoValue;
                            LowTopBanner.HideTopBanner();
                        }
                        else
                        {
                            Low_HInfoValue += 1;
                            Low_HInfo.text = "Already LowBanner HideBanner " + Low_HInfoValue;
                        }
                        

                        return true;
                    }
                    else
                        return false;
                case 99:
                    return false;
                default:
                    return false;
            }
        }

        #endregion
        #region Bottom Banner
        public void LoadBottomBanner()
        {
            if (RemoveAdsChecker())
                return;

            //Admob
            // Banners.AssignBannerPoition();

            HighBottomBanner.AssignBottomBannerPoition(AdPosition.BottomRight);
            MedBottomBanner.AssignBottomBannerPoition(AdPosition.BottomRight);
            LowBottomBanner.AssignBottomBannerPoition(AdPosition.BottomRight);

            LHigh_InfoValue += 1;
            LMed_InfoValue += 1;
            LLow_InfoValue += 1;

            LHigh_Info.text = "HighBanner LoadBanner " + LHigh_InfoValue;
            HighBottomBanner.LoadBottomBanner();
            LMed_Info.text = "MedBanner LoadBanner " + LMed_InfoValue;
            MedBottomBanner.LoadBottomBanner();
            LLow_Info.text = "LowBanner LoadBanner " + LLow_InfoValue;
            LowBottomBanner.LoadBottomBanner();

        }
        public void ShowBottomBanner()
        {
            if (RemoveAdsChecker())
                return;

            PriorityCheckBottomBannerLoad();
        }
        public void HideBottomBanneronRequest(int value)
        {
            HideBottomBannerLoadCheck(value);
        }
        void PriorityCheckBottomBannerLoad()
        {
            if (!SwitchNextBottomBannerLoadCheck(0))
            {
                if (!SwitchNextBottomBannerLoadCheck(1))
                {
                    if (!SwitchNextBottomBannerLoadCheck(2))
                    {
                    }
                }
            }
        }
        bool SwitchNextBottomBannerLoadCheck(int Priority)
        {
            switch (Priority)
            {
                case 0:
                    if (HighBottomBanner.IsLoadBottomBanner())
                    {

                        if (!HighBottomBanner.IsAlreadyLoadBottomBanner())
                        {
                            High_InfoValue += 1;
                            High_Info.text = " HighBanner Show " + High_InfoValue;
                            CurrentBottomBannerShow = 0;

                            HideBottomBannerLoadCheck(1);
                            HideBottomBannerLoadCheck(2);
                            HighBottomBanner.ShowBottomBanner();
                        }
                        else
                        {
                            High_InfoValue += 1;
                            High_Info.text = "Already HighBanner Show " + High_InfoValue;
                        }

                        return true;
                    }
                    else
                    {
                        LHigh_InfoValue += 1;
                        LHigh_Info.text = "HighBanner LoadBanner " + LHigh_InfoValue;
                        HighBottomBanner.LoadBottomBanner();
                        return false;
                    }

                case 1:
                    if (MedBottomBanner.IsLoadBottomBanner())
                    {
                        CurrentBottomBannerShow = 1;
                        if (!MedBottomBanner.IsAlreadyLoadBottomBanner())
                        {
                            Med_InfoValue += 1;
                            Med_Info.text = "MedBanner Show " + Med_InfoValue;
                            CurrentBottomBannerShow = 0;

                            HideBottomBannerLoadCheck(0);
                            HideBottomBannerLoadCheck(2);
                            MedBottomBanner.ShowBottomBanner();
                        }
                        else
                        {
                            Med_InfoValue += 1;
                            Med_Info.text = "Already MedBanner Show" + Med_InfoValue;
                        }
                        return true;
                    }
                    else
                    {
                        LMed_InfoValue += 1;
                        LMed_Info.text = "MedBanner LoadBanner " + LMed_InfoValue;
                        MedBottomBanner.LoadBottomBanner();

                        return false;
                    }
                case 2:
                    if (LowBottomBanner.IsLoadBottomBanner())
                    {
                        CurrentBottomBannerShow = 2;
                        if (!LowBottomBanner.IsAlreadyLoadBottomBanner())
                        {
                            Low_InfoValue += 1;
                            Low_Info.text = "LowBanner Show " + Low_InfoValue;
                            CurrentBottomBannerShow = 0;

                            bool value00 = HideBottomBannerLoadCheck(0);
                            bool value01 = HideBottomBannerLoadCheck(1);
                            LowBottomBanner.ShowBottomBanner();
                        }
                        else
                        {
                            Low_InfoValue += 1;
                            Low_Info.text = "Already LowBanner Show " + Low_InfoValue;
                        }
                        return true;
                    }
                    else
                    {
                        LLow_InfoValue += 1;
                        LLow_Info.text = "LowBanner LoadBanner " + LLow_InfoValue;

                        LowBottomBanner.LoadBottomBanner();
                        return false;
                    }
                case 99:
                    return false;
                default:
                    return false;
            }
        }
        //void PriorityBanner(int priority, int Position)
        //{
        //    switch (priority)
        //    {
        //        case 0:

        //            break;
        //        case 1:
        //            if (!bannerAlreadyShown)
        //            {
        //                bannerAlreadyShown = true;
        //                Banners.BannerPositionIndex = Position;
        //                Banners.AssignBannerPoition();
        //                Banners.ShowBanner();
        //                //Banners.ChangePositionBanner();
        //            }
        //            else
        //            {
        //                Banners.BannerPositionIndex = Position;
        //                Banners.ChangePositionBanner();
        //            }
        //            break;
        //    }
        //}
        public void HideBottomBanner()
        {
            if (!HideBottomBannerLoadCheck(CurrentBottomBannerShow))
            {
                if (!HideBottomBannerLoadCheck(0))
                {
                    if (!HideBottomBannerLoadCheck(1))
                    {
                        if (!HideBottomBannerLoadCheck(2))
                        {
                        }
                    }
                }
            }
        }

        bool HideBottomBannerLoadCheck(int Priority)
        {

            switch (Priority)
            {
                case 0:

                    if (HighBottomBanner.IsLoadBottomBanner())
                    {
                        if (HighBottomBanner.IsAlreadyLoadBottomBanner())
                        {

                            High_HInfoValue += 1;
                            High_HInfo.text = "HighBanner HideBanner " + High_HInfoValue;
                            HighBottomBanner.HideBottomBanner();
                        }
                        else
                        {
                            High_HInfoValue += 1;
                            High_HInfo.text = "already HighBanner HideBanner " + High_HInfoValue;
                        }


                        return true;
                    }
                    else
                        return false;
                case 1:
                    if (MedBottomBanner.IsLoadBottomBanner())
                    {
                        if (MedBottomBanner.IsAlreadyLoadBottomBanner())
                        {
                            Med_HInfoValue += 1;
                            //&& MedBanner.IsAlreadyShowBanner()
                            Med_HInfo.text = "MedBanner HideBanner " + Med_HInfoValue;
                            MedBottomBanner.HideBottomBanner();
                        }
                        else
                        {
                            Med_HInfoValue += 1;
                            Med_HInfo.text = "Already MedBanner HideBanner " + Med_HInfoValue;
                        }


                        return true;
                    }
                    else
                        return false;
                case 2:
                    if (LowBottomBanner.IsLoadBottomBanner())
                    {
                        if (LowBottomBanner.IsAlreadyLoadBottomBanner())
                        {
                            Low_HInfoValue += 1;
                            //&& LowBanner.IsAlreadyShowBanner()
                            Low_HInfo.text = "LowBanner HideBanner " + Low_HInfoValue;
                            LowBottomBanner.HideBottomBanner();
                        }
                        else
                        {
                            Low_HInfoValue += 1;
                            Low_HInfo.text = "Already LowBanner HideBanner " + Low_HInfoValue;
                        }


                        return true;
                    }
                    else
                        return false;
                case 99:
                    return false;
                default:
                    return false;
            }
        }

        #endregion
        #region Big Banner
        public void LoadBigBanner()
        {
            if (RemoveAdsChecker())
                return;



            HighBigBanner.AssignBigBannerPoition(AdPosition.BottomLeft);
            MedBigBanner.AssignBigBannerPoition(AdPosition.BottomLeft);
            LowBigBanner.AssignBigBannerPoition(AdPosition.BottomLeft);

            LHigh_InfoValue += 1;
            LMed_InfoValue += 1;
            LLow_InfoValue += 1;

            LHigh_Info.text = "HighBanner LoadBanner " + LHigh_InfoValue;
            HighBigBanner.LoadBigBanner();
            LMed_Info.text = "MedBanner LoadBanner " + LMed_InfoValue;
            MedBigBanner.LoadBigBanner();
            LLow_Info.text = "LowBanner LoadBanner " + LLow_InfoValue;
            LowBigBanner.LoadBigBanner();

        }
        public void ShowBigBanner()
        {
            if (RemoveAdsChecker())
                return;

            PriorityCheckBigBannerLoad();
        }
        public void HideBigBanneronRequest(int value)
        {
            SwitchNextBigBannerLoadCheck(value);
        }
        void PriorityCheckBigBannerLoad()
        {
            if (!SwitchNextBigBannerLoadCheck(0))
            {
                if (!SwitchNextBigBannerLoadCheck(1))
                {
                    if (!SwitchNextBigBannerLoadCheck(2))
                    {
                    }
                }
            }
        }
        bool SwitchNextBigBannerLoadCheck(int Priority)
        {
            switch (Priority)
            {
                case 0:
                    if (HighBigBanner.IsLoadBigBanner())
                    {

                        if (!HighBigBanner.IsAlreadyLoadBigBanner())
                        {
                            High_InfoValue += 1;
                            High_Info.text = " HighBigBanner Show " + High_InfoValue;
                            CurrentBigBannerShow = 0;

                            HideBigBannerLoadCheck(1);
                            HideBigBannerLoadCheck(2);
                            HighBigBanner.ShowBigBanner();
                        }
                        else
                        {
                            High_InfoValue += 1;
                            High_Info.text = "Already HighBigBanner Show " + High_InfoValue;
                        }

                        return true;
                    }
                    else
                    {
                        LHigh_InfoValue += 1;
                        LHigh_Info.text = "HighBigBanner LoadBanner " + LHigh_InfoValue;
                        HighBigBanner.LoadBigBanner();
                        return false;
                    }

                case 1:
                    if (MedBigBanner.IsLoadBigBanner())
                    {
                        CurrentBigBannerShow = 1;
                        if (!MedBigBanner.IsAlreadyLoadBigBanner())
                        {
                            Med_InfoValue += 1;
                            Med_Info.text = "MedBigBanner Show " + Med_InfoValue;
                            CurrentBigBannerShow = 0;

                            HideBigBannerLoadCheck(0);
                            HideBigBannerLoadCheck(2);
                            MedBigBanner.ShowBigBanner();
                        }
                        else
                        {
                            Med_InfoValue += 1;
                            Med_Info.text = "Already MedBigBanner Show" + Med_InfoValue;
                        }
                        return true;
                    }
                    else
                    {
                        LMed_InfoValue += 1;
                        LMed_Info.text = "MedBigBanner LoadBanner " + LMed_InfoValue;
                        MedBigBanner.LoadBigBanner();

                        return false;
                    }
                case 2:
                    if (LowBigBanner.IsLoadBigBanner())
                    {
                        CurrentBigBannerShow = 2;
                        if (!LowBigBanner.IsAlreadyLoadBigBanner())
                        {
                            Low_InfoValue += 1;
                            Low_Info.text = "LowBigBanner Show " + Low_InfoValue;
                            CurrentBigBannerShow = 0;

                            bool value00 = HideBigBannerLoadCheck(0);
                            bool value01 = HideBigBannerLoadCheck(1);
                            LowBigBanner.ShowBigBanner();
                        }
                        else
                        {
                            Low_InfoValue += 1;
                            Low_Info.text = "Already LowBigBanner Show " + Low_InfoValue;
                        }
                        return true;
                    }
                    else
                    {
                        LLow_InfoValue += 1;
                        LLow_Info.text = "LowBigBanner LoadBanner " + LLow_InfoValue;

                        LowBigBanner.LoadBigBanner();
                        return false;
                    }
                case 99:
                    return false;
                default:
                    return false;
            }
        }
        
        public void HideBigBanner()
        {
            if (!HideBigBannerLoadCheck(CurrentBigBannerShow))
            {
                if (!HideBigBannerLoadCheck(0))
                {
                    if (!HideBigBannerLoadCheck(1))
                    {
                        if (!HideBigBannerLoadCheck(2))
                        {
                        }
                    }
                }
            }
        }

        bool HideBigBannerLoadCheck(int Priority)
        {

            switch (Priority)
            {
                case 0:

                    if (HighBigBanner.IsLoadBigBanner())
                    {
                        if (HighBigBanner.IsAlreadyLoadBigBanner())
                        {

                            High_HInfoValue += 1;
                            High_HInfo.text = "HighBigBanner HideBanner " + High_HInfoValue;
                            HighBigBanner.HideBigBanner();
                        }
                        else
                        {
                            High_HInfoValue += 1;
                            High_HInfo.text = "already HighBigBanner HideBanner " + High_HInfoValue;
                        }


                        return true;
                    }
                    else
                        return false;
                case 1:
                    if (MedBigBanner.IsLoadBigBanner())
                    {
                        if (MedBigBanner.IsAlreadyLoadBigBanner())
                        {
                            Med_HInfoValue += 1;
                            //&& MedBanner.IsAlreadyShowBanner()
                            Med_HInfo.text = "MedBigBanner HideBanner " + Med_HInfoValue;
                            MedBigBanner.HideBigBanner();
                        }
                        else
                        {
                            Med_HInfoValue += 1;
                            Med_HInfo.text = "Already MedBigBanner HideBanner " + Med_HInfoValue;
                        }


                        return true;
                    }
                    else
                        return false;
                case 2:
                    if (LowBigBanner.IsLoadBigBanner())
                    {
                        if (LowBigBanner.IsAlreadyLoadBigBanner())
                        {
                            Low_HInfoValue += 1;
                            //&& LowBanner.IsAlreadyShowBanner()
                            Low_HInfo.text = "LowBigBanner HideBanner " + Low_HInfoValue;
                            LowBigBanner.HideBigBanner();
                        }
                        else
                        {
                            Low_HInfoValue += 1;
                            Low_HInfo.text = "Already LowBigBanner HideBanner " + Low_HInfoValue;
                        }


                        return true;
                    }
                    else
                        return false;
                case 99:
                    return false;
                default:
                    return false;
            }
        }

        #endregion

        #region AppOpen Ad
        public void LoadAppOpenAd()
        {
            if (RemoveAdsChecker())
                return;

            //Admob
            LHigh_InfoValue += 1;
            LHigh_Info.text = "Load HighAppOpen " + LHigh_InfoValue;
            HighAppOpen.LoadAppOpenAd();
            LMed_InfoValue += 1;
            LMed_Info.text = "Load MedAppOpen " + LMed_InfoValue;
            MedAppOpen.LoadAppOpenAd();
            LLow_InfoValue += 1;
            LLow_Info.text = "Load LowAppOpen " + LLow_InfoValue;
            LowAppOpen.LoadAppOpenAd();
        }
        public void ShowAppOpenAd()
        {
            if (RemoveAdsChecker())
                return;

            PrioritySettingAppOpen(0, 1, 2);
        }
        void PrioritySettingAppOpen(int FirstPriority, int SecondPriority, int ThirdPriority)
        {
            if (!SwitchAppOpen(FirstPriority))
            {
                if (!SwitchAppOpen(SecondPriority))
                {
                    if (!SwitchAppOpen(ThirdPriority))
                    {
                    }
                }
            }
        }
        bool SwitchAppOpen(int Priority)
        {
            switch (Priority)
            {
                
                case 0:
                    if (HighAppOpen.IsAppOpenLoaded())
                    {
                        ActiveAppOpenImage();
                        High_InfoValue += 1;
                        High_Info.text = "Show HighAppOpen "+ High_InfoValue;
                        HighAppOpen.ShowAppOpen();
                        return true;
                    }
                    else
                    {
                        LHigh_InfoValue += 1;
                        LHigh_Info.text = "Load HighAppOpen " + LHigh_InfoValue;
                        HighAppOpen.LoadAppOpenAd();
                        return false;
                    }
                        
                case 1:
                    if (MedAppOpen.IsAppOpenLoaded())
                    {
                        ActiveAppOpenImage();
                        Med_InfoValue += 1;
                        Med_Info.text = "Show MedAppOpen "+ Med_InfoValue;
                        MedAppOpen.ShowAppOpen();
                        return true;
                    }
                    else
                    {
                        LMed_InfoValue += 1;
                        LMed_Info.text = "Load MedAppOpen " + LMed_InfoValue;
                        MedAppOpen.LoadAppOpenAd();
                        return false;
                    }
                case 2:
                    if (LowAppOpen.IsAppOpenLoaded())
                    {
                        ActiveAppOpenImage();
                        Low_InfoValue += 1;
                        Low_Info.text = "Show LowAppOpen "+ Low_InfoValue;
                        ForeGroundedAD = true;
                        LowAppOpen.ShowAppOpen();
                        return true;
                    }
                    else
                    {
                        LLow_InfoValue += 1;
                        LLow_Info.text = "Load LowAppOpen " + LLow_InfoValue;
                        LowAppOpen.LoadAppOpenAd();
                        return false;
                    }
                case 99:
                    return false;
                default:
                    return false;
            }
        }
        void ActiveAppOpenImage()
        {
            AppOpenBGScreen.SetActive(true);
            Invoke(nameof(DeactiveAppOpenImage), .5f);
        }
        void DeactiveAppOpenImage()
        {
            ForeGroundedAD = false;
            AppOpenBGScreen.SetActive(false);
        }
        private void OnApplicationPause(bool isPaused)
        {
            if (!isPaused)
            {

                if (SceneManager.GetActiveScene().buildIndex == 0)
                {
                    return;
                }
                if (!ForeGroundedAD)
                {
                    
                    ShowAppOpenAd();
                }
                else
                {
                    ForeGroundedAD = false;
                }
            }
        }

        public void GamePause()
        {
            Time.timeScale = 0;
        }
        public void GameResume()
        {
            Time.timeScale = 1;
        }
        #endregion
    }

}