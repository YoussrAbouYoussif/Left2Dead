using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject Main;
    public GameObject CreditsMenu;
    public GameObject HowToPlay;
    public GameObject OptionsMenu;
    public GameObject AudioMenu;
    public GameObject StartMenu;
    public GameObject InfectedMenu;
    public GameObject InfectedModels;

    AudioSource[] allAudios;
    AudioSource pause;

    public void Start()
    {
        allAudios = GetComponents<AudioSource>();
        pause = allAudios[0];
        pause.Play();
    }
    public void StartButton()
    {
        SceneManager.LoadScene("MenuScene");
        pause.Play();
    }
    public void BackButton()
    {
        Main.SetActive(true);
        StartMenu.SetActive(true);
        CreditsMenu.SetActive(false);
        HowToPlay.SetActive(false);
        OptionsMenu.SetActive(false);
        AudioMenu.SetActive(false);
        InfectedMenu.SetActive(false);
              InfectedModels.SetActive(false);

    }
    public void BackToOptionsButton()
    {
        OptionsMenu.SetActive(true);
        Main.SetActive(true);
        StartMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        HowToPlay.SetActive(false);
        AudioMenu.SetActive(false);
        InfectedMenu.SetActive(false);
                InfectedModels.SetActive(false);

    }
   
    public void CreditsButton()
    {
        CreditsMenu.SetActive(true);
        Main.SetActive(true);
        StartMenu.SetActive(false);
        HowToPlay.SetActive(false);
        OptionsMenu.SetActive(false);
        AudioMenu.SetActive(false);
        InfectedMenu.SetActive(false);
             InfectedModels.SetActive(false);

    }
    public void HowToPlayButton()
    {
        HowToPlay.SetActive(true);
        CreditsMenu.SetActive(false);
        Main.SetActive(true);
        StartMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        AudioMenu.SetActive(false);
        InfectedMenu.SetActive(false);
           InfectedModels.SetActive(false);


    }

    public void Options()
    {
        OptionsMenu.SetActive(true);
        CreditsMenu.SetActive(false);
        Main.SetActive(true);
        StartMenu.SetActive(false);
        AudioMenu.SetActive(false);
        HowToPlay.SetActive(false);
        InfectedMenu.SetActive(false);
             InfectedModels.SetActive(false);



    }
    public void Audio()
    {
        AudioMenu.SetActive(true);
        OptionsMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        Main.SetActive(true);
        StartMenu.SetActive(false);
        HowToPlay.SetActive(false);
        InfectedMenu.SetActive(false);
        InfectedModels.SetActive(false);


    }

    public void InfectedButton()
    {
        InfectedMenu.SetActive(true);
        AudioMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        Main.SetActive(true);
        StartMenu.SetActive(false);
        HowToPlay.SetActive(false);
        InfectedModels.SetActive(true);
        
    

    }

  

    public void MuteButton()
    {
        if (AudioListener.volume > 0.2)
            AudioListener.volume = 0;
        else
        {
            AudioListener.volume = 1;
        }
    }
  

    public void QuitButton()
    {
        // Quit Game
        Application.Quit();
    }
}
