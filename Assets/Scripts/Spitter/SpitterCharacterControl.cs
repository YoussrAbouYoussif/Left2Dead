using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(SpitterPersonCharacter))]
public class SpitterCharacterControl : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent { get; private set; }
    public SpitterPersonCharacter character { get; private set; }
    GameObject prefab;
    Transform target;
    public static float healthPoints;
    private float maxHealth = 100f;
    public bool notSpit = true;
    private float meleeRange = 100f;
    private float rotationSpeed = 2f;
    public GameObject particleSystem;
    public GameObject acidPuddle;
    bool spitCreated = false;
    private Transform rotAgent;
    public GameObject healthBarUI;
    public Slider slider;
    private bool dead = false;
    [SerializeField]
    public Item item;
    public AudioSource[] audioSource;
    public AudioSource running;
    public AudioSource dying;
    public AudioSource hit;
    public AudioSource attack;
    public AudioSource breath;
    public GameObject puddle;
    // public GameObject spit;
    public static bool joelSeen = false;
    public Animator animator;
    bool checkBomb = true;
    Vector3 posBomb;
    bool bombNear = false;
    public GameObject bloodParticleSystem;
    bool rageUpdated = false;

    public static bool joelBombSeen = false;
    public static bool bileEffect = false;
    public static GameObject targetAfterBomb;
    float bileTimer = 0;
    float oldBileTimer = 0;
    public bool bombDead = false;



    public static void Reset()
    {
        healthPoints = 100f;
        joelBombSeen = false;
        joelSeen = false;
        bileEffect = false;

    }

    private bool IsInMeleeRangeOf(Transform target)
    {
        float distance = Vector3.Distance(transform.position, target.position);
        return distance < meleeRange;
    }
    private void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }



    public void Die()
    {


        if (!rageUpdated)
        {
            InventoryScript.MyInstance.AddItem((Ingredients)Instantiate(item));
            JoelScript.updateRage(false);
            rageUpdated = true;
        }
        StartCoroutine(bloodParticleSystem.GetComponent<bloodScript>().opened());

        animator.SetBool("Dying", true);
        animator.SetBool("Grounded", false);
        animator.SetBool("Attack", false);
        animator.SetBool("Idle", false);

        agent.velocity = Vector3.zero;
        agent.isStopped = true;

        dying.PlayOneShot(dying.clip, 0.7f);
        agent.GetComponent<NavMeshAgent>().Stop();
        dead = true;
        healthBarUI.SetActive(false);

    }


    private void Start()
    {
        prefab = GameObject.FindGameObjectWithTag("Joel");
        animator = GetComponent<Animator>();
        target = prefab.GetComponent<Transform>();
        puddle = Instantiate(acidPuddle, new Vector3(target.transform.position.x - 1.0f, 0.55f, target.transform.position.z - 1.0f), target.transform.rotation);
        puddle.SetActive(false);
        agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        character = GetComponent<SpitterPersonCharacter>();

        agent.updateRotation = false;
        agent.updatePosition = true;
        healthPoints = maxHealth;

        audioSource = GetComponents<AudioSource>();
        attack = audioSource[0];
        running = audioSource[1];
        hit = audioSource[2];
        dying = audioSource[3];
        breath = audioSource[4];

        running.loop = true;
        running.Play();
    }

    float CalculateHealth()
    {
        return healthPoints / maxHealth;
    }


    //when a bullet collides with the Tank than its health decreases
    void OnCollisionEnter(Collision other)
    {
        running.mute = true;
        //add pistolbullet
        if (other.gameObject.CompareTag("AssaultRifleBullet") || other.gameObject.CompareTag("PistolBullet") || other.gameObject.CompareTag("ShotgunBullet")
            || other.gameObject.CompareTag("SMGBullet") || other.gameObject.CompareTag("SniperBullet"))
        {
            //to not do the hit animation when it should die
            attack.Stop();
            hit.PlayOneShot(hit.clip, 0.7f);
            StartCoroutine(bloodParticleSystem.GetComponent<bloodScript>().opened());

            if (other.gameObject.CompareTag("AssaultRifleBullet"))
            {
                healthPoints -= 33 * (JoelScript.rageMode ? 2 : 1);
            }
            if (other.gameObject.CompareTag("PistolBullet"))
            {
                healthPoints -= 36 * (JoelScript.rageMode ? 2 : 1);
            }
            if (other.gameObject.CompareTag("SMGBullet"))
            {
                healthPoints -= 20 * (JoelScript.rageMode ? 2 : 1);
            }
            if (other.gameObject.CompareTag("SniperBullet"))
            {
                healthPoints -= 90 * (JoelScript.rageMode ? 2 : 1);
            }
            if (other.gameObject.CompareTag("ShotgunBullet"))
            {
                healthPoints -= 25 * (JoelScript.rageMode ? 2 : 1);
            }

            //character died
            if (healthPoints <= 0 && !dead)
            {
                Die();
            }



            if (healthPoints > 0)
            {
                animator.SetTrigger("HitReaction");
                animator.SetBool("Grounded", false);
                animator.SetBool("Attack", false);
                animator.SetBool("Idle", false);


            }



        }

        //if health is 0 the animation of dying will play and the agent will be stopped

        slider.value = CalculateHealth();


    }
    private IEnumerator puddleDestroy()
    {
        // GameObject puddle =  Instantiate(acidPuddle, new Vector3(target.transform.position.x-1.0f,0.55f,target.transform.position.z-1.0f), target.transform.rotation);
        puddle.transform.position = new Vector3(target.transform.position.x - 1.0f, 0.55f, target.transform.position.z - 1.0f);
        puddle.SetActive(true);
        yield return new WaitForSeconds(10f);
        puddle.SetActive(false);
    }

    private IEnumerator MyCoroutine()
    {
        spitCreated = true;
        GameObject spit = Instantiate(particleSystem, new Vector3(this.transform.position.x + 0.5f, this.transform.position.y + 1.8f, this.transform.position.z + 0.4f), this.transform.rotation);
        spit.transform.SetParent(this.transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform);
        attack.Play();
        yield return new WaitForSeconds(5f);
        attack.Stop();
        breath.PlayOneShot(breath.clip, 0.7f);
        StartCoroutine(puddleDestroy());
        Destroy(spit);
    }


    private void Update()
    {

        slider.value = CalculateHealth();
        if (!dead)
        {

            //character died
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
                if (agent.remainingDistance <= agent.stoppingDistance && !animator.GetBool("BombAttack"))
                {
                    running.mute = true;

                    animator.SetBool("BombAttack", true);

                    agent.transform.LookAt(new Vector3(targetAfterBomb.transform.position.x, targetAfterBomb.transform.position.y, targetAfterBomb.transform.position.z));
                    bileTimer = 0;
                    oldBileTimer = 0;
                }
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("BombAttack"))
                {
                    if ((((int)bileTimer) > ((int)oldBileTimer)))
                    {
                        bileTimer = oldBileTimer;
                        switch (targetAfterBomb.tag)
                        {
                            case "Tank":
                                TankCharacterControl.healthPoints -= 20;
                                break;
                            case "JockeyAttack":
                                JockeyScript.healthChanged = true;
                                JockeyScript.healthPoints -= 20;
                                break;
                            case "ChargerAttack":
                                ChargerScript.healthChanged = true;
                                ChargerScript.healthPoints -= 20;
                                break;
                            case "Hunter":
                                HealthHun.MyInstance.health -= 20;
                                break;
                            case "Boomer":
                                Health.health -= 20;
                                break;
                        }
                    }
                    bileTimer += Time.deltaTime;
                }

                //if agent distance to special infected as target is more than the stopping distance then start moving
                if (agent.remainingDistance > agent.stoppingDistance)
                {
                    // growl.Play();
                    character.Move(agent.desiredVelocity, false, false);
                    running.mute = false;
                    animator.SetBool("BombAttack", false);

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
                animator.SetBool("BombAttack", false);

                // to rotate agent when its within the stopping distance
                if (IsInMeleeRangeOf(target))
                {
                    RotateTowards(target);
                }

                if (target != null)
                    agent.SetDestination(target.position);
                //to prevent having the remainingDistance with 0 in first iteration which causes attack animation to play in the beginning
                if (agent.remainingDistance == 0)
                    return;
                //if agent reached target then trigger the attack animation
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    running.mute = true;

                    animator.SetBool("Idle", false);
                    animator.SetBool("Attack", true);
                    animator.SetBool("Grounded", false);

                }
                this.GetComponent<NavMeshAgent>().isStopped = false;
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
                {

                    if (spitCreated == false)
                    {
                        StartCoroutine(MyCoroutine());

                    }
                    this.GetComponent<NavMeshAgent>().isStopped = true;
                }

                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    spitCreated = false;

                }
                //if agent distance to target is more than the stopping distance then start moving
                if (agent.remainingDistance > agent.stoppingDistance)
                {
                    character.Move(agent.desiredVelocity, false, false);
                    running.mute = false;

                    animator.SetBool("Grounded", true);
                    animator.SetBool("Attack", false);
                    animator.SetBool("Idle", false);
                }
                //if agent distance to target is equal to stopping distance then stop moving
                else
                {
                    character.Move(Vector3.zero, false, false);
                }
            }
            else
            {

                Collider[] Colliders = Physics.OverlapSphere(this.transform.position, 20.0f);
                Collider[] furtherColliders = Physics.OverlapSphere(this.transform.position, 60.0f);

                foreach (Collider collider in Colliders)
                {
                    Rigidbody rb = collider.GetComponent<Rigidbody>();


                    if (rb != null && rb.gameObject.CompareTag("Joel") && rb.gameObject.GetComponent<AudioSource>().isPlaying)
                    {
                        joelSeen = true;
                        breath.PlayOneShot(breath.clip, 0.7f);
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
                        breath.PlayOneShot(breath.clip, 0.7f);
                    }
                    else if (checkBomb && rb != null && (((rb.gameObject.CompareTag("StunGrenade") || rb.gameObject.CompareTag("BileBomb") || rb.gameObject.CompareTag("MoltovCocktail")) && rb.gameObject.GetComponent<AudioSource>().isPlaying)
                    || (rb.gameObject.CompareTag("PipeBomb") && rb.gameObject.GetComponents<AudioSource>()[1].isPlaying)))
                    {
                        breath.PlayOneShot(breath.clip, 0.7f);
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
        else
        {
            if (bombDead == false)
            {
                Die();
                bombDead = true;
            }

        }
    }


    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    IEnumerator DieDestroy()
    {
        //destroy object after 5 seconds
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }

}

