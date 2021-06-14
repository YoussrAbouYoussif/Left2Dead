using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JockeyCollide : MonoBehaviour
{
    float timer = 0;
    float oldTimer = 0;
    GameObject jockey;
    GameObject joel;
    Animator jockeyAnim;

    public AudioSource walkingAudio;
    bool audioWalkEnable = true;
    bool jockeyDamage = true;
    // Start is called before the first frame update
    void Start()
    {
        jockey = GameObject.FindWithTag("JockeyAttack");
        joel = GameObject.FindWithTag("Joel");
        jockeyAnim = jockey.GetComponent<Animator>();
        walkingAudio = GetComponent<AudioSource>();
    }

    void healthDamage()
    {
        if (jockeyDamage)
        {
            JoelScript.changeHealth(-20);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!JockeyScript.jockeyEnable)
        {
            if(((int) timer) > ((int) oldTimer))
            {
                Invoke("healthDamage", 1);
                oldTimer = timer;
            }
            if (timer > 5)
            {
                joel.GetComponent<Rigidbody>().mass = 5;
                timer = 0;
                oldTimer = 0;
                JockeyScript.jockeyEnable = true;
                jockey.SetActive(true);
                this.transform.LookAt(new Vector3(joel.transform.position.x, joel.transform.position.y, -joel.transform.position.z));
                jockeyAnim.SetBool("isCollided", true);
                if ((joel.transform.eulerAngles.y - 360 >= -90 && joel.transform.eulerAngles.y - 360 <= 90) || joel.transform.eulerAngles.y == 0)
                {
                    this.transform.localPosition = new Vector3(joel.transform.position.x, joel.transform.position.y, joel.transform.position.z - 3);
                }
                else
                {
                    this.transform.localPosition = new Vector3(joel.transform.position.x, joel.transform.position.y, joel.transform.position.z + 3);
                }
                jockey.transform.localPosition = new Vector3(0, 0, 0);
                audioWalkEnable = true;
                walkingAudio.Stop();
            }
            else if(timer > 1f)
            {
                joel.GetComponent<Rigidbody>().mass = 5;
                if (JockeyScript.randomNumber == 1)
                {
                    //Move Right
                    jockey.SetActive(false);
                    timer += Time.deltaTime;
                    joel.transform.Translate(new Vector3(timer * 0.02f, 0f, 0f));
                    if (audioWalkEnable)
                    {
                        audioWalkEnable = false;
                        walkingAudio.Play();
                    }
                    if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
                    {
                        jockeyDamage = false;
                    }
                    else
                    {
                        jockeyDamage = true;
                    }
                }
                else if(JockeyScript.randomNumber == 2)
                {
                    //Move Left
                    jockey.SetActive(false);
                    timer += Time.deltaTime;
                    joel.transform.Translate(new Vector3(-timer * 0.02f, 0f, 0f));
                    if (audioWalkEnable)
                    {
                        audioWalkEnable = false;
                        walkingAudio.Play();
                    }
                    if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
                    {
                        jockeyDamage = false;
                    }
                    else
                    {
                        jockeyDamage = true;
                    }
                }
                else
                {
                    //Move Forward
                    jockey.SetActive(false);
                    timer += Time.deltaTime;
                    joel.transform.Translate(new Vector3(0f, 0f, timer * 0.02f));
                    if (audioWalkEnable)
                    {
                        audioWalkEnable = false;
                        walkingAudio.Play();
                    }
                    if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow))
                    {
                        jockeyDamage = false;
                    }
                    else
                    {
                        jockeyDamage = true;
                    }
                }
            }
            else
            {
                timer += Time.deltaTime;
                if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)) && JockeyScript.randomNumber ==1)
                {
                    jockeyDamage = false;
                }
                else if((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)) && JockeyScript.randomNumber == 2)
                {
                    jockeyDamage = false;
                }
                else if((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.DownArrow)) && JockeyScript.randomNumber == 3)
                {
                    jockeyDamage = false;
                }
                else
                {
                    jockeyDamage = true;
                }
            }
        }
    }
}
