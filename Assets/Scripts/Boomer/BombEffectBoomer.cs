using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEffectBoomer : MonoBehaviour
{
    float timerBoomer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(BombEffect.stunGrenadeBoomer)
        {
            if (timerBoomer > 3f)
            {
                timerBoomer = 0;
                boomerControl.joelBombSeen = false;
                BombEffect.stunGrenadeCollided = false;
                BombEffect.stunGrenadeBoomer = false;
            }
            else if (timerBoomer > 0.05f && BombEffect.counterBoomer == 1)
            {
                BombEffect.counterBoomer = 0;
                boomerControl.joelBombSeen = true;
                timerBoomer += Time.deltaTime;
            }
            else
            {
                timerBoomer += Time.deltaTime;
            }
        }
    }
}
