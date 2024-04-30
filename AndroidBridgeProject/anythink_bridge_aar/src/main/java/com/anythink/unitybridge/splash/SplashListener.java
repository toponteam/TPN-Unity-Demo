package com.anythink.unitybridge.splash;

import com.anythink.unitybridge.base.BaseAdListener;

/**
 * Description:
 * Created by Quin on 2023/4/24.
 **/
public interface SplashListener extends BaseAdListener {
    void onSplashAdLoad(String unitId, boolean isTimeout);

    void onSplashAdLoadTimeOut(String unitId);

    void onSplashAdLoadFailed(String unitId, String code, String msg);

    void onSplashAdShow(String unitId, String callbackJson);

    void onSplashAdClick(String unitId, String callbackJson);

    void onSplashAdDismiss(String unitId, String callbackJson);

    void onSplashAdDeeplinkCallback(String unitId, String callbackJson, boolean isSuccess);

    void onSplashAdDownloadConfirm(String unitId, String callbackJson);
}
