using UnityEngine;

public class SpriteController : MonoBehaviour
{
    public GameLogic gameLogic;
    public bool amICorrectOption = false;

    private void OnMouseDown()
    {
        //Play click sound
        AudioManager.Instance.PlayClickSound();

        //Haptic feedback
        //Handheld.Vibrate();
        //Need to find something to get it worked

        //Debug.Log("i am pressed");

        if (amICorrectOption)
        {
            gameLogic.CorrectOption();
        }
        else
        {
            gameLogic.IncorrectOption();
        }
    }
}
