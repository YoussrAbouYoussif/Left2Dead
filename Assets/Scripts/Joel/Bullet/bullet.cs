using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class bullet : MonoBehaviour
{
    public GameObject particleSystem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    

    IEnumerator bloodCreate()
    {
        GameObject x = Instantiate(particleSystem, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
        yield return new WaitForSeconds(0.5f);
    }

    

    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "ChargerAttack":
                StartCoroutine(bloodCreate());
                break;
            case "JockeyAttack":
                StartCoroutine(bloodCreate());
                break;
            case "Hunter":
                StartCoroutine(bloodCreate());
                break;
            case "Tank":
                StartCoroutine(bloodCreate());

                break;
            case "Spitter":
                StartCoroutine(bloodCreate());
                break;
            case "Boomer":
                StartCoroutine(bloodCreate());
                break;
            case "yaku":
                StartCoroutine(bloodCreate());
                break;
            default:
                break;
        }
    }
}
