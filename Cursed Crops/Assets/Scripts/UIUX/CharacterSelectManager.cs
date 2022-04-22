using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectManager : MonoBehaviour
{
    // the character Select Wheels
    public CharacterSelectWheel Player1;
    public CharacterSelectWheel Player2;
    public CharacterSelectWheel Player3;
    public CharacterSelectWheel Player4;

    public List<string> AvailableChars = new List<string>();

    // internal 

    void Start()
    {
        // adding available characters to string
        AvailableChars.Add("Doug");
        AvailableChars.Add("Cecil");
        AvailableChars.Add("Harvey");
        //AvailableChars.Add("Carlisle");
    }

    void Update()
    {
        /*if (Player1.charSelected && Player2.charSelected && Player3.charSelected && Player4.charSelected)
        {
            // store player selections
            PlayerPrefs.SetString("Player1", Player1.character.ToString());
            PlayerPrefs.SetString("Player2", Player2.character.ToString());
            PlayerPrefs.SetString("Player3", Player3.character.ToString());
            PlayerPrefs.SetString("Player4", Player4.character.ToString());

            StartLevel();
        }*/
    }

    public void StartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Exit()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
