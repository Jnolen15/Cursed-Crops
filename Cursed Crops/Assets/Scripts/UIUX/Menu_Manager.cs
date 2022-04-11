using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu_Manager : MonoBehaviour
{
    public GameObject menu;
    public GameObject options;
    public GameObject credits;

    public Button primaryButton;

    private bool menuActive = true;

    private void Start()
    {
        primaryButton.Select();
    }

    private void Update()
    {
        if (menu.activeSelf && !menuActive)
        {
            menuActive = true;
            primaryButton.Select();
        } 
        else if (credits.activeSelf)
        {
            if (menuActive) menuActive = false;
            credits.GetComponentInChildren<Button>().Select();
        }
        else if (options.activeSelf)
        {
            if (menuActive) menuActive = false;
            options.GetComponentInChildren<Button>().Select();
        }
    }

    public void startGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void endGame()
    {
        Debug.Log("quitting game");
        Application.Quit();
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
