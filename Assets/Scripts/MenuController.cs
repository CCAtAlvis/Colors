using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Text highScoreText;

    void Start()
    {
        if (!PlayerPrefs.HasKey("high-score-clicks"))
        {
            PlayerPrefs.SetInt("high-score-clicks", 0);
        }

        int highScore = PlayerPrefs.GetInt("high-score-clicks");
        if (highScore != 0)
        {
            highScoreText.text = "HIGH SCORE\n" + highScore;
        }
        else
        {
            highScoreText.text = "";
        }
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
        SceneManager.LoadScene(1);
    }
}
