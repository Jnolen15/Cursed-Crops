using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemies;
    public Transform Spawner;
    public int enemyNum;
    public float enemyTimer = 5f;
    private int enemyCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        //Spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Transform>();
        StartCoroutine(spawnEnemy());
    }

    IEnumerator spawnEnemy()
    {
        while(enemyCounter < enemyNum)
        {
            Instantiate(enemies, Spawner.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(enemyTimer);
            enemyCounter += 1;
        }
    }
}
