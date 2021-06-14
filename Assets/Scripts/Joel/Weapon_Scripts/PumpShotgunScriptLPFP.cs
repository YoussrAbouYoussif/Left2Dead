using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PumpShotgunScriptLPFP : MonoBehaviour
{
    private static PumpShotgunScriptLPFP instance;

    public static PumpShotgunScriptLPFP MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PumpShotgunScriptLPFP>();
            }

            return instance;
        }
    }
    //Animator component attached to weapon
    Animator anim;
    public Camera gunCamera;
    public float fovSpeed = 15.0f;
    public float defaultFov = 40.0f;
    public string weaponName = "Shot Gun";
    private string storedWeaponName = "Shot Gun";

    //Toggle iron sights
    public bool ironSights;
    public bool alwaysShowIronSights;
    //Iron sights camera fov
    public float ironSightsAimFOV = 16;

    //Weapon attachments components
    [System.Serializable]
    public class weaponAttachmentRenderers
    {
        public SkinnedMeshRenderer ironSightsRenderer;
    }
    public weaponAttachmentRenderers WeaponAttachmentRenderers;

    public bool weaponSway;

    public float swayAmount = 0.02f;
    public float maxSwayAmount = 0.06f;
    public float swaySmoothValue = 4.0f;

    private Vector3 initialSwayPosition;

    //Used for fire rate
    private float lastFired;
    public float fireRate;
    private bool isReloading;
    private bool isShooting;
    private bool isRunning;
    private bool isAiming;
    private bool isWalking;
    private bool isInspecting;

    //How much ammo is currently left
    private int currentAmmo;
    //Total amount of ammo
    public static int ammo;
    //Check if out of ammo
    private bool outOfAmmo;

    public float bulletForce = 400;
    public float grenadeSpawnDelay = 0.35f;
    public bool randomMuzzleflash = false;
    private int minRandomValue = 1;

    public int maxRandomValue = 5;

    private int randomMuzzleflashValue;

    public bool enableMuzzleFlash;
    public ParticleSystem muzzleParticles;
    public bool enableSparks;
    public ParticleSystem sparkParticles;
    public int minSparkEmission = 1;
    public int maxSparkEmission = 7;
    public Light muzzleFlashLight;
    public float lightDuration = 0.02f;

    public AudioSource mainAudioSource;
    //Audio source used for shoot sound
    public AudioSource shootAudioSource;
    [Header("UI Components")]
    private Text currentWeaponText;
    private Text currentAmmoText;
    private Text totalAmmoText;

    [System.Serializable]
    public class prefabs
    {
        [Header("Prefabs")]
        public Transform bulletPrefab;
        public Transform casingPrefab;
        public Transform grenadePrefab;
        public Transform[] BombsList;
    }
    public prefabs Prefabs;

    [System.Serializable]
    public class spawnpoints
    {
        [Header("Spawnpoints")]
        //Array holding casing spawn points 
        //(some weapons use more than one casing spawn)
        public float casingDelayTimer;
        //Casing spawn point array
        public Transform casingSpawnPoint;
        //Bullet prefab spawn from this point
        public Transform[] bulletSpawnPoint;
        public bool useRandomBulletSpawnRotation;
        [Range(-10, 10)]
        public float bulletSpawnPointMinRotation = -5.0f;
        [Range(-10, 10)]
        public float bulletSpawnPointMaxRotation = 5.0f;

        public Transform grenadeSpawnPoint;
    }
    public spawnpoints Spawnpoints;

    [System.Serializable]
    public class soundClips
    {
        public AudioClip shootSound;
        public AudioClip takeOutSound;
        public AudioClip holsterSound;
        public AudioClip aimSound;
        public AudioClip switchSound;
        public AudioClip companionSound;
    }
    public soundClips SoundClips;

    private bool soundHasPlayed = false;

    public static bool isFired = false;

    private void Awake()
    {

        GameObject slot = GameObject.FindWithTag("WeaponSlot4");
        currentWeaponText = slot.transform.GetChild(2).gameObject.GetComponent<Text>();
        currentAmmoText = slot.transform.GetChild(8).gameObject.GetComponent<Text>();
        totalAmmoText = slot.transform.GetChild(10).gameObject.GetComponent<Text>();

        // ammo = 130;
        //Set the animator component
        anim = GetComponent<Animator>();
        //Set current ammo to total ammo value
        currentAmmo = 10;
        // ammo-=10;

        //If alwaysShowIronSights is true
        if (alwaysShowIronSights == true && WeaponAttachmentRenderers.ironSightsRenderer != null)
        {
            WeaponAttachmentRenderers.ironSightsRenderer.GetComponent
            <SkinnedMeshRenderer>().enabled = true;
        }

        //If ironSights is true
        if (ironSights == true && WeaponAttachmentRenderers.ironSightsRenderer != null)
        {
            //If scope1 is true, enable scope renderer
            WeaponAttachmentRenderers.ironSightsRenderer.GetComponent
            <SkinnedMeshRenderer>().enabled = true;

        }
        else if (!alwaysShowIronSights &&
          WeaponAttachmentRenderers.ironSightsRenderer != null)
        {
            //If scope1 is false, disable scope renderer
            WeaponAttachmentRenderers.ironSightsRenderer.GetComponent
            <SkinnedMeshRenderer>().enabled = false;
        }
    }

    private void Start()
    {
        //Save the weapon name
        storedWeaponName = weaponName;
        //Get weapon name from string to text
        currentWeaponText.text = weaponName;
        //Set total ammo text from total ammo int
        totalAmmoText.text = AmmoCounts.MyInstance.currTacticalShotgunAmmo.ToString();

        //Weapon sway
        initialSwayPosition = transform.localPosition;
        shootAudioSource.clip = SoundClips.shootSound;
    }

    private void LateUpdate()
    {
        //Weapon sway
        if (weaponSway == true)
        {
            float movementX = -Input.GetAxis("Mouse X") * swayAmount;
            float movementY = -Input.GetAxis("Mouse Y") * swayAmount;
            //Clamp movement to min and max amount
            movementX = Mathf.Clamp
                (movementX, -maxSwayAmount, maxSwayAmount);
            movementY = Mathf.Clamp
                (movementY, -maxSwayAmount, maxSwayAmount);
            //Lerp local pos
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
                mainAudioSource.clip = SoundClips.switchSound;
                mainAudioSource.Play();
            }
            if (Input.GetKeyDown(KeyCode.Q) && canMove)
            {
                mainAudioSource.clip = SoundClips.companionSound;
                mainAudioSource.Play();

            }


            totalAmmoText.text = AmmoCounts.MyInstance.currTacticalShotgunAmmo.ToString();
            //Aiming
            //Toggle camera FOV when right click is held down
            if (Input.GetButton("Fire2") && !isReloading && !isRunning && !isInspecting && canMove)
            {
                if (ironSights == true)
                {
                    gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView,
                        ironSightsAimFOV, fovSpeed * Time.deltaTime);
                }

                isAiming = true;

                //If iron sights are enabled, use normal aim
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

                //If iron sights are enabled, use normal aim out
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
            if (Input.GetKeyDown(KeyCode.E) && canMove)
            {
                this.GetComponent<Animator>().Play("Pick", 0, 0f);
            }


            if (Input.GetKeyUp(KeyCode.LeftControl) && canMove)
            {
                this.GetComponent<Animator>().Play("Evade", 0, 0f);
            }

            //Set current ammo text from ammo int
            currentAmmoText.text = currentAmmo.ToString();
            AnimationCheck();


            //If out of ammo
            if (currentAmmo == 0)
            {
                //Show out of ammo text
                currentWeaponText.text = "OUT OF AMMO";
                //Toggle bool
                outOfAmmo = true;

            }
            else
            {
                //When ammo is full, show weapon name again
                currentWeaponText.text = storedWeaponName.ToString();
                //Toggle bool
                outOfAmmo = false;
                //anim.SetBool ("Out Of Ammo", false);
            }

            //Fire
            if (Input.GetMouseButton(0) && !outOfAmmo && !isReloading && !isInspecting && !isRunning && canMove)
            {
                if (Time.time - lastFired > 1 / fireRate)
                {
                    lastFired = Time.time;

                    //Remove 1 bullet from ammo
                    currentAmmo -= 1;

                    shootAudioSource.clip = SoundClips.shootSound;
                    shootAudioSource.Play();
                    StartCoroutine(isFiredEnm());

                    if (!isAiming) //if not aiming
                    {
                        anim.Play("Fire", 0, 0f);
                        //If random muzzle is false
                        if (!randomMuzzleflash &&
                            enableMuzzleFlash == true)
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
                                if (enableMuzzleFlash == true)
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
                        if (!randomMuzzleflash)
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
                                if (enableMuzzleFlash == true)
                                {
                                    muzzleParticles.Emit(1);
                                    //Light flash start
                                    StartCoroutine(MuzzleFlashLight());
                                }
                            }
                        }
                    }

                    //Bullet spawnpoints array
                    for (int i = 0; i < Spawnpoints.bulletSpawnPoint.Length; i++)
                    {
                        //If random bullet spawn point is enabled
                        // (Used for shotgun bullet spread)
                        if (Spawnpoints.useRandomBulletSpawnRotation == true)
                        {
                            //Rotate all bullet spawnpoints in array randomly based on min and max values
                            Spawnpoints.bulletSpawnPoint[i].transform.localRotation = Quaternion.Euler(
                                //Rotate random X
                                Random.Range(Spawnpoints.bulletSpawnPointMinRotation,
                                    Spawnpoints.bulletSpawnPointMaxRotation),
                                //Rotate random Y
                                Random.Range(Spawnpoints.bulletSpawnPointMinRotation,
                                    Spawnpoints.bulletSpawnPointMaxRotation),
                                //Don't rotate z
                                0);
                        }

                        //Spawn bullets from bullet spawnpoint positions using array
                        var bullet = (Transform)Instantiate(
                            Prefabs.bulletPrefab,
                            Spawnpoints.bulletSpawnPoint[i].transform.position,
                            Spawnpoints.bulletSpawnPoint[i].transform.rotation);

                        //Add velocity to the bullets
                        bullet.GetComponent<Rigidbody>().velocity =
                            bullet.transform.forward * bulletForce;
                    }

                    StartCoroutine(CasingDelay());
                }
            }

            //Reload 
            if (Input.GetKeyDown(KeyCode.R) && !isReloading && !isInspecting && canMove)
            {
                //Reload
                Reload();
            }

            //Walking when pressing down WASD keys
            if (Input.GetKey(KeyCode.W) && !isRunning && !isShooting ||
                Input.GetKey(KeyCode.A) && !isRunning && !isShooting ||
                Input.GetKey(KeyCode.S) && !isRunning && !isShooting ||
                Input.GetKey(KeyCode.D) && !isRunning && !isShooting && canMove)
            {
                anim.SetBool("Walk", true);
            }
            else
            {
                anim.SetBool("Walk", false);
            }

            //Running when pressing down W and Left Shift key
            if ((Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift)) && canMove)
            {
                isRunning = true;
            }
            else
            {
                isRunning = false;
            }

            //Run anim toggle
            if (isRunning == true && canMove)
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
        //Wait for set amount of time before spawning grenade
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

    private IEnumerator CasingDelay()
    {
        //Wait before spawning casing
        yield return new WaitForSeconds(Spawnpoints.casingDelayTimer);
        //Instantiate casing prefab at spawnpoint
        Instantiate(Prefabs.casingPrefab,
            Spawnpoints.casingSpawnPoint.transform.position,
            Spawnpoints.casingSpawnPoint.transform.rotation);
    }


    //Reload
    private void Reload()
    {
        if (outOfAmmo == true)
        {
            //Play diff anim if out of ammo
            anim.Play("Reload Open", 0, 0f);
        }
        else
        {
            //Play diff anim if ammo left
            anim.Play("Reload Open", 0, 0f);
        }
        //Restore ammo when reloading
        if (AmmoCounts.MyInstance.currTacticalShotgunAmmo > 10)
        {
            AmmoCounts.MyInstance.currTacticalShotgunAmmo = AmmoCounts.MyInstance.currTacticalShotgunAmmo - 10 + currentAmmo;
            currentAmmo = 10;
            outOfAmmo = false;
        }
        else
        {
            if (AmmoCounts.MyInstance.currTacticalShotgunAmmo > 0)
            {
                currentAmmo = AmmoCounts.MyInstance.currTacticalShotgunAmmo;
                AmmoCounts.MyInstance.currTacticalShotgunAmmo = 0;
                outOfAmmo = false;
            }
        }
    }

    //Show light when shooting, then disable after set amount of time
    private IEnumerator MuzzleFlashLight()
    {
        muzzleFlashLight.enabled = true;
        yield return new WaitForSeconds(lightDuration);
        muzzleFlashLight.enabled = false;
    }

    //Check current animation playing
    private void AnimationCheck()
    {
        //Check if reloading
        //Check both animations
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Reload Open") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Reload Open") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Inser Shell") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Reload Close"))
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