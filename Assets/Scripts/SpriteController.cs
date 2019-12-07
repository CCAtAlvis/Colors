using UnityEngine;

public class SpriteController : MonoBehaviour
{
    public GameObject levelController;
    public bool amICorrectOption = false;
    public int soundToPlay = 0;

    private ILevelController gameLogic;

    void Awake()
    {
        gameLogic = levelController.GetComponent<ILevelController>();    
    }

    private void OnMouseDown()
    {
        //Play click sound
        AudioManager.Instance.PlayClickSound(soundToPlay);

        //Haptic feedback
        //Handheld.Vibrate();
        //Need to find something to get it worked

        //Debug.Log("i am pressed");

        if (amICorrectOption)
        {
            gameLogic.CorrectOption(true);
        }
        else
        {
            gameLogic.CorrectOption(true);
            //gameLogic.CorrectOption(false);
        }
    }
}
