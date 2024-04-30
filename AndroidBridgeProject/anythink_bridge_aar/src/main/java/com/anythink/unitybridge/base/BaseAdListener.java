package com.anythink.unitybridge.base;

/**
 * Description:
 * Created by Quin on 2023/4/24.
 **/
public interface BaseAdListener {
    //    -----------------AdSource listener
    void onAdSourceBiddingAttempt(String unitId, String callbackJson);

    void onAdSourceBiddingFilled(String unitId, String callbackJson);

    void onAdSourceBiddingFail(String unitId, String callbackJson, String code, String error);

    void onAdSourceAttempt(String unitId, String callbackJson);

    void onAdSourceLoadFilled(String unitId, String callbackJson);

    void onAdSourceLoadFail(String unitId, String callbackJson, String code, String error);
}
