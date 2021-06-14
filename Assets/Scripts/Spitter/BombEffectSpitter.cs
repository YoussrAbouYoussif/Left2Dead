using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEffectSpitter : MonoBehaviour
{
    float timerSpitter = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(BombEffect.stunGrenadeSpitter)
        {
            if (timerSpitter > 2.8f)
            {
                timerSpitter = 0;
                SpitterCharacterControl.joelBombSeen = false;
                BombEffect.stunGrenadeCollided = false;
                BombEffect.stunGrenadeSpitter = false;
            }
            else if (timerSpitter > 0.01f && BombEffect.counterSpitter == 1)
            {
                BombEffect.counterSpitter = 0;
                SpitterCharacterControl.joelBombSeen = true;
                timerSpitter += Time.deltaTime;
            }
            else
            {
                timerSpitter += Time.deltaTime;
            }
        } 
    }
}
