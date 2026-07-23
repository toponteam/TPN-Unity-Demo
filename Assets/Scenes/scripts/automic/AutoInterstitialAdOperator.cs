using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AnyThinkAds.Api;
using UnityEngine.UI;
using System;

using AnyThinkAds.ThirdParty.LitJson;

public class AutoInterstitialAdOperator : BaseAdOperator
{
#if UNITY_ANDROID
    static string mPlacementId_interstitial_all = "b5baca53984692";
    static string showingScenario = "";

#elif UNITY_IOS || UNITY_IPHONE
    static string mPlacementId_interstitial_all = "b5bacad26a752a";
    static string showingScenario = "f5e549727efc49";

#endif

    private static readonly AutoInterstitialAdOperator instance = new AutoInterstitialAdOperator();

    private AutoInterstitialAdOperator() 
	{
        
	}

	public static AutoInterstitialAdOperator Instance 
	{
        get 
		{
			return instance;
		}
	}

    public override void initializeAd() 
    {
        ATInterstitialAd.Instance.client.onAdLoadEvent += onAdLoad;
        ATInterstitialAd.Instance.client.onAdClickEvent += onAdClick;
        ATInterstitialAd.Instance.client.onAdCloseEvent += onAdClose;
        ATInterstitialAd.Instance.client.onAdShowEvent += onShow;
        ATInterstitialAd.Instance.client.onAdLoadFailureEvent += onAdLoadFail;
        ATInterstitialAd.Instance.client.onAdShowFailureEvent += onAdShowFail;
        ATInterstitialAd.Instance.client.onAdVideoStartEvent += onAdVideoStart;
        ATInterstitialAd.Instance.client.onAdVideoEndEvent += onAdVideoEnd;
        ATInterstitialAd.Instance.client.onAdVideoFailureEvent += onAdVideoFailure;
        ATInterstitialAd.Instance.client.onAdSourceAttemptEvent += startLoadingADSource;
        ATInterstitialAd.Instance.client.onAdSourceFilledEvent += finishLoadingADSource;
        ATInterstitialAd.Instance.client.onAdSourceLoadFailureEvent += failToLoadADSource;
        ATInterstitialAd.Instance.client.onAdSourceBiddingAttemptEvent += startBiddingADSource;
        ATInterstitialAd.Instance.client.onAdSourceBiddingFilledEvent += finishBiddingADSource;
        ATInterstitialAd.Instance.client.onAdSourceBiddingFailureEvent += failBiddingADSource;

        setLoading();
        loadAd();
    }

    public override void destroyAd() 
    {
        ATInterstitialAd.Instance.client.onAdLoadEvent -= onAdLoad;
        ATInterstitialAd.Instance.client.onAdClickEvent -= onAdClick;
        ATInterstitialAd.Instance.client.onAdCloseEvent -= onAdClose;
        ATInterstitialAd.Instance.client.onAdShowEvent -= onShow;
        ATInterstitialAd.Instance.client.onAdLoadFailureEvent -= onAdLoadFail;
        ATInterstitialAd.Instance.client.onAdShowFailureEvent -= onAdShowFail;
        ATInterstitialAd.Instance.client.onAdVideoStartEvent -= onAdVideoStart;
        ATInterstitialAd.Instance.client.onAdVideoEndEvent -= onAdVideoEnd;
        ATInterstitialAd.Instance.client.onAdVideoFailureEvent -= onAdVideoFailure;
        ATInterstitialAd.Instance.client.onAdSourceAttemptEvent -= startLoadingADSource;
        ATInterstitialAd.Instance.client.onAdSourceFilledEvent -= finishLoadingADSource;
        ATInterstitialAd.Instance.client.onAdSourceLoadFailureEvent -= failToLoadADSource;
        ATInterstitialAd.Instance.client.onAdSourceBiddingAttemptEvent -= startBiddingADSource;
        ATInterstitialAd.Instance.client.onAdSourceBiddingFilledEvent -= finishBiddingADSource;
        ATInterstitialAd.Instance.client.onAdSourceBiddingFailureEvent -= failBiddingADSource;
    }

    public override void loadAd()
    {
        Dictionary<string, object> jsonmap = new Dictionary<string, object>();
        jsonmap.Add(ATConst.USE_REWARDED_VIDEO_AS_INTERSTITIAL, ATConst.USE_REWARDED_VIDEO_AS_INTERSTITIAL_NO);
        setLoading();
        ATInterstitialAd.Instance.loadInterstitialAd(mPlacementId_interstitial_all, jsonmap);
    }

    public override void showAd()
    {
        bool isAdReady = ATInterstitialAd.Instance.hasInterstitialAdReady(mPlacementId_interstitial_all);
        if (isAdReady) {
            Dictionary<string, string> jsonmap = new Dictionary<string, string>();
            jsonmap.Add(AnyThinkAds.Api.ATConst.SCENARIO, showingScenario);
            ATInterstitialAd.Instance.showInterstitialAd(mPlacementId_interstitial_all, jsonmap);
        } else {
            setAdReadyStatus(false);
            loadAd();
        }
    }

    public void onAdLoad(object sender, ATAdEventArgs erg)
    {
        Debug.Log("Developer callback onAdLoad :" + erg.placementId);
        setLoadSuccess();
    }

    public void onAdLoadFail(object sender, ATAdErrorEventArgs erg)
    {
        Debug.Log("Developer callback onAdLoadFail : : " + erg.placementId + "--erg.errorCode:" + erg.errorCode + "--msg:" + erg.errorMessage);
        setLoadFailed(erg);
        retryAdAttempt();
    }

    public void onAdClick(object sender, ATAdEventArgs erg)
    {
        Debug.Log("Developer callback onAdClick :" + erg.placementId + "->" + JsonMapper.ToJson(erg.callbackInfo.toDictionary()));
    }

    public void onAdClose(object sender, ATAdEventArgs erg)
    {
        Debug.Log("Developer callback onAdClose :" + erg.placementId);
        loadAd();
    }

    public void onShow(object sender, ATAdEventArgs erg)
    {
        Debug.Log("Developer callback onShow :" + erg.placementId);
    }

    public void onAdShowFail(object sender, ATAdErrorEventArgs erg)
    {
        Debug.Log("Developer callback show fail :" + erg.placementId);
        loadAd();
    }

    public void onAdVideoStart(object sender, ATAdEventArgs erg)
    {
        Debug.Log("Developer callback onAdVideoStart :" + erg.placementId);
    }

    public void onAdVideoEnd(object sender, ATAdEventArgs erg)
    {
        Debug.Log("Developer callback onAdVideoEnd :" + erg.placementId);
    }

    public void onAdVideoFailure(object sender, ATAdEventArgs erg)
    {
        Debug.Log("Developer callback onAdVideoFailure :" + erg.placementId);
    }

    // AdSource Listener
    public void startLoadingADSource(object sender, ATAdEventArgs erg)
    {
        Debug.Log("Developer startLoadingADSource------" + "->" + JsonMapper.ToJson(erg.callbackInfo.toAdsourceDictionary()));
    }

    public void finishLoadingADSource(object sender, ATAdEventArgs erg)
    {
        Debug.Log("Developer finishLoadingADSource------" + "->" + JsonMapper.ToJson(erg.callbackInfo.toAdsourceDictionary()));
    }

    public void failToLoadADSource(object sender, ATAdErrorEventArgs erg)
    {
        Debug.Log("Developer failToLoadADSource------erg.errorCode:" + erg.errorCode + "---erg.errorMessage:" + erg.errorMessage + "->" + JsonMapper.ToJson(erg.callbackInfo.toAdsourceDictionary()));
    }

    public void startBiddingADSource(object sender, ATAdEventArgs erg)
    {
        Debug.Log("Developer startBiddingADSource------" + "->" + JsonMapper.ToJson(erg.callbackInfo.toAdsourceDictionary()));
    }

    public void finishBiddingADSource(object sender, ATAdEventArgs erg)
    {
        Debug.Log("Developer finishBiddingADSource------" + "->" + JsonMapper.ToJson(erg.callbackInfo.toAdsourceDictionary()));
    }

    public void failBiddingADSource(object sender, ATAdErrorEventArgs erg)
    {
        Debug.Log("Developer failBiddingADSource------erg.errorCode:" + erg.errorCode + "---erg.errorMessage:" + erg.errorMessage + "->" + JsonMapper.ToJson(erg.callbackInfo.toAdsourceDictionary()));
    }
}
