using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Public Variables
    public float harvestTimer;
    public string phase = "Pre"; // Stars in pre then goes to morning, afternoon and night
    public string previousphase = "Pre";
    public float morningStartTime = 5f;
    public float afternoonStartTime = 35f;
    public float nightStartTime = 65f;
    public float endTime = 95f;

    // For rn spawner works with an array. But change to a list when planting phase is in
    // Because it will be easier to dynamically add and remove to
    public static List<GameObject> spawners = new List<GameObject>();
    public GameObject[] spawnArray;

    void Update()
    {
        harvestTimer += Time.deltaTime;

        // Change Phases
        if (harvestTimer < morningStartTime)
            phase = "Pre";
        else if(harvestTimer > morningStartTime && harvestTimer < afternoonStartTime)
            phase = "Morning";
        else if (harvestTimer > afternoonStartTime && harvestTimer < nightStartTime)
            phase = "Afternoon";
        else if (harvestTimer > nightStartTime && harvestTimer < endTime)
            phase = "Night";
        else
            phase = "Over";
    }

    private void FixedUpdate()
    {
        // Morning phase
        if (phase == "Morning")
        {
            // Initial Burst Spawn
            if (previousphase == "Pre")
            {
                previousphase = phase;
                BurstSpawn("Morning");
            }
            WaveSpawn("Morning", 1f, "Half");
        }
        // Afternoon phase
        else if (phase == "Afternoon")
        {
            if (previousphase == "Morning")
            {
                previousphase = phase;
                BurstSpawn("Afternoon");
            }
            WaveSpawn("Afternoon", 1f, "Half");
            WaveSpawn("Morning", 1.5f, "Half");
        }
        // Night phase
        else if (phase == "Night")
        {
            if (previousphase == "Afternoon")
            {
                previousphase = phase;
                BurstSpawn("Night");
            }
            WaveSpawn("Night", 1f, "Half");
            WaveSpawn("Afternoon", 1.5f, "Half");
            WaveSpawn("Morning", 1.75f, "Half");
        }
    }

    // Spawn a burst of enemies from all sources
    private void BurstSpawn(string currnetPhase)
    {
        for (int i = 0; i < spawnArray.Length; i++)
        {
            Spawner currentSpawner = spawnArray[i].GetComponent<Spawner>();
            if (currentSpawner.germinationPeriod == currnetPhase)
            {
                currentSpawner.Spawn("Full");
                currentSpawner.lastTimeSpawned = harvestTimer;
            }
        }
    }

    // Spawn enemies as the wave progresses. Time between spawns is measured by a spawners potency
    private void WaveSpawn(string currnetPhase, float addedDelay, string type)
    {
        for (int i = 0; i < spawnArray.Length; i++)
        {
            Spawner currentSpawner = spawnArray[i].GetComponent<Spawner>();
            if (currentSpawner.germinationPeriod == currnetPhase)
            {
                if ((currentSpawner.lastTimeSpawned + (currentSpawner.potency * addedDelay)) <= harvestTimer)
                {
                    currentSpawner.Spawn(type);
                    currentSpawner.lastTimeSpawned = harvestTimer;
                }
            }
        }
    }
}
