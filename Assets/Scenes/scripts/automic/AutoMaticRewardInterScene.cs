using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using AnyThinkAds.Api;
using AnyThinkAds.ThirdParty.LitJson;
public class AutoMaticRewardInterScene : MonoBehaviour
{

    public Button showInterstitialButton;
    public Button showRewardButton;
    public Button backButton;

    public Text interstitialStatusText;
    public Text rewardStatusText;

    // Start is called before the first frame update
    void Start()
    {
        showInterstitialButton.onClick.AddListener(ShowInterstitialAd);
        showRewardButton.onClick.AddListener(ShowRewardedAd);
        backButton.onClick.AddListener(BackToMainPage);

        InitializeAutoInterstitialAds();
        InitializeAutoRewardedAds();
    }

    void OnDestroy()
    {
        AutoInterstitialAdOperator.Instance.destroyAd();
        AutoInterstitialAdOperator.Instance.destroyAd();
    }

    private void BackToMainPage()
    {
        SceneManager.LoadScene("HomeScreenScene");
    }

    private void InitializeAutoInterstitialAds()
    {
        AutoInterstitialAdOperator.Instance.statusChangeEvent += statusChange;
        AutoInterstitialAdOperator.Instance.initializeAd();
    }

    private void InitializeAutoRewardedAds() 
    {
        AutoRewardVideoAdOperator.Instance.statusChangeEvent += statusChange;
        AutoRewardVideoAdOperator.Instance.initializeAd();
    }

    public void ShowInterstitialAd()
    {
        AutoInterstitialAdOperator.Instance.showAd();
    }

    public void ShowRewardedAd() 
    {
        AutoRewardVideoAdOperator.Instance.showAd();
    }

    public void statusChange(object selfer, string status)
    {
        Debug.Log("statusChange() >>> selfer: " + selfer + " status: " + status);
        if (Object.ReferenceEquals(selfer, AutoInterstitialAdOperator.Instance))
        {
            interstitialStatusText.text = status;
        } else if(Object.ReferenceEquals(selfer, AutoRewardVideoAdOperator.Instance)) {
            rewardStatusText.text = status;
        }
    }
    
}
