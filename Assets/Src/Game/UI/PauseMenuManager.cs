using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager
{
    public static bool gameIsPaused = false; //it could be used in soundManager in the future

    private Level.Stats stats;

    private List<Button> buttonList;
    private GameObject pauseMenu;
    private GameObject healtbarRoot;

    public PauseMenuManager(PublicGameObjects publicGameObjects, Level.Stats stats)
    {
        this.stats = stats;

        pauseMenu = publicGameObjects.pauseMenuGO;
        buttonList = publicGameObjects.pauseMenuButtons;

        onInit();
    }

    private void onInit()
    {
        foreach(Button btn in buttonList)
        {
            if(btn.name.Contains("Settings"))
            {
                btn.onClick.AddListener(openSettings);
            }

            if (btn.name.Contains("Continue"))
            {
                btn.onClick.AddListener(resumeGame);
            }

            if (btn.name.Contains("Restart"))
            {
                btn.onClick.AddListener(restartGame);
            }

            if (btn.name.Contains("GiveUp"))
            {
                btn.onClick.AddListener(quitGame);
            }
        }
    }

    public void onUpdate()
    {
        if (healtbarRoot == null)
        {
            healtbarRoot = GameObject.Find("HealthbarRoot");
        }

        checkIfPlayerPausedTheGame();
    }

    private void checkIfPlayerPausedTheGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                resumeGame();
            }
            else
            {
                pauseGame();
            }
        }

    }

    private void resumeGame()
    {
        pauseMenu.SetActive(false);
        healtbarRoot.SetActive(true);

        Time.timeScale = 1.0f;
        gameIsPaused = false;
    }

    private void pauseGame()
    {
        pauseMenu.SetActive(true);
        healtbarRoot.SetActive(false);

        Time.timeScale = 0.0f;
        gameIsPaused = true;
    }

    private void openSettings()
    {
        Debug.Log("Settings here");
    }

    private void restartGame()
    {
        Debug.Log("Restart lvl " + SceneManager.GetActiveScene().buildIndex);

        var energy = PlayerPrefs.GetInt("ENERGY_AMOUNT");
        if(energy - stats.energyCost >= 0)
        {
            Time.timeScale = 1.0f;

            energy = energy - stats.energyCost;
            PlayerPrefs.SetInt("ENERGY_AMOUNT", energy);

            SceneAnimator.loadLevel(SceneManager.GetActiveScene().buildIndex, 1f, 1f, Color.black);
        }
        else
        {
            Debug.Log("Sorry, u do not have enough energy to restart :(");
        }
        
    }

    private void quitGame() //aka giveUp :/
    {
        Time.timeScale = 1.0f;
        SceneAnimator.loadLevel(0, 1f, 1f, Color.black);
    }
}
