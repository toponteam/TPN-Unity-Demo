package com.anythink.unitybridge.splash;

import android.app.Activity;
import android.content.Context;
import android.text.TextUtils;
import android.util.Log;
import android.view.ViewGroup;
import android.widget.FrameLayout;

import com.anythink.core.api.ATAdInfo;
import com.anythink.core.api.ATAdSourceStatusListener;
import com.anythink.core.api.ATAdStatusInfo;
import com.anythink.core.api.ATNetworkConfirmInfo;
import com.anythink.core.api.ATSDK;
import com.anythink.core.api.AdError;
import com.anythink.splashad.api.ATSplashAd;
import com.anythink.splashad.api.ATSplashAdExtraInfo;
import com.anythink.splashad.api.ATSplashExListener;
import com.anythink.unitybridge.MsgTools;
import com.anythink.unitybridge.UnityPluginUtils;
import com.anythink.unitybridge.download.DownloadHelper;
import com.anythink.unitybridge.utils.Const;
import com.anythink.unitybridge.utils.TaskManager;

import org.json.JSONArray;
import org.json.JSONObject;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * Description:
 * Created by Quin on 2023/4/24.
 **/
public class SplashHelper {
    private static final String TAG = SplashHelper.class.getSimpleName() + ": ";
    private Activity mActivity;
    private Activity mShowActivity;
    private SplashListener mSplashListener;
    private ATSplashAd mSplashAd;
    private String mPlacementId;
    private ViewGroup mDecorView;
    private ViewGroup mAdContainer;

    public SplashHelper(SplashListener splashListener) {
        MsgTools.printMsg("SplashHelper: " + this);
        if (splashListener == null) {
            MsgTools.printMsg("Listener == null: ");
        }
        mSplashListener = new SynSplashListener(splashListener);
        mActivity = UnityPluginUtils.getActivity("SplashHelper");
    }

    public void initSplash(String placementId, int fetchAdTimeout, String defaultAdSourceConfig) {
        this.mPlacementId = placementId;
        mSplashAd = new ATSplashAd(mActivity, placementId, new ATSplashExListener() {
            @Override
            public void onDeeplinkCallback(ATAdInfo atAdInfo, boolean b) {
                if (mSplashListener != null) {
                    mSplashListener.onSplashAdDeeplinkCallback(mPlacementId, formatATAdInfo(atAdInfo), b);
                }
            }

            @Override
            public void onDownloadConfirm(Context context, ATAdInfo atAdInfo, ATNetworkConfirmInfo atNetworkConfirmInfo) {
                if (mSplashListener != null) {
                    mSplashListener.onSplashAdDownloadConfirm(mPlacementId, formatATAdInfo(atAdInfo));
                }
            }

            @Override
            public void onAdLoaded(boolean b) {
                if (mSplashListener != null) {
                    mSplashListener.onSplashAdLoad(mPlacementId, b);
                }
            }

            @Override
            public void onAdLoadTimeout() {
                if (mSplashListener != null) {
                    mSplashListener.onSplashAdLoadTimeOut(mPlacementId);
                }
            }

            @Override
            public void onNoAdError(AdError adError) {
                if (mSplashListener != null) {
                    mSplashListener.onSplashAdLoadFailed(mPlacementId, adError != null ? adError.getCode() : "", adError != null ? adError.toString() : "");
                }
            }

            @Override
            public void onAdShow(ATAdInfo atAdInfo) {
                if (mSplashListener != null) {
                    mSplashListener.onSplashAdShow(mPlacementId, formatATAdInfo(atAdInfo));
                }
            }

            @Override
            public void onAdClick(ATAdInfo atAdInfo) {
                if (mSplashListener != null) {
                    mSplashListener.onSplashAdClick(mPlacementId, formatATAdInfo(atAdInfo));
                }
            }

            @Override
            public void onAdDismiss(ATAdInfo atAdInfo, ATSplashAdExtraInfo atSplashAdExtraInfo) {
                if (mDecorView != null && mAdContainer != null) {
                    mAdContainer.removeAllViews();
                    mDecorView.removeView(mAdContainer);
                }
                if (mSplashListener != null) {
                    mSplashListener.onSplashAdDismiss(mPlacementId, formatATAdInfo(atAdInfo));
                }
            }
        }, fetchAdTimeout, defaultAdSourceConfig);

        mSplashAd.setAdSourceStatusListener(new ATAdSourceStatusListener() {
            @Override
            public void onAdSourceBiddingAttempt(ATAdInfo atAdInfo) {
                if (mSplashListener != null) {
                    mSplashListener.onAdSourceBiddingAttempt(mPlacementId, formatATAdInfo(atAdInfo));
                }
            }

            @Override
            public void onAdSourceBiddingFilled(ATAdInfo atAdInfo) {
                if (mSplashListener != null) {
                    mSplashListener.onAdSourceBiddingFilled(mPlacementId, formatATAdInfo(atAdInfo));
                }
            }

            @Override
            public void onAdSourceBiddingFail(ATAdInfo atAdInfo, AdError adError) {
                if (mSplashListener != null) {
                    final String code = adError != null ? adError.getCode() : "";
                    final String msg = adError != null ? adError.getFullErrorInfo() : "";
                    mSplashListener.onAdSourceBiddingFail(mPlacementId, formatATAdInfo(atAdInfo), code, msg);
                }
            }

            @Override
            public void onAdSourceAttempt(ATAdInfo atAdInfo) {
                if (mSplashListener != null) {
                    mSplashListener.onAdSourceAttempt(mPlacementId, formatATAdInfo(atAdInfo));
                }
            }

            @Override
            public void onAdSourceLoadFilled(ATAdInfo atAdInfo) {
                if (mSplashListener != null) {
                    mSplashListener.onAdSourceLoadFilled(mPlacementId, formatATAdInfo(atAdInfo));
                }
            }

            @Override
            public void onAdSourceLoadFail(ATAdInfo atAdInfo, AdError adError) {
                if (mSplashListener != null) {
                    final String code = adError != null ? adError.getCode() : "";
                    final String msg = adError != null ? adError.getFullErrorInfo() : "";
                    mSplashListener.onAdSourceLoadFail(mPlacementId, formatATAdInfo(atAdInfo), code, msg);
                }
            }
        });
        MsgTools.printMsg(TAG + "initSplash() >>> called");

        try {
            if (ATSDK.isCnSDK()) {
                mSplashAd.setAdDownloadListener(DownloadHelper.getDownloadListener(mPlacementId));
            }
        } catch (Throwable e) {
            MsgTools.printMsg(TAG + "mSplashAd.setAdDownloadListener() failed: " + e.getMessage());
        }
    }

    public void loadAd(final String jsonMap) {
        MsgTools.printMsg(TAG + "loadAd() >>> mPlacementId: " + mPlacementId + ", jsonMap: " + jsonMap);
        if (!TextUtils.isEmpty(jsonMap)) {
            try {
                Map<String, Object> localExtra = new HashMap<>();
                JSONObject jsonObject = new JSONObject(jsonMap);
                Const.fillMapFromJsonObject(localExtra, jsonObject);
                if (mSplashAd != null) {
                    mSplashAd.setLocalExtra(localExtra);
                }
            } catch (Exception e) {
                MsgTools.printMsg(TAG + "loadAd() >>> failed: " + e.getMessage());
            }
        }
        UnityPluginUtils.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                if (mSplashAd != null) {
                    mSplashAd.loadAd();
                } else {
                    Log.e(TAG, "loadAd error, you must call initSplash first " + this);
                    TaskManager.getInstance().run_proxy(new Runnable() {
                        @Override
                        public void run() {
                            if (mSplashListener != null) {
                                synchronized (SplashHelper.this) {
                                    mSplashListener.onSplashAdLoadFailed(mPlacementId, "-1", "you must call initSplash first ..");
                                }
                            }
                        }
                    });
                }
            }
        });
    }

    public void showAd(final String jsonMap) {
        MsgTools.printMsg(TAG + "showAd() >>> jsonMap: " + jsonMap + " mShowActivity: " + mShowActivity);
        if (mSplashAd == null) {
            MsgTools.printMsg(TAG + "showAd failed, you must call initSplash first.");
            TaskManager.getInstance().run_proxy(new Runnable() {
                @Override
                public void run() {
                    if (mSplashListener != null) {
                        synchronized (SplashHelper.this) {
                            mSplashListener.onSplashAdLoadFailed(mPlacementId, "-1", "you must call initSplash first.");
                        }
                    }
                }
            });
            return;
        }
        UnityPluginUtils.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                String scenario = "";
                if (!TextUtils.isEmpty(jsonMap)) {
                    try {
                        JSONObject _jsonObject = new JSONObject(jsonMap);
                        if (_jsonObject.has(Const.SCENARIO)) {
                            scenario = _jsonObject.optString(Const.SCENARIO);
                        }
                    } catch (Exception e) {
                        if (Const.DEBUG) {
                            e.printStackTrace();
                        }
                    }
                }
                MsgTools.printMsg(TAG + "showAd scenario: " + scenario);
                try {
                    mShowActivity = UnityPluginUtils.getActivity("SplashHelper");
                    mDecorView = mShowActivity.findViewById(android.R.id.content);
                    if (mAdContainer == null) {
                        mAdContainer = new FrameLayout(mShowActivity);
                        mAdContainer.setLayoutParams(new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.MATCH_PARENT));
                    }
                    mAdContainer.removeAllViews();
                    if (mDecorView != null) {
                        mDecorView.removeView(mAdContainer);
                        mDecorView.addView(mAdContainer);
                    }
                    if (!TextUtils.isEmpty(scenario)) {
                        mSplashAd.show(mShowActivity, mAdContainer, scenario);
                    } else {
                        mSplashAd.show(mShowActivity, mAdContainer);
                    }
                } catch (Exception e) {
                    MsgTools.printMsg(TAG + "showAd failed: " + e.getMessage());
                }
            }
        });
    }

    public boolean isAdReady() {
        boolean result = mSplashAd != null && mSplashAd.isAdReady();
        MsgTools.printMsg(TAG + "isAdReady() >>> mPlacementId: " + mPlacementId + " isAdReady: " +result);
        return result;
    }

    public String checkAdStatus() {
        MsgTools.printMsg(TAG + "checkAdStatus: " + mPlacementId);
        if (mSplashAd != null) {
            ATAdStatusInfo atAdStatusInfo = mSplashAd.checkAdStatus();
            boolean loading = atAdStatusInfo.isLoading();
            boolean ready = atAdStatusInfo.isReady();
            ATAdInfo atTopAdInfo = atAdStatusInfo.getATTopAdInfo();

            try {
                JSONObject jsonObject = new JSONObject();
                jsonObject.put("isLoading", loading);
                jsonObject.put("isReady", ready);
                try {
                    if (atTopAdInfo != null) {
                        jsonObject.put("adInfo", new JSONObject(atTopAdInfo.toString()));
                    }
                } catch (Throwable e) {
                    e.printStackTrace();
                }
                MsgTools.printMsg("checkAdStatus: result = " + jsonObject);

                return jsonObject.toString();
            } catch (Throwable e) {
                e.printStackTrace();
            }
        }
        return "";
    }

    public String getValidAdCaches() {
        MsgTools.printMsg(TAG + "getValidAdCaches:" + mPlacementId);

        if (mSplashAd != null) {
            JSONArray jsonArray = new JSONArray();

            List<ATAdInfo> vaildAds = mSplashAd.checkValidAdCaches();
            if (vaildAds == null) {
                return "";
            }

            int size = vaildAds.size();

            for (int i = 0; i < size; i++) {
                try {
                    jsonArray.put(new JSONObject(vaildAds.get(i).toString()));
                } catch (Throwable e) {
                    e.printStackTrace();
                }
            }
            return jsonArray.toString();
        }
        return "";
    }

    public void entryAdScenario(final String scenarioId) {
        MsgTools.printMsg(TAG + "entryAdScenario mPlacementId: " + mPlacementId + ", scenarioId: " + scenarioId);
        UnityPluginUtils.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                if (!TextUtils.isEmpty(mPlacementId)) {
                    ATSplashAd.entryAdScenario(mPlacementId, scenarioId);
                } else {
                    MsgTools.printMsg("entryAdScenario error, you must call initSplash first " + mPlacementId);
                }
            }
        });
    }

    private String formatATAdInfo(ATAdInfo atAdInfo) {
        return atAdInfo != null ? atAdInfo.toString() : "";
    }

    private static class SynSplashListener implements SplashListener {
        private final Object lockObj = new Object();
        private final SplashListener splashListener;

        public SynSplashListener(SplashListener splashListener) {
            this.splashListener = splashListener;
        }

        private void runOnThread(Runnable runnable) {
            TaskManager.getInstance().run_proxy(runnable);
        }

        @Override
        public void onSplashAdLoad(final String unitId, final boolean isTimeout) {
            if (splashListener != null) {
                runOnThread(new Runnable() {
                    @Override
                    public void run() {
                        synchronized (lockObj) {
                            splashListener.onSplashAdLoad(unitId, isTimeout);
                        }
                    }
                });
            }
        }

        @Override
        public void onSplashAdLoadTimeOut(final String unitId) {
            if (splashListener != null) {
                runOnThread(new Runnable() {
                    @Override
                    public void run() {
                        synchronized (lockObj) {
                            splashListener.onSplashAdLoadTimeOut(unitId);
                        }
                    }
                });
            }
        }

        @Override
        public void onSplashAdLoadFailed(final String unitId, final String code, final String msg) {
            if (splashListener != null) {
                runOnThread(new Runnable() {
                    @Override
                    public void run() {
                        synchronized (lockObj) {
                            splashListener.onSplashAdLoadFailed(unitId, code, msg);
                        }
                    }
                });
            }
        }

        @Override
        public void onSplashAdShow(final String unitId, final String callbackJson) {
            if (splashListener != null) {
                runOnThread(new Runnable() {
                    @Override
                    public void run() {
                        synchronized (lockObj) {
                            splashListener.onSplashAdShow(unitId, callbackJson);
                        }
                    }
                });
            }
        }

        @Override
        public void onSplashAdClick(final String unitId, final String callbackJson) {
            if (splashListener != null) {
                runOnThread(new Runnable() {
                    @Override
                    public void run() {
                        synchronized (lockObj) {
                            splashListener.onSplashAdClick(unitId, callbackJson);
                        }
                    }
                });
            }
        }

        @Override
        public void onSplashAdDismiss(final String unitId, final String callbackJson) {
            if (splashListener != null) {
                runOnThread(new Runnable() {
                    @Override
                    public void run() {
                        synchronized (lockObj) {
                            splashListener.onSplashAdDismiss(unitId, callbackJson);
                        }
                    }
                });
            }
        }

        @Override
        public void onSplashAdDeeplinkCallback(final String unitId, final String callbackJson, final boolean isSuccess) {
            if (splashListener != null) {
                runOnThread(new Runnable() {
                    @Override
                    public void run() {
                        synchronized (lockObj) {
                            splashListener.onSplashAdDeeplinkCallback(unitId, callbackJson, isSuccess);
                        }
                    }
                });
            }
        }

        @Override
        public void onSplashAdDownloadConfirm(final String unitId, final String callbackJson) {
            if (splashListener != null) {
                runOnThread(new Runnable() {
                    @Override
                    public void run() {
                        synchronized (lockObj) {
                            splashListener.onSplashAdDownloadConfirm(unitId, callbackJson);
                        }
                    }
                });
            }
        }

        @Override
        public void onAdSourceBiddingAttempt(final String unitId, final String callbackJson) {
            if (splashListener != null) {
                runOnThread(new Runnable() {
                    @Override
                    public void run() {
                        synchronized (lockObj) {
                            splashListener.onAdSourceBiddingAttempt(unitId, callbackJson);
                        }
                    }
                });
            }
        }

        @Override
        public void onAdSourceBiddingFilled(final String unitId, final String callbackJson) {
            if (splashListener != null) {
                runOnThread(new Runnable() {
                    @Override
                    public void run() {
                        synchronized (lockObj) {
                            splashListener.onAdSourceBiddingFilled(unitId, callbackJson);
                        }
                    }
                });
            }
        }

        @Override
        public void onAdSourceBiddingFail(final String unitId, final String callbackJson, final String code, final String error) {
            if (splashListener != null) {
                runOnThread(new Runnable() {
                    @Override
                    public void run() {
                        synchronized (lockObj) {
                            splashListener.onAdSourceBiddingFail(unitId, callbackJson, code, error);
                        }
                    }
                });
            }
        }

        @Override
        public void onAdSourceAttempt(final String unitId, final String callbackJson) {
            if (splashListener != null) {
                runOnThread(new Runnable() {
                    @Override
                    public void run() {
                        synchronized (lockObj) {
                            splashListener.onAdSourceAttempt(unitId, callbackJson);
                        }
                    }
                });
            }
        }

        @Override
        public void onAdSourceLoadFilled(final String unitId, final String callbackJson) {
            if (splashListener != null) {
                runOnThread(new Runnable() {
                    @Override
                    public void run() {
                        synchronized (lockObj) {
                            splashListener.onAdSourceLoadFilled(unitId, callbackJson);
                        }
                    }
                });
            }
        }

        @Override
        public void onAdSourceLoadFail(final String unitId, final String callbackJson, final String code, final String error) {
            if (splashListener != null) {
                runOnThread(new Runnable() {
                    @Override
                    public void run() {
                        synchronized (lockObj) {
                            splashListener.onAdSourceLoadFail(unitId, callbackJson, code, error);
                        }
                    }
                });
            }
        }
    }
}
