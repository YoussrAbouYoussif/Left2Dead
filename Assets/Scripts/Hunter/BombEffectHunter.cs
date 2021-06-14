using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEffectHunter : MonoBehaviour
{
    float timerHunter = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (BombEffect.stunGrenadeHunter)
        {
            if (timerHunter > 3f)
            {
                timerHunter = 0;
                hunterControl.MyInstance.joelBombSeen = false;
                BombEffect.stunGrenadeCollided = false;
                BombEffect.stunGrenadeHunter = false;
            }
            else if (timerHunter > 0.01f && BombEffect.counterHunter == 1)
            {
                BombEffect.counterHunter = 0;
                hunterControl.MyInstance.joelBombSeen = true;
                timerHunter += Time.deltaTime;
            }
            else
            {
                timerHunter += Time.deltaTime;
            }
        }
    }
}
