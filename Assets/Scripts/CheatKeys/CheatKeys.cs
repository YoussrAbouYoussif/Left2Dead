using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatKeys : MonoBehaviour
{
    public GameObject healthPackObject;
    public GameObject[] ammoBoxes;
    //length of first level 4 specials
    public GameObject[] SpawnObjectsFirstLevel;
    //length of second level 4 specials
    public GameObject[] SpawnObjectsSecondLevel;
    public GameObject tank;
    public GameObject normal;
    public GameObject normalHorde;
    public GameObject Joel;

    void Update()
    {
        //Get position if joel
        var positionJoel = Joel.transform.position;
        positionJoel.x = Joel.transform.position.x;
        positionJoel.z = Joel.transform.position.z;

        //Get active scene
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;


        if (Input.GetKeyDown(KeyCode.G))
        {
            JoelScript.changeHealth(30);
        }


        if (Input.GetKeyDown(KeyCode.H))
        {
            JoelScript.changeRagePlus(10);
        }


        if (Input.GetKeyDown(KeyCode.K))
        {
            if (sceneName.Equals("Mountain"))
            {
                Health.health = 0;
                ChargerScript.healthPoints = 0;
                HealthHun.MyInstance.health = 0;
                SpitterCharacterControl.healthPoints = 0;
                ChargerScript.healthChanged = true;
            }
            else if (sceneName.Equals("Forest"))
            {
                HealthHun.MyInstance.health = 0;
                JockeyScript.healthPoints = 0;
                SpitterCharacterControl.healthPoints = 0;
                ChargerScript.healthPoints = 0;
                ChargerScript.healthChanged = true;
                JockeyScript.healthChanged = true;
            }
            else
            {
                TankCharacterControl.healthPoints = 0;
            }
        }


        if (Input.GetKeyDown(KeyCode.L))
        {
            if (sceneName.Equals("Mountain"))
            {
                Health.health -= 10;
                ChargerScript.healthPoints -= 10;
                HealthHun.MyInstance.health -= 10;
                SpitterCharacterControl.healthPoints -= 10;
                ChargerScript.healthChanged = true;
            }
            else if (sceneName.Equals("Forest"))
            {
                ChargerScript.healthPoints -= 10;
                HealthHun.MyInstance.health -= 10;
                SpitterCharacterControl.healthPoints -= 10;
                JockeyScript.healthPoints -= 10;
                ChargerScript.healthChanged = true;
                JockeyScript.healthChanged = true;
            }
            else
            {
                TankCharacterControl.healthPoints -= 10;
            }
        }


        if (Input.GetKeyDown(KeyCode.X))
        {
            if (CompanionBehavior.numberOfClips < CompanionBehavior.maxClips)
            {
                CompanionBehavior.numberOfClips += 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            AmmoCounts.MyInstance.pickUpAmmo(700, 1);
            AmmoCounts.MyInstance.pickUpAmmo(165, 2);
            AmmoCounts.MyInstance.pickUpAmmo(130, 3);
            AmmoCounts.MyInstance.pickUpAmmo(450, 4);
        }


        if (Input.GetKeyDown(KeyCode.N))
        {
            float x = Random.Range(-10, 10);
            float z = Random.Range(-10, 10);
            int RandomOption = Random.Range(0, 3);
            if (RandomOption == 0)
            {
                Instantiate(ammoBoxes[0], new Vector3(x + positionJoel.x, 0, z + positionJoel.z), Quaternion.identity);
            }
            else if (RandomOption == 1)
            {
                Instantiate(ammoBoxes[1], new Vector3(x + positionJoel.x, -0.028f, z + positionJoel.z), Quaternion.identity);
            }
            else if (RandomOption == 2)
            {
                Instantiate(ammoBoxes[2], new Vector3(x + positionJoel.x, -0.028f, z + positionJoel.z), Quaternion.identity);
            }
            else
            {
                Instantiate(ammoBoxes[3], new Vector3(x + positionJoel.x, -1.049f, z + positionJoel.z), Quaternion.identity);
            }

        }


        if (Input.GetKeyDown(KeyCode.V))
        {
            float x = Random.Range(-10, 10);
            float z = Random.Range(-10, 10);
            Instantiate(healthPackObject, new Vector3(x + positionJoel.x, 0.5f, z + positionJoel.z), Quaternion.identity);
        }


        if (Input.GetKeyDown("3"))
        {
            float x = Random.Range(-10, 10);
            float z = Random.Range(-10, 10);
            if (sceneName.Equals("Mountain"))
            {
                int RandomOption = Random.Range(0, SpawnObjectsFirstLevel.Length);
                Instantiate(SpawnObjectsFirstLevel[RandomOption], new Vector3(x + positionJoel.x, 1, z + positionJoel.z), Quaternion.identity);
            }
            else if (sceneName.Equals("Forest"))
            {
                int RandomOption = Random.Range(0, SpawnObjectsSecondLevel.Length);
                Instantiate(SpawnObjectsSecondLevel[RandomOption], new Vector3(x + positionJoel.x, 1, z + positionJoel.z), Quaternion.identity);
            }
            else
            {
                Instantiate(tank, new Vector3(x + positionJoel.x, 1, z + positionJoel.z), Quaternion.identity);
            }
        }


        if (Input.GetKeyDown(KeyCode.I))
        {
            float x = Random.Range(-10, 10);
            float z = Random.Range(-10, 10);
            Instantiate(normalHorde, new Vector3(x + positionJoel.x, 1, z + positionJoel.z), Quaternion.identity);
        }


        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (sceneName.Equals("Mountain"))
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
                        //HealthHun.MyInstance.Reset();
                        //hunterControl.Reset();
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
            else if (sceneName.Equals("Forest"))
            {
                //Hunter
                if (GameObject.FindWithTag("Hunter") != null)
                {
                    if (GameObject.FindWithTag("Hunter").activeInHierarchy != null)
                    {
                        //HealthHun.MyInstance.Reset();
                        //hunterControl.Reset();
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


        if (Input.GetKeyDown(KeyCode.U))
        {
            if (JoelScript.rageMode == false)
            {

                JoelScript.changeRagePlus(100);
                JoelScript.rageMode = true;
            }
            else
            {
                JoelScript.rageMode = false;
            }
        }


        if (Input.GetKeyDown("2"))
        {
            int x = Random.Range(0, 3);
            JoelScript.MyInstance.AddBombById(x, 1);
        }


        if (Input.GetKeyDown("1"))
        {
            GameFlow.timerIsRunning = false;
        }
    }
}
