using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestFlag : MonoBehaviour
{
    // ================ Public ================
    public int totalPlayers;
    public int playersReady;

    // ================ Private ================
    private SpawnManager spawnManager;
    private PlayerManager playerManager;
    private GameRuleManager grm;


    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("playerManager").GetComponent<PlayerManager>();
        spawnManager = this.transform.parent.gameObject.GetComponent<SpawnManager>();

        // Game Rule Manager
        grm = GameObject.Find("GameRuleManager").GetComponent<GameRuleManager>();
    }

    void Update()
    {
        if(playerManager.players.Count < 1)
            totalPlayers = 1;
        else
            totalPlayers = playerManager.players.Count;

        if(totalPlayers == playersReady)
        {
            // If last wave, make sure bounty is met before starting
            if (spawnManager.currentPhase == "Night")
            {
                if (grm.bountyMet())
                    spawnManager.StartHarvest();
                else
                    Debug.Log("You must meet the bounty requirements in order to procede to the next wave");
            } else
            {
                spawnManager.StartHarvest();
            }
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
