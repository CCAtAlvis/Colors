using UnityEngine;
using UnityEngine.UI;

public class TimerLevel : MonoBehaviour, ILevelController
{
    public GameController gameController;
    public GameLogic gameLogic;
    public Text endGameText;
    public Button getLife;

    public Text scoreText;

    [HideInInspector]
    public bool _enabled { get; set; }

    private float timer = 30f;

    void Start()
    {
        if (!_enabled)
            return;

        gameLogic.enabled = true;
        gameLogic.flashSpeedMultipler = 2f;
    }

    void Update()
    {
        if (!_enabled)
            return;

        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            scoreText.text = "" + timer.ToString("F2");
        }
        else
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        Enable(false);
        CheckAndSetHighscore();
        gameController.EndGame(GetScore());
        scoreText.text = "Score: " + GetScore();

        if (timer <= 0f)
        {
            endGameText.text = "Time Over";
            getLife.gameObject.SetActive(false);
        }
    }

    public int GetScore()
    {
        return gameLogic.GetScore();
    }

    public void AddLife()
    {
        Enable(true);
        gameLogic.AddLife();
    }

    public void Enable(bool status)
    {
        _enabled = status;
    }

    public void Restart()
    {
        _enabled = true;
        Start();
    }

    public void CorrectOption(bool status)
    {
        if (!_enabled)
            return;

        if (status)
        {
            //Correct option => play on
            gameLogic.CorrectOption();
        }
        else
        {
            //End Game
            EndGame();
        }
    }

    public void CheckAndSetHighscore()
    {
        int currentScore = GetScore();
        int prevHighScore = PlayerPrefs.GetInt("high-score-timer");

        if (currentScore > prevHighScore)
        {
            PlayerPrefs.SetInt("high-score-timer", currentScore);
            PlayerPrefs.SetFloat("high-score-timer-time", 60 - timer);
            //display some high score thingy
            PlayerPrefs.Save();
        }
    }
}
