using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestFlag : MonoBehaviour
{
    // Publig Variables
    public int totalPlayers;
    public int playersReady;

    // Private Variables
    private GameObject spawnManager;
    private PlayerManager playerManager;

    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("playerManager").GetComponent<PlayerManager>();
        spawnManager = this.transform.parent.gameObject;
    }

    void Update()
    {
        if(playerManager.players.Count < 1)
            totalPlayers = 1;
        else
            totalPlayers = playerManager.players.Count;

        if(totalPlayers == playersReady)
        {
            spawnManager.GetComponent<SpawnManager>().StartHarvest();
        }
    }

    private void OnEnable()
    {
        playersReady = 0;
    }

    private void OnDisable()
    {
        playersReady = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(other.gameObject.tag == "Player")
        //    playersReady++;
        if (other.gameObject.name == "MeleeAttack")
        {
            Debug.Log("PLAYER HIT HARVEST FLAG");
            bool playerReady = other.gameObject.GetComponentInParent<PlayerControler>().ready;
            if (playerReady)
            {
                playerReady = false;
                playersReady--;
            } else
            {
                playerReady = true;
                playersReady++;
            }
            other.gameObject.GetComponentInParent<PlayerControler>().ready = playerReady;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.tag == "Player")
        //    playersReady--;
    }
}
