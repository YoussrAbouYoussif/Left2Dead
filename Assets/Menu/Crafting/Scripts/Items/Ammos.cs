using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Ammos",menuName ="Items/Ammo", order =1)]
public class Ammos : Item, IUseable
{

	[SerializeField]
    private string description;
    [SerializeField]
    private int AmmoCount;
    // Start is called before the first frame update
    public void Use()
    {
    }

 	public override string GetDescription()
    {
        return string.Format("<color=#00ff00ff>{0}</color>",description);
    }
}


