using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Ingredients",menuName ="Items/Ingredient", order =1)]
public class Ingredients : Item
{
	[SerializeField]
    private string description;


    public override string GetDescription()
    {
        return string.Format("<color=#00ff00ff>{0}</color>",description);
    }
}
