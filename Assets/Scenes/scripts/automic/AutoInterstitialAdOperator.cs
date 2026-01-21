using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AnyThinkAds.Api;
using UnityEngine.UI;
using System;

using AnyThinkAds.ThirdParty.LitJson;

public class AutoInterstitialAdOperator : BaseAdOperator{
#if UNITY_ANDROID
    static string mPlacementId_interstitial_all = "b6602833ace122";
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
        ATInterstitialAutoAd.Instance.client.onAdLoadEvent += onAdLoad;
        ATInterstitialAutoAd.Instance.client.onAdClickEvent += onAdClick;
        ATInterstitialAutoAd.Instance.client.onAdCloseEvent += onAdClose;
        ATInterstitialAutoAd.Instance.client.onAdShowEvent += onShow;
        ATInterstitialAutoAd.Instance.client.onAdLoadFailureEvent += onAdLoadFail;
        ATInterstitialAutoAd.Instance.client.onAdShowFailureEvent += onAdShowFail;
        ATInterstitialAutoAd.Instance.client.onAdVideoStartEvent += onAdVideoStart;
        ATInterstitialAutoAd.Instance.client.onAdVideoEndEvent += onAdVideoEnd;
        ATInterstitialAutoAd.Instance.client.onAdVideoFailureEvent += onAdVideoFailure;
        //ATInterstitialAutoAd No AdSource Event
        string[] jsonList = { mPlacementId_interstitial_all };

        setLoading();
        ATInterstitialAutoAd.Instance.addAutoLoadAdPlacementID(jsonList);
    }

    public override void destroyAd() 
    {
        ATInterstitialAutoAd.Instance.client.onAdLoadEvent -= onAdLoad;
        ATInterstitialAutoAd.Instance.client.onAdClickEvent -= onAdClick;
        ATInterstitialAutoAd.Instance.client.onAdCloseEvent -= onAdClose;
        ATInterstitialAutoAd.Instance.client.onAdShowEvent -= onShow;
        ATInterstitialAutoAd.Instance.client.onAdLoadFailureEvent -= onAdLoadFail;
        ATInterstitialAutoAd.Instance.client.onAdShowFailureEvent -= onAdShowFail;
        ATInterstitialAutoAd.Instance.client.onAdVideoStartEvent -= onAdVideoStart;
        ATInterstitialAutoAd.Instance.client.onAdVideoEndEvent -= onAdVideoEnd;
        ATInterstitialAutoAd.Instance.client.onAdVideoFailureEvent -= onAdVideoFailure;

        string[] jsonList = { mPlacementId_interstitial_all };
        ATInterstitialAutoAd.Instance.removeAutoLoadAdPlacementID(jsonList);
    }

    public override void loadAd(){}

    public override void showAd()
    {
        Dictionary<string, string> jsonmap = new Dictionary<string, string>();
        jsonmap.Add(AnyThinkAds.Api.ATConst.SCENARIO, showingScenario);

        ATInterstitialAutoAd.Instance.showAutoAd(mPlacementId_interstitial_all, jsonmap);
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
        //Auto Interestitial ad failed to load. SDK will automatically try loading a new ad internally.
    }


    public void onAdClick(object sender, ATAdEventArgs erg)
    {
        Debug.Log("Developer callback onAdClick :" + erg.placementId + "->" + JsonMapper.ToJson(erg.callbackInfo.toDictionary()));
    }

    public void onAdClose(object sender, ATAdEventArgs erg)
    {
        Debug.Log("Developer callback onAdClose :" + erg.placementId);
    }

    public void onShow(object sender, ATAdEventArgs erg)
    {
        Debug.Log("Developer callback onShow :" + erg.placementId);
    }

    public void onAdShowFail(object sender, ATAdErrorEventArgs erg)
    {
        Debug.Log("Developer callback show fail :" + erg.placementId);
        //Auto Interestitial ad failed to show. SDK will automatically try loading a new ad internally.
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
}