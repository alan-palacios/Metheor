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
    private static bool displayAd = true;
    public int timeBetweenAds;
    public string adName;


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
        interstitialAd = RequestAndLoadInterstitialAd("ca-app-pub-3940256099942544/1033173712");
        doubleCoinAd = RequestAndLoadRewardedAd("ca-app-pub-3940256099942544/5224354917");
        extraLifeAd = RequestAndLoadRewardedAd("ca-app-pub-3940256099942544/5224354917");

        StartCoroutine(Contar());
    }



    public RewardedAd RequestAndLoadRewardedAd(string adUnitId){
            Debug.Log("Requesting RewardedAd Ad.");
            RewardedAd ad = new RewardedAd(adUnitId);

            // Add Event Handlers
            //interstitialAd.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent.Invoke();
            ad.OnAdClosed += HandleOnRewardedAdClosed;
            ad.OnUserEarnedReward += HandleUserEarnedReward;

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

    public void ShowRewardedAd(string adName)
    {
        this.adName = adName;
        switch (adName) {
            case "DoubleCoins":
                if (doubleCoinAd.IsLoaded()) {
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

                displayAd = false;
                timer = 0;
                interstitialAd.Show();
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
    public void HandleOnRewardedAdClosed(object sender, EventArgs args)
    {
        ((RewardedAd)sender).LoadAd(CreateAdRequest());
    }


    public void HandleUserEarnedReward(object sender, Reward args)
    {
        Debug.Log(adName);
        switch (adName) {
            case "ExtraLife"://ExtraLife
                generalGC.giveExtraLife=true;                
                break;
            case "DoubleCoins"://DoubleCoins

                break;
        }
        /*switch (args.Type) {
            case "coin"://ExtraLife
                generalGC.ExtraLife();
                break;
            case "DoubleCoins"://DoubleCoins

                break;
        }*/
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
