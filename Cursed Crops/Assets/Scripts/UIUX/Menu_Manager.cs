using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;

// only using this for a debug thing
using UnityEngine.EventSystems;


public class Menu_Manager : MonoBehaviour
{
    public GameObject menu;
    public GameObject options;
    public GameObject gameplaySettings;
    public GameObject soundSettings;
    public GameObject graphicsSettings;
    public GameObject credits;

    private List<GameObject> MenuHistory = new List<GameObject>();
    private List<Button> ButtonHistory = new List<Button>();

    public Button primaryButton;

    private GameObject CurrentMenu;
    private Button CurrentButton;
    // public List<GameObject> = new List<GameObject>();

    // Graphic Settings Variables
    Resolution[] resolutions;
    public TMP_Dropdown ResolutionDropdown;
    public TMP_Dropdown QualityDropdown;
    public Toggle FullScreenToggle;
    public Toggle DevMode;

    private void Start()
    {
        primaryButton.Select();
        // setting current menu, just in case game starts with options instead of main menu open
        if (menu.activeSelf)
        {
            CurrentMenu = menu;
        } else if (options.activeSelf)
        {
            CurrentMenu = options;
        } else if (credits.activeSelf)
        {
            CurrentMenu = credits;
        }
        CurrentButton = primaryButton;

        // aquiring resolutions list and assigning them to element
        if (ResolutionDropdown != null)
        {
            resolutions = Screen.resolutions;
            ResolutionDropdown.ClearOptions();

            List<string> resOptions = new List<string>();

            int currentResolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string resOption = resolutions[i].width + " x " + resolutions[i].height;
                resOptions.Add(resOption);

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            ResolutionDropdown.AddOptions(resOptions);
            ResolutionDropdown.value = currentResolutionIndex;
            ResolutionDropdown.RefreshShownValue();
        }

        // Retreiving devMode bool
        //DevMode.isOn = (PlayerPrefs.GetInt("DevMode") == 1 ? true : false);
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

    public void PreviousScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void loadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    // Sets the starting selection
    public void SetSelection(Button selectedButton)
    {
        selectedButton.Select();
        CurrentButton = selectedButton;
    }


    // Disables current menu and enables the next one 
    public void ChangeMenu(GameObject nextMenu)
    {
        CurrentMenu.SetActive(false);
        nextMenu.SetActive(true);

        if (MenuHistory.Count > 0) // check to see if list is emtpy (at main menu)
        {
            if (MenuHistory[MenuHistory.Count - 1] == nextMenu) // check if you're going back to the previous menu
            {
                MenuHistory.RemoveAt(MenuHistory.Count - 1);
                ButtonHistory.RemoveAt(ButtonHistory.Count - 1); // also doing the same for buttons
            } else
            {
                MenuHistory.Add(CurrentMenu); // add menu to history
                ButtonHistory.Add(CurrentButton);
            }
        } else
        {
            MenuHistory.Add(CurrentMenu); // add menu to history
            ButtonHistory.Add(CurrentButton);
        }
        CurrentMenu = nextMenu;
    }

    public void GoBack()
    {
        // can we go back?
        if (MenuHistory.Count > 0)
        {
            Button temp = ButtonHistory[ButtonHistory.Count - 1];
            ChangeMenu(MenuHistory[MenuHistory.Count - 1]);
            SetSelection(temp);
        } 
    }


    // Options Menu Functions
    public void SetFullscreen(bool fullScreenOn)
    {
        Screen.fullScreen = fullScreenOn;
    }

    // a list of possible resolutions is created in the start function and 
    // assigned to the box
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    // quality settings must match overall project quality settings
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void ResetPauseMenu()
    {
        graphicsSettings.SetActive(false);
        gameplaySettings.SetActive(false);
        soundSettings.SetActive(false);
        options.SetActive(false);
        menu.SetActive(true);
        CurrentMenu = menu;
        primaryButton.Select();
        MenuHistory.Clear();
        ButtonHistory.Clear();
    }

    public void toggleDevMode(bool devMode)
    {
        //PlayerPrefs.SetInt("DevMode", devMode == true ? 1 : 0);
    }
}
