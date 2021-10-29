using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static List<GameObject> players = new List<GameObject>();
    public PlayerAnimOCManager animManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        players.Add(playerInput.gameObject);
        animManager = playerInput.gameObject.GetComponent<PlayerAnimOCManager>();
        if (players.IndexOf(playerInput.gameObject) == 0) animManager.selectedCharacter = PlayerAnimOCManager.character.harvey;
        else if (players.IndexOf(playerInput.gameObject) == 1) animManager.selectedCharacter = PlayerAnimOCManager.character.doug;

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
}
