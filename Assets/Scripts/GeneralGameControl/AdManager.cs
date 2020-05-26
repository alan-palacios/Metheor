using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds;

public class AdManager : MonoBehaviour
{
    public GeneralGameControl generalGC;
    private static InterstitialAd interstitialAd;
    private static RewardedAd extraLifeAd;
    private static RewardedAd doubleCoinAd;

    private static int timer = 0;
    private static bool displayAd = false;
    public int timeBetweenAds;
     static bool giveExtraLife;
     static bool giveDoubleCoins;

    void Start()
    {

        //Debug.Log("id: "+SystemInfo.deviceUniqueIdentifier);
        MobileAds.Initialize(initStatus => { });

        /*#if UNITY_EDITOR
                string adUnitId = "ca-app-pub-3940256099942544/5224354917";
        #elif UNITY_ANDROID
                string adUnitId = "ca-app-pub-3940256099942544/5224354917";
        #elif UNITY_IPHONE
                string adUnitId = "ca-app-pub-3940256099942544/5224354917";
        #else
                string adUnitId = "unexpected_platform";
        #endif*/
        if (Purchaser.adsPersistance != null) {
            if (!Purchaser.adsPersistance.getAdsRemoved()) {
                if (interstitialAd == null) {
                    interstitialAd = RequestAndLoadInterstitialAd("ca-app-pub-3940256099942544/1033173712");
                }
            }
        }else{
            if (interstitialAd == null) {
                interstitialAd = RequestAndLoadInterstitialAd("ca-app-pub-3940256099942544/1033173712");
            }            
        }
        if (doubleCoinAd == null) {
            doubleCoinAd = RequestAndLoadRewardedAd("ca-app-pub-3940256099942544/5224354917", "DoubleCoins");
        }
        if (extraLifeAd == null) {
            extraLifeAd = RequestAndLoadRewardedAd("ca-app-pub-3940256099942544/5224354917", "ExtraLife");
        }

        StartCoroutine(Contar());
    }

    void Update(){

        if (giveExtraLife) {
            giveExtraLife = false;
            generalGC.ShowExtraLifeBtn();
        }

        if (giveDoubleCoins) {
            giveDoubleCoins = false;
            generalGC.ShowDoubleCoinsBtn();
        }
    }

    public RewardedAd RequestAndLoadRewardedAd(string adUnitId, string adName){
            Debug.Log("Requesting RewardedAd Ad.");
            RewardedAd ad = new RewardedAd(adUnitId);

            switch (adName) {
                case "DoubleCoins":
                    ad.OnUserEarnedReward += HandleUserEarnedCoins;
                    ad.OnAdClosed += HandleOnRewardedAdDoubleCoinsClosed;
                    break;
                case "ExtraLife":
                    ad.OnUserEarnedReward += HandleUserEarnedLife;
                    ad.OnAdClosed += HandleOnRewardedAdExtraLifeClosed;
                    break;
            }




            // Load an interstitial ad
            ad.LoadAd(CreateAdRequest());
            return ad;
    }

    public InterstitialAd RequestAndLoadInterstitialAd(string adUnitId)
    {
                Debug.Log("Requesting Interstitial Ad.");
                InterstitialAd ad = new InterstitialAd(adUnitId);
                // Add Event Handlers
                //interstitialAd.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent.Invoke();
                ad.OnAdClosed += HandleOnInterstitialAdClosed;

                ad.LoadAd(CreateAdRequest());
                return ad;
    }

    private AdRequest CreateAdRequest()
    {
        Debug.Log("Requesting new ad");
        return new AdRequest.Builder()
            //.AddTestDevice(AdRequest.TestDeviceSimulator)
            .AddTestDevice("3cc162965ce260b056da4b04ec7252a3")
            /*.AddKeyword("unity-admob-sample")
            .TagForChildDirectedTreatment(false)
            .AddExtra("color_bg", "9B30FF")*/
            .Build();
    }

    public bool RewardedAdIsLoaded(string adName){
        switch (adName) {
            case "DoubleCoins":
                return doubleCoinAd.IsLoaded();
            case "ExtraLife":
                return extraLifeAd.IsLoaded();
        }
        return true;
    }

    public void ShowRewardedAd(string adName)
    {
        switch (adName) {
            case "DoubleCoins":
                if (doubleCoinAd.IsLoaded()) {
                        Debug.Log("entra "+adName);
                    doubleCoinAd.Show();
                }
                break;
            case "ExtraLife":
                if (extraLifeAd.IsLoaded()) {
                    extraLifeAd.Show();
                }
                break;
        }
    }

    public void ShowInterstitialAd()
    {
        if (displayAd) {
            if (interstitialAd.IsLoaded())
            {
                Debug.Log("SHOW AD");
                Debug.Log("TIME: " +timer);
                interstitialAd.Show();
                displayAd = false;
                timer = 0;

            }else{
                Debug.Log("Interstitial ad is not ready yet");
            }
        }else{
            Debug.Log("not enough time since the last Ad");
        }
    }


    public void HandleOnInterstitialAdClosed(object sender, EventArgs args)
    {
        ((InterstitialAd)sender).LoadAd(CreateAdRequest());
    }
    public void HandleOnRewardedAdDoubleCoinsClosed(object sender, EventArgs args)
    {
        doubleCoinAd = RequestAndLoadRewardedAd("ca-app-pub-3940256099942544/5224354917", "DoubleCoins");
    }
    public void HandleOnRewardedAdExtraLifeClosed(object sender, EventArgs args)
    {
        extraLifeAd = RequestAndLoadRewardedAd("ca-app-pub-3940256099942544/5224354917", "ExtraLife");
    }

    public void HandleUserEarnedLife(object sender, Reward args)
    {
            giveExtraLife=true;
            Debug.Log("extra life setted true");

    }
    public void HandleUserEarnedCoins(object sender, Reward args)
    {
            giveDoubleCoins = true;
            Debug.Log("double coins setted true");

    }


    IEnumerator Contar(){
        while(true){
            timer++;
            //Debug.Log("TIME: " +timer);
            if (timer >= timeBetweenAds) {
                displayAd = true;
                timer = 0;
            }
            yield return new WaitForSecondsRealtime(1);
        }
    }

}
