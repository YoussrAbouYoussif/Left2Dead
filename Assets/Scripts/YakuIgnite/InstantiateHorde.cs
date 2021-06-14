using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateHorde : MonoBehaviour
{
    public GameObject normalInfected;
    GameObject mainCamera;
    AudioSource[] audios;
    bool instantiated = false;


    // Start is called before the first frame update
    void Start()
    {
        audios = GetComponents<AudioSource>();
        mainCamera = GameObject.FindWithTag("Joel");
    }

    void hordeOfFour()
    {
        audios[0].PlayOneShot(audios[0].clip, 0.7f);

        GameObject normal_1 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 mainCamera.transform.rotation);
        GameObject normal_2 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 mainCamera.transform.rotation);

        GameObject normal_3 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 mainCamera.transform.rotation);

        GameObject normal_4 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 mainCamera.transform.rotation);


        GameObject normal_5 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 mainCamera.transform.rotation);

        GameObject normal_6 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 mainCamera.transform.rotation);

        GameObject normal_7 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 mainCamera.transform.rotation);

        GameObject normal_8 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 mainCamera.transform.rotation);




        GameObject normal_9 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 mainCamera.transform.rotation);

        GameObject normal_10 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 mainCamera.transform.rotation);

        GameObject normal_11 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 mainCamera.transform.rotation);

        GameObject normal_12 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 mainCamera.transform.rotation);


        GameObject normal_13 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 mainCamera.transform.rotation);

        GameObject normal_14 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 mainCamera.transform.rotation);

        GameObject normal_15 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 mainCamera.transform.rotation);

        GameObject normal_16 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 mainCamera.transform.rotation);
        GameObject normal_17 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 mainCamera.transform.rotation);

        GameObject normal_18 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 mainCamera.transform.rotation);

        GameObject normal_19 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 mainCamera.transform.rotation);

        GameObject normal_20 = Instantiate(normalInfected,
                 new Vector3(Random.Range(this.transform.position.x - 20.0f, this.transform.position.x + 20.0f), this.transform.position.y, Random.Range(this.transform.position.z - 20.0f, this.transform.position.z + 20.0f)),
                 mainCamera.transform.rotation);


    }
    private void OnTriggerEnter(Collider other)
    {
        if (!instantiated && other.gameObject.CompareTag("Joel"))
        {
            hordeOfFour();
            instantiated = true;
        }
    }

}
