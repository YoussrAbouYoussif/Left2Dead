using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowModelUI : MonoBehaviour
{
    [SerializeField] 
    private ShowModelButton buttonPrefab;
    public Vector3 nextPos;
    public static AudioSource[] a;
    public static AudioSource pause;
    public static AudioSource back;

    void Start()
    {
        a = GetComponents<AudioSource>();
        pause = a[0];
        back = a[1];
        pause.Play();
        nextPos = new Vector3(-500f, 0f, 0.0f);
        var models = FindObjectOfType<ShowModelController>().GetModels();
        foreach (var model in models)
        {
            CreateButtonForModel(model);
        }
    }

    private void CreateButtonForModel(Transform model)
    {
        var button = Instantiate(buttonPrefab);
        button.transform.SetParent(this.transform);
        button.transform.localScale = Vector3.one;
        button.transform.localRotation = Quaternion.identity;
        button.transform.localPosition = nextPos;
        nextPos.x = nextPos.x + 350;
        var controller = FindObjectOfType<ShowModelController>();
        button.Initialize(model, controller.EnableModel);
    }
}
