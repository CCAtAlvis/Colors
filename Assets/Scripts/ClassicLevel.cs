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

    public int GetScore()
    {
        return gameLogic.GetScore();
    }

    public void AddLife()
    {
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
            scoreText.text = "" + gameLogic.GetScore();
        }
        else
        {
            //End Game
            Enable(false);
            gameController.EndGame(gameLogic.GetScore());
        }
    }
}
