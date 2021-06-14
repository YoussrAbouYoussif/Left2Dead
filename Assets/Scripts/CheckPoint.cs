using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheckPoint : MonoBehaviour
{
    Scene currentScene;
    string sceneName;

    bool canGO = false;
    public GameObject KillThemPanel;
    public GameObject runPanel;
    public GameObject Joel;
    public int remaning;
    bool hunterIsDead = false;
    bool chargerIsDead = false;
    bool spitterIsDead = false;
    bool boomerIsDead = false;

    bool hunterIsEntered = false;
    bool chargerIsEntered = false;
    bool spitterIsEntered = false;
    bool boomerIsEntered = false;

    private GameObject tooltip;
    private Text tooltipText;
    void Awake()
    {
        remaning = 4;
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        tooltip = GameObject.FindWithTag("GameScreen").transform.GetChild(15).gameObject;
        tooltipText = tooltip.GetComponentInChildren<Text>();
    }

    void Update()
    {
        if (Health.dead && ChargerScript.healthPoints <= 0 && HealthHun.MyInstance.dead && SpitterCharacterControl.healthPoints <= 0 && sceneName.Equals("Mountain"))
        {
            canGO = true;
        }

        if(Health.dead)
        {
            if(!boomerIsEntered){
                remaning -=1;
                boomerIsEntered=true;
                StartCoroutine(UpdateRemaining());
            }
        }

        if(HealthHun.MyInstance.dead)
        {
            if(!hunterIsEntered){
                remaning -=1;
                hunterIsEntered=true;
                StartCoroutine(UpdateRemaining());
            }
        }

        if(ChargerScript.healthPoints <= 0 )
        {
            if(!chargerIsEntered){
                remaning -=1;
                chargerIsEntered=true;
               StartCoroutine(UpdateRemaining());
            }
        }
        if(SpitterCharacterControl.healthPoints <= 0)
        {
            if(!spitterIsEntered){
                remaning -=1;
                spitterIsEntered=true;
                StartCoroutine(UpdateRemaining());
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {

        if (other.tag.Equals("Joel"))
        {
            if (sceneName.Equals("Mountain") && canGO)
            {
                //Charger
                if (GameObject.FindWithTag("ChargerAttack") != null)
                {
                    if (GameObject.FindWithTag("ChargerAttack").activeInHierarchy != null)
                    {
                        ChargerScript.Reset();
                    }
                }

                //Hunter
                if (GameObject.FindWithTag("Hunter") != null)
                {
                    if (GameObject.FindWithTag("Hunter").activeInHierarchy != null)
                    {
                        // HealthHun.Reset();
                        // hunterControl.Reset();
                    }
                }


                //Bommer
                if (GameObject.FindWithTag("Boomer") != null)
                {
                    if (GameObject.FindWithTag("Boomer").activeInHierarchy != null)
                    {
                        boomerControl.Reset();
                        Health.Reset();
                    }

                }
                //Spitter
                if (GameObject.FindWithTag("Spitter") != null)

                {
                    if (GameObject.FindWithTag("Spitter").activeInHierarchy != null)
                    {
                        SpitterCharacterControl.Reset();
                    }

                }
                SceneManager.LoadScene("Forest");
            }
            else
            {
                if (sceneName.Equals("Forest"))
                {
                    //Hunter

                    if (GameObject.FindWithTag("Hunter") != null)
                    {
                        if (GameObject.FindWithTag("Hunter").activeInHierarchy != null)
                        {
                            // HealthHun.Reset();
                            // hunterControl.Reset();
                        }
                    }
                    //Jockey
                    if (GameObject.FindWithTag("JockeyAttack") != null)
                    {
                        if (GameObject.FindWithTag("JockeyAttack").activeInHierarchy != null)
                        {
                            JockeyScript.Reset();
                        }
                    }
                    //Charger
                    if (GameObject.FindWithTag("ChargerAttack") != null)
                    {
                        if (GameObject.FindWithTag("ChargerAttack").activeInHierarchy != null)
                        {
                            ChargerScript.Reset();
                        }
                    }
                    if (GameObject.FindWithTag("Spitter") != null)

                    {
                        if (GameObject.FindWithTag("Spitter").activeInHierarchy != null)
                        {
                            SpitterCharacterControl.Reset();
                        }

                    }

                    SceneManager.LoadScene("Cave");
                }
            }
        }
    }
    public IEnumerator UpdateRemaining()
    {
        tooltip.SetActive(true);
        tooltipText.text = "Remaining Monsters: " +remaning;
        yield return new  WaitForSecondsRealtime(1.5f);
        tooltip.SetActive(false);
    }
}
