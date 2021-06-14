using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionRescue : MonoBehaviour
{

    public static bool rescued;
    public Transform[] target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Joel"))
        {
            // string companionName;
            // companionName = PlayerPrefs.GetString("Model");
            rescued = true;
            gameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("load").GetComponent<LoadModel>().chosen();
        }
    }
}
