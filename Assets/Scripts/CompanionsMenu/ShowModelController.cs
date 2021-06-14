using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowModelController : MonoBehaviour
{
    private static ShowModelController instance;

    public static ShowModelController MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ShowModelController>();
            }

            return instance;
        }
    }
    public bool isSetted = false;
    private List<Transform> models;
    public string modelName;

    public void Update()
    {
        OnDisable();
    }

    private void Awake()
    {
        models = new List<Transform>();
        for(int i=0; i<transform.childCount; i++)
        {
            var model = transform.GetChild(i);
            models.Add(model);
            model.gameObject.SetActive(i == 0);
        }
        PlayerPrefs.SetString("Model", "Ellie");

    }

    void OnDisable()
    {
        PlayerPrefs.SetString("Model", modelName);
        

    }

    public void EnableModel(Transform modelTransform)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var transformToToggle = transform.GetChild(i);
            bool shouldBeActive = transformToToggle == modelTransform;
            transformToToggle.gameObject.SetActive(shouldBeActive);
        }
         isSetted = true;
        modelName = modelTransform.name;
    }

    public string NameOf()
    {
        return modelName;
    }

    public List<Transform> GetModels()
    {
        return new List<Transform>(models);
    }
}
