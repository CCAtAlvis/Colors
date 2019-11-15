using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public RawImage flashImage;

    public Text scoreText;

    public GameObject deathCanvas;

    public GameObject[] options;

    private List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    private List<SpriteController> spriteControllers = new List<SpriteController>();

    private Vector3[] optionsPosition2 = { new Vector3(), new Vector3() };
    private Vector3[] optionsPosition3 = { new Vector3(), new Vector3(), new Vector3() };
    private Vector3[] optionsPosition4 = { new Vector3(), new Vector3(),
                                           new Vector3(), new Vector3() };
    private Vector3[] optionsPosition5 = { new Vector3(), new Vector3(), new Vector3(),
                                           new Vector3(), new Vector3() };
    private Vector3[] optionsPosition6 = { new Vector3(), new Vector3(), new Vector3(),
                                           new Vector3(), new Vector3(), new Vector3() };

    [SerializeField]
    private float flashSpeedMultipler = 1f;

    [SerializeField]
    private Color32[] colorsToFlash;

    private bool toFlashColor = false;
    private int flashColorIndex;

    private int score = 0;

    private int optionsToDisplay = 2;

    void Start()
    {
        for (int i = 0; i < options.Length; ++i)
        {
            sprites.Add(options[i].GetComponent<SpriteRenderer>());
            spriteControllers.Add(options[i].GetComponent<SpriteController>());
        }

        FlashScreen();
    }

    void Update()
    {
        if (toFlashColor)
        {
            flashImage.color = new Color(flashImage.color.r,
                flashImage.color.g,
                flashImage.color.b,
                flashImage.color.a - Time.deltaTime * flashSpeedMultipler);

            if (flashImage.color.a <= 0)
            {
                toFlashColor = false;
            }
        }
    }

    //Shuffle an array
    int[] ShuffleArray(int[] nums)
    {
        for (int i = 0; i < nums.Length; ++i)
        {
            int randomIndex = Random.Range(0, nums.Length);
            int temp = nums[randomIndex];
            nums[randomIndex] = nums[i];
            nums[i] = temp;
        }

        return nums;
    }

    int[] GetRandomColorIndex()
    {
        int[] indexes = { 0, 1, 2, 3, 4, 5, 6 };

        return ShuffleArray(indexes);
    }

    int[] GetRandomTransform()
    {
        int[] transforms2 = { 0, 1 };
        int[] transforms3 = { 0, 1, 2 };
        int[] transforms4 = { 0, 1, 2, 3 };
        int[] transforms5 = { 0, 1, 2, 3, 4 };
        int[] transforms6 = { 0, 1, 2, 3, 4, 5 };

        switch (optionsToDisplay)
        {
            case 2:
                return ShuffleArray(transforms2);

            case 3:
                return ShuffleArray(transforms3);

            case 4:
                return ShuffleArray(transforms4);

            case 5:
                return ShuffleArray(transforms5);

            case 6:
                return ShuffleArray(transforms6);

            default:
                return transforms2;
        }
    }

    void FlashScreen()
    {
        //Shuffle the indexes
        int[] colorIndexes = GetRandomColorIndex();

        //First index color is to flash (also the right option)
        flashColorIndex = colorIndexes[0];

        toFlashColor = true;
        flashImage.color = colorsToFlash[flashColorIndex];

        //Debug.Log("flashing color: " + colorsToFlash[flashColorIndex]);

        SetOptions(colorIndexes);
    }

    void SetOptions(int[] indexes)
    {
        int[] positionIndexes = GetRandomTransform();

        Vector3[] positions;
        switch (optionsToDisplay)
        {
            case 2:
                positions = optionsPosition2;
                break;

            case 3:
                positions = optionsPosition3;
                break;

            case 4:
                positions = optionsPosition4;
                break;

            case 5:
                positions = optionsPosition5;
                break;

            case 6:
                positions = optionsPosition6;
                break;
        }


        for (int i = 0; i < optionsToDisplay; ++i)
        {
            options[i].transform.position = positions[positionIndexes[i]];
            sprites[i].color = colorsToFlash[indexes[i]];
            spriteControllers[i].amICorrectOption = false;

            if (i == 0)
            {
                spriteControllers[i].amICorrectOption = true;
            }
        }

        if (Random.Range(0, 2) == 0)
        {
            //sprite1.color = colorsToFlash[flashColorIndex];
            //sprite2.color = colorsToFlash[otherOptionIndex];

            //spriteController1.amICorrectOption = true;
            //spriteController2.amICorrectOption = false;
        }
        else
        {
        }
    }

    public void CorrectOption()
    {
        score++;
        scoreText.text = "" + score;
        FlashScreen();
    }

    public void IncorrectOption()
    {
        Option1.SetActive(false);
        Option2.SetActive(false);

        //EndGame();
    }
}
