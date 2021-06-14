using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(TankPersonCharacter))]
public class TankCharacterControl : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent { get; private set; }              // the navmesh agent required for the path finding
    public TankPersonCharacter character { get; private set; }                 // the character we are controlling
    Transform target;
    [SerializeField]
    public Item item;
    public GameObject particleSystem;

    // target to aim for
    public static float healthPoints;
    private float maxHealth = 1000f;
    public GameObject healthBarUI;
    public Slider slider;
    float timer = 0;
    private bool dead = false;
    public AudioSource[] audioSource;
    public AudioSource growl;
    public AudioSource running;
    public AudioSource dying;
    public AudioSource hit;
    public AudioSource attack;
    public AudioSource scream;
    public AudioSource bulletHit;
    private bool attacking = false;
    public static bool joelSeen = false;

    public static bool joelBombSeen = false;
    bool checkBomb = true;
    Vector3 posBomb;
    bool bombNear = false;

    public Animator animator;
    public static bool bileEffect = false;
    public static GameObject targetAfterBomb;
    float bileTimer = 0;
    float oldBileTimer = 0;




    public static void Reset()
    {
        healthPoints = 1000f;
        joelBombSeen = false;
        joelSeen = false;
        bileEffect = false;
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Joel").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        character = GetComponent<TankPersonCharacter>();

        agent.updateRotation = false;
        agent.updatePosition = true;
        healthPoints = maxHealth;
        audioSource = GetComponents<AudioSource>();

        growl = audioSource[0];
        running = audioSource[1];
        dying = audioSource[2];
        hit = audioSource[3];
        attack = audioSource[4];
        scream = audioSource[5];
        bulletHit = audioSource[6];

        // growl.Play();
        running.loop = true;
        running.Play();
        running.mute = true;

    }

    float CalculateHealth()
    {
        return healthPoints / maxHealth;
    }

    public void Die()
    {
        InventoryScript.MyInstance.AddItem((Ingredients)Instantiate(item));
        JoelScript.updateRage(false);
        this.gameObject.GetComponent<Collider>().enabled = false;
        animator.SetBool("Dying", true);
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
        StartCoroutine(particleSystem.GetComponent<bloodScript>().opened());
        dying.PlayOneShot(dying.clip, 0.2f);
        dead = true;
        running.Stop();
        attack.Stop();
        healthBarUI.SetActive(false);
    }
    //when a bullet collides with the Tank than its health decreases
    void OnCollisionEnter(Collision other)
    {

        //add pistolbullet
        if (other.gameObject.CompareTag("AssaultRifleBullet") || other.gameObject.CompareTag("PistolBullet") || other.gameObject.CompareTag("ShotgunBullet")
         || other.gameObject.CompareTag("SMGBullet") || other.gameObject.CompareTag("SniperBullet"))
        {

            hit.PlayOneShot(hit.clip, 0.7f);
            bulletHit.PlayOneShot(bulletHit.clip, 0.7f);
            running.mute = true;
            if (other.gameObject.CompareTag("AssaultRifleBullet"))
            {
                healthPoints -= 33 * (JoelScript.rageMode ? 2 : 1);
            }
            if (other.gameObject.CompareTag("PistolBullet"))
            {
                healthPoints -= 36 * (JoelScript.rageMode ? 2 : 1);
            }
            if (other.gameObject.CompareTag("ShotgunBullet"))
            {
                healthPoints -= 25 * (JoelScript.rageMode ? 2 : 1);
            }
            if (other.gameObject.CompareTag("SMGBullet"))
            {
                healthPoints -= 20 * (JoelScript.rageMode ? 2 : 1);
            }
            if (other.gameObject.CompareTag("SniperBullet"))
            {
                healthPoints -= 90 * (JoelScript.rageMode ? 2 : 1);
            }

            //to not do the hit animation when it should die
            if (healthPoints > 0)
            {
                //blood effect
                StartCoroutine(particleSystem.GetComponent<bloodScript>().opened());

                animator.SetTrigger("HitReaction");
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
                {
                    animator.SetBool("Attack", false);
                }
            }

        }

        //if health is 0 the animation of dying will play and the agent will be stopped
        if (healthPoints <= 0 && !dead)
        {
            Die();
        }
        slider.value = CalculateHealth();
    }

    private IEnumerator attackJoel()
    {
        attacking = true;
        attack.Play();
        yield return new WaitForSeconds(2f);
        JoelScript.changeHealth(-30);
        attack.Stop();
        scream.PlayOneShot(scream.clip, 0.7f);

    }



    private void FixedUpdate()
    {
        slider.value = CalculateHealth();
        if (!dead)
        {

            if (healthPoints <= 0 && !dead)
            {
                Die();
            }
            if (joelBombSeen)
            {
                agent.SetDestination(agent.transform.position);
                character.Move(Vector3.zero, false, false);
            }

            if (bileEffect)
            {

                if (targetAfterBomb != null)
                {
                    agent.SetDestination(targetAfterBomb.transform.position);
                }
                else
                {
                    agent.SetDestination(agent.transform.position);
                    // character.Move(Vector3.zero, false, false);
                }

                //start attacking special infected as target
                if (agent.remainingDistance < agent.stoppingDistance && !animator.GetBool("Attack"))
                {
                    animator.SetBool("Attack", true);
                    running.mute = true;

                    agent.transform.LookAt(new Vector3(targetAfterBomb.transform.position.x, targetAfterBomb.transform.position.y, targetAfterBomb.transform.position.z));
                    bileTimer = 0;
                    oldBileTimer = 0;
                }
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
                {
                    if ((((int)bileTimer) > ((int)oldBileTimer)))
                    {
                        bileTimer = oldBileTimer;
                        switch (targetAfterBomb.tag)
                        {
                            case "Spitter":
                                SpitterCharacterControl.healthPoints -= 30;
                                break;
                            case "JockeyAttack":
                                JockeyScript.healthChanged = true;
                                JockeyScript.healthPoints -= 30;
                                break;
                            case "ChargerAttack":
                                ChargerScript.healthChanged = true;
                                ChargerScript.healthPoints -= 30;
                                break;
                            case "Hunter":
                                HealthHun.MyInstance.health -= 30;
                                break;
                            case "Boomer":
                                Health.health -= 30;
                                break;
                        }
                    }
                    bileTimer += Time.deltaTime;
                }

                //if agent distance to special infected as target is more than the stopping distance then start moving
                if (agent.remainingDistance > agent.stoppingDistance)
                {
                    // growl.Play();
                    running.mute = false;
                    character.Move(agent.desiredVelocity, false, false);
                    animator.SetBool("Attack", false);
                    animator.SetTrigger("Run");
                }
                //if agent distance to special infected as target is equal to stopping distance then stop moving
                else
                {
                    character.Move(Vector3.zero, false, false);
                }
                // agent.SetDestination(target.position);
            }


            if (joelSeen && !joelBombSeen && !bileEffect)
            {
                if (target != null)
                    agent.SetDestination(target.position);
                //to prevent having the remainingDistance with 0 in first iteration which causes attack animation to play in the beginning
                if (agent.remainingDistance == 0)
                    return;
                //if agent reached target then trigger the attack animation
                if (agent.remainingDistance <= agent.stoppingDistance && !animator.GetBool("Attack") && agent.remainingDistance > 0.5)
                {
                    // attack.mute=false;
                    animator.SetBool("Attack", true);
                    running.mute = true;
                }

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
                {
                    if (attacking == false)
                    {
                        StartCoroutine(attackJoel());
                    }
                }

                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Screaming"))
                {
                    attacking = false;
                }

                //if agent distance to target is more than the stopping distance then start moving
                if (agent.remainingDistance > agent.stoppingDistance)
                {
                    // growl.Play();
                    running.mute = false;
                    character.Move(agent.desiredVelocity, false, false);
                    animator.SetBool("Attack", false);
                    animator.SetTrigger("Run");
                }
                //if agent distance to target is equal to stopping distance then stop moving
                else
                {
                    character.Move(Vector3.zero, false, false);
                }
            }

            else if (!joelSeen && !joelBombSeen)
            {

                character.Move(Vector3.zero, false, false);

                Collider[] Colliders = Physics.OverlapSphere(this.transform.position, 20.0f);
                Collider[] furtherColliders = Physics.OverlapSphere(this.transform.position, 60.0f);

                foreach (Collider collider in Colliders)
                {
                    Rigidbody rb = collider.GetComponent<Rigidbody>();


                    if (rb != null && rb.gameObject.CompareTag("Joel") && rb.gameObject.GetComponent<AudioSource>().isPlaying)
                    {
                        joelSeen = true;
                        growl.Play();
                    }

                }

                foreach (Collider collider in furtherColliders)
                {
                    Rigidbody rb = collider.GetComponent<Rigidbody>();

                    if (rb != null && rb.gameObject.CompareTag("Joel") && (SniperScriptLPFP.isFired
                    || AutomaticGunScriptLPFP.isFired
                    || HandgunScriptLPFP.isFired
                    || PumpShotgunScriptLPFP.isFired))
                    {
                        joelSeen = true;
                        growl.Play();
                    }
                    else if (checkBomb && rb != null && (((rb.gameObject.CompareTag("StunGrenade") || rb.gameObject.CompareTag("BileBomb") || rb.gameObject.CompareTag("MoltovCocktail")) && rb.gameObject.GetComponent<AudioSource>().isPlaying)
                    || (rb.gameObject.CompareTag("PipeBomb") && rb.gameObject.GetComponents<AudioSource>()[1].isPlaying)))
                    {
                        growl.Play();
                        posBomb = ThrowBombs.bombPostion;
                        bombNear = true;

                    }
                    if (bombNear)
                    {
                        checkBomb = false;
                        agent.SetDestination(posBomb);
                        character.Move(agent.desiredVelocity, false, false);
                        if (agent.remainingDistance <= agent.stoppingDistance)
                        {
                            character.Move(Vector3.zero, false, false);
                            checkBomb = true;
                            bombNear = false;
                        }

                    }
                }
            }
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}

