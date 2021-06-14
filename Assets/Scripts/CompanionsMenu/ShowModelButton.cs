using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ShowModelButton : MonoBehaviour
{
    public Transform objectToShow;
    public UnityAction<Transform> clickAction;
    public string nameOfObject;
    public bool pressed ;

    public void Initialize(Transform objectToShow, UnityAction<Transform> clickAction)
    {
        this.objectToShow = objectToShow;
        this.clickAction = clickAction;
        GetComponentInChildren<Text>().text = objectToShow.gameObject.name;
    }

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(HandleClickButton);
        pressed = false;
    }

    private void HandleClickButton()
    {
        clickAction(objectToShow);
        pressed = true;
        if(pressed)
        {
            nameOfObject = NameOfModel();
        }
    }

    public string NameOfModel()
    {
        return objectToShow.gameObject.name;
    }

    public void buttonClick()
    {   
        if(ShowModelController.MyInstance.isSetted)
            SceneManager.LoadScene("Mountain");
    }

    public void BackClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
