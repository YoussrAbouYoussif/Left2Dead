using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	
public class AmmoCounts : MonoBehaviour
{
	private static AmmoCounts instance;

        public static AmmoCounts MyInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<AmmoCounts>();
                }

                return instance;
            }
        }

 	public  int currSubMachineAmmo;
    public  int maxSubMachineAmmo;

    public  int currHuntingRifleAmmo;
    public  int maxHuntingRifleAmmo;

    public  int currTacticalShotgunAmmo;
    public  int maxTacticalShotgunAmmo;   
    
    public  int currAssaultRifleAmmo;
    public  int maxAssaultRifleAmmo;

    void Awake()
    {
    	currSubMachineAmmo = 0;
    	currHuntingRifleAmmo = 0;
    	currTacticalShotgunAmmo = 0;
    	currAssaultRifleAmmo = 0;
    	maxSubMachineAmmo = 700;
    	maxHuntingRifleAmmo = 165;
    	maxTacticalShotgunAmmo = 130;
    	maxAssaultRifleAmmo = 450;


    }

    public void pickUpAmmo(int ammoCount, int itemID)
    {

    	switch(itemID)
    	{
    		case 1:
    			currSubMachineAmmo += ammoCount;
    			if (currSubMachineAmmo > maxSubMachineAmmo)
    				currSubMachineAmmo = maxSubMachineAmmo;
    			break;
    		case 2:
    			currHuntingRifleAmmo += ammoCount;
    			if (currHuntingRifleAmmo > maxHuntingRifleAmmo)
    				currHuntingRifleAmmo = maxHuntingRifleAmmo;
    			break;
    		case 3:
    			currTacticalShotgunAmmo += ammoCount;
    			if (currTacticalShotgunAmmo > maxTacticalShotgunAmmo)
    				currTacticalShotgunAmmo = maxTacticalShotgunAmmo;
    			break;
    		case 4:
    			currAssaultRifleAmmo += ammoCount;
    			if (currAssaultRifleAmmo > maxAssaultRifleAmmo)
    				currAssaultRifleAmmo = maxAssaultRifleAmmo;
    			break;
    		default:
    		  break;
    	}

    }
}
