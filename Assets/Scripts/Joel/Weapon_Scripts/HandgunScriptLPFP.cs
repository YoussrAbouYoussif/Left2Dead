using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HandgunScriptLPFP : MonoBehaviour
{

	private static HandgunScriptLPFP instance;

    public static HandgunScriptLPFP MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HandgunScriptLPFP>();
            }

            return instance;
        }
    }

    //Animator component attached to weapon
    Animator anim;
    public Camera gunCamera;
    public float fovSpeed = 15.0f;
    public float defaultFov = 40.0f;
    public string weaponName;
    private string storedWeaponName;

    private int x = 0;
    public bool ironSights;
    public bool alwaysShowIronSights;
    public float ironSightsAimFOV = 16;
    public bool silencer;
    //Weapon attachments components
    [System.Serializable]
    public class weaponAttachmentRenderers
    {
        //All attachment renderer components
        public SkinnedMeshRenderer ironSightsRenderer;
        public SkinnedMeshRenderer silencerRenderer;
    }
    public weaponAttachmentRenderers WeaponAttachmentRenderers;

    public bool weaponSway;

    public float swayAmount = 0.02f;
    public float maxSwayAmount = 0.06f;
    public float swaySmoothValue = 4.0f;

    private Vector3 initialSwayPosition;

    public float sliderBackTimer = 1.58f;
    private bool hasStartedSliderBack;

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

    public float bulletForce = 400;
    public float showBulletInMagDelay = 0.6f;
    public SkinnedMeshRenderer bulletInMagRenderer;

    public float grenadeSpawnDelay = 0.35f;

    public bool randomMuzzleflash = false;
    private int minRandomValue = 1;

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

    public static bool isFired = false;

    private void Awake()
    {
        GameObject slot = GameObject.FindWithTag("WeaponSlot1");
        currentWeaponText = slot.transform.GetChild(2).gameObject.GetComponent<Text>();
        currentAmmoText = slot.transform.GetChild(8).gameObject.GetComponent<Text>();
        totalAmmoText = slot.transform.GetChild(10).gameObject.GetComponent<Text>();


        //Set the animator component
        anim = GetComponent<Animator>();
        //Set current ammo to total ammo value
        currentAmmo = 15;
        // ammo=750;
        muzzleflashLight.enabled = false;

        //If alwaysShowIronSights is true
        if (alwaysShowIronSights == true)
        {
            WeaponAttachmentRenderers.ironSightsRenderer.GetComponent
            <SkinnedMeshRenderer>().enabled = true;
        }

        //If ironSights is true
        if (ironSights == true)
        {
            //If scope1 is true, enable scope renderer
            WeaponAttachmentRenderers.ironSightsRenderer.GetComponent
            <SkinnedMeshRenderer>().enabled = true;
            //If always show iron sights is enabled, don't disable 
            //Do not use if iron sight renderer is not assigned in inspector
        }
        else
        {
            //If scope1 is false, disable scope renderer
            WeaponAttachmentRenderers.ironSightsRenderer.GetComponent
            <SkinnedMeshRenderer>().enabled = false;
        }
        //If silencer is true and assigned in the inspector
        if (silencer == true &&
            WeaponAttachmentRenderers.silencerRenderer)
        {
            //If scope1 is true, enable scope renderer
            WeaponAttachmentRenderers.silencerRenderer.GetComponent
            <SkinnedMeshRenderer>().enabled = true;
        }
        else
        {
            //If scope1 is false, disable scope renderer
            WeaponAttachmentRenderers.silencerRenderer.GetComponent
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
        totalAmmoText.text = "INF";

        //Weapon sway
        initialSwayPosition = transform.localPosition;

        //Set the shoot sound to audio source
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
            if (Input.GetKeyDown(KeyCode.LeftControl) && canMove)
            {
                this.GetComponent<Animator>().Play("Evade", 0, 0f);
                // Time.timeScale = 0;
            }

            //Collect Weapon or Grenade when pressing E
            if (Input.GetKeyDown(KeyCode.E) && canMove)
            {
                this.GetComponent<Animator>().Play("Pick", 0, 0f);
            }


            //Set current ammo text from ammo int
            currentAmmoText.text = currentAmmo.ToString();
            // totalAmmoText.text = ammo.ToString();

            //Continosuly check which animation 
            //is currently playing
            AnimationCheck();




            //If out of ammo
            if (currentAmmo == 0)
            {
                //Show out of ammo text
                currentWeaponText.text = "OUT OF AMMO";
                //Toggle bool
                outOfAmmo = true;

                //Set slider back
                anim.SetBool("Out Of Ammo Slider", true);
                //Increase layer weight for blending to slider back pose
                anim.SetLayerWeight(1, 1.0f);
            }
            else
            {
                //When ammo is full, show weapon name again
                currentWeaponText.text = storedWeaponName.ToString();
                //Toggle bool
                outOfAmmo = false;
                //anim.SetBool ("Out Of Ammo", false);
                anim.SetLayerWeight(1, 0.0f);
            }

            //Shooting 
            if (Input.GetMouseButtonDown(0) && !outOfAmmo && !isReloading && !isInspecting && !isRunning && canMove)
            {
                anim.Play("Fire", 0, 0f);
                if (!silencer)
                {
                    muzzleParticles.Emit(1);
                }

                //Remove 1 bullet from ammo
                currentAmmo -= 1;

                //If silencer is enabled, play silencer shoot sound
                if (silencer == true)
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

                //Light flash start
                StartCoroutine(MuzzleFlashLight());

                if (!isAiming) //if not aiming
                {
                    anim.Play("Fire", 0, 0f);
                    if (!silencer)
                    {
                        muzzleParticles.Emit(1);
                    }

                    if (enableSparks == true)
                    {
                        //Emit random amount of spark particles
                        sparkParticles.Emit(Random.Range(1, 6));
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
                                sparkParticles.Emit(Random.Range(1, 6));
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

                //Spawn bullet at bullet spawnpoint
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

            //Reload 
            if (Input.GetKeyDown(KeyCode.R) && !isReloading && !isInspecting && canMove)
            {
                //Reload
                Reload();

                if (!hasStartedSliderBack)
                {
                    hasStartedSliderBack = true;
                    StartCoroutine(HandgunSliderBackDelay());
                }
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
    private IEnumerator HandgunSliderBackDelay()
    {
        //Wait set amount of time
        yield return new WaitForSeconds(sliderBackTimer);
        //Set slider back
        anim.SetBool("Out Of Ammo Slider", false);
        //Increase layer weight for blending to slider back pose
        anim.SetLayerWeight(1, 0.0f);

        hasStartedSliderBack = false;
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
        currentAmmo = 15;
        // ammo -=15;
        outOfAmmo = false;
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