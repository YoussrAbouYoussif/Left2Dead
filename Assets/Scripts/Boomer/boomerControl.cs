using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;


    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter_Boomer))]
    public class boomerControl : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             
        public ThirdPersonCharacter_Boomer character { get; private set; } 
        public Transform target;
        public GameObject bombPrefab;
        public bool bombCreated = false;

        Animator m_Animator;
        public GameObject bomb;
        public bool joelSeen = false;
        bool hidden = false;

        public AudioSource[] audioSource;
        public AudioSource angrySound;
        public AudioSource attackSound;
    
        public float timeRemaining;
        Vector3 posBomb;
        bool bombNear = false;
        bool checkBomb = true;
        GameObject joel;
        public static bool joelBombSeen = false;




    private void Start()
        {

            m_Animator = gameObject.GetComponent<Animator>();

            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter_Boomer>();

            joel = GameObject.FindWithTag("Joel");
            target = joel.transform;

            audioSource = GetComponents<AudioSource>();
            angrySound  = audioSource[0];
            attackSound = audioSource[1];

	        agent.updateRotation = false;
	        agent.updatePosition = true;

            timeRemaining = 0f;
        }


        private void Update()
        {

        if(joelBombSeen){
            agent.SetDestination(agent.transform.position);
            character.Move(Vector3.zero, false, false);
         }

        else if (joelSeen && !Health.dead && !joelBombSeen)
        {
            agent.stoppingDistance = 6;

            if (target != null && !Health.dead){
                agent.SetDestination(target.position);
            }

            if (agent.remainingDistance > agent.stoppingDistance)
            {
                m_Animator.SetBool("attack", false);
                character.Move(agent.desiredVelocity, false, false);
            }

            else
            {
                character.Move(Vector3.zero, false, false);

                if(timeRemaining <= 0f){
                    m_Animator.SetBool("attack", true);
                }

                transform.LookAt(new Vector3(target.position.x, this.transform.position.y, target.position.z));

            }

            if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && timeRemaining <= 0f)
            {
                if (bombCreated == false)
                {
                    attackSound.Play();
                    StartCoroutine(fireBile());
                }
                timeRemaining = 10f;
                bombCreated = false;

            }
            else if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Scream"))
            {
                bombCreated = false;
            }

             if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
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

                if (rb != null && rb.gameObject.CompareTag("Joel") && rb.gameObject.GetComponent<AudioSource>().isPlaying && !Health.dead) {
                    joelSeen = true;
                    angrySound.Play();
                }
                
            }

            foreach (Collider collider in furtherColliders)
            {
                Rigidbody rb = collider.GetComponent<Rigidbody>();

                if (rb != null && rb.gameObject.CompareTag("Joel") && (SniperScriptLPFP.isFired 
                || AutomaticGunScriptLPFP.isFired
                || HandgunScriptLPFP.isFired
                || PumpShotgunScriptLPFP.isFired ) && !Health.dead) {
                    joelSeen = true;
                    angrySound.Play();
                }

                else if( checkBomb && rb != null && (((rb.gameObject.CompareTag("StunGrenade") || rb.gameObject.CompareTag("BileBomb") || rb.gameObject.CompareTag("MoltovCocktail")) && rb.gameObject.GetComponent<AudioSource>().isPlaying)
                    || (rb.gameObject.CompareTag("PipeBomb") && rb.gameObject.GetComponents<AudioSource>()[1].isPlaying)) && !Health.dead){  
                    posBomb = ThrowBombs.bombPostion;
                    bombNear = true;
                    angrySound.Play();
                }

                if(bombNear){
                    checkBomb = false;
                    agent.stoppingDistance = 0;
                    agent.SetDestination(posBomb);
                    character.Move(agent.desiredVelocity, false, false);
                    if(agent.remainingDistance <= agent.stoppingDistance){
                        character.Move(Vector3.zero, false, false);
                        checkBomb = true;
                        bombNear = false;
                    }

                }
             
            }

        }

    }

    public static void Reset(){
        joelBombSeen = false;
    }



    IEnumerator fireBile()
    {
        bombCreated = true;
        yield return new WaitForSeconds(0.5f);

        Instantiate(bombPrefab,
                    new Vector3(this.transform.position.x, this.transform.position.y + 2.4f, this.transform.position.z),
                    this.transform.rotation);

       

    }

}

