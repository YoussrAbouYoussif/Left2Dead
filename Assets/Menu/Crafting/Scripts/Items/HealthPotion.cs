using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="HealthPotion",menuName ="Items/Potion", order =1)]
public class HealthPotion : Item, IUseable
{
    [SerializeField]
    private float healthValue;

    private Image healthBarFill;
    public void Use()
    {
       healthBarFill =(Image)GameObject.FindGameObjectsWithTag("HealthBar")[0].GetComponent<Image>();
       healthBarFill.fillAmount += healthValue/300f;
    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n<color=#00ff00ff>Use: Restores {0} health</color>", healthValue);
    }

}
