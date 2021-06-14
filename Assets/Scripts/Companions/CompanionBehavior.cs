using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;
using FPSControllerLPFP;

public class CompanionBehavior : MonoBehaviour
{
    Animator anim;
    public GameObject companion;
    private NavMeshAgent agent;
    public Transform target;
    AudioSource[] a;
    AudioSource fireS;
    AudioSource look;
    AudioSource lookGirl;
    AudioSource bulletFire;

    public static int maxClips;
    public static int numberOfClips;
    int currentClips;
    public static int firingNumber;
    int numberOfInfectedKilled;
    public int gunAmmo;
    int oldKilledCount;

    [SerializeField]
    private Item[] items;

    bool collectiblesFlag;
    bool AmmoInc;
    bool healthInc;
    bool rageInc;
    public static bool fire = false;


    bool boomer = false;
    bool hunter = false;
    bool charger = false;
    bool jockey = false;
    bool spitter = false;
    bool tank = false;
    bool normal = false;
    public static bool notInfectedSeen = false;
    public static bool seen = false;

    public GameObject compName;
    public GameObject compClips;

    public Collider[] Colliders;
    Quaternion currentRotation;

    float timer = 0;
    int alreadyShot;
    bool saidLook = false;

    GameObject joel;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        a = GetComponents<AudioSource>();
        fireS = a[2];
        look = a[1];
        //lookGirl = a[3];
        bulletFire = a[0];
        numberOfClips = 1;
        firingNumber = 1;
        oldKilledCount = 0;
        collectiblesFlag = false;
        AmmoInc = false;
        healthInc = false;
        rageInc = false;
        

        if (companion.CompareTag("Zoey"))
        {
            maxClips = 5;
            gunAmmo = 15;
            firingNumber = numberOfClips * gunAmmo;
            currentClips = numberOfClips;
            compName.GetComponent<Text>().text = "Zoey";
        }

        else if (companion.CompareTag("Bill"))
        {
            maxClips = 6;
            gunAmmo = 50;
            firingNumber = numberOfClips * gunAmmo;
            currentClips = numberOfClips;
            compName.GetComponent<Text>().text = "Bill";
        }

        else if (companion.CompareTag("Ellie"))
        {
            maxClips = 3;
            //infinte ??
            gunAmmo = 15;
            firingNumber = numberOfClips * gunAmmo;
            currentClips = numberOfClips;
            compName.GetComponent<Text>().text = "Ellie";
        }

        else if (companion.CompareTag("Louis"))
        {
            maxClips = 4;
            gunAmmo = 50;
            firingNumber = numberOfClips * gunAmmo;
            currentClips = numberOfClips;
            compName.GetComponent<Text>().text = "Louis";
        }
    }
    void FixedUpdate()
    {
        Colliders = Physics.OverlapSphere(this.transform.position, 20.0f);
        foreach (Collider collider in Colliders)
        {
            if (collider.CompareTag("Boomer"))
            {
                if (!boomer)
                {
                    if (!look.isPlaying)
                    {
                        look.Play();
                        boomer = true;
                    }
                }
            }
            else if (collider.CompareTag("Hunter"))
            {
                if (!hunter)
                {
                    if (!look.isPlaying)
                    {
                        look.Play();
                        hunter = true;
                    }
                }
            }
            else if (collider.CompareTag("Tank"))
            {
                if (!tank)
                {
                    if (!look.isPlaying)
                    {
                        look.Play();
                        tank = true;
                    }
                }
            }
            else if (collider.CompareTag("JockeyAttack"))
            {
                if (!jockey)
                {
                    if (!look.isPlaying)
                    {
                        look.Play();
                        jockey = true;
                    }
                }
            }
            else if (collider.CompareTag("Spitter"))
            {
                if (!spitter)
                {
                    if (!look.isPlaying)
                    {
                        look.Play();
                        spitter = true;
                    }
                }
            }
            else if (collider.CompareTag("ChargerAttack"))
            {
                if (!charger)
                {
                    if (!look.isPlaying)
                    {
                        look.Play();
                        charger = true;
                    }
                }
            }
            else if (collider.CompareTag("yaku"))
            {
                if (!normal)
                {
                    if (!look.isPlaying)
                    {
                        look.Play();
                        normal = true;
                    }
                }
            }
        }
        /*if (companion.CompareTag("Louis") || companion.CompareTag("Bill"))
        {
            //look.Play();
        }
        else
        {
            if (!isPlayed)
            {
                if (!look.isPlaying)
                {
                    look.Play();
                    isPlayed = true;
                }
            }
        }*/

        bool run = Input.GetKey(KeyCode.LeftShift);
        bool jump = Input.GetKey("space");
        if (Input.GetKeyUp("q"))
        {
            fire = true;
        }


        if (companion.CompareTag("Zoey"))
        {
            if (collectiblesFlag == false)
            {
                Invoke("CollectiblesNumber", 60f);
                collectiblesFlag = true;
            }
            if (currentClips != numberOfClips)
            {
                alreadyShot = firingNumber;
                firingNumber = numberOfClips * gunAmmo;
                firingNumber -= alreadyShot;
                currentClips = numberOfClips;
            }

        }
        else if (companion.CompareTag("Bill"))
        {
            if (AmmoInc == false)
            {
                Invoke("AmmoIncrease", 60f);
                AmmoInc = true;
            }
            if (currentClips != numberOfClips)
            {
                alreadyShot = firingNumber;
                firingNumber = numberOfClips * gunAmmo;
                firingNumber -= alreadyShot;
                currentClips = numberOfClips;
            }
        }

        else if (companion.CompareTag("Louis"))
        {
            if (healthInc == false)
            {
                Invoke("HealthPointIncrease", 1f);
                healthInc = true;
            }
            if (currentClips != numberOfClips)
            {
                alreadyShot = firingNumber;
                firingNumber = numberOfClips * gunAmmo;
                firingNumber -= alreadyShot;
                currentClips = numberOfClips;
            }
        }
        else if (companion.CompareTag("Ellie"))
        {
            if (rageInc == false)
            {
                Invoke("RageInc", 3f);
                rageInc = true;
            }
            if (currentClips != numberOfClips)
            {
                alreadyShot = firingNumber;
                firingNumber = numberOfClips * gunAmmo;
                firingNumber -= alreadyShot;
                currentClips = numberOfClips;
            }
        }

        if (FpsControllerLPFP.isEvading == true)
        {
            agent.SetDestination(target.position);
            agent.speed = 20;
        }
        else
        {
            agent.speed = 6;
        }

        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        if (v != 0 || h != 0)
        {
            if (!notInfectedSeen)
            {
                this.transform.eulerAngles = new Vector3(target.transform.eulerAngles.x, target.transform.eulerAngles.y, target.transform.eulerAngles.z);
            }

            agent.destination = target.position;
            anim.SetBool("isWalking", true);

            if (run == true)
            {
                anim.SetBool("isWalking", false);
                anim.SetBool("Jumping", false);
                anim.SetBool("Fire", false);
                anim.SetBool("Running", true);
            }
            else
            {
                anim.SetBool("isWalking", true);
                anim.SetBool("Running", false);
                if (jump == true)
                {
                    anim.SetBool("isWalking", false);
                    anim.SetBool("Running", false);
                    anim.SetBool("Fire", false);
                    anim.SetBool("Jumping", true);
                }
                else
                {
                    anim.SetBool("isWalking", true);
                    anim.SetBool("Running", false);
                    anim.SetBool("Jumping", false);
                    //Joel must tell first
                    if (fire == true)
                    {
                        bulletFire.Play();
                        if (timer == 0)
                        {
                            if (!fireS.isPlaying && notInfectedSeen && !saidLook)
                            {
                                fireS.Play();
                                saidLook = true;
                            }
                        }
                        else if (timer > 1)
                        {
                            bulletFire.Play();
                            timer = 0;
                        }
                        else
                        {
                            timer += Time.deltaTime;
                        }
                        Colliders = Physics.OverlapSphere(this.transform.position, 20.0f);
                        foreach (Collider collider in Colliders)
                        {
                            if (collider.CompareTag("Boomer") && !Health.dead)
                            {
                                notInfectedSeen = true;
                                Rigidbody rb = collider.GetComponent<Rigidbody>();
                                agent.transform.LookAt(new Vector3(rb.position.x, rb.position.y, rb.position.z));
                                seen = true;
                            }
                            else if (collider.CompareTag("Hunter") && !HealthHun.MyInstance.dead)
                            {
                                notInfectedSeen = true;
                                Rigidbody rb = collider.GetComponent<Rigidbody>();
                                agent.transform.LookAt(new Vector3(rb.position.x, target.position.y - 1.5f, rb.position.z));
                                seen = true;
                            }
                            else if (collider.CompareTag("Tank") && TankCharacterControl.healthPoints > 0)
                            {
                                seen = true;
                                notInfectedSeen = true;
                                Rigidbody rb = collider.GetComponent<Rigidbody>();
                                agent.transform.LookAt(new Vector3(rb.position.x, rb.position.y, rb.position.z));
                            }
                            else if (collider.CompareTag("JockeyAttack") && JockeyScript.healthPoints > 0)
                            {
                                notInfectedSeen = true;
                                Rigidbody rb = collider.GetComponent<Rigidbody>();
                                agent.transform.LookAt(new Vector3(rb.position.x, rb.position.y, rb.position.z));
                                seen = true;
                            }
                            else if (collider.CompareTag("ChargerAttack") && ChargerScript.healthPoints > 0)
                            {
                                notInfectedSeen = true;
                                Rigidbody rb = collider.GetComponent<Rigidbody>();
                                agent.transform.LookAt(new Vector3(rb.position.x, rb.position.y, rb.position.z));
                                seen = true;
                            }
                            else if (collider.CompareTag("Spitter") && SpitterCharacterControl.healthPoints > 0)
                            {
                                notInfectedSeen = true;
                                Rigidbody rb = collider.GetComponent<Rigidbody>();
                                agent.transform.LookAt(new Vector3(rb.position.x, rb.position.y, rb.position.z));
                                seen = true;
                            }
                            else if (collider.CompareTag("yaku") && collider.gameObject.GetComponent<AICharacterControl>().getHealth() > 0)
                            {
                                notInfectedSeen = true;
                                Rigidbody rb = collider.GetComponent<Rigidbody>();
                                agent.transform.LookAt(new Vector3(rb.position.x, rb.position.y, rb.position.z));
                                seen = true;
                            }
                            else if (collider.CompareTag("Tank") && TankCharacterControl.healthPoints <= 0)
                            {
                                notInfectedSeen = false;
                                saidLook = false;
                            }
                            else if (collider.CompareTag("Boomer") && Health.dead)
                            {
                                notInfectedSeen = false;
                                saidLook = false;
                            }
                            else if (collider.CompareTag("Hunter") && HealthHun.MyInstance.dead)
                            {
                                notInfectedSeen = false;
                                saidLook = false;
                            }
                            else if (collider.CompareTag("JockeyParent") && JockeyScript.healthPoints <= 0)
                            {
                                notInfectedSeen = false;
                                saidLook = false;
                            }
                            else if (collider.CompareTag("ChargerAttack") && ChargerScript.healthPoints <= 0)
                            {
                                notInfectedSeen = false;
                                saidLook = false;
                            }
                            else if (collider.CompareTag("Spitter") && SpitterCharacterControl.healthPoints <= 0)
                            {
                                notInfectedSeen = false;
                                saidLook = false;
                            }
                            else if (collider.CompareTag("yaku") && collider.gameObject.GetComponent<AICharacterControl>().getHealth() <= 0)
                            {
                                notInfectedSeen = false;
                                saidLook = false;
                            }
                        }

                        anim.SetBool("isWalking", true);
                        anim.SetBool("Running", false);
                        anim.SetBool("Jumping", false);
                        //anim.SetBool("Fire", true);
                    }
                    else
                    {
                        anim.SetBool("isWalking", true);
                        anim.SetBool("Running", false);
                        anim.SetBool("Jumping", false);
                        anim.SetBool("Fire", false);
                        //notInfectedSeen = false;
                    }
                }
            }
        }
        else
        {
            
            /* if (!notInfectedSeen)
             {

                 this.transform.eulerAngles = new Vector3(target.transform.eulerAngles.x, target.transform.eulerAngles.y, target.transform.eulerAngles.z);
             }*/

            if (agent.remainingDistance <= agent.stoppingDistance && !notInfectedSeen)
            {
                anim.SetBool("isWalking", false);
                this.transform.eulerAngles = new Vector3(target.transform.eulerAngles.x, target.transform.eulerAngles.y, target.transform.eulerAngles.z);
            }
            else
            {
                agent.destination = target.position;
                anim.SetBool("isWalking", true);
            }

            if (fire == true)
            {
                bulletFire.Play();
                if (timer == 0)
                {
                    if (!fireS.isPlaying && notInfectedSeen && !saidLook)
                    {
                        fireS.Play();
                        saidLook = true;
                    }
                }
                else if (timer > 2.5)
                {
                    bulletFire.Play();
                    timer = 0;

                }
                else
                {
                    timer += Time.deltaTime;
                }
                Colliders = Physics.OverlapSphere(this.transform.position, 20.0f);
                foreach (Collider collider in Colliders)
                {
                    if (collider.CompareTag("Boomer") && !Health.dead)
                    {
                        notInfectedSeen = true;
                        Rigidbody rb = collider.GetComponent<Rigidbody>();
                        agent.transform.LookAt(new Vector3(rb.position.x, rb.position.y, rb.position.z));
                        seen = true;
                    }
                    else if (collider.CompareTag("Hunter") && !HealthHun.MyInstance.dead)
                    {
                        notInfectedSeen = true;
                        Rigidbody rb = collider.GetComponent<Rigidbody>();
                        agent.transform.LookAt(new Vector3(rb.position.x, target.position.y - 1.5f, rb.position.z));
                        seen = true;
                    }
                    else if (collider.CompareTag("Tank") && TankCharacterControl.healthPoints > 0)
                    {
                        seen = true;
                        notInfectedSeen = true;
                        Rigidbody rb = collider.GetComponent<Rigidbody>();
                        agent.transform.LookAt(new Vector3(rb.position.x, rb.position.y, rb.position.z));

                    }
                    else if (collider.CompareTag("JockeyParent") && JockeyScript.healthPoints > 0)
                    {
                        notInfectedSeen = true;
                        Rigidbody rb = collider.GetComponent<Rigidbody>();
                        agent.transform.LookAt(new Vector3(rb.position.x, rb.position.y, rb.position.z));
                        seen = true;
                    }
                    else if (collider.CompareTag("ChargerAttack") && ChargerScript.healthPoints > 0)
                    {
                        notInfectedSeen = true;
                        Rigidbody rb = collider.GetComponent<Rigidbody>();
                        agent.transform.LookAt(new Vector3(rb.position.x, rb.position.y, rb.position.z));
                        seen = true;
                    }
                    else if (collider.CompareTag("Spitter") && SpitterCharacterControl.healthPoints > 0)
                    {
                        notInfectedSeen = true;
                        Rigidbody rb = collider.GetComponent<Rigidbody>();
                        agent.transform.LookAt(new Vector3(rb.position.x, rb.position.y, rb.position.z));
                        seen = true;
                    }
                    else if (collider.CompareTag("yaku") && collider.gameObject.GetComponent<AICharacterControl>().getHealth() > 0)
                    {
                        notInfectedSeen = true;
                        Rigidbody rb = collider.GetComponent<Rigidbody>();
                        agent.transform.LookAt(new Vector3(rb.position.x, rb.position.y, rb.position.z));
                        seen = true;
                    }
                    else if (collider.CompareTag("Tank") && TankCharacterControl.healthPoints <= 0)
                    {
                        notInfectedSeen = false;
                        saidLook = false;
                    }
                    else if (collider.CompareTag("Boomer") && Health.dead)
                    {
                        notInfectedSeen = false;
                        saidLook = false;
                    }
                    else if (collider.CompareTag("Hunter") && HealthHun.MyInstance.dead)
                    {
                        notInfectedSeen = false;
                        saidLook = false;
                    }
                    else if (collider.CompareTag("JockeyParent") && JockeyScript.healthPoints <= 0)
                    {
                        notInfectedSeen = false;
                        saidLook = false;
                    }
                    else if (collider.CompareTag("ChargerAttack") && ChargerScript.healthPoints <= 0)
                    {
                        notInfectedSeen = false;
                        saidLook = false;
                    }
                    else if (collider.CompareTag("Spitter") && SpitterCharacterControl.healthPoints <= 0)
                    {
                        notInfectedSeen = false;
                        saidLook = false;
                    }
                    else if (collider.CompareTag("yaku") && collider.gameObject.GetComponent<AICharacterControl>().getHealth() <= 0)
                    {
                        notInfectedSeen = false;
                        saidLook = false;
                    }
                }

                anim.SetBool("isWalking", false);
                anim.SetBool("Running", false);
                anim.SetBool("Jumping", false);
                //anim.SetBool("Fire", true);
            }
            else
            {
                anim.SetBool("Running", false);
                anim.SetBool("Jumping", false);
                anim.SetBool("Fire", false);
                //notInfectedSeen = false;
            }
            if (jump == true)
            {
                anim.SetBool("isWalking", false);
                anim.SetBool("Running", false);
                anim.SetBool("Fire", false);
                anim.SetBool("Jumping", true);
            }

        }

        //calculating number of infected killed in the game
        int NInfected = JoelScript.killedNInfecteds;
        int SInfected = JoelScript.killedSInfecteds;
        numberOfInfectedKilled = NInfected + SInfected;
        int difference = numberOfInfectedKilled - oldKilledCount;
        if ((difference % 10 == 0) && (difference != 0))
        {
            if (numberOfClips < maxClips)
            {
                numberOfClips += 1;
            }
            oldKilledCount = numberOfInfectedKilled;
        }
        compClips.GetComponent<Text>().text = "" + numberOfClips;
    }

    public void HealthPointIncrease()
    {
        healthInc = false;
        JoelScript.changeHealth(1);
    }

    public void AmmoIncrease()
    {
        AmmoInc = false;
        AmmoCounts.MyInstance.pickUpAmmo(350, 1);
        AmmoCounts.MyInstance.pickUpAmmo(82, 2);
        AmmoCounts.MyInstance.pickUpAmmo(65, 3);
        AmmoCounts.MyInstance.pickUpAmmo(225, 4);
    }

    public void CollectiblesNumber()
    {
        collectiblesFlag = false;
        int[] collectibles = InventoryScript.MyInstance.MyIngredientsCount;
        for (int i = 0; i < collectibles.Length; i++)
        {
            for (int j = 0; j < collectibles[i]; j++)
            {
                InventoryScript.MyInstance.AddItem(items[i]);
            }
        }
    }

    public void RageInc()
    {
        rageInc = false;
        JoelScript.changeRage(2);
    }

    public static void Reset()
    {
        numberOfClips = 1;
        firingNumber = 1;
        maxClips = 0;
        fire = false;
        notInfectedSeen = false;
    }
}