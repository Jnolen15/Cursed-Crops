using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectManager : MonoBehaviour
{
    // the visual objects that are being changed
    public GameObject DougImage;
    public GameObject CecilImage;
    public GameObject HarveyImage;
    public GameObject CarlisleImage;
    public Button ScrollUpButton;
    public Button ScrollDownButton;

    public GameObject SelectionIndicator;
    public TextMeshProUGUI NameText;

    // internal variables
    public bool charSelected = false;
    public GameObject previousChar;

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
        character = 0;
        previousChar = DougImage;
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

    // scroll up decreases char#
    public void ScrollUp()
    {
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

    // toggles wether a character is selected or not
    public void ToggleSelect()
    {
        charSelected = !charSelected;
        SelectionIndicator.SetActive(!SelectionIndicator.activeSelf);
        // disabling buttons
        ScrollUpButton.interactable = !ScrollUpButton.interactable;
        ScrollDownButton.interactable = !ScrollDownButton.interactable;
    }
}
