using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{

    private static BGMManager instance;

    private AudioSource audioSource;
    public AudioClip bgmA;
    public AudioClip bgmB;

    private bool isBgmBPlaying = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (bgmA != null)
        {
            playBGM(bgmA);
        }
        else
        {
            playBGM(bgmB);
            audioSource.loop = true;
            isBgmBPlaying = true;
        }

       
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            if (!isBgmBPlaying)
            {
                playBGM(bgmB);
                audioSource.loop = true;
                isBgmBPlaying = true;
            }
        }
    }

    void playBGM(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void Pause()
    { audioSource.Pause(); }

    public void Resume()
    { audioSource.UnPause(); }

    public void SetVolume(float volume)
    { audioSource.volume = volume; }

    public float getVolume()
    { return audioSource.volume; }
}
