using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider soundEffectSlider;
    public float soundEffectVolume = 0.5f;
    public GameObject bgmObject;
    public GameObject soundObject;
    private BGMManager bgmManager; 
    private SoundEffectManager soundEffectManager;

    // Start is called before the first frame update
    void Start()
    {


        if (bgmObject != null)
        {
            bgmManager = bgmObject.GetComponent<BGMManager>();
        }
        if (soundObject != null)
        {
            soundEffectManager = soundObject.GetComponent<SoundEffectManager>();
        }

        if (bgmSlider != null && bgmManager != null)
        {
            bgmSlider.value = bgmManager.getVolume();
            bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        }

        if (soundEffectSlider != null && soundEffectManager != null)
        {
            soundEffectSlider.value = soundEffectManager.getVolume();
            soundEffectSlider.onValueChanged.AddListener(SetBGMVolume);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSoundEffectVolume(float volume)
    {
        if (soundEffectManager != null)
        {
            soundEffectManager.SetVolume(volume);
        }
    }

    public void SetBGMVolume(float volume)
    {
        if (bgmManager != null)
        {
            bgmManager.SetVolume(volume);
        }
    }
}
