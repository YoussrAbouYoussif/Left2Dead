using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEffectCharger : MonoBehaviour
{
    float timerCharger = 0;
    bool isRunningCharger = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(BombEffect.stunGrenadeCollided)
        {
            if (ChargerScript.chargerAnim.GetCurrentAnimatorStateInfo(0).IsName("Running"))
            {
                isRunningCharger = true;
            }
            if (timerCharger > 2.8f && isRunningCharger)
            {
                timerCharger = 0;
                BombEffect.stunGrenadeCollided = false;
                ChargerScript.chargerAnim.SetBool("isRunning", true);
                ChargerScript.chargerAnim.SetBool("isCollided", false);
                ChargerScript.chargerAnim.SetBool("isAttacked", false);
                ChargerScript.chargerAnim.SetBool("isIdle", false);
                ChargerScript.timerBreak = 3.5f;
                ChargerScript.bombCollided = false;
                isRunningCharger = false;
                BombEffect.stunGrenadeCharger = false;
            }
            else if (timerCharger > 2.8f && !isRunningCharger)
            {
                timerCharger = 0;
                BombEffect.stunGrenadeCollided = false;
                ChargerScript.timerBreak = 4.5f;
                ChargerScript.bombCollided = false;
                BombEffect.stunGrenadeCharger = false;
            }
            else if (timerCharger > 0.1f && BombEffect.counterCharger == 1)
            {
                BombEffect.counterCharger = 0;
                ChargerScript.chargerAnim.SetBool("isRunning", false);
                ChargerScript.chargerAnim.SetBool("isCollided", false);
                ChargerScript.chargerAnim.SetBool("isAttacked", false);
                ChargerScript.chargerAnim.SetBool("isIdle", true);
                ChargerScript.timerBreak = 0;
                timerCharger += Time.deltaTime;
                ChargerScript.bombCollided = true;
            }
            else
            {
                timerCharger += Time.deltaTime;
            }
        }
        
    }
}
