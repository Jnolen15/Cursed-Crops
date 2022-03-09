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
    private GameObject quotaWarning;        // Placeholder text to tell players quota isn't met

    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("playerManager").GetComponent<PlayerManager>();
        spawnManager = this.transform.parent.gameObject.GetComponent<SpawnManager>();

        // Game Rule Manager
        grm = GameObject.Find("GameRuleManager").GetComponent<GameRuleManager>();

        // Quota popup notification
        quotaWarning = this.transform.GetChild(0).gameObject;
        quotaWarning.SetActive(false);
    }

    void Update()
    {
        if(playerManager.players.Count < 1)
            totalPlayers = 1;
        else
            totalPlayers = playerManager.players.Count;

        if(totalPlayers == playersReady)
        {
            // Before each wave, make sure bounty is met before starting
            if (spawnManager.currentPhase == "Morning") checkStartPhase(0.2f);
            else if (spawnManager.currentPhase == "Afternoon") checkStartPhase(0.5f);
            else if (spawnManager.currentPhase == "Night") checkStartPhase(1);
        }
    }

    private void checkStartPhase(float percent)
    {
        if (grm.bountyMet(percent))
        {
            spawnManager.StartHarvest();
            quotaWarning.SetActive(false);
        }
        else
        {
            Debug.Log("You must meet the bounty requirements in order to procede to the next wave");
            quotaWarning.SetActive(true);
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
