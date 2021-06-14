using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapLogic : MonoBehaviour
{
    private GameObject TankMiniMap;
    private GameObject SpitterMiniMap;
    private GameObject JockeyMiniMap;
    private GameObject HunterMiniMap;
    private GameObject ChargerMiniMap;
    private GameObject BoomerMiniMap;


    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Tank") != null)
        {
            TankMiniMap = GameObject.FindGameObjectWithTag("Tank").transform.FindChild("TankMiniMap").gameObject;
        }
        if (GameObject.FindGameObjectWithTag("Spitter") != null)
        {
            SpitterMiniMap = GameObject.FindGameObjectWithTag("Spitter").transform.FindChild("SpitterMiniMap").gameObject;
        }
        if (GameObject.FindGameObjectWithTag("Boomer") != null)
        {
            BoomerMiniMap = GameObject.FindGameObjectWithTag("Boomer").transform.FindChild("BoomerMiniMap").gameObject;
        }
        if (GameObject.FindGameObjectWithTag("Hunter") != null)
        {
            HunterMiniMap = GameObject.FindGameObjectWithTag("Hunter").transform.FindChild("HunterMiniMap").gameObject;
        }
        if (GameObject.FindGameObjectWithTag("JockeyParent") != null)
        {
            JockeyMiniMap = GameObject.FindGameObjectWithTag("JockeyParent").transform.FindChild("JockeyMiniMap").gameObject;
        }
        if (GameObject.FindGameObjectWithTag("ChargerAttack") != null)
        {
            ChargerMiniMap = GameObject.FindGameObjectWithTag("ChargerAttack").transform.FindChild("ChargerMiniMap").gameObject;
        }


    }

    // Update is called once per frame
    void Update()
    {

        if (TankMiniMap != null && TankCharacterControl.healthPoints <= 0)
        {
            TankMiniMap.SetActive(false);
        }
        if (SpitterMiniMap != null && SpitterCharacterControl.healthPoints <= 0)
        {
            SpitterMiniMap.SetActive(false);
        }
        if (BoomerMiniMap != null && Health.health <= 0)
        {
            BoomerMiniMap.SetActive(false);
        }
        if (HunterMiniMap != null && HealthHun.MyInstance.dead == true)
        {
            HunterMiniMap.SetActive(false);
        }
        if (JockeyMiniMap != null && JockeyScript.healthPoints <= 0)
        {
            JockeyMiniMap.SetActive(false);
        }
        if (ChargerMiniMap != null && ChargerScript.healthPoints <= 0)
        {
            ChargerMiniMap.SetActive(false);
        }

    }
}
