using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject rightArrow;
    public GameObject leftArrow;

    public Text classicHighScoreText;
    public Text timerHighScoreText;
    public Text timerModeName;

    public Text playSoundText;
    public Toggle playSoundToggle;

    public Text showFPSText;
    public Toggle showFPSToggle;

    public Animator menuAnimator;
    public Animator settingsAnimator;

    private int menuIndex = 0;
    private int sceneToLoad = 1;

    private const int highScoreForTimer = 100;

    private int playSound = 1;
    private int showFPS = 0;

    private bool isSettingsOpen = false;

    void Start()
    {
        if (!PlayerPrefs.HasKey("high-score-clicks"))
        {
            PlayerPrefs.SetInt("high-score-clicks", 0);
        }

        int clasicHighScore = PlayerPrefs.GetInt("high-score-clicks");
        if (clasicHighScore != 0)
        {
            classicHighScoreText.text = "" + clasicHighScore;
        }
        else
        {
            classicHighScoreText.text = "";
        }



        if (!PlayerPrefs.HasKey("high-score-timer"))
        {
            PlayerPrefs.SetInt("high-score-timer", 0);
        }

        int timerHighScore = PlayerPrefs.GetInt("high-score-timer");
        if (clasicHighScore >= highScoreForTimer)
        {
            timerModeName.text = "timer mode";
            if (timerHighScore != 0)
            {
                timerHighScoreText.text = "" + timerHighScore;
            }
            else
            {
                timerHighScoreText.text = "";
            }
        }
        else
        {
            timerHighScoreText.text = "";
        }


        if (!PlayerPrefs.HasKey("play-sound"))
        {
            PlayerPrefs.SetInt("play-sounc", 1);
            playSoundText.text = "   Sound: On";
            playSoundToggle.isOn = true;
        }
        else if (PlayerPrefs.GetInt("play-sound") == 1)
        {
            playSoundText.text = "   Sound: On";
            playSoundToggle.isOn = true;
        }
        else
        {
            playSoundText.text = "   Sound: Off";
            playSoundToggle.isOn = false;
        }

        if (!PlayerPrefs.HasKey("show-fps"))
        {
            PlayerPrefs.SetInt("show-fps", 0);
            showFPSText.text = "    FPS: Off";
            showFPSToggle.isOn = false;
        }
        else if (PlayerPrefs.GetInt("show-fps") == 1)
        {
            showFPSText.text = "    FPS: On";
            showFPSToggle.isOn = true;
        }
        else
        {
            showFPSText.text = "    FPS: Off";
            showFPSToggle.isOn = false;
        }

        leftArrow.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (isSettingsOpen)
            {
                HideSettings();
            }
            else
            {
                Debug.Log("exiting game");
                Application.Quit();
            }
        }
    }

    public void StartGame()
    {
        if (sceneToLoad != -1)
        {
            SceneManager.LoadScene(sceneToLoad);

        }
    }

    public void ArrowButtonClick(int change)
    {
        menuIndex += change;
        //Debug.Log(menuIndex);

        switch (menuIndex)
        {
            case 0:
                sceneToLoad = 1;
                leftArrow.SetActive(false);
                rightArrow.SetActive(true);
                menuAnimator.SetInteger("GameState", menuIndex);
                break;

            case 1:
                sceneToLoad = -1;
                leftArrow.SetActive(true);
                rightArrow.SetActive(true);

                if (PlayerPrefs.GetInt("high-score-clicks", 0) >= highScoreForTimer)
                {
                    sceneToLoad = 2;
                }
                menuAnimator.SetInteger("GameState", menuIndex);
                break;

            case 2:
                sceneToLoad = -1;
                leftArrow.SetActive(true);
                rightArrow.SetActive(false);
                menuAnimator.SetInteger("GameState", menuIndex);
                break;

            default:
                break;
        }
    }

    public void SoundToggle()
    {
        playSound += 1;
        playSound %= 2;
        //Debug.Log(playSound);

        PlayerPrefs.SetInt("play-sound", playSound);
        if (playSound == 1)
        {
            playSoundText.text = "   Sound: On";
        }
        else
        {
            playSoundText.text = "   Sound: Off";
        }
    }

    public void FPSToggle()
    {
        showFPS += 1;
        showFPS %= 2;
        //Debug.Log(showFPS);

        PlayerPrefs.SetInt("show-fps", showFPS);
        if (showFPS == 1)
        {
            showFPSText.text = "    FPS: On";
        }
        else
        {
            showFPSText.text = "    FPS: Off";
        }
    }

    public void ShowSettings()
    {
        settingsAnimator.SetTrigger("showSettings");
        settingsAnimator.ResetTrigger("hideSettings");
        isSettingsOpen = true;
    }

    public void HideSettings()
    {
        settingsAnimator.SetTrigger("hideSettings");
        settingsAnimator.ResetTrigger("showSettings");
        isSettingsOpen = false;
    }
}
