package com.anythink.unitybridge.nativead;
import com.anythink.unitybridge.base.BaseAdListener;

public interface NativeListener extends BaseAdListener {

    public void onAdImpressed(String unitId, String callbackJson);

    public void onAdClicked(String unitId, String callbackJson);

    public void onAdVideoStart(String unitId);

    public void onAdVideoEnd(String unitId);

    public void onAdVideoProgress(String unitId, int progress);


    public void onNativeAdLoaded(String unitId);

    public void onNativeAdLoadFail(String unitId, String code, String msg);

    public void onAdCloseButtonClicked(String unitId, String callbackJson);
}
