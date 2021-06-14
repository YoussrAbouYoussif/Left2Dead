using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

using UnityStandardAssets.Characters.ThirdPerson;
public class GameMenu : MonoBehaviour
{

    private static GameMenu instance;

    public static GameMenu MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameMenu>();
            }

            return instance;
        }
    }
    public GameObject PauseMenu;
    public GameObject GameOverMenu;
    public GameObject wonGamePanel;
    public static bool restart = false;
    public static bool pause = false;
    public bool gameOver = false;

    AudioSource[] tracks;
    public AudioSource backgroundMusic;
    public AudioSource pauseMusic;
    public AudioSource gameOverMusic;

    [SerializeField]
    private GameObject ButtonBar;


    public AudioSource secondBackgroundAudio;
    bool changeAudio;
    float timer;
    private Scene currentScene;
    private string sceneName;
    [SerializeField]
    private GameObject Timer;
    [SerializeField]
    private GameObject Map;


    GameObject iconSound;
    Image soundImage;


    [SerializeField]
    private Sprite soundIcon;
    [SerializeField]
    private Sprite muteIcon;
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        tracks = GetComponents<AudioSource>();
        backgroundMusic = tracks[0];
        pauseMusic = tracks[1];
        gameOverMusic = tracks[2];
        secondBackgroundAudio = tracks[4];
        GameOverMenu.SetActive(false);
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        changeAudio = false;
        timer = 0;

        iconSound = GameObject.FindWithTag("SoundIcon");
        soundImage = iconSound.GetComponent<Image>();

        if (sceneName.Equals("Mountain") || sceneName.Equals("Forest"))
        {
            Timer.SetActive(false);
            Map.SetActive(true);
        }
        else
        {
            Timer.SetActive(true);
            Map.SetActive(false);
        }
    }


    public void MuteButton()
    {
        if (AudioListener.volume > 0.2){
            AudioListener.volume = 0;
            soundImage.sprite = muteIcon;
        }
        else
        {
            AudioListener.volume = 1;
            soundImage.sprite = soundIcon;
        }
    }


    void Update()
    {

        if (timer > 60f)
        {
            if (!changeAudio && backgroundMusic.isPlaying)
            {
                changeAudio = true;
                timer = 0;
                backgroundMusic.Stop();
                secondBackgroundAudio.Play();
            }
        }
        else
        {
            timer += Time.fixedDeltaTime;
        }
        if ((Input.GetKeyDown("escape") || Input.GetKeyDown(KeyCode.P)) && !ButtonBar.activeInHierarchy)
        {

            if (Time.timeScale == 0)
            {
                ResumeButton();
            }
            else
            {
                PauseButton();
            }
        }

        if (JoelScript.health <= 0 && !gameOver)
        {
            gameOver = true;
            pause = true;
            GameOverMenu.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            gameOverMusic.Play();
            pauseMusic.Stop();
            backgroundMusic.Stop();
        }
    }

    public void ResumeButton()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        pause = false;
        pauseMusic.Stop();
        gameOverMusic.Stop();
        //backgroundMusic.Play();
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        if (!secondBackgroundAudio.isPlaying && changeAudio)
        {
            secondBackgroundAudio.UnPause();
        }
        else if (!backgroundMusic.isPlaying && !changeAudio)
        {
            backgroundMusic.UnPause();
        }

    }
    public void PauseButton()
    {
        if (backgroundMusic.isPlaying)
        {
            backgroundMusic.Pause();
        }
        else if (secondBackgroundAudio.isPlaying && changeAudio)
        {
            secondBackgroundAudio.Pause();
        }
        gameOverMusic.Stop();
        pauseMusic.Play();
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
        GameOverMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        pause = true;
    }
    public void Restart()
    {
        pause = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;

        //Joel
        JoelScript.Reset();

        //Charger
        if (GameObject.FindWithTag("ChargerAttack") != null)
        {
            if (GameObject.FindWithTag("ChargerAttack").activeInHierarchy != null)
            {
                ChargerScript.Reset();
            }
        }
        //Jockey
        if (GameObject.FindWithTag("JockeyAttack") != null)
        {
            if (GameObject.FindWithTag("JockeyAttack").activeInHierarchy != null)
            {
                JockeyScript.Reset();
            }
        }


        //Bommer
        if (GameObject.FindWithTag("Boomer") != null)
        {
            if (GameObject.FindWithTag("Boomer").activeInHierarchy != null)
            {
                boomerControl.Reset();
                Health.Reset();
            }

        }
        //Companions
        if (GameObject.FindWithTag("Ellie") != null)
        {

            if (GameObject.FindWithTag("Ellie").activeInHierarchy != null)
            {
                CompanionBehavior.Reset();
            }
        }
        if (GameObject.FindWithTag("Bill") != null)
        {

            if (GameObject.FindWithTag("Bill").activeInHierarchy != null)
            {
                CompanionBehavior.Reset();
            }
        }
        if (GameObject.FindWithTag("Louis") != null)
        {

            if (GameObject.FindWithTag("Louis").activeInHierarchy != null)
            {
                CompanionBehavior.Reset();
            }
        }

        if (GameObject.FindWithTag("Zoey") != null)
        {

            if (GameObject.FindWithTag("Zoey").activeInHierarchy != null)
            {
                CompanionBehavior.Reset();
            }
        }


        //Tank
        if (GameObject.FindWithTag("Tank") != null)
        {

            if (GameObject.FindWithTag("Tank").activeInHierarchy != null)
            {
                TankCharacterControl.Reset();
            }
        }

        //Spitter
        if (GameObject.FindWithTag("Spitter") != null)

        {
            if (GameObject.FindWithTag("Spitter").activeInHierarchy != null)
            {
                SpitterCharacterControl.Reset();
            }

        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);



    }


    public void QuitToMainMenu()
    {
        GameOverMenu.SetActive(false);
        pause = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;

        BombEffect.Reset();
        //Joel
        JoelScript.Reset();

        //Charger
        if (GameObject.FindWithTag("ChargerAttack") != null)
        {
            if (GameObject.FindWithTag("ChargerAttack").activeInHierarchy != null)
            {
                ChargerScript.Reset();
            }
        }
        //Jockey
        if (GameObject.FindWithTag("JockeyAttack") != null)
        {
            if (GameObject.FindWithTag("JockeyAttack").activeInHierarchy != null)
            {
                JockeyScript.Reset();
            }
        }
        //Hunter
        if (GameObject.FindWithTag("Hunter") != null)
        {
            Debug.Log("here1");
            if (GameObject.FindWithTag("Hunter").activeInHierarchy != null)
            {
                Debug.Log("here2");
                //hunterControl.Reset();
                //HealthHun.Reset();
            }
        }


        //Bommer
        if (GameObject.FindWithTag("Boomer") != null)
        {
            if (GameObject.FindWithTag("Boomer").activeInHierarchy != null)
            {
                boomerControl.Reset();
                Health.Reset();
            }

        }
        //Companions
        if (GameObject.FindWithTag("Ellie") != null)
        {

            if (GameObject.FindWithTag("Ellie").activeInHierarchy != null)
            {
                CompanionBehavior.Reset();
            }
        }
        if (GameObject.FindWithTag("Bill") != null)
        {

            if (GameObject.FindWithTag("Bill").activeInHierarchy != null)
            {
                CompanionBehavior.Reset();
            }
        }
        if (GameObject.FindWithTag("Louis") != null)
        {

            if (GameObject.FindWithTag("Louis").activeInHierarchy != null)
            {
                CompanionBehavior.Reset();
            }
        }

        if (GameObject.FindWithTag("Zoey") != null)
        {

            if (GameObject.FindWithTag("Zoey").activeInHierarchy != null)
            {
                CompanionBehavior.Reset();
            }
        }

        if (GameObject.FindWithTag("Tank") != null)
        {

            if (GameObject.FindWithTag("Tank").activeInHierarchy != null)
            {
                TankCharacterControl.Reset();
            }
        }
        //Spitter
        if (GameObject.FindWithTag("Spitter") != null)

        {
            if (GameObject.FindWithTag("Spitter").activeInHierarchy != null)
            {
                SpitterCharacterControl.Reset();
            }

        }

        SceneManager.LoadScene("MainMenu");

    }

}
