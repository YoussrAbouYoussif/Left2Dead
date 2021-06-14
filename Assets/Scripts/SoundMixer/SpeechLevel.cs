using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class SpeechLevel : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;

    void Awake(){

        slider.value = 1.0f;
        mixer.SetFloat("Speech", Mathf.Log10(slider.value) * 20);
        PlayerPrefs.SetFloat("MusicVolume2", slider.value);

    }

    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("MusicVolume2", 1.0f);
    }
    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("Speech", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume2", sliderValue);
    }
}
