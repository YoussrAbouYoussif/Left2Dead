using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoelDamage : MonoBehaviour
{

    float timer = 0;
    float oldTimer = 0;

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Joel")){
            
            // decrease Joel health every second when standing on acid
            if (((int)timer) > 1){
                JoelScript.changeHealth(-20); 
                timer=0;  
            }
            else{
                timer += Time.deltaTime;
            }

         }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
