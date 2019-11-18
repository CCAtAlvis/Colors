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

    [SerializeField]
    private int timeLeft = 3;

    void Awake()
    {
        gameLogic.enabled = false;
        optionSprites.SetActive(false);
    }

    void Start()
    {
        //Debug.Log("starting");
        countdown.text = "" + timeLeft;
        StartCoroutine("CountdownTimer");
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
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }
}
