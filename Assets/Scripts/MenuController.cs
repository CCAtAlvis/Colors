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

    public Animator menuAnimator;

    private int menuIndex = 0;
    private int sceneToLoad = 1;

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
        if (timerHighScore != 0)
        {
            timerHighScoreText.text = "" + timerHighScore;
        }
        else
        {
            timerHighScoreText.text = "";
        }


        leftArrow.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("exiting game");
            Application.Quit();
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
        Debug.Log(menuIndex);

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

                if (PlayerPrefs.HasKey("asd"))
                {
                    sceneToLoad = 2;
                    timerModeName.text = "timer mode";
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
}
