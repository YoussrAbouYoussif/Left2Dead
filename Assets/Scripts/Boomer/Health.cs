using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Health : MonoBehaviour
{
    [SerializeField]
    public Item item;

    public static float health;
    float maxHealth = 50f;

    public GameObject healthBarUI;
    public Slider slider;
    Animator m_Animator;
	boomerControl aiScr;
	GameObject boomer;
    public static bool dead = false;
    public AudioSource[] audioSource;
    public AudioSource hitBoomer;
    public AudioSource deadBoomer;
    public GameObject particleSystem;

    

    


    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();

        boomer = GameObject.FindWithTag("Boomer");
		aiScr = boomer.GetComponent<boomerControl>();

        audioSource = GetComponents<AudioSource>();
        hitBoomer  = audioSource[2];
        deadBoomer = audioSource[3];


        health = maxHealth;
        slider.value = CalculateHealth();
    }

    void Update()
    {
        slider.value = CalculateHealth();
        if(health <= 0)
        {
            aiScr.joelSeen = false;
            m_Animator.SetBool("died", true);
            StartCoroutine(particleSystem.GetComponent<bloodScript>().opened());

             if(dead == false){
                JoelScript.updateRage(false);
                InventoryScript.MyInstance.AddItem((Ingredients)Instantiate(item));
                deadBoomer.Play();
                dead = true;
                StartCoroutine(destroyBoomer());
        }
        }
    }

    float CalculateHealth()
    {
        return health / maxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {

    if(dead == false){
        if (collision.gameObject.CompareTag("AssaultRifleBullet"))
        {
            StartCoroutine(hitReaction());
            if(JoelScript.rageMode){
                health -= 66f;
            }
            else{
                health -= 33f;
            }

        }
        if (collision.gameObject.CompareTag("PistolBullet"))
        {
            StartCoroutine(hitReaction());
            if(JoelScript.rageMode){
                health -= 72;
            }
            else{
                health -= 36f;
            }
        }
        if (collision.gameObject.CompareTag("ShotgunBullet"))
        {
            StartCoroutine(hitReaction());
            if(JoelScript.rageMode){
                health -= 50f;
            }
            else{
                health -= 25f;
            }
        }
        if (collision.gameObject.CompareTag("SMGBullet"))
        {
            StartCoroutine(hitReaction());
            if(JoelScript.rageMode){
                health -= 40f;
            }
            else{
                health -= 20f;
            }
        }
        if (collision.gameObject.CompareTag("SniperBullet"))
        {
            StartCoroutine(hitReaction());
            if(JoelScript.rageMode){
                health -= 180f;
            }
            else{
                health -= 90f;
            }

        }
        }
       

    }

    public static void Reset(){
        health = 50f;
        dead = false;
    }

    IEnumerator hitReaction()
    {
        m_Animator.SetBool("hit", true);
        hitBoomer.Play();
        StartCoroutine(particleSystem.GetComponent<bloodScript>().opened());

        yield return new WaitForSeconds(0.4f);

        m_Animator.SetBool("hit", false);


    }

    IEnumerator destroyBoomer(){
        yield return new WaitForSeconds(5.0f);
        Destroy(healthBarUI.gameObject);
    }
}
