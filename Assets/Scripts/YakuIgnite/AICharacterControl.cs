using System.Collections;

using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target;                                    // target to aim for
        public bool isWithingRange = false;
        public GameObject particleSystem;

        public int health = 50;
        GameObject Joel;
        public bool attacked = false;
        float timer = 0;
        private float rotationSpeed = 2f;
        float oldTimer = 0;
        bool dead = false;
        public AudioSource[] audios;
        public AudioSource deadYaku;
        public AudioSource attackYaku;
        bool checkBomb = false;
        Vector3 posBomb;
        bool bombNear = false;
        bool stunBomb = false;
        bool bileBomb = false;
        bool pipeBomb = false;
        GameObject targetAfterBomb;
        Transform[] targets;
        float timerStunBomb = 0.0f;




        private static AICharacterControl instance;

        public static AICharacterControl MyInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<AICharacterControl>();
                }

                return instance;
            }
        }
        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();
            Joel = GameObject.FindGameObjectWithTag("Joel");
            agent.updateRotation = false;
            agent.updatePosition = true;
            target = Joel.transform;
            audios = GetComponents<AudioSource>();
            this.attackYaku = audios[1];
            this.deadYaku = audios[0];
        }


        private void FixedUpdate()
        {
            if (!dead)
            {
                if (stunBomb)
                {
                    timer += Time.deltaTime;
                    this.gameObject.GetComponent<Animator>().SetBool("attack", false);
                    character.Move(Vector3.zero, false, false);
                    agent.SetDestination(agent.transform.position);
                    if (timer >= 2.8f)
                    {
                        this.timer = 0;
                        this.stunBomb = false;
                    }
                }
                else if (bileBomb)
                {
                    agent.SetDestination(targetAfterBomb.transform.position);
                    if (agent.remainingDistance > agent.stoppingDistance &&  agent.remainingDistance>0.5f)
                    {
                        this.gameObject.GetComponent<Animator>().SetBool("attack", false);
                        character.Move(agent.desiredVelocity, false, false);
                    }
                    else
                    {
                        if ((timer) > 2)
                        {
                            attackYakuFriend();
                        }
                        else
                        {
                            timer += Time.deltaTime;
                        }
                        this.gameObject.GetComponent<Animator>().SetBool("attack", true);
                        character.Move(Vector3.zero, false, false);
                    }
                }
                else if (this.pipeBomb)
                {
                    this.health = 0;
                    this.gameObject.GetComponent<HealthYakuBar>().setHealth(this.health);
                    StartCoroutine(die());
                }
                else
                {
                    chechCast();
                    if (isWithingRange)
                    {
                        if (target != null)
                            agent.SetDestination(target.position);

                        if (agent.remainingDistance > agent.stoppingDistance &&  agent.remainingDistance>0.5f)
                        {
                            this.gameObject.GetComponent<Animator>().SetBool("attack", false);
                            character.Move(agent.desiredVelocity, false, false);
                        }
                        else
                        {
                            if ((timer) > 2)
                            {
                                attackJoel();
                            }
                            else
                            {
                                timer += Time.deltaTime;
                            }
                            this.gameObject.GetComponent<Animator>().SetBool("attack", true);
                            character.Move(Vector3.zero, false, false);
                        }
                    }
                    else
                    {
                        Collider[] Colliders = Physics.OverlapSphere(transform.position, 20.0f);
                        Collider[] furtherColliders = Physics.OverlapSphere(this.transform.position, 60.0f);
                        bool entered = false;
                        foreach (Collider collider in Colliders)
                        {
                            Rigidbody rb = collider.GetComponent<Rigidbody>();
                            if (rb != null && ((rb.gameObject.CompareTag("Joel") && rb.gameObject.GetComponent<AudioSource>().isPlaying)))
                            {
                                entered = true;
                                if (target != null)
                                    agent.SetDestination(target.position);

                                if (agent.remainingDistance > agent.stoppingDistance &&  agent.remainingDistance>0.5f)
                                {
                                    attackYaku.Stop();
                                    this.gameObject.GetComponent<Animator>().SetBool("attack", false);
                                    character.Move(agent.desiredVelocity, false, false);
                                }
                                else
                                {
                                    if ((timer) > 2)
                                    {
                                        attackJoel();
                                    }
                                    else
                                    {
                                        timer += Time.deltaTime;
                                    }

                                    this.gameObject.GetComponent<Animator>().SetBool("attack", true);
                                    character.Move(Vector3.zero, false, false);
                                }
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
                                entered = true;
                                if (target != null)
                                    agent.SetDestination(target.position);

                                if (agent.remainingDistance > agent.stoppingDistance &&  agent.remainingDistance>0.5f)
                                {
                                    this.gameObject.GetComponent<Animator>().SetBool("attack", false);
                                    character.Move(agent.desiredVelocity, false, false);
                                }
                                else
                                {
                                    if ((timer) > 2)
                                    {
                                        attackJoel();
                                    }
                                    else
                                    {
                                        timer += Time.deltaTime;
                                    }
                                    this.gameObject.GetComponent<Animator>().SetBool("attack", true);
                                    character.Move(Vector3.zero, false, false);
                                }
                            }
                            else if (checkBomb && rb != null && (((rb.gameObject.CompareTag("StunGrenade") || rb.gameObject.CompareTag("BileBomb") || rb.gameObject.CompareTag("MoltovCocktail")) && rb.gameObject.GetComponent<AudioSource>().isPlaying)
                    || (rb.gameObject.CompareTag("PipeBomb") && rb.gameObject.GetComponents<AudioSource>()[1].isPlaying)))
                            {
                                posBomb = ThrowBombs.bombPostion;
                                bombNear = true;
                            }
                            if (bombNear)
                            {
                                checkBomb = false;
                                agent.stoppingDistance = 0;
                                agent.SetDestination(posBomb);
                                character.Move(agent.desiredVelocity, false, false);
                                RotateTowards(target);
                                bombNear = false;
                                if (agent.remainingDistance <= agent.stoppingDistance)
                                {
                                    agent.SetDestination(target.position);
                                    character.Move(Vector3.zero, false, false);
                                    checkBomb = true;
                                    bombNear = false;
                                }
                            }
                        }
                        if (!entered)
                        {
                            agent.SetDestination(new Vector3(target.position.x + UnityEngine.Random.Range(-2, 2), transform.position.y, transform.position.z));
                            character.Move(agent.desiredVelocity, false, false);
                        }
                    }
                }
            }
            else
            {
                character.Move(Vector3.zero, false, false);
            }
            if (transform.position.x - Joel.transform.position.x > 100f || transform.position.z - Joel.transform.position.z > 100f)
            {
                Destroy(this.gameObject);
            }
        }
        void chechCast()
        {
            RaycastHit hit;
            Vector3 i = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            if (Physics.SphereCast(i, 20, transform.forward, out hit, 1))
            {
                if (hit.collider.gameObject.CompareTag("Joel"))
                {
                    this.setWithinRange(true);
                }
            }
            if (Physics.SphereCast(i, 10, transform.forward, out hit, 1))
            {
                if (hit.collider.gameObject.CompareTag("Joel"))
                {
                    this.setWithinRange(true);
                }
            }
            if (Physics.SphereCast(i, 3, transform.forward, out hit, 1))
            {
                if (hit.collider.gameObject.CompareTag("Joel"))
                {
                    this.setWithinRange(true);
                }
            }


        }
        public void setPipeBomb()
        {
            this.pipeBomb = true;
        }
        private void RotateTowards(Transform target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
        IEnumerator die()
        {
            this.gameObject.GetComponent<Animator>().SetTrigger("hit");
            this.gameObject.GetComponent<Animator>().SetBool("death", true);
            JoelScript.updateRage(true);
            this.gameObject.GetComponent<Collider>().enabled = false;
            StartCoroutine(particleSystem.GetComponent<bloodScript>().opened());
            character.Move(Vector3.zero, false, false);
            this.gameObject.GetComponent<HealthYakuBar>().setHealth(this.health);
            if (!this.deadYaku.isPlaying)
            {
                this.deadYaku.Play();
            }
            dead = true;
            yield return new WaitForSeconds(5);
            Destroy(this.gameObject);
        }
        void hit()
        {
            this.gameObject.GetComponent<HealthYakuBar>().setHealth(this.health);
            this.gameObject.GetComponent<Animator>().SetBool("attack", false);
            this.gameObject.GetComponent<Animator>().SetTrigger("hit");
            StartCoroutine(particleSystem.GetComponent<bloodScript>().opened());
        }
        public void setHealth(int x)
        {
            this.health += x;
            if (this.health <= 0)
            {
                StartCoroutine(die());
            }
            else
            {
                this.gameObject.GetComponent<HealthYakuBar>().setHealth(this.health);
                this.gameObject.GetComponent<Animator>().SetBool("attack", false);
                this.gameObject.GetComponent<Animator>().SetTrigger("hit");
            }
        }
        public void setBile(bool x)
        {
            bileBomb = x;
            if (!x)
            {
                this.gameObject.GetComponent<Animator>().SetBool("attack", true);
            }
        }
        public void setTargetAfterBomb(GameObject x)
        {
            this.targetAfterBomb = x;
        }

        void attackJoel()
        {
            if (!this.attackYaku.isPlaying)
            {
                this.attackYaku.Play();
            }
            attacked = true;
            JoelScript.changeHealth(-5);
            timer = 0;
        }
        void attackYakuFriend()
        {
            if (targetAfterBomb != null)
            {
                if (targetAfterBomb.gameObject.tag == "yaku")
                {
                    targetAfterBomb.GetComponent<AICharacterControl>().setHealth(-5);
                    timer = 0;
                }
                else
                {
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
            }
        }
        public void SetTarget(Transform target)
        {
            this.target = target;
        }
        public void setWithinRange(bool x)
        {
            isWithingRange = x;
        }
        public void setStunBomb(bool x)
        {
            this.stunBomb = x;
        }
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("AssaultRifleBullet"))
            {

                this.health -= 33 * (JoelScript.rageMode ? 2 : 1);

                //to not do the hit animation when it should die
                if (this.health > 0)
                {
                    hit();
                }
            }
            if (other.gameObject.CompareTag("PistolBullet"))
            {
                this.health -= 36 * (JoelScript.rageMode ? 2 : 1);
                //to not do the hit animation when it should die
                if (this.health > 0)
                {
                    hit();
                }
            }
            if (other.gameObject.CompareTag("ShotgunBullet"))
            {

                this.health -= 25 * (JoelScript.rageMode ? 2 : 1);
                //to not do the hit animation when it should die
                if (this.health > 0)
                {
                    hit();
                }
            }
            if (other.gameObject.CompareTag("SMGBullet"))
            {

                this.health -= 20 * (JoelScript.rageMode ? 2 : 1);
                //to not do the hit animation when it should die
                if (this.health > 0)
                {
                    hit();
                }
            }
            if (other.gameObject.CompareTag("SniperBullet"))
            {

                this.health -= 90 * (JoelScript.rageMode ? 2 : 1);
                //to not do the hit animation when it should die
                if (this.health > 0)
                {
                    hit();
                }
            }
            if (this.health <= 0)
            {
                StartCoroutine(die());
            }
        }
        public int getHealth()
        {
            return this.health;
        }
    }

}