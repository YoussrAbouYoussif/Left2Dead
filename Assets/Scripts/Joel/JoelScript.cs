using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class JoelScript : MonoBehaviour
{
    private static JoelScript instance;

    public static JoelScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<JoelScript>();
            }

            return instance;
        }
    }
    // Start is called before the first frame update
    public GameObject[] availWeapons;
    public List<GameObject> equipedWeapons;
    public List<int> equipedGrenades;
    // public List<int> equipedWeapons_INDEX;
    private List<int> myQueue;
    private String[] availGrenades = { "BileBomb", "MolotovCocktail", "PipeBomb", "StunGrenade" };
    public static int weaponIndex = 0;
    public static int health = 300;
    public static int rage = 0;
    public static String activeGrenade = "";
    public static int grenadeIndex = 0;

    private static Image healthBarFill;


    public static int killedNInfecteds = 0;
    public static int killedSInfecteds = 0;
    public static float rageTimer = 0;
    public static bool rageMode = false;

    private static Image rageBarFill;
    private Text[] bombTexts;
    public Text[] soltsTexts;
    GameObject gameScreen;
    Image effectImage;

    AudioSource[] audios;
    AudioSource Rage;
    AudioSource hitJoel;

    [SerializeField]
    private Sprite damageIcon;
    [SerializeField]
    private Sprite blurIcon;
    [SerializeField]
    private Sprite killIcon;
    [SerializeField]
    private Sprite runIcon;

    private Scene currentScene;
    private string sceneName;


    void Awake()
    {
        bombTexts = new Text[] {
            GameObject.FindWithTag("BileText").GetComponent<Text>(),
            GameObject.FindWithTag("MolotovText").GetComponent<Text>(),
            GameObject.FindWithTag("PipeText").GetComponent<Text>(),
            GameObject.FindWithTag("StunText").GetComponent<Text>()
           };
        GameObject craftMenu = GameObject.FindWithTag("CraftingMenu");
        gameScreen = GameObject.FindWithTag("GameScreen");
        effectImage = gameScreen.GetComponent<Image>();
        soltsTexts = new Text[] {
            craftMenu.transform.GetChild(0).GetChild(4).GetChild(0).GetChild(0).GetComponent<Text>(),
            craftMenu.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>(),
            craftMenu.transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetComponent<Text>(),
            craftMenu.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>(),
        };
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;


    }
    void Start()
    {
        healthBarFill = (Image)GameObject.FindGameObjectsWithTag("HealthBar")[0].GetComponent<Image>();
        rageBarFill = (Image)GameObject.FindGameObjectsWithTag("RageBar")[0].GetComponent<Image>();
        equipedWeapons = new List<GameObject>();
        equipedGrenades = new List<int>();
        myQueue = new List<int>();
        myQueue.Add(0);
        // availWeapons[weaponIndex].SetActive(true);
        equipedWeapons.Add(availWeapons[0]);
        equipedWeapons[weaponIndex].SetActive(true);
        equipedWeapons.Add(availWeapons[1]);
        equipedWeapons.Add(availWeapons[2]);
        equipedWeapons.Add(availWeapons[3]);
        equipedWeapons.Add(availWeapons[4]);
        equipedGrenades.Add(0);
        equipedGrenades.Add(0);
        equipedGrenades.Add(0);
        equipedGrenades.Add(0);

        healthBarFill.fillAmount = (health / 300.0f);
        rageBarFill.fillAmount = (rage / 100.0f);
        bombTexts[0].color = new Color(0.165f, 0.33f, 0.77f, 1f);

        audios = GetComponents<AudioSource>();
        Rage = audios[1];
        hitJoel = audios[3];
        StartEffect();

    }
    void Update()
    {
        //Dont Erase
        //  mainAudioSource.clip = SoundClips.hitSound;
        // 	mainAudioSource.Play ();
        //  StartCoroutine(Shake(0.5f, 0.4f));
        if (!GameMenu.pause)
        {

            bool canMove = true;
            if (GameObject.FindWithTag("Hunter") != null)
            {
                if (GameObject.FindWithTag("Hunter").activeInHierarchy != null)
                {
                    canMove = hunterControl.MyInstance.canMove;
                }
            }

            if (Input.GetKeyDown(KeyCode.C) && canMove)
            {
                int currentWeaponIndex = myQueue[0];
                myQueue.Remove(currentWeaponIndex);
                myQueue.Add(currentWeaponIndex);
                equipedWeapons[currentWeaponIndex].SetActive(false);
                weaponIndex = myQueue[0];
                equipedWeapons[weaponIndex].SetActive(true);

            }
            if (Input.GetKeyDown(KeyCode.B) && canMove)
            {
                StartCoroutine(Shake(0.5f, 0.4f));

            }
            if (Input.GetKeyDown(KeyCode.Z) && canMove)
            {
                bombTexts[grenadeIndex].color = new Color(1f, 1f, 1f, 1f);

                int grenadeLen = availGrenades.Length;
                if (grenadeIndex == grenadeLen - 1)
                {
                    activeGrenade = availGrenades[grenadeIndex];
                    grenadeIndex = 0;
                }
                else
                {
                    activeGrenade = availGrenades[grenadeIndex];
                    grenadeIndex++;
                }
                bombTexts[grenadeIndex].color = new Color(0.165f, 0.33f, 0.77f, 1f);


            }

            if (Input.GetKeyDown(KeyCode.F) && canMove)
            {
                if (rage == 100)
                {
                    rageMode = true;
                    rageTimer = 0;
                    Rage.PlayOneShot(Rage.clip, 0.7f);
                }

            }
            rageTimer += Time.deltaTime;
            if (rageMode)
            {

                if (rageTimer > 7)
                {
                    rage = 0;
                    rageMode = false;
                    rageBarFill.fillAmount = rage;
                }
                else
                {
                    rageBarFill.fillAmount =rageBarFill.fillAmount - (rageTimer/7.0f);
                }

            }
            else
            {
                if (rageTimer > 3)
                {
                    if( Rage != null)
                        Rage.Stop();
                    rage = 0;
                    rageBarFill.fillAmount = rage;
                }
            }
        }
    }



    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = availWeapons[weaponIndex].transform.position;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-0.5f, 0.5f) * magnitude;
            float y = UnityEngine.Random.Range(-0.3f, 0.3f) * magnitude;
            float z = UnityEngine.Random.Range(-0.3f, 0.3f) * magnitude;
            availWeapons[weaponIndex].transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z + z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        availWeapons[weaponIndex].transform.position = originalPos;
    }

    public IEnumerator DamageShake(int alpha, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        var tempColor = effectImage.color;
        tempColor.a = alpha / 255f;
        effectImage.color = tempColor;
    }
    public IEnumerator StopEffetc()
    {
        yield return new WaitForSeconds(1.5f);
        effectImage.enabled = false;
    }

    public void AddBombById(int id, int value)
    {
        switch (id)
        {
            case 0:
                equipedGrenades[id] += value;
                if (equipedGrenades[id] > 1)
                {
                    equipedGrenades[id] = 1;
                }
                break;
            case 1:
                equipedGrenades[id] += value;
                if (equipedGrenades[id] > 3)
                {
                    equipedGrenades[id] = 3;
                }
                break;
            case 2:
                equipedGrenades[id] += value;
                if (equipedGrenades[id] > 2)
                {
                    equipedGrenades[id] = 2;
                }
                break;
            case 3:
                equipedGrenades[id] += value;
                if (equipedGrenades[id] > 2)
                {
                    equipedGrenades[id] = 2;
                }
                break;
            default:
                break;
        }
        bombTexts[id].text = equipedGrenades[id].ToString();
        soltsTexts[id].text = equipedGrenades[id].ToString();
    }

        public void AddBombByIdByBag(int id, int value)
    {
        switch (id)
        {
            case 0:
                equipedGrenades[id] += value;
                if (equipedGrenades[id] > 1)
                {
                    equipedGrenades[id] = 1;
                }
                break;
            case 1:
                equipedGrenades[id] += value;
                if (equipedGrenades[id] > 3)
                {
                    equipedGrenades[id] = 3;
                }
                break;
            case 2:
                equipedGrenades[id] += value;
                if (equipedGrenades[id] > 2)
                {
                    equipedGrenades[id] = 2;
                }
                break;
            case 3:
                equipedGrenades[id] += value;
                if (equipedGrenades[id] > 2)
                {
                    equipedGrenades[id] = 2;
                }
                break;
            default:
                break;
        }
        bombTexts[id].text = equipedGrenades[id].ToString();
        // soltsTexts[id].text = equipedGrenades[id].ToString();
    }
    public void AddWeaponById(int id)
    {
        // myQueue.Add(id);
        if (!myQueue.Contains(id))
        {
            myQueue.Insert(1, id);
        }

        // equipedWeapons.Add(availWeapons[id]);
    }
    public static void changeHealth(int x)
    {
        health += x;
        if (x < 0)
        {
            MyInstance.hitJoel.PlayOneShot(MyInstance.hitJoel.clip, 0.7f);
            if (!MyInstance.effectImage.enabled)
                MyInstance.DamageEffect();
        }
        healthBarFill.fillAmount = (health / 300.0f);
    }

    public static void changeRage(int x)
    {
        rage *= x;
        if (rage >= 100)
        {
            rage = 100;
        }
        rageBarFill.fillAmount = (rage / 100.0f);
    }


    public static void updateRage(bool isNormal)
    {
        if (isNormal)
        {
            killedNInfecteds += 1;
            rage += 10;
        }
        else
        {
            killedSInfecteds += 1;
            rage += 50;
        }
        if (rage >= 100)
        {
            rage = 100;
        }
        rageBarFill.fillAmount = (rage / 100.0f);
        rageTimer = 0;
    }

    public static void changeRagePlus(int x)
    {
        rage += x;
        if (rage >= 100)
        {
            rage = 100;
        }
        rageTimer = 0;
        rageBarFill.fillAmount = (rage / 100.0f);
    }

    public static void Reset()
    {
        weaponIndex = 0;
        health = 300;
        rage = 0;
        activeGrenade = "";
        grenadeIndex = 0;
        killedNInfecteds = 0;
        killedSInfecteds = 0;
        rageTimer = 0;
        rageMode = false;
    }


    public void DamageEffect()
    {
        effectImage.enabled = true;
        effectImage.sprite = damageIcon;
        effectImage.color = new Color(0.6886792f, 0.2631274f, 0.2631274f, 0.4039216f);
        StartCoroutine(DamageShake(50, 0.25f));
        StartCoroutine(DamageShake(125, 0.5f));
        StartCoroutine(DamageShake(200, 0.75f));
        StartCoroutine(DamageShake(125, 1f));
        StartCoroutine(DamageShake(50, 1.25f));
        StartCoroutine(StopEffetc());

    }

    public void StartEffect()
    {

        if (sceneName.Equals("Mountain"))
        {
            effectImage.enabled = true;
            effectImage.sprite = killIcon;
            effectImage.color = new Color(0.5f, 0f, 0f, 1f);
            StartCoroutine(DamageShake(255, 3.5f));
            StartCoroutine(StopEffetc());
        }
        else {
        if (sceneName.Equals("Forest"))
            {
            effectImage.enabled = true;
            effectImage.sprite = runIcon;
            effectImage.color = new Color(0.5f, 0f, 0f, 1f);
            StartCoroutine(DamageShake(255, 3.5f));
            StartCoroutine(StopEffetc());
            }
        }
       


    }
    public void BlurEffect()
    {

        effectImage.enabled = true;
        effectImage.sprite = blurIcon;
        effectImage.color = new Color(0.8056345f, 0.7783019f, 0.0f, 0.8784314f);
        StartCoroutine(DamageShake(224, 2.5f));
        StartCoroutine(StopEffetc());

    }

}
