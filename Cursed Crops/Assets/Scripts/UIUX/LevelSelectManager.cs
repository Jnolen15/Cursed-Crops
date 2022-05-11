using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    public GameObject Shop;
    public int LevelsCleared;
    public bool DevMode = false;

    // array of selectable/unselectable levels beyond tutorial and lvl1
    public Button[] LevelButtons = new Button[6];

    private void Start()
    {
        // fetching devMode
        DevMode = (PlayerPrefs.GetInt("DevMode") == 1 ? true : false);
        if (!DevMode)
        {
            // disabling buttons initially
            foreach (Button B in LevelButtons)
            {
                B.interactable = false;
            }

            // fetch levelsCleared and enable buttons
            LevelsCleared = PlayerPrefs.GetInt("LevelsCleared");
            for (int i = 0; i < LevelsCleared - 1; i++) // lvl 1 is not counted here, so -1
            {
                LevelButtons[i].interactable = true;
            }
        }
    }

    public void loadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void exitToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

    public void toggleShop()
    {
        Shop.SetActive(!Shop.activeSelf);
    }    

    public void ToggleDevMode()
    {
        DevMode = !DevMode;
        PlayerPrefs.SetInt("DevMode", DevMode == true ? 1 : 0);
        if (DevMode)
        {
            foreach (Button B in LevelButtons)
            {
                B.interactable = true;
            }
        } else
        {
            foreach (Button B in LevelButtons)
            {
                B.interactable = false;
            }

            LevelsCleared = PlayerPrefs.GetInt("LevelsCleared");
            for (int i = 0; i < LevelsCleared - 1; i++) // lvl 1 is not counted here, so -1
            {
                LevelButtons[i].interactable = true;
            }
        }
    }

}
