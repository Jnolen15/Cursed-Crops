using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Public variables
    public string plantName;
    [Header("Germination Period: Morning, Afternoon, Night")]
    public string germinationPeriod;
    public GameObject enemyPrefab;
    public float numToSpawn = 1; // How many enemies should be made each time this spawner spawns
    [Header("Potency: Minimum time between spawns")]
    public float potency = 10f;
    public int spawnRadius = 2; // X and Z limit to where the enemys spawn
    public float lastTimeSpawned = 0f; // X and Z limit to where the enemys spawn

    public void Spawn(string type)
    {
        if (type == "Full")
        {
            for (int i = 0; i < numToSpawn; i++)
            {
                // Random position to spawn too within the radius
                Vector3 newPos = new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f, Random.Range(-spawnRadius, spawnRadius));
                newPos = newPos + transform.position;
                Instantiate(enemyPrefab, newPos, transform.rotation);
                //Debug.Log("Spawning to: " + newPos);
            }
        }
        else if (type == "Half")
        {
            float newNumToSpawn = Mathf.Round((numToSpawn / 2) + 0.1f);
            for (int i = 0; i < newNumToSpawn; i++)
            {
                Vector3 newPos = new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f, Random.Range(-spawnRadius, spawnRadius));
                newPos = newPos + transform.position;
                Instantiate(enemyPrefab, newPos, transform.rotation);
            }
        }
    }
}
