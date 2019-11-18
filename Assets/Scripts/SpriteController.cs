using UnityEngine;

public class SpriteController : MonoBehaviour
{
    public GameLogic gameLogic;
    public bool amICorrectOption = false;

    private void OnMouseDown()
    {
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
