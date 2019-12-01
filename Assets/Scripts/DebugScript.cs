﻿using UnityEngine;
using UnityEngine.UI;

public class DebugScript : MonoBehaviour
{
    private static DebugScript instance;

    public static DebugScript Instance { get { return instance; } }

    public Text fpsText;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {

    }

    void Update()
    {
        float fps = 1 / Time.deltaTime;
        fpsText.text = "" + fps.ToString("F2"); ;

    }
}
