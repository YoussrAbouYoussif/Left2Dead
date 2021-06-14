using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MusicLevel : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;


    void Awake(){

        slider.value = 1.0f;
        mixer.SetFloat("Music", Mathf.Log10(slider.value) * 20);
        PlayerPrefs.SetFloat("MusicVolume1", slider.value);

    }

    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("MusicVolume1", 1.0f);
    }
    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("Music", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume1", sliderValue);
    }
}
