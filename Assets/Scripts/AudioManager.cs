using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    public static AudioManager Instance { get { return instance; } }

    public AudioSource audioSource;

    private bool playSound;

    //Sa 240 Hz, Re 270 Hz, Ga 300 Hz, Ma 320 Hz,Pa 360 Hz, Dha 400 Hz, and Ni 450 Hz
    public float[] frequencies = new float[7] { 240, 270, 300, 320, 360, 400, 450 };
    public AudioClip[] audioClips = new AudioClip[7];

    public const int sampleLength = 4400;
    public int sampleFreq = 44000;

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

        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    private void Start()
    {
        for (int i = 0; i < frequencies.Length; i++)
        {
            //float frequency = 90000;
            float frequency = frequencies[i];

            float[] samples = new float[sampleLength];
            for (int j = 0; j < sampleLength; j++)
            {
                samples[j] = Mathf.Sin(Mathf.PI * 2 * j * frequency / sampleFreq);
            }

            AudioClip ac = AudioClip.Create("Test-"+frequencies[i], sampleLength, 2, sampleFreq, false);
            ac.SetData(samples, 0);
            audioClips[i] = ac;

            samples = null;
            ac = null;
        }
    }

    public void PlayClickSound(int clipToPlay)
    {
        if (playSound)
        {
            audioSource.Stop();
            audioSource.clip = audioClips[clipToPlay];
            audioSource.Play();
        }
    }
}
