using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public Text countdown;
    public MainLevelController mlc;
    public GameObject optionSprites;

    [SerializeField]
    private int timeLeft = 3;

    void Awake()
    {
        mlc.enabled = false;
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

        mlc.enabled = true;
        optionSprites.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }
}
