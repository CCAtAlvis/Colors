using UnityEngine;
using UnityEngine.UI;

public class MainLevelController : MonoBehaviour
{
    public RawImage flashImage;

    public Text scoreText;

    public GameObject deathCanvas;

    public GameObject Option1;
    public GameObject Option2;

    private SpriteRenderer sprite1;
    private SpriteRenderer sprite2;

    private SpriteController spriteController1;
    private SpriteController spriteController2;

    [SerializeField]
    private float flashSpeedMultipler = 1f;

    [SerializeField]
    private Color32[] colorsToFlash;

    private bool toFlashColor = false;
    private int flashColorIndex;
    private int otherOptionIndex;

    private int score = 0;

    void Start()
    {
        sprite1 = Option1.GetComponent<SpriteRenderer>();
        sprite2 = Option2.GetComponent<SpriteRenderer>();

        spriteController1 = Option1.GetComponent<SpriteController>();
        spriteController2 = Option2.GetComponent<SpriteController>();

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

    void FlashScreen()
    {
        flashColorIndex = Random.Range(0, colorsToFlash.Length);
        otherOptionIndex = Random.Range(0, colorsToFlash.Length);

        if (flashColorIndex == otherOptionIndex)
        {
            otherOptionIndex = (otherOptionIndex + 1) % colorsToFlash.Length;
        }

        toFlashColor = true;
        flashImage.color = colorsToFlash[flashColorIndex];

        //Debug.Log("flashing color: " + colorsToFlash[flashColorIndex]);

        SetOptions();
    }

    void SetOptions()
    {
        if (Random.Range(0, 2) == 0)
        {
            sprite1.color = colorsToFlash[flashColorIndex];
            sprite2.color = colorsToFlash[otherOptionIndex];

            spriteController1.amICorrectOption = true;
            spriteController2.amICorrectOption = false;
        }
        else
        {
            sprite1.color = colorsToFlash[otherOptionIndex];
            sprite2.color = colorsToFlash[flashColorIndex];

            spriteController1.amICorrectOption = false;
            spriteController2.amICorrectOption = true;
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

        EndGame();
    }

    private void EndGame()
    {
        deathCanvas.SetActive(true);
    }
}