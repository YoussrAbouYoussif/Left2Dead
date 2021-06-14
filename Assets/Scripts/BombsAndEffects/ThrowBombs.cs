using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBombs : MonoBehaviour
{
	public Transform spwanPoint;
	public GameObject[] bombs;
	float range = 10.0f;
    Animator anim;
    GameObject grandBomb;
    private bool isShooted =false;

    private Vector3 old_postion;
    public static Vector3 bombPostion;
    
    void Start()
    {
        old_postion = new Vector3(0,0,0);
        bombPostion = new Vector3(0,0,0);
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameMenu.pause)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (JoelScript.MyInstance.equipedGrenades[JoelScript.grenadeIndex] > 0)
                {
                    anim = transform.GetChild(JoelScript.weaponIndex).GetChild(0).GetChild(1).gameObject.GetComponent<Animator>();
                    Launch();
                    JoelScript.MyInstance.AddBombById(JoelScript.grenadeIndex, -1);
                    if (JoelScript.activeGrenade == "PipeBomb" || JoelScript.activeGrenade == "StunGrenade" || JoelScript.activeGrenade == "MolotovCocktail" || JoelScript.activeGrenade == "BileBomb")
                    {
                        anim.Play("GrenadeThrow", 0, 0.0f);
                        //a.Play();
                    }
                }
            }

            if (isShooted)
            {
                if (V3Equal(old_postion, grandBomb.transform.position))
                {
                    isShooted = false;
                    bombPostion = grandBomb.transform.position;
                    old_postion = new Vector3(0, 0, 0);


                }
                else
                {
                    old_postion = grandBomb.transform.position;
                }
            }

        }
    }

    void Launch() {

        grandBomb = Instantiate(bombs[JoelScript.grenadeIndex], spwanPoint.position, spwanPoint.rotation);
        
        Collider[] colliders;
        colliders = grandBomb.GetComponents<Collider>();
        colliders[1].enabled =false;
        
        grandBomb.GetComponent<Rigidbody>().AddForce(spwanPoint.forward *range , ForceMode.VelocityChange);
        grandBomb.GetComponent<BombEffect>().enabled = true;
        isShooted = true;
        if(grandBomb.gameObject.CompareTag("PipeBomb"))
        {
            BombEffect.loudPeeps.Play();
            BombEffect.loudPeeps.loop = true;
        }

    }
     public bool V3Equal(Vector3 a, Vector3 b){
     return Vector3.SqrMagnitude(a - b) < 0.0001;
 }
   
}
