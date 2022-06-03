using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initializeCutscene : MonoBehaviour
{
    public GameObject playerConfig;
    public GameObject playerController;

    void Start()
    {
        var playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs().ToArray();
        playerConfig = GameObject.Find("Player Config Manager");
        for (int i = 0; i < playerConfigs.Length; i++)
        {
            playerController = playerConfig.transform.GetChild(i).gameObject;

            // Initialize the cutscene player
            playerConfig.transform.GetChild(i).GetComponent<PlayerInputHandler>().InitializeCutscenePlayer(playerConfigs[i]);
        }
    }

    private void OnDestroy()
    {
        var playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs().ToArray();
        playerConfig = GameObject.Find("Player Config Manager");
        for (int i = 0; i < playerConfigs.Length; i++)
        {
            playerController = playerConfig.transform.GetChild(i).gameObject;

            // Uninitialize the player
            playerConfig.transform.GetChild(i).GetComponent<PlayerInputHandler>().UninitializeCutscenePlayer();
        }
    }
}
