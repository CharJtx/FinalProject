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
    private BGMManager bgmManager; 

    // Start is called before the first frame update
    void Start()
    {


        if (bgmObject != null)
        {
            bgmManager = bgmObject.GetComponent<BGMManager>();
        }

        if (bgmSlider != null && bgmManager != null)
        {
            bgmSlider.value = bgmManager.getVolume();
            bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        }

        if (soundEffectSlider != null)
        {
            soundEffectSlider.value = soundEffectVolume;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBGMVolume(float volume)
    {
        if (bgmManager != null)
        {
            bgmManager.SetVolume(volume);
        }
    }
}
