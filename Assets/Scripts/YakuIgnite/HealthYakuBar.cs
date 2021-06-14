using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthYakuBar : MonoBehaviour
{
    float health;
    float maxHealth = 50f;

    public GameObject healthBarUI;
    public Slider slider;
    Animator m_Animator;
    hunterControl aiScr;
    GameObject hunter;

    void Start()
    {

        health = maxHealth;
        slider.value = CalculateHealth();
    }

    void Update()
    {
        slider.value = CalculateHealth();

    }

    float CalculateHealth()
    {
        return health / maxHealth;
    }



    public void setHealth(int x)
    {
        this.health = x;
    }
}