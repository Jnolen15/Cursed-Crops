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
    //private PlayerManager playerManager;

    void Start()
    {
        //playerManager = GameObject.FindGameObjectWithTag("playerManager").GetComponent<PlayerManager>();
        spawnManager = this.transform.parent.gameObject;
    }

    void Update()
    {
        if(PlayerManager.players.Count < 1)
            totalPlayers = 1;
        else
            totalPlayers = PlayerManager.players.Count;

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
        if(other.gameObject.tag == "Player")
            playersReady++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            playersReady--;
    }
}
