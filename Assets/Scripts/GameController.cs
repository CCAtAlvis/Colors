using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

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

    }

    private void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#else
            string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
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
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ResumeGame()
    {
        pauseCanvas.SetActive(false);
    }

    public void QuitGame()
    {
        CheckAndSetHighscore();
        SceneManager.LoadScene(0);
    }
}
