using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthJockey : MonoBehaviour
{
    float health;
    float maxHealth = 325f;

    public GameObject healthBarUI;
    public Slider slider;

    AudioSource[] audios;
    AudioSource screamJockey;

    void Start()
    {
        health = JockeyScript.healthPoints;
        slider.value = CalculateHealth();
        audios = GetComponents<AudioSource>();
        screamJockey = audios[2];
    }

    void Update()
    {
        if (JockeyScript.healthChanged)
        {
            if(!screamJockey.isPlaying)
            {
                screamJockey.Play();
            }
            health = JockeyScript.healthPoints;
            slider.value = CalculateHealth();
            JockeyScript.healthChanged = false;
        }
        if(JockeyScript.healthPoints < 0)
        {
            healthBarUI.SetActive(false);
        }
    }

    float CalculateHealth()
    {
        return health / maxHealth;
    }

}
