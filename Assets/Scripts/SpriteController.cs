using UnityEngine;

public class SpriteController : MonoBehaviour
{
    public MainLevelController mlc;
    public bool amICorrectOption = false;

    private void OnMouseDown()
    {
        Debug.Log("i am pressed");

        if (amICorrectOption)
        {
            mlc.CorrectOption();
        }
        else
        {
            mlc.IncorrectOption();
        }
    }
}
