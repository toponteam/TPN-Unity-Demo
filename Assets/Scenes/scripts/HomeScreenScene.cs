using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using AnyThinkAds.Api;
using AnyThinkAds.ThirdParty.LitJson;

public class HomeScreenScene : MonoBehaviour, ATSDKInitListener
{
#if UNITY_ANDROID    
    private const string SdkAppId = "a5aa1f9deda26d";
    private const string SdkKey = "4f7b9ac17decb9babec83aac078742c7";
#elif UNITY_IOS || UNITY_IPHONE
    private const string SdkAppId = "a5b0e8491845b3";
    private const string SdkKey = "7eae0567827cfe2b22874061763f30c9";
#endif

    public Button showInterstitialButton;
    public Button showRewardButton;
    public Button showSplashButton;
    public Button showBannerButton;
    public Button showNativeButton;
    public Button mediationDebuggerButton;
    public Button automicRewardInterButton;
    public Text interstitialStatusText;
    public Text rewardStatusText;
    public Text splashStatusText;
    public Text nativeStatusText;
    public Text bannerStatusText;

    private bool isBannerAdShowing;
    private bool isNativeAdShowing;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start() >>> ");
        showInterstitialButton.onClick.AddListener(ShowInterstitialAd);
        showRewardButton.onClick.AddListener(ShowRewardedAd);
        showSplashButton.onClick.AddListener(ShowSplashAd);
        showBannerButton.onClick.AddListener(ShowBannerAd);
        showNativeButton.onClick.AddListener(ShowNativeAd);
        mediationDebuggerButton.onClick.AddListener(OpenDebuggerUITool);
        automicRewardInterButton.onClick.AddListener(JumpToAutomicPage);

        InitializeSDK();
    }

    void OnDestroy()
    {
        InterstitialAdOperator.Instance.destroyAd();
        RewardVideoAdOperator.Instance.destroyAd();
        SplashAdOperator.Instance.destroyAd();
        BannerAdOperator.Instance.destroyAd();
        NativeAdOperator.Instance.destroyAd();
    }

    private void OpenDebuggerUITool()
    {
        ATSDKAPI.showDebuggerUI();
    }

    private void JumpToAutomicPage()
    {
        SceneManager.LoadScene("AutomicRewardInterScene");
    }

    private void InitializeSDK() 
    {
        Debug.Log("SDK Start InitializeSdk");
        Debug.Log("UnityVersion of the runtime: " + Application.unityVersion);
        Debug.Log("Device Screen size : {" + Screen.width + ", " + Screen.height + "}");

        //Configure the distribution channel.
        ATSDKAPI.setChannel("unity3d_test_channel");
        ATSDKAPI.setSubChannel("unity3d_test_Subchannel");
        //To set custom parameters and retrieve them in ad callbacks,
        ATSDKAPI.initCustomMap(new Dictionary<string, string> { { "unity3d_data", "test_data" } });
        ATSDKAPI.setCustomDataForPlacementID(new Dictionary<string, string> { { "unity3d_data_pl", "test_data_pl" } }, "b5b728e7a08cd4");
        //Enable logging for ad debugging; must be set to false or removed before release.
        ATSDKAPI.setLogDebug(true);

        Debug.Log("Developer DataConsent: " + ATSDKAPI.getGDPRLevel());
        Debug.Log("Developer isEUTrafic: " + ATSDKAPI.isEUTraffic());
        ATSDKAPI.getUserLocation(new GetLocationListener());

        //Only for Android China SDK (CSJ)
#if UNITY_ANDROID
        ATDownloadManager.Instance.setListener(new ATDownloadListener());
#endif

        ATSDKAPI.initSDK(SdkAppId, SdkKey, this);
    }
    //This method will be called back after the SDK is initialized successfully
    public void initSuccess()
    {
        if (InitSdkHelper.IsInited)
        {
            return;
        }
        InitSdkHelper.IsInited = true;

        InitializeInterstitialAds();
        InitializeRewardedAds();
        InitializeBannerAds();
        InitializeNativeAds();
        InitializeSplashAds();

        //Splash ads display directly
        ShowSplashAd();
    }
    //This method will be called back after the SDK is initialized failed
    public void initFail(string msg)
    {
        Debug.Log("Developer callback SDK initFail:" + msg);
    }

    private void InitializeInterstitialAds()
    {
        InterstitialAdOperator.Instance.statusChangeEvent += statusChange;
        InterstitialAdOperator.Instance.retryLoadAdAttemptEvent += retryLoadAdAttempt;
        InterstitialAdOperator.Instance.initializeAd();
        LoadInterstitialAd();
    }

    private void InitializeRewardedAds() 
    {
        RewardVideoAdOperator.Instance.statusChangeEvent += statusChange;
        RewardVideoAdOperator.Instance.retryLoadAdAttemptEvent += retryLoadAdAttempt;
        RewardVideoAdOperator.Instance.initializeAd();
        LoadRewardVideoAd();
    }

    private void InitializeSplashAds()
    {
        SplashAdOperator.Instance.statusChangeEvent += statusChange;
        SplashAdOperator.Instance.retryLoadAdAttemptEvent += retryLoadAdAttempt;
        SplashAdOperator.Instance.initializeAd();
        LoadSplashAd();
    }

    private void InitializeBannerAds()
    {
        BannerAdOperator.Instance.statusChangeEvent += statusChange;
        BannerAdOperator.Instance.retryLoadAdAttemptEvent += retryLoadAdAttempt;
        BannerAdOperator.Instance.initializeAd();
        LoadBannerAd();
    }

    private void InitializeNativeAds()
    {
        NativeAdOperator.Instance.statusChangeEvent += statusChange;
        NativeAdOperator.Instance.retryLoadAdAttemptEvent += retryLoadAdAttempt;
        NativeAdOperator.Instance.initializeAd();
        LoadNativeAd();
    }
        
    public void ShowInterstitialAd()
    {
        InterstitialAdOperator.Instance.showAd();
    }

    public void ShowRewardedAd() 
    {
        RewardVideoAdOperator.Instance.showAd();
    }

    public void ShowSplashAd()
    {
        SplashAdOperator.Instance.showAd();
    }

    public void ShowBannerAd() 
    {
        if (!isBannerAdShowing)
        {
            BannerAdOperator.Instance.showAd();
            showBannerButton.GetComponentInChildren<Text>().text = "Hide Banner";
        }
        else
        {
            BannerAdOperator.Instance.hideBannerAd();
            showBannerButton.GetComponentInChildren<Text>().text = "Show Banner";
        }

        isBannerAdShowing = !isBannerAdShowing;
    }

    public void ShowNativeAd() 
    {
        if (!isNativeAdShowing)
        {
            NativeAdOperator.Instance.showAd();
            showNativeButton.GetComponentInChildren<Text>().text = "Hide Native";
        }
        else
        {
            NativeAdOperator.Instance.cleanAdView();
            showNativeButton.GetComponentInChildren<Text>().text = "Show Native";
        }

        isNativeAdShowing = !isNativeAdShowing;
    }

    public void showGDPRAuth()
    {
        Debug.Log("Developer showGDPRAuth");

        ATSDKAPI.showGDPRAuth();
    }

    public void LoadInterstitialAd()
    {
        Debug.Log("LoadInterstitialAd() >>> ");
        InterstitialAdOperator.Instance.loadAd();   
    }

    public void LoadRewardVideoAd()
    {
        RewardVideoAdOperator.Instance.loadAd();
    }

    public void LoadSplashAd()
    {
        SplashAdOperator.Instance.loadAd();
    }

    public void LoadBannerAd()
    {
        BannerAdOperator.Instance.loadAd();
    }

    public void LoadNativeAd()
    {
        NativeAdOperator.Instance.loadAd();
    }

    public void statusChange(object selfer, string status)
    {
        Debug.Log("statusChange() >>> selfer: " + selfer + " status: " + status);
        if (Object.ReferenceEquals(selfer, InterstitialAdOperator.Instance))
        {
            interstitialStatusText.text = status;
        } else if(Object.ReferenceEquals(selfer, RewardVideoAdOperator.Instance)) {
            rewardStatusText.text = status;
        } else if (Object.ReferenceEquals(selfer, SplashAdOperator.Instance)) {
            splashStatusText.text = status;
        } else if (Object.ReferenceEquals(selfer, BannerAdOperator.Instance)) {
            bannerStatusText.text = status;
        } else if (Object.ReferenceEquals(selfer, NativeAdOperator.Instance)) {
            nativeStatusText.text = status;
        }
    }

    public void retryLoadAdAttempt(object selfer, float retryDelay)
    {
        Debug.Log("retryLoadAdAttempt() >>> selfer: " + selfer + " retryDelay: " + retryDelay);
        if (Object.ReferenceEquals(selfer, InterstitialAdOperator.Instance))
        {
            Invoke("LoadInterstitialAd", retryDelay);
        } else if(Object.ReferenceEquals(selfer, RewardVideoAdOperator.Instance)) {
            Invoke("LoadRewardVideoAd", retryDelay);
        } else if (Object.ReferenceEquals(selfer, SplashAdOperator.Instance)) {
            Invoke("LoadSplashAd", retryDelay);
        } else if (Object.ReferenceEquals(selfer, BannerAdOperator.Instance)) {
            Invoke("LoadBannerAd", retryDelay);
        } else if (Object.ReferenceEquals(selfer, NativeAdOperator.Instance)) {
            Invoke("LoadNativeAd", retryDelay);
        }
    }

    private class GetLocationListener : ATGetUserLocationListener
    {
        public void didGetUserLocation(int location)
        {
            Debug.Log("Developer callback didGetUserLocation(): " + location);
            if (location == ATSDKAPI.kATUserLocationInEU && ATSDKAPI.getGDPRLevel() == ATSDKAPI.UNKNOWN)
            {
                ATSDKAPI.showGDPRAuth();
            }
        }
    }

    //Only for Android China SDK (Pangle)
    class ATDownloadListener : ATDownloadAdListener
    {
        public void onDownloadFail(string placementId, ATCallbackInfo callbackInfo, long totalBytes, long currBytes, string fileName, string appName)
        {
            Debug.Log("Developer onDownloadFail------->" + JsonMapper.ToJson(callbackInfo.toDictionary())
                + "\n, totalBytes: " + totalBytes + ", currBytes:" + currBytes
                + "\n, fileName: " + fileName + ", appName: " + appName);
        }

        public void onDownloadFinish(string placementId, ATCallbackInfo callbackInfo, long totalBytes, string fileName, string appName)
        {
            Debug.Log("Developer onDownloadFinish------->" + JsonMapper.ToJson(callbackInfo.toDictionary())
              + "\n, totalBytes: " + totalBytes
              + "\n, fileName: " + fileName + ", appName: " + appName);
        }

        public void onDownloadPause(string placementId, ATCallbackInfo callbackInfo, long totalBytes, long currBytes, string fileName, string appName)
        {
            Debug.Log("Developer onDownloadPause------->" + JsonMapper.ToJson(callbackInfo.toDictionary())
              + "\n, totalBytes: " + totalBytes + ", currBytes:" + currBytes
              + "\n, fileName: " + fileName + ", appName: " + appName);
        }

        public void onDownloadStart(string placementId, ATCallbackInfo callbackInfo, long totalBytes, long currBytes, string fileName, string appName)
        {
            Debug.Log("Developer onDownloadStart------->" + JsonMapper.ToJson(callbackInfo.toDictionary())
               + "\n, totalBytes: " + totalBytes + ", currBytes:" + currBytes
               + "\n, fileName: " + fileName + ", appName: " + appName);
        }

        public void onDownloadUpdate(string placementId, ATCallbackInfo callbackInfo, long totalBytes, long currBytes, string fileName, string appName)
        {
            Debug.Log("Developer onDownloadUpdate------->" + JsonMapper.ToJson(callbackInfo.toDictionary())
               + "\n, totalBytes: " + totalBytes + ", currBytes:" + currBytes
               + "\n, fileName: " + fileName + ", appName: " + appName);
        }

        public void onInstalled(string placementId, ATCallbackInfo callbackInfo, string fileName, string appName)
        {
            Debug.Log("Developer onInstalled------->" + JsonMapper.ToJson(callbackInfo.toDictionary())
              + "\n, fileName: " + fileName + ", appName: " + appName);
        }
    }
}
