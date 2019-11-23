using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using GoogleMobileAds.Api;

using UnityEngine.Networking;

public class GameController : MonoBehaviour
{
#if UNITY_ANDROID
    string appId = "ca-app-pub-3737555426073384~3657301496";
    string bannerAdUnitId = "ca-app-pub-3737555426073384/4778811477";
    string rewardedAdUnitId = "ca-app-pub-3737555426073384/4587239787";
#else
    string appId = "unexpected_platform";
    string bannerAdUnitId = "unexpected_platform";
    string rewardedAdUnitId = "unexpected_platform";
#endif

    public Text countdown;
    public GameLogic gameLogic;
    public GameObject optionSprites;

    public GameObject deathCanvas;
    public GameObject pauseCanvas;
    public GameObject noAdAvailable;

    [SerializeField]
    private int timeLeft = 3;

    private bool hasGameEnded = false;

    //BannerView to hold ads
    private BannerView bannerView;

    //RewardedAd
    private RewardedAd rewardedAd;
    private bool rewardReceived = false;
    private bool rewardAdClosed = false;

    void Awake()
    {
        gameLogic.enabled = false;
        optionSprites.SetActive(false);
        deathCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
    }

    void Start()
    {
        //Debug.Log("starting");
        countdown.text = "" + timeLeft;
        StartCoroutine(CountdownTimer());

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);

        //THIS STUFF FOR BANNE ADs
        this.RequestBannerAd();


        //THIS STUFF FOR REWARDED ADS
        this.rewardedAd = new RewardedAd(rewardedAdUnitId);

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest requestRewardedAd = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(requestRewardedAd);


        //Check if internet is working
        StartCoroutine(CheckInternetConnection());
    }


    //CHECKING IF INTERNET IS WORKING
    IEnumerator CheckInternetConnection()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://x.com");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
        }
    }

    //THIS STUFF FOR REWARDED AD
    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        Debug.Log(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        Debug.Log(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
    }


    //ALL THIS HACKY STUFF BELONGS TO/FROM
    //https://stackoverflow.com/questions/53916533/setactive-can-only-be-called-from-the-main-thread
    //Of the answers on this post, the with UnityMainThread isn't working
    //Sooo... next lets try out this
    //https://forum.unity.com/threads/unity-run-on-main-thread.423147/
    //And even this is not working... now lets try the other answer from SO
    //And even that doesn't seem to be working :cry:
    //Time to try a new blog
    //https://www.pmichaels.net/tag/findgameobjectwithtag-can-only-be-called-from-the-main-thread/
    //And nope.. thats also not working :(
    //Now from a video tutorial...
    //This guy creates a singleton patter for his UIManager, which in later videos
    //also handles the Google AdMobs stuff.
    //https://www.youtube.com/watch?v=IUA_vMDRTxg - Implementing singleton
    //https://www.youtube.com/watch?v=3gbOEH7FNn0 - Google AdMob Rewarded ads
    //and even this is not working
    //Sooo finally the method that worked!
    //its simple, check for the flags in the Update loop
    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        this.CreateAndLoadRewardedAd();

        Debug.Log("HandleRewardedAdClosed");
        if (rewardReceived)
        {
            rewardAdClosed = true;
        }
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        Debug.Log("HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);

        this.bannerView.Hide();
        rewardReceived = true;
    }

    private void CreateAndLoadRewardedAd()
    {
        this.rewardedAd = new RewardedAd(rewardedAdUnitId);
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }


    //THIS STUFF FOR BANNER ADS
    private void RequestBannerAd()
    {
        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(bannerAdUnitId, AdSize.Banner, AdPosition.Bottom);
        // Create an empty ad request.

        // Called when an ad request has successfully loaded.
        this.bannerView.OnAdLoaded += this.HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.bannerView.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        this.bannerView.OnAdOpening += this.HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        this.bannerView.OnAdClosed += this.HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.bannerView.OnAdLeavingApplication += this.HandleOnAdLeavingApplication;

        AdRequest requestBannerAd = new AdRequest.Builder().Build();
        // Load the banner with the request.
        this.bannerView.LoadAd(requestBannerAd);
        // Hide th bannerView by default
        this.bannerView.Hide();
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        Debug.Log("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        Debug.Log("HandleAdClosed event received");
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        Debug.Log("HandleAdLeavingApplication event received");
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && timeLeft <= 0)
        {
            if (hasGameEnded)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                pauseCanvas.SetActive(!pauseCanvas.activeSelf);
            }
        }

        if (rewardReceived && rewardAdClosed)
        {
            deathCanvas.SetActive(false);
            hasGameEnded = false;
            countdown.gameObject.SetActive(true);

            timeLeft = 3;
            StartCoroutine(RestartGameAfterRewardedAd());

            rewardReceived = false;
            rewardAdClosed = false;
        }
    }

    IEnumerator CountdownTimer()
    {
        while (timeLeft >= 0)
        {
            //Debug.Log(timeLeft);
            countdown.text = "" + timeLeft;
            if (timeLeft == 0)
            {
                countdown.text = "GO!";
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                yield return new WaitForSeconds(1);
            }

            timeLeft--;
        }

        countdown.gameObject.SetActive(false);
        //Debug.Log("end");

        gameLogic.enabled = true;
        optionSprites.SetActive(true);
    }

    IEnumerator RestartGameAfterRewardedAd()
    {
        while (timeLeft >= 0)
        {
            //Debug.Log(timeLeft);
            countdown.text = "" + timeLeft;
            if (timeLeft == 0)
            {
                countdown.text = "GO!";
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                yield return new WaitForSeconds(1);
            }

            timeLeft--;
        }

        countdown.gameObject.SetActive(false);
        //Debug.Log("end");

        gameLogic.AddLife();
        optionSprites.SetActive(true);
    }

    private void CheckAndSetHighscore()
    {
        int currentScore = gameLogic.GetScore();
        int prevHighScore = PlayerPrefs.GetInt("high-score-clicks");

        if (currentScore > prevHighScore)
        {
            PlayerPrefs.SetInt("high-score-clicks", currentScore);
            //display some high score thingy
            PlayerPrefs.Save();
        }
    }

    public void EndGame(int currentScore)
    {
        optionSprites.SetActive(false);

        CheckAndSetHighscore();

        deathCanvas.SetActive(true);
        hasGameEnded = true;

        this.bannerView.Show();
    }

    public void RestartGame()
    {
        CheckAndSetHighscore();

        this.bannerView.Hide();
        this.bannerView.Destroy();

        SceneManager.LoadScene(1);
    }

    public void ResumeGame()
    {
        pauseCanvas.SetActive(false);
    }

    public void QuitGame()
    {
        this.bannerView.Hide();
        this.bannerView.Destroy();
        CheckAndSetHighscore();
        SceneManager.LoadScene(0);
    }

    public void UserChoseToWatchAd()
    {
        rewardReceived = false;

        Debug.Log("user wants to watch ad!!");
        Debug.Log("is rewarded ad loaded: " + this.rewardedAd.IsLoaded());

        this.bannerView.Hide();
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
        else
        {
            noAdAvailable.SetActive(true);
        }
    }
}
