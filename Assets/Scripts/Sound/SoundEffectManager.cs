using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager instance;

    private AudioSource audioSource;
    //public AudioClip audioClip;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            GameManager.Instance.RegisterPersistentObject(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void playSoundEffect(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVolume(float volume)
    { audioSource.volume = volume; }

    public float getVolume()
    { return audioSource.volume; }
}
