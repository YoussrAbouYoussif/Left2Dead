using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SniperScriptLPFP : MonoBehaviour
{
    private static SniperScriptLPFP instance;

    public static SniperScriptLPFP MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SniperScriptLPFP>();
            }

            return instance;
        }
    }
    //Animator component attached to weapon
    Animator anim;
    public Camera gunCamera;
    public float fovSpeed = 15.0f;
    public float defaultFov = 40.0f;
    public float aimFOV = 20.0f;

    [Header("UI Weapon Name")]
    [Tooltip("Name of the current weapon, shown in the game UI.")]
    public string weaponName = "Sniper";
    private string storedWeaponName;

    [Header("Weapon Attachments")]
    [Space(10)]
    //Toggle silencer
    public bool silencer;
    //Weapon attachments components
    [System.Serializable]
    public class weaponAttachmentRenderers
    {
        public SkinnedMeshRenderer silencerRenderer;
    }
    public weaponAttachmentRenderers WeaponAttachmentRenderers;

    [Header("Weapon Sway")]
    //Enables weapon sway
    [Tooltip("Toggle weapon sway.")]
    public bool weaponSway;

    public float swayAmount = 0.02f;
    public float maxSwayAmount = 0.06f;
    public float swaySmoothValue = 4.0f;

    private Vector3 initialSwayPosition;

    //Used for fire rate
    private float lastFired;
    public float fireRate;
    private bool isReloading;
    //Check if running
    private bool isRunning;
    //Check if aiming
    private bool isAiming;
    //Check if walking
    private bool isWalking;
    //Check if inspecting weapon
    private bool isInspecting;

    //How much ammo is currently left
    private int currentAmmo;
    //Totalt amount of ammo
    [Tooltip("How much ammo the weapon should have.")]
    public static int ammo;
    //Check if out of ammo
    private bool outOfAmmo;

    [Header("Bullet Settings")]
    //Bullet
    [Tooltip("How much force is applied to the bullet when shooting.")]
    public float bulletForce = 400;
    [Tooltip("How long after reloading that the bullet model becomes visible " +
        "again, only used for out of ammo reload aniamtions.")]
    public float showBulletInMagDelay;
    [Tooltip("The bullet model inside the mag, not used for all weapons.")]
    public SkinnedMeshRenderer bulletInMagRenderer;

    [Header("Grenade Settings")]
    public float grenadeSpawnDelay;

    [Header("Scope Settings")]
    //Material used to render zoom effect
    public Material scopeRenderMaterial;
    //Scope color when not aiming
    public Color fadeColor;
    //Scope color when aiming
    public Color defaultColor;

    [Header("Muzzleflash Settings")]
    public bool randomMuzzleflash = false;
    //min should always bee 1
    private int minRandomValue = 1;

    [Range(2, 25)]
    public int maxRandomValue = 5;

    private int randomMuzzleflashValue;

    public bool enableMuzzleFlash;
    public ParticleSystem muzzleParticles;
    public bool enableSparks;
    public ParticleSystem sparkParticles;
    public int minSparkEmission = 1;
    public int maxSparkEmission = 7;

    [Header("Muzzleflash Light Settings")]
    public Light muzzleFlashLight;
    public float lightDuration = 0.02f;

    [Header("Audio Source")]
    //Main audio source
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
        //Casing spawn point array
        public Transform casingSpawnPoint;
        //Bullet prefab spawn from this point
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

    public static bool isFired = false;

    private void Awake()
    {
        GameObject slot = GameObject.FindWithTag("WeaponSlot3");
        currentWeaponText = slot.transform.GetChild(2).gameObject.GetComponent<Text>();
        currentAmmoText = slot.transform.GetChild(8).gameObject.GetComponent<Text>();
        totalAmmoText = slot.transform.GetChild(10).gameObject.GetComponent<Text>();

        // ammo = 165;
        //Set the animator component
        anim = GetComponent<Animator>();
        //Set current ammo to total ammo value
        currentAmmo = 15;
        // ammo -=15;

        muzzleFlashLight.enabled = false;

        //If silencer is true and assigned in the inspector
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
        totalAmmoText.text = AmmoCounts.MyInstance.currHuntingRifleAmmo.ToString();
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
            totalAmmoText.text = AmmoCounts.MyInstance.currHuntingRifleAmmo.ToString();
            //Aiming
            //Toggle camera FOV when right click is held down
            if (Input.GetButton("Fire2") && !isReloading && !isRunning && !isInspecting && canMove)
            {
                gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView,
                aimFOV, fovSpeed * Time.deltaTime);
                scopeRenderMaterial.color = defaultColor;
                isAiming = true;
                anim.SetBool("Aim", true);
                if (!soundHasPlayed)
                {
                    mainAudioSource.clip = SoundClips.aimSound;
                    mainAudioSource.Play();
                    soundHasPlayed = true;
                }
            }
            else
            {
                gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView,
                    defaultFov, fovSpeed * Time.deltaTime);
                scopeRenderMaterial.color = fadeColor;
                isAiming = false;
                anim.SetBool("Aim", false);
                soundHasPlayed = false;
            }
            if (randomMuzzleflash == true)
            {
                randomMuzzleflashValue = Random.Range
                    (minRandomValue, maxRandomValue);
            }
            //Collect Weapon or Grenade when pressing E
            if (Input.GetKeyDown(KeyCode.E) && canMove)
            {
                this.GetComponent<Animator>().Play("Pick", 0, 0f);
            }

            if (Input.GetKeyDown(KeyCode.LeftControl) && canMove)
            {
                this.GetComponent<Animator>().Play("Evade", 0, 0f);
            }

            //Set current ammo text from ammo int
            currentAmmoText.text = currentAmmo.ToString();
            AnimationCheck();

            //If out of ammo
            if (currentAmmo == 0)
            {
                currentWeaponText.text = "OUT OF AMMO";
                outOfAmmo = true;
            }
            else
            {
                currentWeaponText.text = storedWeaponName.ToString();
                outOfAmmo = false;
            }

            //Fire
            if (Input.GetMouseButton(0) && !outOfAmmo && !isReloading && !isInspecting && !isRunning && canMove)
            {
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
                        //Play fire animation
                        anim.Play("Fire", 0, 0f);
                        //If random muzzle is false
                        if (!randomMuzzleflash &&
                            enableMuzzleFlash == true && !silencer)
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
                                if (enableMuzzleFlash == true && !silencer)
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
                        //Play aim fire animation
                        anim.Play("Aim Fire", 0, 0f);

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
                                if (enableMuzzleFlash == true && !silencer)
                                {
                                    muzzleParticles.Emit(1);
                                    //Light flash start
                                    StartCoroutine(MuzzleFlashLight());
                                }
                            }
                        }
                    }

                    //Spawn bullet from spawnpoint
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
            if (Input.GetKeyDown(KeyCode.R) && !isReloading && !isInspecting && canMove)
            {
                //Reload
                Reload();
            }

            //Walking when pressing down WASD keys
            if (Input.GetKey(KeyCode.W) && !isRunning ||
                Input.GetKey(KeyCode.A) && !isRunning ||
                Input.GetKey(KeyCode.S) && !isRunning ||
                Input.GetKey(KeyCode.D) && !isRunning && canMove)
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
        if (AmmoCounts.MyInstance.currHuntingRifleAmmo > 10)
        {
            AmmoCounts.MyInstance.currHuntingRifleAmmo = AmmoCounts.MyInstance.currHuntingRifleAmmo - 15 + currentAmmo;
            currentAmmo = 15;
            outOfAmmo = false;
        }
        else
        {
            if (AmmoCounts.MyInstance.currHuntingRifleAmmo > 0)
            {
                currentAmmo = AmmoCounts.MyInstance.currHuntingRifleAmmo;
                AmmoCounts.MyInstance.currHuntingRifleAmmo = 0;
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
        muzzleFlashLight.enabled = true;
        yield return new WaitForSeconds(lightDuration);
        muzzleFlashLight.enabled = false;
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