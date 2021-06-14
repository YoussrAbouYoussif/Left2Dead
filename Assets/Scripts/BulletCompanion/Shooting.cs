using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    float bulletSpeed = 10000;
    public GameObject bullet;
    List<GameObject> bulletList;
    public GameObject compAmmo;
    public static int number;
    int count = 0;
    bool fired = false;
    bool shoot = false;
    bool decrAmmo = false;
    public GameObject remainBull;
    bool feedBackBull = true;

    void Start()
    {
        //remainBull = GameObject.FindWithTag("remainBullet");
        compAmmo.GetComponent<Text>().text = "" + CompanionBehavior.firingNumber;

        bulletList = new List<GameObject>();
        for (int i = 0; i < 10; i++)
        {
            GameObject objBullet = (GameObject)Instantiate(bullet);
            objBullet.SetActive(false);
            bulletList.Add(objBullet);
        }
    }

    void Fire()
    {
        /*for (int i = 0; i < bulletList.Count; i++)
        {*/
        if (!bulletList[0].activeInHierarchy)
        {
            bulletList[0].transform.position = transform.position;
            bulletList[0].transform.rotation = transform.rotation;
            bulletList[0].SetActive(true);
            Rigidbody tempRigidBodyBullet = bulletList[0].GetComponent<Rigidbody>();


            if(feedBackBull){
                StartCoroutine(remainInm());
            }

            if (bulletList[0].CompareTag("AssaultRifleBullet"))
            {
                tempRigidBodyBullet.AddForce(-tempRigidBodyBullet.transform.forward * bulletSpeed);
            }
            else if (bulletList[0].CompareTag("SniperBullet"))
            {
                tempRigidBodyBullet.AddForce(-tempRigidBodyBullet.transform.right * bulletSpeed);
            }
            else
            {
                tempRigidBodyBullet.AddForce(tempRigidBodyBullet.transform.forward * bulletSpeed);
            }
            CompanionBehavior.firingNumber = CompanionBehavior.firingNumber - 1;
            

            //break;

            IEnumerator remainInm()
        {
            feedBackBull = false;
            GameObject bullRem = (GameObject)Instantiate(remainBull);
            bullRem.transform.position = transform.position;

            yield return new WaitForSeconds(2.0f);

            Destroy(bullRem);
            feedBackBull = true;
        }

        }
        //}
    }



    void Update()
    {

        /*if (CompanionBehavior.numberOfClips > 1)
        {
            number = CompanionBehavior.numberOfClips * CompanionBehavior.gunAmmo;
            compAmmo.GetComponent<Text>().text = "" + number;
        }*/
        compAmmo.GetComponent<Text>().text = "" + CompanionBehavior.firingNumber;
        if (Input.GetKeyUp("q") && CompanionBehavior.notInfectedSeen)
        {
            fired = true;
        }
        if (CompanionBehavior.notInfectedSeen)
        {
            if ((CompanionBehavior.firingNumber > 0) && (JoelScript.rageMode == false))
            {

                if (shoot)
                {
                    Fire();
                    shoot = false;

                }
                else
                {
                    StartCoroutine(ShootEnm());
                }


            }

            else if (JoelScript.rageMode == true)
            {
                if (shoot)
                {
                    Fire();
                    shoot = false;
                }
                else
                {
                    StartCoroutine(ShootEnm());
                }

            }
        }
        IEnumerator ShootEnm()
        {
            shoot = false;
            yield return new WaitForSeconds(0.6f);
            shoot = true;
        }

        // IEnumerator remainInm()
        // {
        //     feedBackBull = false;
        //     GameObject bullRem = (GameObject)Instantiate(remainBull);

        //     yield return new WaitForSeconds(1.0f);

        //     Destroy(bullRem);
        //     feedBackBull = true;
        // }


    }
}