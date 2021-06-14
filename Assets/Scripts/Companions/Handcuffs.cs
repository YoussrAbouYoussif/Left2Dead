using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handcuffs : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Joel"))
        {
            gameObject.SetActive(false);
        }
    }
}
