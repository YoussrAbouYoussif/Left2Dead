using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blurr : MonoBehaviour
{

    GameObject Joel;
    public GameObject blurrPanel;
    public GameObject normalInfected;
    bool collided = false;


    // Start is called before the first frame update
    void Start()
    {
        Joel = GameObject.FindWithTag("Joel");
    }

    // Update is called once per frame
    void Update()
    {


    }

    private void OnCollisionEnter(Collision collision)
    {


        if (collision.gameObject.CompareTag("Bomb") && collided == false)
        {
            JoelScript.MyInstance.BlurEffect();
            // StartCoroutine(explosionBlurr());
            StartCoroutine(hordeOfFour());
            collided = true;
        }

    }

    IEnumerator explosionBlurr()
    {
        GameObject blurrPanelCreated = Instantiate(blurrPanel,
                 new Vector3(Joel.transform.position.x, Joel.transform.position.y, Joel.transform.position.z),
                 Joel.transform.rotation);

        yield return new WaitForSeconds(4);

        Destroy(blurrPanelCreated);
        collided = false;
    }

    IEnumerator hordeOfFour()
    {



        GameObject normal_1 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 Joel.transform.rotation);

        GameObject normal_2 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 Joel.transform.rotation);

        GameObject normal_3 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 Joel.transform.rotation);

        GameObject normal_4 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 Joel.transform.rotation);

        yield return new WaitForSeconds(1);

        GameObject normal_5 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 Joel.transform.rotation);

        GameObject normal_6 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 Joel.transform.rotation);

        GameObject normal_7 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 Joel.transform.rotation);

        GameObject normal_8 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 Joel.transform.rotation);


        yield return new WaitForSeconds(1);


        GameObject normal_9 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 Joel.transform.rotation);

        GameObject normal_10 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 Joel.transform.rotation);

        GameObject normal_11 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 Joel.transform.rotation);

        GameObject normal_12 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 Joel.transform.rotation);

        yield return new WaitForSeconds(1);

        GameObject normal_13 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 Joel.transform.rotation);

        GameObject normal_14 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 Joel.transform.rotation);

        GameObject normal_15 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 Joel.transform.rotation);

        GameObject normal_16 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 Joel.transform.rotation);

        yield return new WaitForSeconds(1);

    }
}
