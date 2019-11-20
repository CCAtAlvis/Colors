using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public void EndGame(int currentScore)
    {
        optionSprites.SetActive(false);

        int prevHighScore = PlayerPrefs.GetInt("high-score-clicks");

        if (currentScore > prevHighScore)
        {
            PlayerPrefs.SetInt("high-score-clicks", currentScore);
            //display some high score thingy
            PlayerPrefs.Save();
        }

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
        SceneManager.LoadScene(0);
    }
}
