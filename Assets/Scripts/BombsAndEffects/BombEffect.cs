using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;


public class BombEffect : MonoBehaviour
{
    List<Collider> list_tag;
    public float delay;
    public GameObject effect;
    float radius = 25.0f;
    public float explosionForce = 400f;
    public GameObject bombPrefab;
    GameObject afterExplosion;
    public Collider[] Colliders;
    AudioSource[] audios;
    public static AudioSource loudPeeps;
    AudioSource a;
    Renderer[] renderers;

    public static GameObject[] x;

    public static bool stunGrenadeCharger = false;
    public static bool stunGrenadeJockey = false;
    public static bool stunGrenadeTank = false;
    public static bool stunGrenadeSpitter = false;
    public static bool stunGrenadeBoomer = false;
    public static bool stunGrenadeHunter = false;
    public static bool stunGrenadeYaku = false;

    public static int counterCharger = 0;
    public static int counterJockey = 0;
    public static int counterSpitter = 0;
    public static int counterTank = 0;
    public static int counterHunter = 0;
    public static int counterBoomer = 0;
    public static int counterYaku = 0;

    public static bool stunGrenadeCollided = false;

    bool bileCollided = false;
    bool molotovCollided = false;
    bool pipeCollided = false;

    bool chargerAffected = false;
    bool jockeyAffected = false;
    bool tankAffected = false;
    bool boomerAffected = false;
    bool hunterAffected = false;
    bool spitterAffected = false;
    bool yakuAffected = false;

    bool chargerAffectedPipe = false;
    bool jockeyAffectedPipe = false;
    bool tankAffectedPipe = false;
    bool boomerAffectedPipe = false;
    bool hunterAffectedPipe = false;
    bool spitterAffectedPipe = false;
    public static bool yakuAffectedPipe = false;

    bool isEntered = false;

    float oldTimerCharger = 0;
    float oldTimerJockey = 0;
    float oldTimerSpitter = 0;
    float oldTimerTank = 0;
    float oldTimerHunter = 0;
    float oldTimerBoomer = 0;
    float oldTimerYaku = 0;

    float timer = 0;
    float timerCharger = 0;
    float timerJockey = 0;
    float timerSpitter = 0;
    float timerTank = 0;
    float timerHunter = 0;
    float timerBoomer = 0;
    float timerYaku = 0;

    // Start is called before the first frame update
    void Awake()
    {
        audios = GetComponents<AudioSource>();
        a = audios[0];
        if (bombPrefab.CompareTag("PipeBomb"))
        {
            loudPeeps = audios[1];

        }
    }
    void Start()
    {
        Invoke("explode", 2.0f);
        renderers = GetComponentsInChildren<Renderer>();
        a = GetComponent<AudioSource>();
    }

    void healthDamage()
    {
        if (molotovCollided)
        {
            if (chargerAffected)
            {
                ChargerScript.healthPoints -= 25;
                ChargerScript.healthChanged = true;
            }
            if (jockeyAffected)
            {
                JockeyScript.healthPoints -= 25;
                JockeyScript.healthChanged = true;
            }
            if (boomerAffected)
            {
                Health.health -= 25;
            }
            if (hunterAffected)
            {
                HealthHun.MyInstance.health -= 25;
            }
            if (tankAffected)
            {
                TankCharacterControl.healthPoints -= 25;
            }
            if (spitterAffected)
            {
                SpitterCharacterControl.healthPoints -= 25;
            }
        }
        if (chargerAffectedPipe)
        {
            ChargerScript.healthPoints -= 100;
            ChargerScript.healthChanged = true;
            chargerAffectedPipe = false;
        }
        if (jockeyAffectedPipe)
        {
            JockeyScript.healthPoints -= 100;
            JockeyScript.healthChanged = true;
            jockeyAffectedPipe = false;
        }
        if (boomerAffectedPipe)
        {
            Health.health -= 100;
            boomerAffectedPipe = false;
        }
        if (hunterAffectedPipe)
        {
            HealthHun.MyInstance.health -= 100;
            hunterAffectedPipe = false;
        }
        if (tankAffectedPipe)
        {
            TankCharacterControl.healthPoints -= 100;
            tankAffectedPipe = false;
        }
        if (spitterAffectedPipe)
        {
            SpitterCharacterControl.healthPoints -= 100;
            spitterAffectedPipe = false;
        }
        if (hunterAffectedPipe)
        {
            HealthHun.MyInstance.health -= 100;
            hunterAffectedPipe = false;
        }
        if (boomerAffectedPipe)
        {
            Health.health -= 100;
            boomerAffectedPipe = false;
        } 
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (stunGrenadeCollided)
        {
            list_tag = canAttack(Colliders);

            foreach (Collider collider in list_tag)
            {
                if (collider != null)
                {
                    if (collider.CompareTag("Hunter"))
                    {
                        stunGrenadeHunter = true;
                        counterHunter = 1;
                    }
                    if (collider.CompareTag("Boomer"))
                    {
                        stunGrenadeBoomer = true;
                        counterBoomer = 1;
                    }

                    if (collider.CompareTag("Spitter"))
                    {
                        stunGrenadeSpitter = true;
                        counterSpitter = 1;
                    }
                    if (collider.CompareTag("Tank"))
                    {
                        stunGrenadeTank = true;
                        counterTank = 1;
                    }
                    if (collider.CompareTag("ChargerAttack"))
                    {
                        stunGrenadeCharger = true;
                        counterCharger = 1;
                    }
                    if (collider.CompareTag("JockeyAttack"))
                    {
                        stunGrenadeJockey = true;
                        counterJockey = 1;
                    }
                    if (collider.CompareTag("yaku"))
                    {
                        stunGrenadeYaku = true;
                        collider.gameObject.GetComponent<AICharacterControl>().setStunBomb(true);
                    }
                }
            }
        }
        if (molotovCollided)
        {
            list_tag = canAttack(Colliders);
            foreach (Collider collider in list_tag)
            {
                if (collider != null)
                {
                    if (collider.CompareTag("Hunter"))
                    {
                        if ((((int)timerHunter) > ((int)oldTimerHunter)))
                        {
                            Invoke("healthDamage", 1);
                            oldTimerHunter = timerHunter;
                        }
                        if ((int)timerHunter == 6)
                        {
                            timerHunter = 0;
                            oldTimerHunter = 0;
                            molotovCollided = false;
                            hunterAffected = false;
                        }
                        else
                        {
                            timerHunter += Time.deltaTime;
                            hunterAffected = true;
                        }
                    }
                    if (collider.CompareTag("Boomer"))
                    {
                        if ((((int)timerBoomer) > ((int)oldTimerBoomer)))
                        {
                            Invoke("healthDamage", 1);
                            oldTimerBoomer = timerBoomer;
                        }
                        if ((int)timerBoomer == 6)
                        {
                            timerBoomer = 0;
                            oldTimerBoomer = 0;
                            molotovCollided = false;
                            boomerAffected = false;
                        }
                        else
                        {
                            timerBoomer += Time.deltaTime;
                            boomerAffected = true;
                        }
                    }
                    if (collider.CompareTag("Tank"))
                    {
                        if ((((int)timerTank) > ((int)oldTimerTank)))
                        {
                            Invoke("healthDamage", 1);
                            oldTimerTank = timerTank;
                        }
                        if ((int)timerTank == 6)
                        {
                            timerTank = 0;
                            oldTimerTank = 0;
                            molotovCollided = false;
                            tankAffected = false;
                        }
                        else
                        {
                            timerTank += Time.deltaTime;
                            tankAffected = true;
                        }
                    }
                    if (collider.CompareTag("Spitter"))
                    {
                        if ((((int)timerSpitter) > ((int)oldTimerSpitter)))
                        {
                            Invoke("healthDamage", 1);
                            oldTimerSpitter = timerSpitter;
                        }
                        if ((int)timerSpitter == 6)
                        {
                            timerSpitter = 0;
                            oldTimerSpitter = 0;
                            molotovCollided = false;
                            spitterAffected = false;
                        }
                        else
                        {
                            timerSpitter += Time.deltaTime;
                            spitterAffected = true;
                        }
                    }
                    if (collider.CompareTag("ChargerAttack"))
                    {
                        if ((((int)timerCharger) > ((int)oldTimerCharger)))
                        {
                            Invoke("healthDamage", 1);
                            oldTimerCharger = timerCharger;
                        }
                        if ((int)timerCharger == 6)
                        {
                            timerCharger = 0;
                            oldTimerCharger = 0;
                            molotovCollided = false;
                            chargerAffected = false;
                        }
                        else
                        {
                            timerCharger += Time.deltaTime;
                            chargerAffected = true;
                        }
                    }
                    if (collider.CompareTag("JockeyAttack"))
                    {
                        if ((((int)timerJockey) > ((int)oldTimerJockey)))
                        {
                            Invoke("healthDamage", 1);
                            oldTimerJockey = timerJockey;
                        }
                        if ((int)timerJockey == 6)
                        {
                            timerJockey = 0;
                            oldTimerJockey = 0;
                            molotovCollided = false;
                            jockeyAffected = false;
                        }
                        else
                        {
                            timerJockey += Time.deltaTime;
                            jockeyAffected = true;
                        }
                    }
                    if (collider.CompareTag("yaku"))
                    {
                        if ((((int)timerYaku) > ((int)oldTimerYaku)))
                        {
                            collider.gameObject.GetComponent<AICharacterControl>().setHealth(-25);
                            oldTimerYaku = timerYaku;
                        }
                        if ((int)timerYaku == 6)
                        {
                            timerYaku = 0;
                            oldTimerYaku = 0;
                            molotovCollided = false;
                            yakuAffected = false;
                        }
                        else
                        {
                            timerYaku += Time.deltaTime;
                            yakuAffected = true;
                        }
                    }
                }
            }
        }
        if (bileCollided)
        {
            int index = 0;
            int length = 0;
            if (timer > 4.5f)
            {
                timer = 0;
                isEntered = false;
                bileCollided = false;
                foreach (Collider collider in list_tag)
                {
                    if (collider != null)
                    {
                        switch (collider.tag)
                        {
                            case "ChargerAttack":
                                ChargerScript.bileEffect = false;
                                ChargerScript.chargerAnim.SetBool("isRunning", true);
                                ChargerScript.chargerAnim.SetBool("isCollided", false);
                                ChargerScript.timerBreak = 4;
                                break;
                            case "JockeyAttack":
                                JockeyScript.bileEffect = false;
                                JockeyScript.jockeyAnim.SetBool("isRunning", true);
                                JockeyScript.jockeyAnim.SetBool("isAttackedBile", false);
                                JockeyScript.timerBreak = 4;
                                break;
                            case "Hunter":
                                hunterControl.MyInstance.bileEffect = false;
                                break;
                            case "Tank":
                                TankCharacterControl.bileEffect = false;
                                break;
                            case "Spitter":
                                SpitterCharacterControl.bileEffect = false;
                                break;
                            case "yaku":
                                collider.gameObject.GetComponent<AICharacterControl>().setBile(false);
                                break;
                            case "Boomer":
                                boomerControl.joelBombSeen = false;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            else
            {
                timer += Time.deltaTime;
                if (!isEntered)
                {
                    isEntered = true;
                    list_tag = canAttack(Colliders);
                    length = list_tag.Count;
                    foreach (Collider collider in list_tag)
                    {
                        if (collider != null)
                        {
                            if (collider.CompareTag("Hunter"))
                            {
                                hunterControl.MyInstance.bileEffect = true;
                                if (list_tag[(index + 1) % length] != null)
                                {
                                    hunterControl.targetAfterBomb = list_tag[(index + 1) % length].gameObject;

                                }
                            }
                            if (collider.CompareTag("Tank"))
                            {
                                TankCharacterControl.bileEffect = true;
                                if (list_tag[(index + 1) % length] != null)
                                {
                                    TankCharacterControl.targetAfterBomb = list_tag[(index + 1) % length].gameObject;
                                }
                            }
                            if (collider.CompareTag("Spitter"))
                            {
                                SpitterCharacterControl.bileEffect = true;
                                if (list_tag[(index + 1) % length] != null)
                                {
                                    SpitterCharacterControl.targetAfterBomb = list_tag[(index + 1) % length].gameObject;
                                }
                            }
                            if (collider.CompareTag("ChargerAttack"))
                            {
                                ChargerScript.bileEffect = true;
                                ChargerScript.chargerAnim.SetBool("isRunning", true);
                                if (list_tag[(index + 1) % length] != null)
                                {
                                    ChargerScript.targetAfterBomb = list_tag[(index + 1) % length].gameObject;
                                }
                            }
                            if (collider.CompareTag("JockeyAttack"))
                            {
                                JockeyScript.bileEffect = true;
                                JockeyScript.jockeyAnim.SetBool("isRunning", true);
                                if (list_tag[(index + 1) % length] != null)
                                {
                                    JockeyScript.targetAfterBomb = list_tag[(index + 1) % length].gameObject;
                                }
                            }
                            if (collider.CompareTag("yaku"))
                            {
                                AICharacterControl x = collider.gameObject.GetComponent<AICharacterControl>();
                                x.setBile(true);
                                if (list_tag[(index + 1) % length] != null)
                                {
                                    x.setTargetAfterBomb(list_tag[(index + 1) % length].gameObject);
                                }
                            }
                            if (collider.CompareTag("Boomer"))
                            {
                                boomerControl.joelBombSeen = true;
                            }
                            index++;
                        }
                    }
                }
            }
        }
        if (pipeCollided)
        {
            list_tag = canAttack(Colliders);
            foreach (Collider collider in list_tag)
            {
                if (collider != null)
                {
                    if (collider.CompareTag("ChargerAttack"))
                    {
                        chargerAffectedPipe = true;
                        healthDamage();
                        pipeCollided = false;
                    }
                    if (collider.CompareTag("JockeyAttack"))
                    {
                        jockeyAffectedPipe = true;
                        healthDamage();
                        pipeCollided = false;
                    }
                    if (collider.CompareTag("Tank"))
                    {
                        tankAffectedPipe = true;
                        healthDamage();
                        pipeCollided = false;
                    }
                    if (collider.CompareTag("Spitter"))
                    {
                        spitterAffectedPipe = true;
                        healthDamage();
                        pipeCollided = false;
                    }
                    if (collider.CompareTag("Hunter"))
                    {
                        hunterAffectedPipe = true;
                        healthDamage();
                        pipeCollided = false;
                    }
                    if (collider.CompareTag("Boomer"))
                    {
                        boomerAffectedPipe = true;
                        healthDamage();
                        pipeCollided = false;
                    }
                    if (collider.CompareTag("yaku"))
                    {
                        collider.gameObject.GetComponent<AICharacterControl>().setPipeBomb();
                        
                        pipeCollided = false;
                    }
                }
            }
        }
    }

    void explode()
    {

        if (bombPrefab.CompareTag("MoltovCocktail"))
        {
            afterExplosion = Instantiate(effect, transform.position, transform.rotation);
            Colliders = Physics.OverlapSphere(transform.position, radius);
            molotovCollided = true;
            foreach (Renderer r in renderers)
                r.enabled = false;
            a.Play();

        }
        if (bombPrefab.CompareTag("PipeBomb"))
        {
            Invoke("PipeEffect", 4.0f);
        }
        if (bombPrefab.CompareTag("StunGrenade"))
        {
            afterExplosion = Instantiate(effect, transform.position, transform.rotation);
            Colliders = Physics.OverlapSphere(transform.position, radius);
            stunGrenadeCollided = true;
            foreach (Renderer r in renderers)
                r.enabled = false;
            a.Play();
        }
        if (bombPrefab.CompareTag("BileBomb"))
        {
            afterExplosion = Instantiate(effect, transform.position, transform.rotation);
            Colliders = Physics.OverlapSphere(transform.position, radius);
            bileCollided = true;
            foreach (Renderer r in renderers)
                r.enabled = false;
            a.Play();
        }
        Invoke("destroy", delay);
    }

    void destroy()
    {
        Destroy(afterExplosion);
        Destroy(gameObject);
    }

    List<Collider> canAttack(Collider[] Colliders)
    {
        List<Collider> list_tags = new List<Collider>();
        foreach (Collider collider in Colliders)
        {
            if (collider != null)
            {
                switch (collider.tag)
                {
                    case "ChargerAttack":
                        list_tags.Add(collider);
                        break;
                    case "JockeyAttack":
                        list_tags.Add(collider);
                        break;
                    case "Boomer":
                        list_tags.Add(collider);
                        break;
                    case "Hunter":
                        list_tags.Add(collider);
                        break;
                    case "Tank":
                        list_tags.Add(collider);
                        break;
                    case "Spitter":
                        list_tags.Add(collider);
                        break;
                    case "yaku":
                        list_tags.Add(collider);
                        break;
                    default:
                        break;
                }
            }
        }
        return list_tags;
    }
    void PipeEffect()
    {

        afterExplosion = Instantiate(effect, transform.position, transform.rotation);
        Colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Renderer r in renderers)
            r.enabled = false;
        foreach (Collider collider in Colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //Add damage here or the effect that will happen.
                rb.AddExplosionForce(explosionForce, transform.position, radius);
                loudPeeps.Stop();
                a.Play();
                pipeCollided = true;
            }
        }
    }
    public static void Reset()
    {
        stunGrenadeCharger = false;
        stunGrenadeJockey = false;
        stunGrenadeTank = false;
        stunGrenadeSpitter = false;
        stunGrenadeBoomer = false;
        stunGrenadeHunter = false;
        stunGrenadeYaku = false;
        counterCharger = 0;
        counterJockey = 0;
        counterSpitter = 0;
        counterTank = 0;
        counterHunter = 0;
        counterBoomer = 0;
        counterYaku = 0;
        stunGrenadeCollided = false;
    }
}
