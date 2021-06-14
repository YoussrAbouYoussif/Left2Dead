using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Weapons",menuName ="Items/Weapon", order =1)]
public class Weapons : Item, IUseable
{

	[SerializeField]
    private string description;
    // Start is called before the first frame update
    public void Use()
    {
    }

 	public override string GetDescription()
    {
        return string.Format("<color=#00ff00ff>{0}</color>",description);
    }
}


