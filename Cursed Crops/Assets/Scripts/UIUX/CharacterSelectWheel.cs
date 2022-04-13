using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectWheel : MonoBehaviour
{
    // Private Variables
    private int PlayerIndex;
    private float ignoreInputTime = 0.5f;
    private bool inputEnabled = false;

    // Visual objects that are being changed
    public GameObject menuPannel;
    public GameObject readyPannel;
    public GameObject DougImage;
    public GameObject CecilImage;
    public GameObject HarveyImage;
    public GameObject CarlisleImage;
    public GameObject SelectionIndicator;
    public Button ScrollUpButton;
    public Button ScrollDownButton;
    public Button readyButton;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI titleText;

    // Internal variables
    public bool charSelected = false;
    public GameObject previousChar;

    // External Variables
    public CharacterSelectManager CharacterSelectManager;

    // enum for tracking which Character is selected
    public enum Character
    {
        // 0 - 3, respectively
        Doug, 
        Cecil,
        Harvey, 
        Carlisle
    }

    public Character character;


    void Start()
    {
        CharacterSelectManager = this.GetComponentInParent<CharacterSelectManager>();
        character = 0;
        previousChar = DougImage;
    }

    public void SetPlayerIndex(int pi)
    {
        PlayerIndex = pi;
        titleText.SetText("Player " + (pi + 1).ToString());
        ignoreInputTime = Time.time + ignoreInputTime;
    }

    public void Update()
    {
        if(Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        } else
        {
            Debug.Log("Time: " + Time.time + " < " + ignoreInputTime);
        }
    }

    public void ReadyPlayer()
    {
        if (!inputEnabled) { return; }

        PlayerConfigManager.Instance.ReadyPlayer(PlayerIndex);
        SelectionIndicator.SetActive(true);
        readyButton.gameObject.SetActive(false);
    }

    public void SelectCharacter()
    {
        if (!inputEnabled) { return; }

        if (charSelected)
        {
            // re-add character to availableChars
            CharacterSelectManager.AvailableChars.Add(character.ToString());
            ToggleSelect();
        }
        else
        {
            // check to see if char is available, then remove from list
            if (CharacterSelectManager.AvailableChars.Contains(character.ToString()))
            {
                CharacterSelectManager.AvailableChars.Remove(character.ToString());
                ToggleSelect();
                PlayerConfigManager.Instance.SetPlayer(PlayerIndex, character.ToString());
            }
        }
    }

    // scroll up decreases char#
    public void ScrollUp()
    {
        if (!inputEnabled) { return; }

        if (!charSelected)
        {
            // increment Character, or loop back to start
            if (character > 0)
            {
                character -= 1;
            } else
            {
                character += 3;
            }
        }
        ShowCharacter();
    }

    // scroll down increases char#
    public void ScrollDown()
    {
        if (!inputEnabled) { return; }

        if (!charSelected)
        {
            // increment Character, or loop back to start
            // idk why but this won't accept 3 even though Character.Carlisle = 3
            if (character < Character.Carlisle)
            {
                character += 1;
            }
            else
            {
                character -= 3;
            }
        }
        ShowCharacter();
    }

    public void ShowCharacter()
    {
        previousChar.SetActive(false);
        NameText.text = character.ToString();

        switch (character)
        {
            case Character.Doug:
                previousChar = DougImage;

                break;

            case Character.Cecil:
                previousChar = CecilImage;
                break;

            case Character.Harvey:
                previousChar = HarveyImage;
                break;

            case Character.Carlisle:
                previousChar = CarlisleImage;
                break;
        }
        previousChar.SetActive(true);
    }

    // toggles wether a character is selected or not
    public void ToggleSelect()
    {
        charSelected = !charSelected;
        menuPannel.SetActive(false);
        readyPannel.SetActive(true);
        readyButton.Select();
        // disabling buttons
        ScrollUpButton.interactable = !ScrollUpButton.interactable;
        ScrollDownButton.interactable = !ScrollDownButton.interactable;
    }

    public string CurrentCharacter()
    {
        return character.ToString();
    }
}
