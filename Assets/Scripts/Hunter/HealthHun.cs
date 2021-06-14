using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthHun : MonoBehaviour
{

    private static HealthHun instance;

    public static HealthHun MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HealthHun>();
            }

            return instance;
        }
    }
    [SerializeField]
    public Item item;

    public float health;
    float maxHealth = 250f;
    public GameObject healthBarUI;
    public Slider slider;
    Animator m_Animator;
    hunterControl aiScr;
    GameObject hunter;

    GameObject joel;
    Transform target;
    public AudioSource[] audioSource;
    public AudioSource hitHunter;
    public AudioSource deadHunter;
    public bool dead = false;
    public GameObject particleSystem;



    void Start()
    {
        joel = GameObject.FindWithTag("Joel");
        target = joel.transform;
        
        hunterControl.MyInstance.canMove = true;
        hunterControl.MyInstance.inAttack = false;
        m_Animator = gameObject.GetComponent<Animator>();

        hunter = GameObject.FindWithTag("Hunter");
        aiScr = hunter.GetComponent<hunterControl>();

        audioSource = GetComponents<AudioSource>();
        hitHunter = audioSource[4];
        deadHunter = audioSource[5];

        health = maxHealth;
        slider.value = CalculateHealth();

    }

    void Update()
    {
        slider.value = CalculateHealth();
        if (health <= 0)
        {
            aiScr.joelSeen = false;
            m_Animator.SetBool("died", true);

            if (dead == false)
            {
                JoelScript.updateRage(false);
                InventoryScript.MyInstance.AddItem((Ingredients)Instantiate(item));
                deadHunter.Play();
                hunterControl.MyInstance.inAttack = false;
                dead = true;
                hunterControl.MyInstance.canMove = true;
                target.transform.position = new Vector3(target.transform.position.x, hunterControl.MyInstance.yPos, target.transform.position.z);
                StartCoroutine(destroyHunter());
                aiScr.enabled = false;
            }
        }

    }

    float CalculateHealth()
    {
        return health / maxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {


        if (dead == false)
        {

            if (collision.gameObject.CompareTag("AssaultRifleBullet"))
            {

                StartCoroutine(hitReaction());
                if (JoelScript.rageMode)
                {
                    health -= 66f;
                }
                else
                {
                    health -= 33f;
                }

            }
            if (collision.gameObject.CompareTag("PistolBullet"))
            {
                StartCoroutine(hitReaction());
                if (JoelScript.rageMode)
                {
                    health -= 72;
                }
                else
                {
                    health -= 36f;
                }


            }
            if (collision.gameObject.CompareTag("ShotgunBullet"))
            {
                StartCoroutine(hitReaction());
                if (JoelScript.rageMode)
                {
                    health -= 50f;
                }
                else
                {
                    health -= 25f;
                }

            }
            if (collision.gameObject.CompareTag("SMGBullet"))
            {
                StartCoroutine(hitReaction());
                if (JoelScript.rageMode)
                {
                    health -= 40f;
                }
                else
                {
                    health -= 20f;
                }

            }
            if (collision.gameObject.CompareTag("SniperBullet"))
            {
                StartCoroutine(hitReaction());
                if (JoelScript.rageMode)
                {
                    health -= 180f;
                }
                else
                {
                    health -= 90f;
                }

            }

        }

    }

    // public static void Reset()
    // {
    //     health = 250f;
    //     dead = false;
    //     hunterControl.MyInstance.canMove = false;
    // }

    IEnumerator hitReaction()
    {
        StartCoroutine(particleSystem.GetComponent<bloodScript>().opened());

        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Bite") || m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Zombie Check"))
        {

            m_Animator.SetBool("hitDown", true);
            hitHunter.Play();
            yield return new WaitForSeconds(0.4f);
            m_Animator.SetBool("hitDown", false);
        }

        else
        {

            m_Animator.SetBool("hit", true);
            hitHunter.Play();
            yield return new WaitForSeconds(0.4f);
            m_Animator.SetBool("hit", false);
        }

    }

    IEnumerator destroyHunter()
    {
        StartCoroutine(particleSystem.GetComponent<bloodScript>().opened());
        yield return new WaitForSeconds(3.0f);
        Destroy(healthBarUI.gameObject);
    }


}
