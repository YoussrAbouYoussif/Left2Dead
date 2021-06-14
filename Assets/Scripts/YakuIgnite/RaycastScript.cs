using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
public class RaycastScript : MonoBehaviour
{
    GameObject yaku;
    CharacterController x;
    private static RaycastScript instance;

    public static RaycastScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<RaycastScript>();
            }

            return instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        yaku = GameObject.FindGameObjectWithTag("yaku");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray rray = new Ray(transform.position, transform.forward);
        Vector3 i = transform.position;
        if (Physics.SphereCast(i, 1, transform.forward, out hit, 1000))
        {
            if (hit.collider.gameObject.CompareTag("Joel"))
            {
              
                try
                {
                    yaku.GetComponent<AICharacterControl>().setWithinRange(true);
                }
                catch (System.Exception)
                {

                    Destroy(this);
                }
            }
        }
    }
}
