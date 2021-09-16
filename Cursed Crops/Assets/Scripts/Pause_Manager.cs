using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause_Manager : MonoBehaviour
{

    public static bool gamePaused = false;
    public GameObject PauseMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
    }
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

    public void ExitToMenu()
    {
        Unpause();
        // SceneManager.LoadScene("MenuScene");
        SceneManager.LoadScene("Menu Scene");
    }

    public void ExitToMap()
    {
        Unpause();
        // SceneManager.LoadScene("MenuScene");
        SceneManager.LoadScene("Level Select");
    }



    public void Restart()
    {
        Unpause();
        // check to see if this properly reloads the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
