using UnityEngine;
using UnityEngine.UI;

public class ClassicLevel : MonoBehaviour, ILevelController
{
    public GameController gameController;
    public GameLogic gameLogic;

    public Text scoreText;

    [HideInInspector]
    public bool _enabled { get; set; }

    void Start()
    {
        if (!_enabled)
            return;

        gameLogic.enabled = true;
    }

    void Update()
    {
        if (!_enabled)
            return;
    }

    private void EndGame()
    {
        Enable(false);
        CheckAndSetHighscore();
        gameController.EndGame(GetScore());
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
            scoreText.text = "" + GetScore();
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
        int prevHighScore = PlayerPrefs.GetInt("high-score-clicks");

        if (currentScore > prevHighScore)
        {
            PlayerPrefs.SetInt("high-score-clicks", currentScore);
            //display some high score thingy
            PlayerPrefs.Save();
        }
    }
}
