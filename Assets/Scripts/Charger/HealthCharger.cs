using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthCharger : MonoBehaviour
{
    float health;
    float maxHealth = 600f;

    public GameObject healthBarUI;
    public Slider slider;

    AudioSource[] audios;
    AudioSource screamCharger;

    void Start()
    {
        health = ChargerScript.healthPoints;
        slider.value = CalculateHealth();
        audios = GetComponents<AudioSource>();
        screamCharger = audios[0];
    }

    void Update()
    {
        if (ChargerScript.healthChanged)
        {
            if (!screamCharger.isPlaying && !ChargerScript.chargerAnim.GetBool("isDead"))
            {
                screamCharger.Play();
            }
            health = ChargerScript.healthPoints;
            slider.value = CalculateHealth();
            ChargerScript.healthChanged = false;
        }
        if (ChargerScript.healthPoints < 0)
        {
            healthBarUI.SetActive(false);
        }
    }

    float CalculateHealth()
    {
        return health / maxHealth;
    }

}
