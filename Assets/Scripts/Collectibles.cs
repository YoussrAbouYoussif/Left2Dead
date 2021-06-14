using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collectibles : MonoBehaviour
{
    public GameObject[] areas;

    public GameObject[] collectibles;
    public ArrayList InstantiatedCollectibles;
    Scene currentScene;
    string sceneName;
    
        


    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        InstantiatedCollectibles = new ArrayList();
        int sizeOfAreas = areas.Length;
        int sizeOfCollectibles = collectibles.Length;
        int noOfInstantiations = Random.Range(sizeOfAreas, sizeOfAreas*4);
        for (int i = 0; i < sizeOfCollectibles * 4; i++)
        {

            if (sceneName.Equals("Cave"))
            {
                int areaNo = Random.Range(0, sizeOfAreas);
                int collectibleType = i % sizeOfCollectibles;
                int factorOfSpace = areas[areaNo].transform.childCount;
                if (factorOfSpace < 3)
                {
                    float factorOfSpace_x = 0;
                    float factorOfSpace_z = 0;
                    if (factorOfSpace != 0)
                    {
                        factorOfSpace_x = 0.9f * (factorOfSpace % 2) * (1 - 2 * ((int)(factorOfSpace / 2)));
                        factorOfSpace_z = 0.9f * ((factorOfSpace + 1) % 2) * (1 - 2 * ((int)((factorOfSpace + 1) / 2)));
                    }
                    var collectibleInstance = Instantiate(collectibles[collectibleType], areas[areaNo].transform);
                    collectibleInstance.transform.position = new Vector3(areas[areaNo].transform.position.x + factorOfSpace_x, (collectibles[collectibleType].transform.position.y + areas[areaNo].transform.position.y - 0.1f), areas[areaNo].transform.position.z + factorOfSpace_z);

                    InstantiatedCollectibles.Add(collectibleInstance);
                }
            }
            else
            {
                int areaNo = Random.Range(0, sizeOfAreas);
                int collectibleType = i % sizeOfCollectibles;
                int factorOfSpace = areas[areaNo].transform.childCount;
                if (factorOfSpace < 5)
                {
                    float factorOfSpace_x = 0;
                    float factorOfSpace_z = 0;
                    if (factorOfSpace != 0)
                    {
                        factorOfSpace_x = 0.9f * (factorOfSpace % 2) * (1 - 2 * ((int)(factorOfSpace / 2)));
                        factorOfSpace_z = 0.9f * ((factorOfSpace + 1) % 2) * (1 - 2 * ((int)((factorOfSpace + 1) / 2)));
                    }
                    var collectibleInstance = Instantiate(collectibles[collectibleType], areas[areaNo].transform);
                    collectibleInstance.transform.position = new Vector3(areas[areaNo].transform.position.x + factorOfSpace_x, (collectibles[collectibleType].transform.position.y + areas[areaNo].transform.position.y - 0.1f), areas[areaNo].transform.position.z + factorOfSpace_z);

                    InstantiatedCollectibles.Add(collectibleInstance);
                }
            }
                



        }

    }

    // Update is called once per frame
    void Update()
    {
    }


    private void OnTriggerEnter(Collider other)
    {
    }
}
