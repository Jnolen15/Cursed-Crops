using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InitializeLevel : MonoBehaviour
{
    public GameObject playerPrefab;

    void Start()
    {
        var playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs().ToArray();
        for (int i = 0; i < playerConfigs.Length; i++)
        {
            var player = Instantiate(playerPrefab);
            //player.GetComponent<PlayerInput>(). = playerConfigs[i].Input;
            player.SetActive(true);
            //player.GetComponent<PlayerInput>().ActivateInput
            //player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigs[i]);
        }
    }
}
