using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
public class ChargerScript : MonoBehaviour
{
    [SerializeField]
    public Item item;
    private Transform targetJoel;
    private NavMeshAgent chargerInfected;
    AudioSource[] chargerAudios;
    AudioSource dyingChargerAudio;
    AudioSource runningChargerAudio;
    AudioSource attackedChargerAudio;
    public GameObject bloodParticleSystem;

    Vector3 posBomb;
    bool bombNear = false;
    bool checkBomb = true;

    int counter = 0;
    float timer = 0;
    bool joelSeen = false;
    float bileTimer = 0;
    float oldBileTimer = 0;

    public static float healthPoints = 600;
    public static float timerBreak = 4f;
    public static bool healthChanged = false;
    public static bool bileEffect = false;
    public static bool bombCollided = false;

    public static Animator chargerAnim;
    public static GameObject targetAfterBomb;
    // Start is called before the first frame update
    void Start()
    {
        targetJoel = GameObject.FindWithTag("Joel").GetComponent<Transform>();
        chargerInfected = GetComponent<NavMeshAgent>();
        chargerAnim = GetComponent<Animator>();
        chargerAudios = GetComponents<AudioSource>();
        attackedChargerAudio = chargerAudios[0];
        dyingChargerAudio = chargerAudios[1];
        runningChargerAudio = chargerAudios[2];
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PistolBullet") && !chargerAnim.GetBool("isDead"))
        {
            StartCoroutine(bloodParticleSystem.GetComponent<bloodScript>().opened());

            if (JoelScript.rageMode)
            {
                healthPoints = healthPoints - 72;
            }
            else
            {
                healthPoints = healthPoints - 36;
            }
            healthChanged = true;
            if (healthPoints > 0)
            {
                chargerAnim.SetBool("isRunning", false);
                chargerAnim.SetBool("isCollided", false);
                chargerAnim.SetBool("isIdle", false);
                chargerAnim.SetBool("isAttacked", true);
                chargerInfected.destination = chargerInfected.transform.position;
            }
        }
        else if (collision.gameObject.CompareTag("ShotgunBullet") && !chargerAnim.GetBool("isDead"))
        {
            StartCoroutine(bloodParticleSystem.GetComponent<bloodScript>().opened());

            if (JoelScript.rageMode)
            {
                healthPoints -= 50;
            }
            else
            {
                healthPoints -= 25;
            }
            healthChanged = true;
            if (healthPoints > 0)
            {
                chargerAnim.SetBool("isRunning", false);
                chargerAnim.SetBool("isCollided", false);
                chargerAnim.SetBool("isIdle", false);
                chargerAnim.SetBool("isAttacked", true);
                chargerInfected.destination = chargerInfected.transform.position;
            }
        }
        else if (collision.gameObject.CompareTag("AssaultRifleBullet") && !chargerAnim.GetBool("isDead"))
        {
            StartCoroutine(bloodParticleSystem.GetComponent<bloodScript>().opened());

            if (JoelScript.rageMode)
            {
                healthPoints -= 66;
            }
            else
            {
                healthPoints -= 33;
            }
            healthChanged = true;
            if (healthPoints > 0)
            {
                chargerAnim.SetBool("isRunning", false);
                chargerAnim.SetBool("isCollided", false);
                chargerAnim.SetBool("isIdle", false);
                chargerAnim.SetBool("isAttacked", true);
                chargerInfected.destination = chargerInfected.transform.position;
            }
        }
        else if (collision.gameObject.CompareTag("SniperBullet") && !chargerAnim.GetBool("isDead"))
        {
            StartCoroutine(bloodParticleSystem.GetComponent<bloodScript>().opened());

            if (JoelScript.rageMode)
            {
                healthPoints -= 180;
            }
            else
            {
                healthPoints -= 90;
            }
            healthChanged = true;
            if (healthPoints > 0)
            {
                chargerAnim.SetBool("isRunning", false);
                chargerAnim.SetBool("isCollided", false);
                chargerAnim.SetBool("isIdle", false);
                chargerAnim.SetBool("isAttacked", true);
                chargerInfected.destination = chargerInfected.transform.position;
            }
        }
        else if (collision.gameObject.CompareTag("SMGBullet") && !chargerAnim.GetBool("isDead"))
        {
            StartCoroutine(bloodParticleSystem.GetComponent<bloodScript>().opened());

            if (JoelScript.rageMode)
            {
                healthPoints -= 40;
            }
            else
            {
                healthPoints -= 20;
            }
            healthChanged = true;
            if (healthPoints > 0)
            {
                chargerAnim.SetBool("isRunning", false);
                chargerAnim.SetBool("isCollided", false);
                chargerAnim.SetBool("isIdle", false);
                chargerAnim.SetBool("isAttacked", true);
                chargerInfected.destination = chargerInfected.transform.position;
            }
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!chargerAnim.GetBool("isDead"))
        {
            if (healthPoints <= 0)
            {
                JoelScript.updateRage(false);
                InventoryScript.MyInstance.AddItem((Ingredients)Instantiate(item));
                this.gameObject.GetComponent<Collider>().enabled = false;
                chargerAnim.SetBool("isRunning", false);
                chargerAnim.SetBool("isCollided", false);
                chargerAnim.SetBool("isAttacked", false);
                chargerAnim.SetBool("isIdle", false);
                chargerAnim.SetBool("isDead", true);
                if (!dyingChargerAudio.isPlaying)
                {
                    dyingChargerAudio.PlayOneShot(dyingChargerAudio.clip, 0.7f);
                }
                runningChargerAudio.Stop();
                attackedChargerAudio.Stop();
                chargerInfected.destination = chargerInfected.transform.position;
            }
            else if (!joelSeen)
            {
                Collider[] Colliders = Physics.OverlapSphere(this.transform.position, 20.0f);
                Collider[] furtherColliders = Physics.OverlapSphere(this.transform.position, 60.0f);

                foreach (Collider collider in Colliders)
                {
                    Rigidbody rb = collider.GetComponent<Rigidbody>();

                    if (rb != null && rb.gameObject.CompareTag("Joel") && rb.gameObject.GetComponent<AudioSource>().isPlaying && !chargerAnim.GetBool("isDead"))
                    {
                        joelSeen = true;
                        runningChargerAudio.Play();
                        chargerInfected.SetDestination(targetJoel.transform.position);
                        chargerAnim.SetBool("isRunning", true);
                        chargerAnim.SetBool("isIdle", false);
                        chargerAnim.SetBool("isCollided", false);
                        chargerAnim.SetBool("isAttacked", false);
                    }
                }
                foreach (Collider collider in furtherColliders)
                {
                    Rigidbody rb = collider.GetComponent<Rigidbody>();

                    if (rb != null && rb.gameObject.CompareTag("Joel") && (SniperScriptLPFP.isFired
                    || AutomaticGunScriptLPFP.isFired
                    || HandgunScriptLPFP.isFired
                    || PumpShotgunScriptLPFP.isFired) && !chargerAnim.GetBool("isDead"))
                    {
                        joelSeen = true;
                        runningChargerAudio.Play();
                        chargerInfected.SetDestination(targetJoel.transform.position);
                        chargerAnim.SetBool("isRunning", true);
                        chargerAnim.SetBool("isIdle", false);
                        chargerAnim.SetBool("isCollided", false);
                        chargerAnim.SetBool("isAttacked", false);
                    }

                    else if (checkBomb && rb != null && (((rb.gameObject.CompareTag("StunGrenade") || rb.gameObject.CompareTag("BileBomb") || rb.gameObject.CompareTag("MoltovCocktail")) && rb.gameObject.GetComponent<AudioSource>().isPlaying)
                    || (rb.gameObject.CompareTag("PipeBomb") && rb.gameObject.GetComponents<AudioSource>()[1].isPlaying)) && !chargerAnim.GetBool("isDead"))
                    {
                        posBomb = ThrowBombs.bombPostion;
                        bombNear = true;
                        runningChargerAudio.Play();

                    }

                    if (bombNear)
                    {
                        checkBomb = false;
                        chargerInfected.stoppingDistance = 0;
                        chargerInfected.SetDestination(posBomb);
                        chargerAnim.SetBool("isRunning", true);
                        chargerAnim.SetBool("isIdle", false);
                        if (chargerInfected.remainingDistance <= 4)
                        {
                            chargerAnim.SetBool("isRunning", false);
                            chargerAnim.SetBool("isIdle", true);
                            checkBomb = true;
                            bombNear = false;
                        }
                    }
                }
            }
            else if (joelSeen)
            {
                if (chargerAnim.GetBool("isRunning"))
                {
                    if (!bileEffect)
                    {
                        attackedChargerAudio.Stop();
                        chargerInfected.SetDestination(targetJoel.transform.position);
                        if (chargerInfected.remainingDistance < 3f && timerBreak >= 5 && chargerInfected.remainingDistance > 0.5f)
                        {

                            targetJoel.GetComponent<Rigidbody>().mass = 1200;
                            chargerInfected.transform.LookAt(new Vector3(targetJoel.position.x, targetJoel.position.y, targetJoel.position.z));
                            chargerAnim.SetBool("isRunning", false);
                            chargerAnim.SetBool("isCollided", true);
                            timerBreak = 0;
                            runningChargerAudio.Stop();
                        }
                        else if (chargerInfected.remainingDistance < 3f && timerBreak < 5)
                        {
                            chargerAnim.SetBool("isRunning", false);
                            chargerAnim.SetBool("isCollided", false);
                            chargerAnim.SetBool("isIdle", true);
                        }
                        else
                        {
                            chargerAnim.SetBool("isCollided", false);
                            chargerAnim.SetBool("isIdle", false);
                            timerBreak += Time.deltaTime;
                            chargerInfected.SetDestination(targetJoel.transform.position);
                            if (!runningChargerAudio.enabled)
                            {
                                runningChargerAudio.Play();
                            }
                        }
                    }
                    else
                    {
                        if (targetAfterBomb != null)
                        {
                            if (targetAfterBomb.tag != "ChargerAttack")
                            {
                                chargerInfected.SetDestination(targetAfterBomb.transform.position);
                                if (chargerInfected.remainingDistance < 3f)
                                {
                                    chargerAnim.SetBool("isRunning", false);
                                    chargerAnim.SetBool("isCollided", true);
                                    chargerInfected.transform.LookAt(new Vector3(targetAfterBomb.transform.position.x, targetAfterBomb.transform.position.y, targetAfterBomb.transform.position.z));
                                    bileTimer = 0;
                                    oldBileTimer = 0;
                                }
                                else
                                {
                                    timerBreak += Time.deltaTime;
                                    chargerAnim.SetBool("isCollided", false);
                                }
                            }
                        }
                        else
                        {
                            chargerInfected.SetDestination(chargerInfected.transform.position);
                        }
                    }
                }
                else if (chargerAnim.GetBool("isCollided"))
                {
                    if (bileEffect)
                    {
                        if ((((int)bileTimer) > ((int)oldBileTimer)))
                        {
                            bileTimer = oldBileTimer;
                            switch (targetAfterBomb.tag)
                            {
                                case "JockeyAttack":
                                    JockeyScript.healthChanged = true;
                                    JockeyScript.healthPoints -= 75;
                                    break;
                                case "Tank":
                                    TankCharacterControl.healthPoints -= 75;
                                    break;
                                case "Spitter":
                                    SpitterCharacterControl.healthPoints -= 75;
                                    break;
                                case "Boomer":
                                    Health.health -= 75;
                                    break;
                                case "Hunter":
                                    HealthHun.MyInstance.health -= 75;
                                    break;
                                case "yaku":
                                    AICharacterControl x = targetAfterBomb.gameObject.GetComponent<AICharacterControl>();
                                    x.setHealth(-25);
                                    break;
                            }
                        }
                        bileTimer += Time.deltaTime;
                    }
                    else
                    {
                        if (counter == 15)
                        {
                            targetJoel.Translate(0, 1f, -3f);
                            JoelScript.changeHealth(-75);
                        }
                        if (timer > 1f)
                        {
                            timer = 0;
                            counter = 0;
                            chargerAnim.SetBool("isRunning", true);
                            chargerAnim.SetBool("isIdle", false);
                            chargerAnim.SetBool("isCollided", false);
                            chargerAnim.SetBool("isAttacked", false);
                            targetJoel.GetComponent<Rigidbody>().mass = 5;
                            attackedChargerAudio.Stop();
                        }
                        else
                        {
                            counter++;
                            timer += Time.deltaTime;
                        }
                    }
                }
                else if (chargerAnim.GetBool("isAttacked"))
                {
                    targetJoel.GetComponent<Rigidbody>().mass = 5;
                    runningChargerAudio.Stop();
                    if (timer > 1.3f)
                    {
                        chargerInfected.SetDestination(targetJoel.transform.position);
                        timer = 0;
                        chargerAnim.SetBool("isRunning", true);
                        chargerAnim.SetBool("isAttacked", false);
                        chargerAnim.SetBool("isCollided", false);
                        chargerAnim.SetBool("isIdle", false);
                        attackedChargerAudio.Stop();
                    }
                    else
                    {
                        timer += Time.deltaTime;
                        timerBreak += Time.deltaTime;
                    }
                }
                else if (chargerAnim.GetBool("isIdle"))
                {
                    if (!bileEffect)
                    {
                        attackedChargerAudio.Stop();
                        if (timerBreak < 5 && !joelSeen)
                        {
                            chargerInfected.destination = chargerInfected.transform.position;
                            timerBreak += Time.deltaTime;
                            runningChargerAudio.Stop();
                            attackedChargerAudio.Stop();
                            targetJoel.GetComponent<Rigidbody>().mass = 5;
                        }
                        else if (timerBreak < 5 && joelSeen)
                        {
                            timerBreak += Time.deltaTime;
                            chargerInfected.SetDestination(targetJoel.position);
                            if (chargerInfected.remainingDistance >= 3f && !bombCollided)
                            {
                                chargerAnim.SetBool("isRunning", true);
                                chargerAnim.SetBool("isCollided", false);
                                chargerAnim.SetBool("isIdle", false);
                            }
                            if (bombCollided)
                            {
                                chargerInfected.destination = chargerInfected.transform.position;
                            }
                        }
                        else if (timerBreak >= 5 && joelSeen && chargerInfected.remainingDistance < 3f)
                        {
                            chargerAnim.SetBool("isRunning", false);
                            chargerAnim.SetBool("isCollided", true);
                            chargerAnim.SetBool("isIdle", false);
                            timerBreak = 0f;
                        }
                        else if (timerBreak >= 5 && joelSeen && chargerInfected.remainingDistance > 3f)
                        {
                            chargerInfected.SetDestination(targetJoel.position);
                            chargerAnim.SetBool("isRunning", true);
                            chargerAnim.SetBool("isCollided", false);
                            chargerAnim.SetBool("isIdle", false);
                        }
                    }
                }
            }
        }
    }
    public static void Reset()
    {
        healthPoints = 600;
        timerBreak = 4f;
        healthChanged = false;
        bileEffect = false;
        bombCollided = false;
        chargerAnim.SetBool("isIdle", true);
        chargerAnim.SetBool("isRunning", false);
        chargerAnim.SetBool("isCollided", false);
        chargerAnim.SetBool("isAttacked", false);
        chargerAnim.SetBool("isDead", false);
    }
}
