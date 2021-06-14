using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SubMachineGunScriptLPFP : MonoBehaviour
{

    private static SubMachineGunScriptLPFP instance;

    public static SubMachineGunScriptLPFP MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SubMachineGunScriptLPFP>();
            }

            return instance;
        }
    }

    Animator anim;
    public Camera gunCamera;
    public float fovSpeed = 15.0f;
    public float defaultFov = 40.0f;
    public string weaponName;
    private string storedWeaponName;
    [Space(10)]
    public bool scope1;
    public Sprite scope1Texture;
    public float scope1TextureSize = 0.0045f;


    [Space(10)]
    public bool ironSights;
    public bool alwaysShowIronSights;


    [Range(5, 40)]
    public float ironSightsAimFOV = 16;
    [Space(10)]
    public bool silencer;
    [System.Serializable]
    public class weaponAttachmentRenderers
    {
        [Space(10)]
        public SkinnedMeshRenderer ironSightsRenderer;
        public SkinnedMeshRenderer silencerRenderer;
    }
    public weaponAttachmentRenderers WeaponAttachmentRenderers;
    public bool weaponSway;

    public float swayAmount = 0.02f;
    public float maxSwayAmount = 0.06f;
    public float swaySmoothValue = 4.0f;

    private Vector3 initialSwayPosition;

    private float lastFired;
    public float fireRate;

    private bool isReloading;
    private bool isRunning;
    private bool isAiming;
    private bool isWalking;
    private bool isInspecting;

    //How much ammo is currently left
    private int currentAmmo;
    //Total amount of ammo
    public int ammo;
    //Check if out of ammo
    private bool outOfAmmo;

    public float bulletForce = 400.0f;
    public float showBulletInMagDelay = 0.6f;
    public SkinnedMeshRenderer bulletInMagRenderer;
    public float grenadeSpawnDelay = 0.35f;
    public bool randomMuzzleflash = false;
    private int minRandomValue = 1;

    [Range(2, 25)]
    public int maxRandomValue = 5;

    private int randomMuzzleflashValue;

    public bool enableMuzzleflash = true;
    public ParticleSystem muzzleParticles;
    public bool enableSparks = true;
    public ParticleSystem sparkParticles;
    public int minSparkEmission = 1;
    public int maxSparkEmission = 7;

    public Light muzzleflashLight;
    public float lightDuration = 0.02f;
    public AudioSource mainAudioSource;
    public AudioSource shootAudioSource;

    [Header("UI Components")]
    private Text currentWeaponText;
    private Text currentAmmoText;
    private Text totalAmmoText;

    [System.Serializable]
    public class prefabs
    {
        public Transform bulletPrefab;
        public Transform casingPrefab;
        public Transform grenadePrefab;
        public Transform[] BombsList;
    }
    public prefabs Prefabs;

    [System.Serializable]
    public class spawnpoints
    {
        public Transform casingSpawnPoint;
        public Transform bulletSpawnPoint;
        public Transform grenadeSpawnPoint;
    }
    public spawnpoints Spawnpoints;

    [System.Serializable]
    public class soundClips
    {
        public AudioClip shootSound;
        public AudioClip silencerShootSound;
        public AudioClip takeOutSound;
        public AudioClip holsterSound;
        public AudioClip reloadSoundOutOfAmmo;
        public AudioClip reloadSoundAmmoLeft;
        public AudioClip aimSound;
        public AudioClip switchSound;
        public AudioClip companionSound;
    }
    public soundClips SoundClips;

    private bool soundHasPlayed = false;

    public bool isAssault;

    public static bool isFired = false;

    private void Awake()
    {
        // ammo = 450;
        GameObject slot = GameObject.FindWithTag("WeaponSlot2");
        currentWeaponText = slot.transform.GetChild(2).gameObject.GetComponent<Text>();
        currentAmmoText = slot.transform.GetChild(8).gameObject.GetComponent<Text>();
        totalAmmoText = slot.transform.GetChild(10).gameObject.GetComponent<Text>();



        anim = GetComponent<Animator>();
        currentAmmo = 50;
        // ammo -=50;

        muzzleflashLight.enabled = false;

        if (alwaysShowIronSights == true && WeaponAttachmentRenderers.ironSightsRenderer != null)
        {
            WeaponAttachmentRenderers.ironSightsRenderer.GetComponent
            <SkinnedMeshRenderer>().enabled = true;
        }

        //If ironSights is true
        if (ironSights == true && WeaponAttachmentRenderers.ironSightsRenderer != null)
        {
            WeaponAttachmentRenderers.ironSightsRenderer.GetComponent
            <SkinnedMeshRenderer>().enabled = true;
        }
        else if (!alwaysShowIronSights &&
          WeaponAttachmentRenderers.ironSightsRenderer != null)
        {
            WeaponAttachmentRenderers.ironSightsRenderer.GetComponent
            <SkinnedMeshRenderer>().enabled = false;
        }
        if (silencer == true &&
            WeaponAttachmentRenderers.silencerRenderer != null)
        {
            WeaponAttachmentRenderers.silencerRenderer.GetComponent
            <SkinnedMeshRenderer>().enabled = true;
        }
        else if (WeaponAttachmentRenderers.silencerRenderer != null)
        {
            WeaponAttachmentRenderers.silencerRenderer.GetComponent
            <SkinnedMeshRenderer>().enabled = false;
        }
    }

    private void Start()
    {
        storedWeaponName = weaponName;
        currentWeaponText.text = weaponName;
        totalAmmoText.text = AmmoCounts.MyInstance.currSubMachineAmmo.ToString();
        initialSwayPosition = transform.localPosition;
        shootAudioSource.clip = SoundClips.shootSound;
    }

    private void LateUpdate()
    {

        if (weaponSway == true)
        {
            float movementX = -Input.GetAxis("Mouse X") * swayAmount;
            float movementY = -Input.GetAxis("Mouse Y") * swayAmount;
            movementX = Mathf.Clamp
                (movementX, -maxSwayAmount, maxSwayAmount);
            movementY = Mathf.Clamp
                (movementY, -maxSwayAmount, maxSwayAmount);
            Vector3 finalSwayPosition = new Vector3
                (movementX, movementY, 0);
            transform.localPosition = Vector3.Lerp
                (transform.localPosition, finalSwayPosition +
                    initialSwayPosition, Time.deltaTime * swaySmoothValue);
        }
    }

    private void Update()
    {
        if (!GameMenu.pause)

        {
            if (Input.GetKeyDown(KeyCode.C) && hunterControl.MyInstance != null && hunterControl.MyInstance.canMove == true)
            {
                mainAudioSource.clip = SoundClips.switchSound;
                mainAudioSource.Play();
            }
            if (Input.GetKeyDown(KeyCode.Q) && hunterControl.MyInstance != null && hunterControl.MyInstance.canMove == true)
            {
                mainAudioSource.clip = SoundClips.companionSound;
                mainAudioSource.Play();

            }
            totalAmmoText.text = AmmoCounts.MyInstance.currSubMachineAmmo.ToString();
            if (Input.GetButton("Fire2") && !isReloading && !isRunning && !isInspecting && hunterControl.MyInstance != null && hunterControl.MyInstance.canMove == true)
            {
                if (ironSights == true)
                {
                    gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView,
                        ironSightsAimFOV, fovSpeed * Time.deltaTime);
                }
                isAiming = true;
                if (ironSights == true)
                {
                    anim.SetBool("Aim", true);
                }
                if (!soundHasPlayed)
                {
                    mainAudioSource.clip = SoundClips.aimSound;
                    mainAudioSource.Play();
                    soundHasPlayed = true;
                }
            }
            else
            {
                //When right click is released
                gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView,
                    defaultFov, fovSpeed * Time.deltaTime);

                isAiming = false;
                if (ironSights == true)
                {
                    anim.SetBool("Aim", false);
                }

                soundHasPlayed = false;
            }
            //Aiming end

            //If randomize muzzleflash is true, genereate random int values
            if (randomMuzzleflash == true)
            {
                randomMuzzleflashValue = Random.Range(minRandomValue, maxRandomValue);
            }

            //Collect Weapon or Grenade when pressing E
            if (Input.GetKeyDown(KeyCode.E) && hunterControl.MyInstance != null && hunterControl.MyInstance.canMove == true)
            {
                this.GetComponent<Animator>().Play("Pick", 0, 0f);
            }


            if (Input.GetKeyDown(KeyCode.LeftControl) && hunterControl.MyInstance != null && hunterControl.MyInstance.canMove == true)
            {
                this.GetComponent<Animator>().Play("Evade", 0, 0f);
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 4);
            }

            currentAmmoText.text = currentAmmo.ToString();
            AnimationCheck();

            //If out of ammo
            if (currentAmmo == 0)
            {
                //Show out of ammo text
                currentWeaponText.text = "OUT OF AMMO";
                //Toggle bool
                outOfAmmo = true;
                //Auto reload if true
            }
            else
            {
                //When ammo is full, show weapon name again
                currentWeaponText.text = storedWeaponName.ToString();
                //Toggle bool
                outOfAmmo = false;
                //anim.SetBool ("Out Of Ammo", false);
            }

            //AUtomatic fire
            //Left click hold 
            if (Input.GetMouseButton(0) && !outOfAmmo && !isReloading && !isInspecting && !isRunning && hunterControl.MyInstance != null && hunterControl.MyInstance.canMove == true)
            {
                //Shoot automatic
                if (Time.time - lastFired > 1 / fireRate)
                {
                    lastFired = Time.time;

                    //Remove 1 bullet from ammo
                    currentAmmo -= 1;

                    //If silencer is enabled, play silencer shoot sound, don't play if there is nothing assigned in the inspector
                    if (silencer == true && WeaponAttachmentRenderers.silencerRenderer != null)
                    {
                        shootAudioSource.clip = SoundClips.silencerShootSound;
                        shootAudioSource.Play();
                    }
                    //If silencer is not enabled, play default shoot sound
                    else
                    {
                        shootAudioSource.clip = SoundClips.shootSound;
                        shootAudioSource.Play();
                        StartCoroutine(isFiredEnm());
                    }

                    if (!isAiming) //if not aiming
                    {
                        anim.Play("Fire", 0, 0f);
                        //If random muzzle is false
                        if (!randomMuzzleflash &&
                            enableMuzzleflash == true && !silencer)
                        {
                            muzzleParticles.Emit(1);
                            //Light flash start
                            StartCoroutine(MuzzleFlashLight());
                        }
                        else if (randomMuzzleflash == true)
                        {
                            //Only emit if random value is 1
                            if (randomMuzzleflashValue == 1)
                            {
                                if (enableSparks == true)
                                {
                                    //Emit random amount of spark particles
                                    sparkParticles.Emit(Random.Range(minSparkEmission, maxSparkEmission));
                                }
                                if (enableMuzzleflash == true && !silencer)
                                {
                                    muzzleParticles.Emit(1);
                                    //Light flash start
                                    StartCoroutine(MuzzleFlashLight());
                                }
                            }
                        }
                    }
                    else //if aiming
                    {
                        if (ironSights == true)
                        {
                            anim.Play("Aim Fire", 0, 0f);
                        }

                        //If random muzzle is false
                        if (!randomMuzzleflash && !silencer)
                        {
                            muzzleParticles.Emit(1);
                            //If random muzzle is true
                        }
                        else if (randomMuzzleflash == true)
                        {
                            //Only emit if random value is 1
                            if (randomMuzzleflashValue == 1)
                            {
                                if (enableSparks == true)
                                {
                                    //Emit random amount of spark particles
                                    sparkParticles.Emit(Random.Range(minSparkEmission, maxSparkEmission));
                                }
                                if (enableMuzzleflash == true && !silencer)
                                {
                                    muzzleParticles.Emit(1);
                                    //Light flash start
                                    StartCoroutine(MuzzleFlashLight());
                                }
                            }
                        }
                    }

                    //Spawn bullet from bullet spawnpoint
                    var bullet = (Transform)Instantiate(
                        Prefabs.bulletPrefab,
                        Spawnpoints.bulletSpawnPoint.transform.position,
                        Spawnpoints.bulletSpawnPoint.transform.rotation);

                    //Add velocity to the bullet
                    bullet.GetComponent<Rigidbody>().velocity =
                        bullet.transform.forward * bulletForce;

                    //Spawn casing prefab at spawnpoint
                    Instantiate(Prefabs.casingPrefab,
                        Spawnpoints.casingSpawnPoint.transform.position,
                        Spawnpoints.casingSpawnPoint.transform.rotation);
                }
            }

            //Reload 
            if (Input.GetKeyDown(KeyCode.R) && !isReloading && !isInspecting && hunterControl.MyInstance != null && hunterControl.MyInstance.canMove == true)
            {
                //Reload
                Reload();
            }

            //Walking when pressing down WASD keys
            if (Input.GetKey(KeyCode.W) && !isRunning ||
                Input.GetKey(KeyCode.A) && !isRunning ||
                Input.GetKey(KeyCode.S) && !isRunning ||
                Input.GetKey(KeyCode.D) && !isRunning && hunterControl.MyInstance != null && hunterControl.MyInstance.canMove == true)
            {
                anim.SetBool("Walk", true);
            }
            else
            {
                anim.SetBool("Walk", false);
            }

            //Running when pressing down W and Left Shift key
            if ((Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift)) && hunterControl.MyInstance != null && hunterControl.MyInstance.canMove == true)
            {
                isRunning = true;
            }
            else
            {
                isRunning = false;
            }

            //Run anim toggle
            if (isRunning == true && hunterControl.MyInstance != null && hunterControl.MyInstance.canMove == true)
            {
                anim.SetBool("Run", true);
            }
            else
            {
                anim.SetBool("Run", false);
            }
        }
    }

    private IEnumerator isFiredEnm()
    {
        isFired = true;
        yield return new WaitForSeconds(0.5f);
        isFired = false;

    }
    private IEnumerator GrenadeSpawnDelay()
    {
        yield return new WaitForSeconds(grenadeSpawnDelay);
        //Spawn grenade prefab at spawnpoint
        if (JoelScript.activeGrenade == "BileBomb")
        {
            Instantiate(Prefabs.BombsList[0],
            Spawnpoints.grenadeSpawnPoint.transform.position,
            Spawnpoints.grenadeSpawnPoint.transform.rotation);
        }
        if (JoelScript.activeGrenade == "MolotovCocktail")
        {
            Instantiate(Prefabs.BombsList[1],
            Spawnpoints.grenadeSpawnPoint.transform.position,
            Spawnpoints.grenadeSpawnPoint.transform.rotation);
        }
        if (JoelScript.activeGrenade == "PipeBomb")
        {
            Instantiate(Prefabs.BombsList[2],
            Spawnpoints.grenadeSpawnPoint.transform.position,
            Spawnpoints.grenadeSpawnPoint.transform.rotation);
        }
        if (JoelScript.activeGrenade == "StunGrenade")
        {
            Instantiate(Prefabs.BombsList[3],
            Spawnpoints.grenadeSpawnPoint.transform.position,
            Spawnpoints.grenadeSpawnPoint.transform.rotation);
        }
    }


    //Reload
    private void Reload()
    {

        if (outOfAmmo == true)
        {
            //Play diff anim if out of ammo
            anim.Play("Reload Out Of Ammo", 0, 0f);

            mainAudioSource.clip = SoundClips.reloadSoundOutOfAmmo;
            mainAudioSource.Play();

            //If out of ammo, hide the bullet renderer in the mag
            //Do not show if bullet renderer is not assigned in inspector
            if (bulletInMagRenderer != null)
            {
                bulletInMagRenderer.GetComponent
                <SkinnedMeshRenderer>().enabled = false;
                //Start show bullet delay
                StartCoroutine(ShowBulletInMag());
            }
        }
        else
        {
            //Play diff anim if ammo left
            anim.Play("Reload Ammo Left", 0, 0f);

            mainAudioSource.clip = SoundClips.reloadSoundAmmoLeft;
            mainAudioSource.Play();

            //If reloading when ammo left, show bullet in mag
            //Do not show if bullet renderer is not assigned in inspector
            if (bulletInMagRenderer != null)
            {
                bulletInMagRenderer.GetComponent
                <SkinnedMeshRenderer>().enabled = true;
            }
        }
        //Restore ammo when reloading
        if (AmmoCounts.MyInstance.currSubMachineAmmo > 50)
        {
            AmmoCounts.MyInstance.currSubMachineAmmo = AmmoCounts.MyInstance.currSubMachineAmmo - 50 + currentAmmo;
            currentAmmo = 50;
            outOfAmmo = false;
        }
        else
        {
            if (AmmoCounts.MyInstance.currSubMachineAmmo > 0)
            {
                currentAmmo = AmmoCounts.MyInstance.currSubMachineAmmo;
                AmmoCounts.MyInstance.currSubMachineAmmo = 0;
                outOfAmmo = false;
            }
        }
    }

    //Enable bullet in mag renderer after set amount of time
    private IEnumerator ShowBulletInMag()
    {

        //Wait set amount of time before showing bullet in mag
        yield return new WaitForSeconds(showBulletInMagDelay);
        bulletInMagRenderer.GetComponent<SkinnedMeshRenderer>().enabled = true;
    }

    //Show light when shooting, then disable after set amount of time
    private IEnumerator MuzzleFlashLight()
    {

        muzzleflashLight.enabled = true;
        yield return new WaitForSeconds(lightDuration);
        muzzleflashLight.enabled = false;
    }

    //Check current animation playing
    private void AnimationCheck()
    {

        //Check if reloading
        //Check both animations
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Reload Out Of Ammo") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Reload Ammo Left"))
        {
            isReloading = true;
        }
        else
        {
            isReloading = false;
        }
    }
    public void IncreaseAmmoBy(int count)
    {
        ammo += count;
    }
}