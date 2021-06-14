using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEffectJockey : MonoBehaviour
{
    float timerJockey = 0;
    bool isRunningJockey = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(BombEffect.stunGrenadeJockey)
        {
            if (JockeyScript.jockeyAnim.GetCurrentAnimatorStateInfo(0).IsName("Running"))
            {
                isRunningJockey = true;
            }
            if (timerJockey > 2.8f && isRunningJockey)
            {
                timerJockey = 0;
                BombEffect.stunGrenadeCollided = false;
                BombEffect.stunGrenadeJockey = false;
                JockeyScript.timerBreak = 4f;
                JockeyScript.jockeyAnim.SetBool("isRunning", true);
                JockeyScript.jockeyAnim.SetBool("isCollided", false);
                JockeyScript.jockeyAnim.SetBool("isAttacked", false);
                JockeyScript.jockeyAnim.SetBool("isIdle", false);
                isRunningJockey = false;
                JockeyScript.bombCollided = false;
            }
            else if (timerJockey > 2.8f && !isRunningJockey)
            {
                timerJockey = 0;
                BombEffect.stunGrenadeCollided = false;
                BombEffect.stunGrenadeJockey = false;
                JockeyScript.timerBreak = 4.5f;
                JockeyScript.bombCollided = false;
            }
            else if (timerJockey > 0.1f && BombEffect.counterJockey == 1)
            {
                BombEffect.counterJockey = 0;
                JockeyScript.jockeyAnim.SetBool("isRunning", false);
                JockeyScript.jockeyAnim.SetBool("isCollided", false);
                JockeyScript.jockeyAnim.SetBool("isAttacked", false);
                JockeyScript.jockeyAnim.SetBool("isIdle", true);
                JockeyScript.timerBreak = 0;
                timerJockey += Time.deltaTime;
                JockeyScript.bombCollided = true;
            }
            else
            {
                timerJockey += Time.deltaTime;
            }
        }
    }
}
