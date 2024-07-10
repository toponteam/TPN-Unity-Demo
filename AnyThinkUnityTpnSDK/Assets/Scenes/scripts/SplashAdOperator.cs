using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AnyThinkAds.Api;
using UnityEngine.UI;
using AnyThinkAds.ThirdParty.LitJson;

public class SplashAdOperator : BaseAdOperator
{
#if UNITY_ANDROID
    private const string SPLASH_PLACEMENT_ID = "b5bea7cc9a4497";   
#elif UNITY_IOS || UNITY_IPHONE
	private static string SPLASH_PLACEMENT_ID = "b5c22f0e5cc7a0";
#endif

    private static readonly SplashAdOperator instance = new SplashAdOperator();

    private SplashAdOperator() 
	{
        
	}

	public static SplashAdOperator Instance 
	{
        get 
		{
			return instance;
		}
	}
        
    public override void initializeAd()
    {
        ATSplashAd.Instance.client.onAdLoadEvent += onSplashAdLoad;
        ATSplashAd.Instance.client.onAdCloseEvent += onSplashAdClose;
        ATSplashAd.Instance.client.onAdShowEvent += onSplashAdShow;
        ATSplashAd.Instance.client.onAdLoadTimeoutEvent += onSplashAdLoadTimeout;
        ATSplashAd.Instance.client.onAdLoadFailureEvent += onSplashAdLoadFailed;
    }

    public override void destroyAd()
    {
        ATSplashAd.Instance.client.onAdLoadEvent -= onSplashAdLoad;
        ATSplashAd.Instance.client.onAdCloseEvent -= onSplashAdClose;
        ATSplashAd.Instance.client.onAdShowEvent -= onSplashAdShow;
        ATSplashAd.Instance.client.onAdLoadTimeoutEvent -= onSplashAdLoadTimeout;
        ATSplashAd.Instance.client.onAdLoadFailureEvent -= onSplashAdLoadFailed;
    }

    public override void showAd() 
    {
        if (ATSplashAd.Instance.hasSplashAdReady(SPLASH_PLACEMENT_ID))
        {
            ATSplashAd.Instance.showSplashAd(SPLASH_PLACEMENT_ID, new Dictionary<string, object>());
        }
        else
        {
            setAdReadyStatus(false);
            loadAd();
        }
    }

    public override void loadAd()
    {
        setLoading();
        ATSplashAd.Instance.loadSplashAd(SPLASH_PLACEMENT_ID, new Dictionary<string, object>());
    }

    public void onSplashAdLoad(object sender, ATAdEventArgs arg)
    {
        Debug.Log("Splash::onSplashAdLoad() >>> " + arg.placementId);
        setLoadSuccess();
        showAd();
    }

    public void onSplashAdClose(object sender, ATAdEventArgs arg) 
    {
        Debug.Log("Splash::onSplashAdClose() >>> " + arg.placementId);
    }

    public void onSplashAdShow(object sender, ATAdEventArgs arg) 
    {
        Debug.Log("Splash::onSplashAdShow() >>> " + arg.placementId);
    }

    public void onSplashAdLoadTimeout(object sender, ATAdEventArgs arg)
    {
         Debug.Log("Splash::onSplashAdLoadTimeout() >>> " + arg.placementId);
    }

    public void onSplashAdLoadFailed(object sender, ATAdErrorEventArgs args)
    {
        Debug.Log("Splash::onSplashAdLoadFailed() >>> " + args.placementId);
        setLoadFailed(args);
        // Splash ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
        retryAdAttempt();
    }
}