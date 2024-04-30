package com.anythink.unitybridge.videoad;
import com.anythink.unitybridge.base.BaseAdListener;

public interface VideoListener extends BaseAdListener {
    public void onRewardedVideoAdLoaded(String unitId);

    public void onRewardedVideoAdFailed(String unitId, String code, String error);

    public void onRewardedVideoAdPlayStart(String unitId, String callbackJson);

    public void onRewardedVideoAdPlayEnd(String unitId, String callbackJson);

    public void onRewardedVideoAdPlayFailed(String unitId, String code, String error);

    public void onRewardedVideoAdClosed(String unitId, boolean isRewarded, String callbackJson);

    public void onRewardedVideoAdPlayClicked(String unitId, String callbackJson);

    public void onReward(String unitId, String callbackJson);

//    -----------------Again

    public void onRewardedVideoAdAgainPlayStart(String unitId, String callbackJson);

    public void onRewardedVideoAdAgainPlayEnd(String unitId, String callbackJson);

    public void onRewardedVideoAdAgainPlayFailed(String unitId, String code, String error);

    public void onRewardedVideoAdAgainPlayClicked(String unitId, String callbackJson);

    public void onAgainReward(String unitId, String callbackJson);
}
