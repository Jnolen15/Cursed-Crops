using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // ================= Public variables =================
    public string plantName;
    //[Header("Germination Period: Morning, Afternoon, Night")]
    //public string germinationPeriod;
    [Header("Plant Type: Basic, Special")]
    public string plantType;
    public GameObject enemyPrefab;
    public float numToSpawn = 1; // How many enemies should be made each time this spawner spawns
    [Header("Potency: Minimum time between spawns")]
    public float potency = 10f;
    public bool randomizePotency = true;
    public int spawnRadius = 2; // X and Z limit to where the enemys spawn
    public float lastTimeSpawned = 0f;
    public int bountyWorth = 0;

    // ================= Private variables =================
    private float spawnChance = 1;  // Used in the spawning of special enemies. Inclreases with each spawn attampt
    private bool hasSpawned = false;    // Used to tell if a special enemy has been spawned yet
    private ParticleSystem psDust;

    private void Start()
    {
        // This slightly alters the potency of each plant to make it feel more random
        if (randomizePotency)
            potency += Random.Range(0.6f, 1.6f);

        psDust = Instantiate(Resources.Load<GameObject>("Effects/DustParticle"), transform.position, transform.rotation, transform).GetComponent<ParticleSystem>();
        psDust.Pause();
    }

    public void Spawn(string type)
    {
        if (plantType == "Basic")
        {
            SpawnBasic(type);
        } else if (plantType == "Special")
        {
            if(!hasSpawned) SpawnSpecial();
        }
    }

    private void SpawnBasic(string type)
    {
        float newNumToSpawn = 0;

        if (type == "Full")
            newNumToSpawn = numToSpawn;
        else if (type == "Half")
            newNumToSpawn = Mathf.Round((numToSpawn / 2) + 0.1f);

        for (int i = 0; i < newNumToSpawn; i++)
        {
            // Random position to spawn too within the radius
            Vector3 newPos = new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f, Random.Range(-spawnRadius, spawnRadius));
            newPos = newPos + transform.position;
            Instantiate(enemyPrefab, newPos, transform.rotation);
        }
    }

    private void SpawnSpecial()
    {
        hasSpawned = true;
        Vector3 newPos = new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f, Random.Range(-spawnRadius, spawnRadius));
        newPos = newPos + transform.position;
        Instantiate(enemyPrefab, newPos, transform.rotation);

        // OLD SPAWN METHOD: % ammount to spawn that gets higher untill it sapwns. with a 100% chance before end of wave
        /*float rand = Random.Range(1, 10);
        if (spawnChance > rand)
        {
            hasSpawned = true;
            Vector3 newPos = new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f, Random.Range(-spawnRadius, spawnRadius));
            newPos = newPos + transform.position;
            Instantiate(enemyPrefab, newPos, transform.rotation);
            psDust.Emit(6);
        } else
        {
            spawnChance++;
            // Doubles each time. Start at 0.25f
            // Gaurenteed spawn by the 6th time
            //spawnChance = spawnChance*2;
        }*/
    }
}
