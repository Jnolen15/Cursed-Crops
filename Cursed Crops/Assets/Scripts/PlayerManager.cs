using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    // This used to be static but I changed it because it caused errors when re-setting the scene
    // IDK why it was static but if it needs to be static again it must be put into an object that isn't destroyed when a scene is changed.
    public List<GameObject> players = new List<GameObject>();
    public PlayerAnimOCManager animManager;

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        players.Add(playerInput.gameObject);
        animManager = playerInput.gameObject.GetComponentInChildren<PlayerAnimOCManager>();

        var playerString = "Player" + players.IndexOf(playerInput.gameObject);
        animManager.SetCharacter(PlayerPrefs.GetString(playerString));

        //if (players.IndexOf(playerInput.gameObject) == 0) animManager.selectedCharacter = PlayerAnimOCManager.character.cecil;
        //else if (players.IndexOf(playerInput.gameObject) == 1) animManager.selectedCharacter = PlayerAnimOCManager.character.doug;

        foreach (var item in players)
        {
            Debug.Log(item.ToString());
        }
    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {
        players.Remove(playerInput.gameObject);
        foreach (var item in players)
        {
            Debug.Log(item.ToString());
        }
    }

    private void OnDisable()
    {
        // Make it rember what player is what
        //Debug.Log("OnDisable called from PlayerManager!");
        /*foreach (var item in players)
        {
            players.Remove(item);
        }*/
    }
}
