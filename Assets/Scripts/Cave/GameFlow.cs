using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameFlow : MonoBehaviour
{
    private float timeRemaining = 300;
    public static bool timerIsRunning = false;
    public GameObject credits;

    public Text timeText;
    public GameObject wonGamePanel;

    public AudioSource audioSource;


    private void Start()
    {
        // Starts the timer automatically
        timerIsRunning = true;
        audioSource = this.GetComponent<AudioSource>();

    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);

                //won the game
                if (CompanionRescue.rescued == true && TankCharacterControl.healthPoints <= 0 && ChargerScript.healthPoints <= 0)
                {
                    StartCoroutine(activate());

                }

            }
            else
            {
                // GameOver Menu appears because he lost the game

                timeRemaining = 0;
                timerIsRunning = false;

                GameMenu.MyInstance.GameOverMenu.SetActive(true);
                GameMenu.MyInstance.gameOver = true;
                GameMenu.MyInstance.gameOverMusic.Play();
                GameMenu.MyInstance.pauseMusic.Stop();
                GameMenu.MyInstance.backgroundMusic.Stop();
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;

            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:0}:{1:00}", minutes, seconds);

    }
    IEnumerator activate()
    {

        wonGamePanel.SetActive(true);
        GameMenu.MyInstance.gameOver = true;
        GameMenu.MyInstance.pauseMusic.Stop();
        GameMenu.MyInstance.backgroundMusic.Stop();
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        timeRemaining = 0;
        timerIsRunning = false;
        audioSource.PlayOneShot(audioSource.clip, 0.7f);
        yield return new WaitForSecondsRealtime(3);
        wonGamePanel.SetActive(false);
        Time.timeScale = 1;
        credits.SetActive(true);
        yield return new WaitForSecondsRealtime(14);
        Time.timeScale = 0;
        // this.gameObject.SetActive(false);

    }
}
