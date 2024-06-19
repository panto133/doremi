using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Advertisements;
using TMPro;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    public AfterDeath afterDeath;
    public TapGameLogic tapGame;
    public ConvertingLogic convertingLogic;

    string googlePlayId = "3698775";
    string myPlacementId = "rewardedVideo";
    bool testMode = false;

    //1 - continue after death
    //2 - booster power up
    //3 - earn 1 cesh
    //4 - earn 100 coins
    int rewardType = 0;

    TimeSpan adCooldown = TimeSpan.FromHours(12);

    public Animation errorAnimation;
    public Animation tooMuchAdsAnimation;
    public AnimationClip adsMenuClosing;
    public AnimationClip adsMenuOpening;

    public GameObject errorMenu;
    public GameObject tooMuchAdsMenu;

    private void Start()
    {
        Advertisement.AddListener(this);

        Advertisement.Initialize(googlePlayId, testMode);

        InvokeRepeating("CheckForCooldown", 0, 10f);
    }

    public void ShowAd()
    {
        try
        {
            Advertisement.Show();
        }
        catch
        {
            Debug.Log("Error showing interstitial ad");
        }
    }

    private void CheckForCooldown()
    {
        if (DateTime.Now - GameLogic.timeFirstAd >= adCooldown)
        {
            GameLogic.adsWatchedForTheDay = 0;
        }
    }

    public void ShowRewardedVideo()
    {
        try
        {
            // Check if UnityAds ready before calling Show method:
            if (rewardType > 1 && GameLogic.adsWatchedForTheDay >= 7)
            {
                OpenTooMuchAdsMenu();
                return;
            }
            if (GameLogic.adsWatchedForTheDay == 0)
            {
                GameLogic.timeFirstAd = DateTime.Now;
            }
            if (Advertisement.IsReady(myPlacementId))
            {
                Advertisement.Show(myPlacementId);
            }
            else
            {
                //sum ting wong
                OpenErrorMenu("Ads are not ready, try again later!");
            }
        }
        catch
        {
            //sum ting wong
            OpenErrorMenu("Ads are not ready, try again later.");
        }
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        try
        {
            // Define conditional logic for each ad completion status:
            if (showResult == ShowResult.Finished)
            {
                // Reward the user for watching the ad to completion.
                if (rewardType == 1)
                {
                    afterDeath.ContinueButton();
                }
                if (rewardType == 2)
                {
                    GameLogic.adsWatchedForTheDay++;
                    tapGame.BoosterWatchAdd();
                }
                if (rewardType == 3)
                {
                    GameLogic.adsWatchedForTheDay++;
                    convertingLogic.WatchAdForCesh();
                }
                if (rewardType == 4)
                {
                    GameLogic.adsWatchedForTheDay++;
                    convertingLogic.WatchAdForCoins();
                }
            }
            else if (showResult == ShowResult.Skipped)
            {
                // Do not reward the user for skipping the ad.
            }
            else if (showResult == ShowResult.Failed)
            {
                OpenErrorMenu("Ads failed for some reason. Try again later!");
            }
        }
        catch
        {
            OpenErrorMenu("Ads failed for some reason. Try again later.");
        }

    }

    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, show the ad:
        if (placementId == myPlacementId)
        {
            // Optional actions to take when the placement becomes ready(For example, enable the rewarded ads button)
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
        Debug.Log("Unity ads did error but idk where or when but they did error");
        OpenErrorMenu("We are experiencing difficulties - " + message + " Try again later!");
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }

    public void AdType(int type)
    {
        rewardType = type;
    }
    
    public void OpenErrorMenu(string message)
    {
        errorMenu.SetActive(true);
        errorMenu.transform.Find("Error Message").gameObject.GetComponent<TextMeshProUGUI>().text = message;
        errorAnimation.clip = adsMenuOpening;
        errorAnimation.Play();
    }  

    public void CloseErrorMenu()
    {
        StartCoroutine(ClosingErrorMenuAnimation());
    }

    IEnumerator ClosingErrorMenuAnimation()
    {
        errorAnimation.clip = adsMenuClosing;
        errorAnimation.Play();

        yield return new WaitForSeconds(0.3f);

        errorMenu.SetActive(false);
    }

    public void OpenTooMuchAdsMenu()
    {
        tooMuchAdsMenu.SetActive(true);
        tooMuchAdsAnimation.clip = adsMenuOpening;
        tooMuchAdsAnimation.Play();
    }

    public void CloseTooMuchAdsMenu()
    {
        StartCoroutine(ClosingTooMuchAdsMenu());

    }

    IEnumerator ClosingTooMuchAdsMenu()
    {
        tooMuchAdsAnimation.clip = adsMenuClosing;
        tooMuchAdsAnimation.Play();

        yield return new WaitForSeconds(0.3f);

        tooMuchAdsMenu.SetActive(false);
    }
}
