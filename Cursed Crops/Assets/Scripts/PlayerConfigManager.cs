using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfigManager : MonoBehaviour
{
    // Code based on 'Tutorial - Create a Local Co-Op Player Setup Screen in Unity with the New Input System' by Broken Knights Games

    private List<PlayerConfiguration> playerConfigs;

    [SerializeField]
    private int maxPlayers = 2;

    public static PlayerConfigManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Singleton trying to create another instance");
        } else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfiguration>();
        }
    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return playerConfigs;
    }

    public void SetPlayer(int index, string choice)
    {
        Debug.Log("Selected the character: " + choice);
        playerConfigs[index].PlayerCharacter = choice;
        var playerNum = "Player" + index.ToString();
        PlayerPrefs.SetString(playerNum, choice);
    }

    public void ReadyPlayer(int index)
    {
        playerConfigs[index].IsReady = true;
        if (playerConfigs.All(p => p.IsReady == true))
        {
            Debug.Log("ALL PLAYERS READY. GO TO NEXT SCENE");
            var rootMenu = GameObject.Find("Map Canvas");
            rootMenu.GetComponent<CharacterSelectManager>().StartLevel();
        }
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        Debug.Log("Player joined: " + pi.playerIndex);
        if(!playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex))
        {
            pi.transform.SetParent(transform);
            playerConfigs.Add(new PlayerConfiguration(pi));
        }
    }

}

public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }
    
    public PlayerInput Input { get; set; }
    public int PlayerIndex { get; set; }
    public string PlayerCharacter { get; set; }
    public bool IsReady { get; set; }
}
