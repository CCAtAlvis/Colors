using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    public static AudioManager Instance { get { return instance; } }

    public AudioSource click;

    private bool playSound;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        playSound = PlayerPrefs.GetInt("play-sound", 1) == 1;
    }

    public void PlayClickSound()
    {
        if (playSound)
        {
            click.Play();
        }
    }
}
