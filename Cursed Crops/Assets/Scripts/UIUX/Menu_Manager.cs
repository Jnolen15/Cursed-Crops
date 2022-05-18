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
    public GameObject credits;

    public Button primaryButton;

    public GameObject CurrentMenu;

    // Audio Components
    public AudioMixer audioMixer;

    // Graphic Settings Variables
    Resolution[] resolutions;
    public TMP_Dropdown ResolutionDropdown;
    public TMP_Dropdown QualityDropdown;
    public Toggle FullScreenToggle;

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
    }

    private void Update()
    {
        
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
    }

    // Disables current menu and enables the next one 
    public void ChangeMenu(GameObject nextMenu)
    {
        CurrentMenu.SetActive(false);
        nextMenu.SetActive(true);
        CurrentMenu = nextMenu;
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
}
