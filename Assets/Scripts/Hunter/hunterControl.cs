using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(ThirdPersonCharacterHunt))]
public class hunterControl : MonoBehaviour
{

    private static hunterControl instance;

    public static hunterControl MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<hunterControl>();
            }

            return instance;
        }
    }

    public UnityEngine.AI.NavMeshAgent agent { get; private set; }
    public ThirdPersonCharacterHunt character { get; private set; }
    Transform target;

    Animator m_Animator;
    public bool joelSeen = false;
    bool targetReached = false;
    public float yPos;
    public bool inAttack = false;

    JoelScript joelScr;

    bool damage = false;
    GameObject joel;
    bool stopSoun = true;
    public bool canMove = true;

    GameObject mainCamera;
    public GameObject bloodPanel;

    bool bloodEff = false;

    bool bombNear = false;
    Vector3 posBomb;

    public AudioSource[] audioSource;
    public AudioSource biteSound;
    public AudioSource screamSound;
    public AudioSource joelSuspected;
    public AudioSource attackOnJoel;
    public AudioSource checkSound;
    bool checkBomb = true;

    public bool joelBombSeen = false;
    public bool bileEffect = false;
    public static GameObject targetAfterBomb;
    float bileTimer = 0f;
    float oldBileTimer = 0f;
    bool hitEachOther = false;



    private void Start()
    {

        m_Animator = gameObject.GetComponent<Animator>();

        joel = GameObject.FindWithTag("Joel");
        target = joel.transform;
        yPos = target.position.y;
        agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        character = GetComponent<ThirdPersonCharacterHunt>();
        joelScr = joel.GetComponent<JoelScript>();

        audioSource = GetComponents<AudioSource>();
        biteSound = audioSource[0];
        screamSound = audioSource[1];
        joelSuspected = audioSource[2];
        attackOnJoel = audioSource[3];
        checkSound = audioSource[6];

        agent.updateRotation = false;
        agent.updatePosition = true;

    }


    private void Update()
    {
        if (bileEffect && !HealthHun.MyInstance.dead)
        {

            if (targetAfterBomb != null && targetAfterBomb.tag != "Hunter")
            {
                agent.SetDestination(targetAfterBomb.transform.position);
                character.Move(agent.desiredVelocity, false, false);
                hitEachOther = true;
            }
            else if (!joelSeen && !joelBombSeen && (targetAfterBomb == null || targetAfterBomb.tag == "Hunter"))
            {
                agent.SetDestination(agent.transform.position);
                character.Move(Vector3.zero, false, false);
            }

            //start attacking special infected as target
            if (agent.remainingDistance <= agent.stoppingDistance && !m_Animator.GetBool("bombAttack") && targetAfterBomb.tag != "Hunter")
            {
                m_Animator.SetBool("bombAttack", true);

                agent.transform.LookAt(new Vector3(targetAfterBomb.transform.position.x, targetAfterBomb.transform.position.y, targetAfterBomb.transform.position.z));
                bileTimer = 0;
                oldBileTimer = 0;
            }
            if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Bomb Attack"))
            {

                if ((((int)bileTimer) > ((int)oldBileTimer)))
                {
                    bileTimer = oldBileTimer;
                    switch (targetAfterBomb.tag)
                    {
                        case "Spitter":
                            SpitterCharacterControl.healthPoints -= 10;
                            break;
                        case "JockeyAttack":
                            JockeyScript.healthChanged = true;
                            JockeyScript.healthPoints -= 10;
                            break;
                        case "ChargerAttack":
                            ChargerScript.healthChanged = true;
                            ChargerScript.healthPoints -= 10;
                            break;
                        case "Boomer":
                            Health.health -= 10;
                            break;
                        case "Tank":
                            TankCharacterControl.healthPoints -= 10;
                            break;
                    }
                }
                bileTimer += Time.deltaTime;
            }


        }
        else
        {
            m_Animator.SetBool("bombAttack", false);
            hitEachOther = false;
        }

        if (joelBombSeen && !HealthHun.MyInstance.dead)
        {
            agent.SetDestination(agent.transform.position);
            character.Move(Vector3.zero, false, false);
        }

        if (!inAttack && HealthHun.MyInstance.dead)
        {
            target.transform.position = new Vector3(target.transform.position.x, yPos, target.transform.position.z);
        }

        if (joelSeen && !HealthHun.MyInstance.dead && !joelBombSeen && !hitEachOther)
        {

            if (target != null && !HealthHun.MyInstance.dead)
                agent.SetDestination(target.position);

            if ((agent.remainingDistance - agent.stoppingDistance) > 3.5f)
            {
                m_Animator.SetBool("attack", false);
                character.Move(agent.desiredVelocity, false, false);
                m_Animator.SetBool("bite", false);

            }
            else if ((agent.remainingDistance - agent.stoppingDistance) <= 2.5f && (agent.remainingDistance - agent.stoppingDistance) > 2.0f)
            {
                attackOnJoel.Play();
                canMove = false;
                m_Animator.SetBool("attack", true);
                transform.LookAt(new Vector3(target.position.x, this.transform.position.y, target.position.z));
            }

            if (agent.remainingDistance <= agent.stoppingDistance)
            {

                m_Animator.SetBool("bite", true);

            }
            else
            {
                m_Animator.SetBool("bite", false);

            }
            if (!inAttack)
            {
                yPos = target.transform.position.y;
            }
            if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Bite") ||
                m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Zombie Check") ||
                m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Hit Down"))
            {
                Rigidbody m_Rigidbody = GetComponent<Rigidbody>();
                m_Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY |
                 RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationX;

                canMove = false;
                if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Bite"))
                {
                    if (stopSoun == true)
                    {
                        biteSound.Play();
                        screamSound.Play();
                        stopSoun = false;

                    }
                    if (damage == false)
                    {
                        StartCoroutine(joelDamage());

                    }

                    JoelScript.MyInstance.DamageEffect();
                }
                if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Zombie Check"))
                {
                    if (stopSoun == false)
                    {
                        checkSound.Play();
                        stopSoun = true;
                    }
                }

                target.transform.position = new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z);
                inAttack = true;
            }
            else
            {
                canMove = true;
                target.transform.position = new Vector3(target.transform.position.x, yPos, target.transform.position.z);
                inAttack = false;
            }

        }

        else
        {
            character.Move(Vector3.zero, false, false);

            Collider[] Colliders = Physics.OverlapSphere(this.transform.position, 20.0f);
            Collider[] furtherColliders = Physics.OverlapSphere(this.transform.position, 60.0f);

            foreach (Collider collider in Colliders)
            {
                Rigidbody rb = collider.GetComponent<Rigidbody>();

                if (rb != null && rb.gameObject.CompareTag("Joel") && rb.gameObject.GetComponent<AudioSource>().isPlaying && !HealthHun.MyInstance.dead)
                {
                    joelSeen = true;
                    joelSuspected.Play();
                }
            }

            foreach (Collider collider in furtherColliders)
            {
                Rigidbody rb = collider.GetComponent<Rigidbody>();

                if (rb != null && rb.gameObject.CompareTag("Joel") && (SniperScriptLPFP.isFired
                || AutomaticGunScriptLPFP.isFired
                || HandgunScriptLPFP.isFired
                || PumpShotgunScriptLPFP.isFired) && !HealthHun.MyInstance.dead)
                {
                    joelSeen = true;
                    joelSuspected.Play();
                }
                else if (!hitEachOther && checkBomb && rb != null && (((rb.gameObject.CompareTag("StunGrenade") || rb.gameObject.CompareTag("BileBomb") || rb.gameObject.CompareTag("MoltovCocktail")) && rb.gameObject.GetComponent<AudioSource>().isPlaying)
                    || (rb.gameObject.CompareTag("PipeBomb") && rb.gameObject.GetComponents<AudioSource>()[1].isPlaying)) && !HealthHun.MyInstance.dead && !joelBombSeen)
                {
                    joelSuspected.Play();
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

    // public static void Reset()
    // {
    //     joelBombSeen = false;
    //     canMove = true;
    //     bileEffect = false;
    //     inAttack = false;
    // }

    IEnumerator screamBite()
    {
        yield return new WaitForSeconds(1);

    }

    IEnumerator joelDamage()
    {
        JoelScript.changeHealth(-10);
        damage = true;
        yield return new WaitForSeconds(1f);
        damage = false;

    }


}
