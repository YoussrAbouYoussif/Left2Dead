using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEffectTank : MonoBehaviour
{
    float timerTank = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(BombEffect.stunGrenadeTank)
        {
            if (timerTank > 2.8f)
            {
                timerTank = 0;
                TankCharacterControl.joelBombSeen = false;
                BombEffect.stunGrenadeCollided = false;
                BombEffect.stunGrenadeTank = false;
            }
            else if (timerTank > 0.05f && BombEffect.counterTank == 1)
            {
                BombEffect.counterTank = 0;
                TankCharacterControl.joelBombSeen = true;
                timerTank += Time.deltaTime;
            }
            else
            {
                timerTank += Time.deltaTime;
            }
        }
    }
}
