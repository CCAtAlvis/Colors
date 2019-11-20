using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using System;

public class GameController : MonoBehaviour
{
    public Text countdown;
    public GameLogic gameLogic;
    public GameObject optionSprites;

    public GameObject deathCanvas;
    public GameObject pauseCanvas;

    [SerializeField]
    private int timeLeft = 3;

    private bool hasGameEnded = false;

    //BannerView to hold ads
    private BannerView bannerView;

    //RewardedAd
    private RewardedAd rewardedAd;


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
        StartCoroutine("CountdownTimer");

#if UNITY_ANDROID
        string appId = "ca-app-pub-4474806217912407~9143176867";
#else
            string appId = "unexpected_platform";
#endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);
        this.RequestBanner();


        //THIS STUFF FOR REWARDED ADS
#if UNITY_ANDROID
        string rewardedAdUnitId = "ca-app-pub-3940256099942544/6300978111";
#else
            string rewardedAdUnitId = "unexpected_platform";
#endif

        this.rewardedAd = new RewardedAd(rewardedAdUnitId);

        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest requestRewardedAd = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(requestRewardedAd);
    }


    //THIS STUFF FOR REWARDED AD
    public void UserChoseToWatchAd()
    {
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        this.CreateAndLoadRewardedAd();
    }


    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        print("HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);

        gameLogic.AddLife();
    }

    private void CreateAndLoadRewardedAd()
    {
#if UNITY_ANDROID
        string rewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";
#else
            string adUnitId = "unexpected_platform";
#endif

        this.rewardedAd = new RewardedAd(rewardedAdUnitId);
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }


    //THIS STUFF FOR BANNER ADS
    private void RequestBanner()
    {
#if UNITY_ANDROID
        string bannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";
#else
            string bannerAdUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(bannerAdUnitId, AdSize.Banner, AdPosition.Bottom);
        // Create an empty ad request.
        AdRequest requestBannerAd = new AdRequest.Builder().Build();
        // Load the banner with the request.
        this.bannerView.LoadAd(requestBannerAd);
        // Hide th bannerView by default
        this.bannerView.Hide();
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
        this.bannerView.Destroy();
        SceneManager.LoadScene(1);
    }

    public void ResumeGame()
    {
        pauseCanvas.SetActive(false);
    }

    public void QuitGame()
    {
        this.bannerView.Destroy();
        CheckAndSetHighscore();
        SceneManager.LoadScene(0);
    }
}
