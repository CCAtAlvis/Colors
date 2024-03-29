﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public RawImage flashImage;

    public GameObject[] options;

    private List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    private List<SpriteController> spriteControllers = new List<SpriteController>();

    private List<Vector2> spritePositions = new List<Vector2>();

    private List<Vector2> optionsPosition2 = new List<Vector2> { new Vector2(-1, 0), new Vector2(1, 0) };

    private List<Vector2> optionsPosition3 = new List<Vector2> {  new Vector2(-1,0.865f),
                                             new Vector2(1,0.865f),
                                             new Vector2(0, -1) };

    private List<Vector2> optionsPosition4 = new List<Vector2> { new Vector2(-1,1), new Vector2(1,1),
                                             new Vector2(-1,-1), new Vector2(1,-1) };

    private List<Vector2> optionsPosition5 = new List<Vector2> {  new Vector2(0,2),
                                             new Vector2(-1.75f,0), new Vector2(-1,-2),
                                             new Vector2(1,-2), new Vector2(1.75f,0) };

    private List<Vector2> optionsPosition6 = new List<Vector2>{  new Vector2(-1,1.8f), new Vector2(1,1.8f),
                                             new Vector2(-1.75f,0), new Vector2(1.75f, 0),
                                             new Vector2(-1,-1.8f), new Vector2(1,-1.8f) };

    public float flashSpeedMultipler = 1f;

    [SerializeField]
    private Color32[] colorsToFlash = { new Color32(255,0,0,255),
                                        new Color32(0,255,0,255),
                                        new Color32(0,0,255,255),
                                        new Color32(0,255,255,255),
                                        new Color32(255,0,255,255),
                                        new Color32(255,255,0,255),
                                        new Color32(255,255,255,255) };

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

        spritePositions.Add(optionsPosition2[0]);
        spritePositions.Add(optionsPosition2[1]);

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
        int[] indexes = new int[] { 0, 1, 2, 3, 4, 5, 6 };

        return ShuffleArray(indexes);
    }

    int[] GetRandomTransform(int displayOptions = 0)
    {
        if (displayOptions == 0)
        {
            displayOptions = optionsToDisplay;
        }

        int[] transforms2 = new int[] { 0, 1 };
        int[] transforms3 = new int[] { 0, 1, 2 };
        int[] transforms4 = new int[] { 0, 1, 2, 3 };
        int[] transforms5 = new int[] { 0, 1, 2, 3, 4 };
        int[] transforms6 = new int[] { 0, 1, 2, 3, 4, 5 };

        switch (displayOptions)
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

        //Debug.Log(colorIndexes[0]);
        //Debug.Log(colorsToFlash[flashColorIndex]);
        //Debug.Log(flashImage.color);

        //Debug.Log("flashing color: " + colorsToFlash[flashColorIndex]);

        SetOptions(colorIndexes);
    }

    void SetOptions(int[] indexes)
    {
        int[] positionIndexes = GetRandomTransform();

        for (int i = 0; i < options.Length; i++)
        {
            options[i].transform.position = new Vector3(100, 100);
        }

        for (int i = 0; i < optionsToDisplay; ++i)
        {
            //Debug.Log(i +":"+ positionIndexes[i] +":"+optionsToDisplay +":"+ spritePositions[positionIndexes[i]]);
            options[i].transform.position = spritePositions[positionIndexes[i]];
            sprites[i].color = colorsToFlash[indexes[i]];
            spriteControllers[i].amICorrectOption = false;
            spriteControllers[i].soundToPlay = indexes[i];

            if (i == 0)
            {
                spriteControllers[i].amICorrectOption = true;
            }
        }
    }

    private void ScoreHandler(int scoreIncrement = 1)
    {
        score += scoreIncrement;

        if (score == 10)
        {
            for (int i = 0; i < spritePositions.Count; ++i)
            {
                spritePositions[i] = optionsPosition3[i];
            }
            spritePositions.Add(optionsPosition3[2]);

            optionsToDisplay++;
        }
        else if (score == 30)
        {
            for (int i = 0; i < spritePositions.Count; i++)
            {
                spritePositions[i] = optionsPosition4[i];
            }
            spritePositions.Add(optionsPosition4[3]);

            optionsToDisplay++;
        }
        else if (score == 60)
        {
            for (int i = 0; i < spritePositions.Count; i++)
            {
                spritePositions[i] = optionsPosition5[i];
            }
            spritePositions.Add(optionsPosition5[4]);

            optionsToDisplay++;
        }
        else if (score == 100)
        {
            for (int i = 0; i < spritePositions.Count; i++)
            {
                spritePositions[i] = optionsPosition6[i];
            }
            spritePositions.Add(optionsPosition6[5]);

            optionsToDisplay++;
        }
        else if (score >= 150)
        {
            optionsToDisplay = Random.Range(4, 7);

            spritePositions.Clear();
            switch (optionsToDisplay)
            {
                case 4:
                    spritePositions = optionsPosition4;
                    break;

                case 5:
                    spritePositions = optionsPosition5;
                    break;

                case 6:
                    spritePositions = optionsPosition6;
                    break;
            }
        }

        if (score % 10 == 0 && flashSpeedMultipler < 15f)
        {
            flashSpeedMultipler += 0.1f;
        }
    }

    public void CorrectOption()
    {
        ScoreHandler();
        FlashScreen();
    }

    public int GetScore()
    {
        return score;
    }

    public void AddLife()
    {
        FlashScreen();
    }
}
