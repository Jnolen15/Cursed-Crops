using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InitializeLevel : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject playerConfig;
    public PlayerManager playerManager;
    public GameObject playerController;

    void Start()
    {
        var playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs().ToArray();
        playerConfig = GameObject.Find("Player Config Manager");
        playerManager = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
        for (int i = 0; i < playerConfigs.Length; i++)
        {
            playerController = playerConfig.transform.GetChild(i).gameObject;

            // Create player
            Vector3 spawnPos = new Vector3(0, 1, 0);
            var player = Instantiate(playerPrefab, spawnPos, Quaternion.identity, playerController.transform);

            // Initialize the player
            playerConfig.transform.GetChild(i).GetComponent<PlayerInputHandler>().InitializePlayer(player, playerConfigs[i]);
            playerManager.OnPlayerJoined(player, playerConfigs[i]);

            // Activate the player
            player.SetActive(true);

            //player.GetComponent<PlayerInput>().ActivateInput
            //player.GetComponent<PlayerInput>(). = playerConfigs[i].Input;
        }
    }

    private void OnDestroy()
    {
        // make sure the playerController has no children. If it does, remove them
        if (playerController.transform.childCount > 0)
        {
            //GameObject.Find("SpriteLeanerManager").GetComponent<SpriteLeaner>().leanedSprites.Remove(
            //    playerController.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().gameObject);
            Destroy(playerController.transform.GetChild(0).gameObject);
        }
    }
}
