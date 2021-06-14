using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour
{

    // public int speed = 1;

    private bool inArea = false;

    private int ItemID = -1;
    private Collider currentItem;
    //For debugging
    [SerializeField]
    private Item[] items;
    private GameObject tooltip;
    private Text tooltipText;

    AudioSource[] audios;
    AudioSource pickUp;

    // Use this for initialization
    void Start()
    {
        tooltip = GameObject.FindWithTag("GameScreen").transform.GetChild(15).gameObject;
        tooltipText = tooltip.GetComponentInChildren<Text>();

        audios = GetComponents<AudioSource>();
        pickUp = audios[2];
    }
	
	// Update is called once per frame
	void Update () {
      
        if (true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (inArea)
                {
                    if (ItemID != -1)
                    {
                        pickUp.PlayOneShot(pickUp.clip, 0.7f);
                        if (ItemID < 5)
                        {
                            InventoryScript.MyInstance.AddItem((Ingredients)Instantiate(items[ItemID]));
                        }
                        else
                        {
                            if (ItemID < 9)
                            {
                                JoelScript.MyInstance.AddWeaponById(ItemID - 4);
                            }
                            else
                            {
                                if (ItemID < 13)
                                {
                                    JoelScript.MyInstance.AddBombById(ItemID - 9, 1);
                                }
                                else
                                {

                                    if (ItemID < 18)
                                    {
                                    	AmmoCounts.MyInstance.pickUpAmmo(50,ItemID-12);
                                    }
                                    else
                                    {
                                        if (ItemID == 18)
                                        {
                                            JoelScript.changeHealth(50);
                                        }
                                    }

                                }
                            }

                        }

                        Destroy(currentItem.gameObject);
                        tooltip.SetActive(false);
                        ItemID = -1;
                    }
                }
            }

        }
	}

    private void OnTriggerEnter(Collider other)
    {
        inArea = true;
        currentItem = other;
        string textField = "";
    	switch (other.tag)
                    {
                        case "Alcohol":
                            ItemID = 0;
                            textField = "<size=33><color=white>Ingredients</color></size>\n<size=25> Alcohol</size>";
                            break;
                        case "Rag":
                            ItemID = 1;
                            textField = "<size=33><color=white>Ingredients</color></size>\n<size=25> Rag</size>";
                            break;
                        case "Sugar":
                            ItemID = 2;
                            textField = "<size=33><color=white>Ingredients</color></size>\n<size=25> Sugar</size>";
                            break;
                        case "Gunpowder":
                            ItemID = 3;
                            textField = "<size=33><color=white>Ingredients</color></size>\n<size=25> Gunpowder</size>";
                            break;
                        case "Canisters":
                            ItemID = 4;
                            textField = "<size=33><color=white>Ingredients</color></size>\n<size=25> Canisters</size>";
                            break;
                        case "SubmachineGun":
                            ItemID = 5;
                            textField = "<size=33><color=white>Weapons</color></size>\n<size=25> Submachine Gun</size>";
                            break;
                        case "HuntingRifle":
                            ItemID = 6;
                            textField = "<size=33><color=whitev>Weapons</color></size>\n<size=25> Hunting Rifle</size>";
                            break;
                        case "TacticalShotgun":
                            ItemID = 7;
                            textField = "<size=33><color=white>Weapons</color></size>\n<size=25> Tactical Shotgun</size>";
                            break;
                        case "AssaultRifle":
                            ItemID = 8;
                            textField = "<size=33><color=white>Weapons</color></size>\n<size=25> Assault Rifle</size>";
                            break;
                        case "BileBomb":
                            ItemID = 9;
                            textField = "<size=33><color=white>Bombs</color></size>\n<size=25> Bile Bomb</size>";
                            break;
                        case "MoltovCocktail":
                            ItemID = 10;
                            textField = "<size=33><color=white>Bombs</color></size>\n<size=25> Moltov Cocktail</size>";
                            break;
                        case "PipeBomb":
                            ItemID = 11;
                            textField = "<size=33><color=white>Bombs</color></size>\n<size=25> Pipe Bomb</size>";
                            break;
                        case "StunGrenade":
                            ItemID = 12;
                            textField = "<size=33><color=white>Bombs</color></size>\n<size=25> Stun Grenade</size>";
                            break;
                        case "Ammo1":
                            ItemID = 13;
                            textField = "<size=33><color=white>Ammos</color></size>\n<size=20> Increases Ammos by 50 of SubMachine Gun</size>";
                            break;
                        case "Ammo2":
                            ItemID = 14;
                            textField = "<size=33><color=white>Ammos</color></size>\n<size=20> Increases Ammos by 50 of Hunting Rifle</size>";
                            break;
                        case "Ammo3":
                            ItemID = 15;
                            textField = "<size=33><color=white>Ammos</color></size>\n<size=20> Increases Ammos by 50 of Tactical Shotgun</size>";
                            break;
                        case "Ammo4":
                            ItemID = 16;
                            textField = "<size=33><color=white>Ammos</color></size>\n<size=20> Increases Ammos by 50 of Assault Rifle</size>";
                            break;
                        case "Ammo5":
                            ItemID = 17;
                            textField = "<size=33><color=white>Ammos</color></size>\n UNKNOWN";
                            break;
                        case "healthPack2":
                            ItemID = 18;
                            textField = "<size=33><color=white>Health</color></size>\n<size=20> Increases Heath by 50 points</size>";
                            break;
                        default:
                            break;
                    }
        if(ItemID!= -1)
        {
            tooltip.SetActive(true);
            tooltipText.text = textField;
        }



    }

    private void OnTriggerExit(Collider other)
    {
        inArea = false;
        ItemID = -1;
        tooltip.SetActive(false);
    }
}
