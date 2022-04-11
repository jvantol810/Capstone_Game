using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public Slider VolumeSlider;
    float Volume;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        Volume = audioSource.volume;
        VolumeSlider.value = Volume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        //Register Slider Events
        VolumeSlider.onValueChanged.AddListener(delegate { changeVolume(VolumeSlider.value); });
    }

    //Called when Slider is moved
    void changeVolume(float sliderValue)
    {
        audioSource.volume = sliderValue;
    }

    void OnDisable()
    {
        //Un-Register Slider Events
        VolumeSlider.onValueChanged.RemoveAllListeners();
    }
}
