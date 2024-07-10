using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AnyThinkAds.Api;
using UnityEngine.UI;
using System;

using AnyThinkAds.ThirdParty.LitJson;

public class AutoRewardVideoAdOperator : BaseAdOperator {
#if UNITY_ANDROID
    static string mPlacementId_rewardvideo_all = "b5b449f025ec7c";  //admob
    static string showingScenario = "f5e71c46d1a28f";
#elif UNITY_IOS || UNITY_IPHONE
    static string mPlacementId_rewardvideo_all = "b5b44a0f115321";//"b5b44a0f115321";
    static string showingScenario = "f5e54970dc84e6";

#endif

    private static readonly AutoRewardVideoAdOperator instance = new AutoRewardVideoAdOperator();

    private AutoRewardVideoAdOperator() 
	{
        
	}

	public static AutoRewardVideoAdOperator Instance 
	{
        get 
		{
			return instance;
		}
	}

    public override void initializeAd() 
    {
        ATRewardedAutoVideo.Instance.client.onAdLoadEvent += onAdLoad;
        ATRewardedAutoVideo.Instance.client.onAdLoadFailureEvent += onAdLoadFail;
        ATRewardedAutoVideo.Instance.client.onAdVideoStartEvent += onAdVideoStart;
        ATRewardedAutoVideo.Instance.client.onAdVideoEndEvent += onAdVideoEnd;
        ATRewardedAutoVideo.Instance.client.onAdVideoFailureEvent += onAdVideoPlayFail;
        ATRewardedAutoVideo.Instance.client.onAdClickEvent += onAdClick;
        ATRewardedAutoVideo.Instance.client.onRewardEvent += onReward;
        ATRewardedAutoVideo.Instance.client.onAdVideoCloseEvent += onAdVideoClosed;

        string[] jsonList = { mPlacementId_rewardvideo_all };
        setLoading();
        ATRewardedAutoVideo.Instance.addAutoLoadAdPlacementID(jsonList);
    }

    public override void destroyAd()
    {
        ATRewardedAutoVideo.Instance.client.onAdLoadEvent -= onAdLoad;
        ATRewardedAutoVideo.Instance.client.onAdLoadFailureEvent -= onAdLoadFail;
        ATRewardedAutoVideo.Instance.client.onAdVideoStartEvent -= onAdVideoStart;
        ATRewardedAutoVideo.Instance.client.onAdVideoEndEvent -= onAdVideoEnd;
        ATRewardedAutoVideo.Instance.client.onAdVideoFailureEvent -= onAdVideoPlayFail;
        ATRewardedAutoVideo.Instance.client.onAdClickEvent -= onAdClick;
        ATRewardedAutoVideo.Instance.client.onRewardEvent -= onReward;
        ATRewardedAutoVideo.Instance.client.onAdVideoCloseEvent -= onAdVideoClosed;
        string[] jsonList = { mPlacementId_rewardvideo_all };
        ATRewardedAutoVideo.Instance.removeAutoLoadAdPlacementID(jsonList);
    }

    public override void showAd()
    {
        bool isAdReady = ATRewardedAutoVideo.Instance.autoLoadRewardedVideoReadyForPlacementID(mPlacementId_rewardvideo_all);
        if (isAdReady) {
            Dictionary<string, string> jsonmap = new Dictionary<string, string>();
            jsonmap.Add(AnyThinkAds.Api.ATConst.SCENARIO, showingScenario);

            // ATRewardedAutoVideo.Instance.showAutoAd(mPlacementId_rewardvideo_all);
            ATRewardedAutoVideo.Instance.showAutoAd(mPlacementId_rewardvideo_all, jsonmap);
        }
    }

    public override void loadAd(){}

    public void onAdLoad(object sender, ATAdEventArgs erg)
    {
        Debug.Log("Developer callback onAdLoad :" + erg.placementId);
        setLoadSuccess();
    }

    public void onAdClick(object sender, ATAdEventArgs erg)
    {
        Debug.Log("Developer callback onAdClick :" + erg.placementId + "->" + JsonMapper.ToJson(erg.callbackInfo.toDictionary()));
    }

    public void onAdClose(object sender, ATAdRewardEventArgs erg)
    {
        Debug.Log("Developer callback onAdClose :" + erg.placementId);
    }

    public void onAdLoadFail(object sender, ATAdErrorEventArgs erg)
    {
        Debug.Log("Developer callback onAdLoadFail : : " + erg.placementId + "--erg.errorCode:" + erg.errorCode + "--msg:" + erg.errorMessage);
        setLoadFailed(erg);
         //Auto Reward ad failed to load. SDK will automatically try loading a new ad internally.
    }

    public void onAdVideoStart(object sender, ATAdEventArgs erg)
    {
        Debug.Log("Developer onAdVideoStart------" + "->" + JsonMapper.ToJson(erg.callbackInfo.toDictionary()));
    }

    public void onAdVideoEnd(object sender, ATAdEventArgs erg)
    {
        Debug.Log("Developer onAdVideoEnd------" + "->" + JsonMapper.ToJson(erg.callbackInfo.toDictionary()));
    }

    public void onAdVideoClosed(object sender, ATAdEventArgs erg)
    {
        Debug.Log("Developer onAdVideoClosed------" + "->" + JsonMapper.ToJson(erg.callbackInfo.toDictionary()));
    }

    public void onAdVideoPlayFail(object sender, ATAdErrorEventArgs erg)
    {
        Debug.Log("Developer onAdVideoPlayFail------" + "->" + JsonMapper.ToJson(erg.errorMessage));
        //Auto Reward ad failed to show. SDK will automatically try loading a new ad internally.
    }

    public void onReward(object sender, ATAdEventArgs erg)
    {
        Debug.Log("Developer onReward------" + "->" + JsonMapper.ToJson(erg.callbackInfo.toDictionary()));
    }
}