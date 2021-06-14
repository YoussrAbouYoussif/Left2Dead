using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadModel : MonoBehaviour
{
    public Transform[] target;
    string companionName;
    bool isActivate = false;
    public GameObject AllCompanions;
    static GameObject companions;
    public int x;
    private void Start()
    {
        companions = AllCompanions;
        companionName = "Ellie";
        if (!isActivate)
        {
            OnEnable();
            for (int i = 0; i < target.Length; i++)
            {
                var model = target[i];
                if (target[i].tag.Equals(companionName))
                {
                    model.gameObject.SetActive(true);
                    isActivate = true;
                }
            }
        }

    }
    public void chosen()
    {
        companions.SetActive(true);
    }
    void OnEnable()
    {
        companionName = PlayerPrefs.GetString("Model");
    }
}
