using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
public class JockeyScript : MonoBehaviour
{
    [SerializeField]
    public Item item;
    private Transform targetJoel;
    private NavMeshAgent jockeyInfected;
    AudioSource[] jockeyAudios;
    AudioSource dyingJockeyAudio;
    AudioSource runningJockeyAudio;
    AudioSource attackedJockeyAudio;
    public GameObject bloodParticleSystem;

    Vector3 posBomb;
    bool bombNear = false;
    bool checkBomb = true;

    float timer = 0;
    float bileTimer = 0;
    float oldBileTimer = 0;
    bool joelSeen = false;

    public static float timerBreak = 4f;
    public static int healthPoints = 325;
    public static bool healthChanged = false;
    public static int randomNumber;
    public static bool jockeyEnable = true;
    public static bool bileEffect = false;
    public static bool bombCollided = false;

    public static Animator jockeyAnim;
    public static GameObject targetAfterBomb;
    // Start is called before the first frame update
    void Start()
    {
        targetJoel = GameObject.FindWithTag("Joel").GetComponent<Transform>();
        jockeyInfected = GetComponent<NavMeshAgent>();
        jockeyAnim = gameObject.GetComponent<Animator>();
        jockeyAudios = GetComponents<AudioSource>();
        runningJockeyAudio = jockeyAudios[0];
        dyingJockeyAudio = jockeyAudios[1];
        attackedJockeyAudio = jockeyAudios[2];
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PistolBullet") && !jockeyAnim.GetBool("isDead") && !jockeyAnim.GetBool("isCollided"))
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
                if (jockeyAnim.GetCurrentAnimatorStateInfo(0).IsName("Running"))
                {
                    jockeyInfected.destination = jockeyInfected.transform.position;
                }
                jockeyAnim.SetBool("isRunning", false);
                jockeyAnim.SetBool("isCollided", false);
                jockeyAnim.SetBool("isIdle", false);
                jockeyAnim.SetBool("isAttacked", true);
            }
        }
        else if (collision.gameObject.CompareTag("ShotgunBullet") && !jockeyAnim.GetBool("isDead") && !jockeyAnim.GetBool("isCollided"))
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
                if (jockeyAnim.GetCurrentAnimatorStateInfo(0).IsName("Running"))
                {
                    jockeyInfected.destination = jockeyInfected.transform.position;
                }
                jockeyAnim.SetBool("isRunning", false);
                jockeyAnim.SetBool("isCollided", false);
                jockeyAnim.SetBool("isIdle", false);
                jockeyAnim.SetBool("isAttacked", true);
            }
        }
        else if (collision.gameObject.CompareTag("AssaultRifleBullet") && !jockeyAnim.GetBool("isDead") && !jockeyAnim.GetBool("isCollided"))
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
                if (jockeyAnim.GetCurrentAnimatorStateInfo(0).IsName("Running"))
                {
                    jockeyInfected.destination = jockeyInfected.transform.position;
                }
                jockeyAnim.SetBool("isIdle", false);
                jockeyAnim.SetBool("isRunning", false);
                jockeyAnim.SetBool("isCollided", false);
                jockeyAnim.SetBool("isAttacked", true);
            }
        }
        else if (collision.gameObject.CompareTag("SniperBullet") && !jockeyAnim.GetBool("isDead") && !jockeyAnim.GetBool("isCollided"))
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
                if (jockeyAnim.GetCurrentAnimatorStateInfo(0).IsName("Running"))
                {
                    jockeyInfected.destination = jockeyInfected.transform.position;
                }
                jockeyAnim.SetBool("isIdle", false);
                jockeyAnim.SetBool("isRunning", false);
                jockeyAnim.SetBool("isCollided", false);
                jockeyAnim.SetBool("isAttacked", true);
            }
        }
        else if (collision.gameObject.CompareTag("SMGBullet") && !jockeyAnim.GetBool("isDead") && !jockeyAnim.GetBool("isCollided"))
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
                if (jockeyAnim.GetCurrentAnimatorStateInfo(0).IsName("Running"))
                {
                    jockeyInfected.destination = jockeyInfected.transform.position;
                }
                jockeyAnim.SetBool("isIdle", false);
                jockeyAnim.SetBool("isRunning", false);
                jockeyAnim.SetBool("isCollided", false);
                jockeyAnim.SetBool("isAttacked", true);
            }
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!jockeyAnim.GetBool("isDead"))
        {
            if (healthPoints <= 0)
            {
                JoelScript.updateRage(false);
                InventoryScript.MyInstance.AddItem((Ingredients)Instantiate(item));
                attackedJockeyAudio.Stop();
                runningJockeyAudio.Stop();
                if (!dyingJockeyAudio.isPlaying)
                {
                    dyingJockeyAudio.PlayOneShot(dyingJockeyAudio.clip, 0.7f);
                }
                jockeyAnim.SetBool("isRunning", false);
                jockeyAnim.SetBool("isCollided", false);
                jockeyAnim.SetBool("isAttacked", false);
                jockeyAnim.SetBool("isIdle", false);
                jockeyAnim.SetBool("isDead", true);
            }
            else if (!joelSeen)
            {
                Collider[] Colliders = Physics.OverlapSphere(this.transform.position, 20.0f);
                Collider[] furtherColliders = Physics.OverlapSphere(this.transform.position, 60.0f);

                foreach (Collider collider in Colliders)
                {
                    Rigidbody rb = collider.GetComponent<Rigidbody>();

                    if (rb != null && rb.gameObject.CompareTag("Joel") && rb.gameObject.GetComponent<AudioSource>().isPlaying && !jockeyAnim.GetBool("isDead"))
                    {
                        joelSeen = true;
                        runningJockeyAudio.Play();
                        jockeyInfected.SetDestination(targetJoel.transform.position);
                        jockeyAnim.SetBool("isRunning", true);
                        jockeyAnim.SetBool("isIdle", false);
                        jockeyAnim.SetBool("isCollided", false);
                        jockeyAnim.SetBool("isAttacked", false);
                    }
                }
                foreach (Collider collider in furtherColliders)
                {
                    Rigidbody rb = collider.GetComponent<Rigidbody>();

                    if (rb != null && rb.gameObject.CompareTag("Joel") && (SniperScriptLPFP.isFired
                    || AutomaticGunScriptLPFP.isFired
                    || HandgunScriptLPFP.isFired
                    || PumpShotgunScriptLPFP.isFired) && !jockeyAnim.GetBool("isDead"))
                    {
                        joelSeen = true;
                        runningJockeyAudio.Play();
                        jockeyInfected.SetDestination(targetJoel.transform.position);
                        jockeyAnim.SetBool("isRunning", true);
                        jockeyAnim.SetBool("isIdle", false);
                        jockeyAnim.SetBool("isCollided", false);
                        jockeyAnim.SetBool("isAttacked", false);
                    }

                    else if (checkBomb && rb != null && (((rb.gameObject.CompareTag("StunGrenade") || rb.gameObject.CompareTag("BileBomb") || rb.gameObject.CompareTag("MoltovCocktail")) && rb.gameObject.GetComponent<AudioSource>().isPlaying)
                    || (rb.gameObject.CompareTag("PipeBomb") && rb.gameObject.GetComponents<AudioSource>()[1].isPlaying)) && !jockeyAnim.GetBool("isDead"))
                    {
                        posBomb = ThrowBombs.bombPostion;
                        bombNear = true;
                        runningJockeyAudio.Play();

                    }

                    if (bombNear)
                    {
                        checkBomb = false;
                        jockeyInfected.stoppingDistance = 0;
                        jockeyInfected.SetDestination(posBomb);
                        jockeyAnim.SetBool("isRunning", true);
                        jockeyAnim.SetBool("isIdle", false);
                        if (jockeyInfected.remainingDistance <= jockeyInfected.stoppingDistance)
                        {
                            jockeyAnim.SetBool("isRunning", false);
                            jockeyAnim.SetBool("isIdle", true);
                            checkBomb = true;
                            bombNear = false;
                        }
                    }
                }
            }
            else if (joelSeen)
            {
                if (jockeyAnim.GetBool("isRunning"))
                {
                    if (!bileEffect)
                    {
                        attackedJockeyAudio.Stop();
                        jockeyInfected.SetDestination(targetJoel.position);
                        if (jockeyInfected.remainingDistance < 3f && timerBreak >= 5 && jockeyInfected.remainingDistance > 0.5f)
                        {
                            targetJoel.GetComponent<Rigidbody>().mass = 1200;
                            randomNumber = Random.Range(1, 4);
                            this.transform.LookAt(new Vector3(targetJoel.transform.position.x, targetJoel.transform.position.y, targetJoel.transform.position.z));
                            jockeyAnim.SetBool("isRunning", false);
                            jockeyAnim.SetBool("isCollided", true);
                            jockeyEnable = false;
                            timerBreak = 0;
                            runningJockeyAudio.Stop();
                        }
                        else if (jockeyInfected.remainingDistance < 3f && timerBreak < 5)
                        {
                            jockeyAnim.SetBool("isRunning", false);
                            jockeyAnim.SetBool("isCollided", false);
                            jockeyAnim.SetBool("isIdle", true);
                        }
                        else
                        {
                            jockeyAnim.SetBool("isCollided", false);
                            jockeyAnim.SetBool("isIdle", false);
                            timerBreak += Time.deltaTime;
                            jockeyInfected.SetDestination(targetJoel.position);
                            if (!runningJockeyAudio.isPlaying)
                            {
                                runningJockeyAudio.Play();
                            }
                        }
                    }
                    else
                    {
                        if (targetAfterBomb != null)
                        {
                            if (targetAfterBomb.tag != "JockeyAttack")
                            {
                                jockeyInfected.SetDestination(targetAfterBomb.transform.position);
                                if (jockeyInfected.remainingDistance < 2f)
                                {
                                    jockeyAnim.SetBool("isRunning", false);
                                    jockeyAnim.SetBool("isAttackedBile", true);
                                    jockeyInfected.transform.LookAt(new Vector3(targetAfterBomb.transform.position.x, targetAfterBomb.transform.position.y, targetAfterBomb.transform.position.z));
                                    jockeyEnable = true;
                                    bileTimer = 0;
                                    oldBileTimer = 0;
                                }
                                else
                                {
                                    timerBreak += Time.deltaTime;
                                    jockeyAnim.SetBool("isAttackedBile", false);
                                }
                            }
                        }
                        else
                        {
                            jockeyInfected.SetDestination(jockeyInfected.transform.position);
                        }
                    }
                }
                else if (jockeyAnim.GetBool("isAttackedBile"))
                {
                    if (bileEffect)
                    {
                        if ((((int)bileTimer) > ((int)oldBileTimer)))
                        {
                            oldBileTimer = bileTimer;
                            switch (targetAfterBomb.tag)
                            {
                                case "ChargerAttack":
                                    ChargerScript.healthChanged = true;
                                    ChargerScript.healthPoints -= 25;
                                    break;
                                case "Tank":
                                    TankCharacterControl.healthPoints -= 25;
                                    break;
                                case "Spitter":
                                    SpitterCharacterControl.healthPoints -= 25;
                                    break;
                                case "Boomer":
                                    Health.health -= 25;
                                    break;
                                case "Hunter":
                                    HealthHun.MyInstance.health -= 25;
                                    break;
                                case "yaku":
                                    AICharacterControl x = targetAfterBomb.gameObject.GetComponent<AICharacterControl>();
                                    x.setHealth(-25);
                                    break;

                            }
                        }
                        bileTimer += Time.deltaTime;
                    }
                }
                else if (jockeyAnim.GetBool("isCollided"))
                {
                    attackedJockeyAudio.Stop();
                    if (jockeyEnable)
                    {
                        jockeyAnim.SetBool("isCollided", false);
                        jockeyAnim.SetBool("isAttacked", false);
                        jockeyAnim.SetBool("isIdle", false);
                        jockeyAnim.SetBool("isRunning", true);
                    }
                }
                else if (jockeyAnim.GetBool("isIdle"))
                {
                    if (!bileEffect)
                    {
                        attackedJockeyAudio.Stop();
                        if (timerBreak < 5 && !joelSeen)
                        {
                            runningJockeyAudio.Stop();
                            timerBreak += Time.deltaTime;
                            targetJoel.GetComponent<Rigidbody>().mass = 5;
                            jockeyInfected.destination = jockeyInfected.transform.position;
                        }
                        else if (timerBreak < 5 && joelSeen)
                        {
                            timerBreak += Time.deltaTime;
                            jockeyInfected.SetDestination(targetJoel.position);
                            if (jockeyInfected.remainingDistance >= 2.3f && !bombCollided)
                            {
                                jockeyAnim.SetBool("isRunning", true);
                                jockeyAnim.SetBool("isCollided", false);
                                jockeyAnim.SetBool("isIdle", false);
                            }
                            if (bombCollided)
                            {
                                jockeyInfected.destination = jockeyInfected.transform.position;
                            }
                        }
                        else if (timerBreak >= 5 && joelSeen && jockeyInfected.remainingDistance < 2.3f)
                        {
                            jockeyAnim.SetBool("isRunning", false);
                            jockeyAnim.SetBool("isCollided", true);
                            jockeyAnim.SetBool("isIdle", false);
                            timerBreak = 0f;
                        }
                        else if (timerBreak >= 5 && joelSeen && jockeyInfected.remainingDistance > 2.3f)
                        {
                            jockeyAnim.SetBool("isRunning", true);
                            jockeyAnim.SetBool("isCollided", false);
                            jockeyAnim.SetBool("isIdle", false);
                        }
                    }
                }
                else if (jockeyAnim.GetBool("isAttacked"))
                {
                    runningJockeyAudio.Stop();
                    targetJoel.GetComponent<Rigidbody>().mass = 5;
                    if (timer > 1.8f)
                    {
                        jockeyInfected.SetDestination(targetJoel.position);
                        timer = 0;
                        jockeyAnim.SetBool("isAttacked", false);
                        jockeyAnim.SetBool("isCollided", false);
                        jockeyAnim.SetBool("isIdle", false);
                        jockeyAnim.SetBool("isRunning", true);
                        jockeyEnable = true;
                        attackedJockeyAudio.Stop();
                    }
                    else
                    {
                        timer += Time.deltaTime;
                        timerBreak += Time.deltaTime;
                    }
                }
            }
        }
    }
    public static void Reset()
    {
        timerBreak = 4f;
        healthPoints = 325;
        healthChanged = false;
        jockeyEnable = true;
        bileEffect = false;
        jockeyAnim.SetBool("isIdle", true);
        jockeyAnim.SetBool("isRunning", false);
        jockeyAnim.SetBool("isCollided", false);
        jockeyAnim.SetBool("isAttacked", false);
        jockeyAnim.SetBool("isAttackedBile", false);
        jockeyAnim.SetBool("isDead", false);
    }
}
