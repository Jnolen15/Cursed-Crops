using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Pause_Manager : MonoBehaviour
{
    // does this need to be static?
    public static bool gamePaused = false;
    public GameObject PauseMenu;


    /* obselete manual pause functions
    public void Pause()
    {
        gamePaused = true;
        Time.timeScale = 0f;
        PauseMenu.SetActive(true);
    }

    public void Unpause()
    {
        gamePaused = false;
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);
    }
    */

    // pauses/unpauses the game
    public void TogglePause()
    {
        gamePaused = !gamePaused;
        if (gamePaused)
        {
            Time.timeScale = 0;
        } else
        {
            Time.timeScale = 1;
        }
        PauseMenu.SetActive(!PauseMenu.activeSelf);
    }

    public void ExitToMenu()
    {
        TogglePause();
        // SceneManager.LoadScene("MenuScene");
        SceneManager.LoadScene("Menu Scene");
    }

    public void ExitToMap()
    {
        TogglePause();
        // SceneManager.LoadScene("MenuScene");
        SceneManager.LoadScene("Level Select");
    }



    public void Restart()
    {
        TogglePause();
        // check to see if this properly reloads the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
