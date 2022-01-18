using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeleter : MonoBehaviour
{
    int active = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log(active);
        if (enemies != null)
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy.activeSelf)
                {
                    Debug.Log("Active");
                    
                    //Destroy(enemy);
                }
                else
                {
                    active++;
                }
            }
        }
    }
}
