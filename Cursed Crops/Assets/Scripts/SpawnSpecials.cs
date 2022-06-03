using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpecials : MonoBehaviour
{
    public SpawnManager sm;

    public GameObject grabbage;
    public GameObject scarrot;
    public GameObject sabomato;
    public GameObject cornon;
    public GameObject mediberry;

    public GameObject spawnAnimator;

    private float timeSinceLastSpawn = 0;
    
    void Start()
    {
        sm = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    void Update()
    {
        if (sm.state == SpawnManager.State.Wave && sm.currentPhase != "Pre")
        {
            if (timeSinceLastSpawn <= sm.elapsedTime)
            {
                // Update timer and spawn values
                timeSinceLastSpawn = sm.elapsedTime + 20f;

                // Decide which enemy to spawn and spawn it
                GameObject selectedEnemy = null;
                float rand = Random.Range(1, 6);
                switch (rand)
                {
                    case 1:
                        selectedEnemy = grabbage;
                        break;
                    case 2:
                        selectedEnemy = scarrot;
                        break;
                    case 3:
                        selectedEnemy = sabomato;
                        break;
                    case 4:
                        selectedEnemy = cornon;
                        break;
                    case 5:
                        selectedEnemy = mediberry;
                        break;
                }

                float rand2 = Random.Range(1, 4);
                var pos = Vector3.zero;
                if (rand2 == 1) pos = this.transform.GetChild(0).position;
                else if (rand2 == 2) pos = this.transform.GetChild(1).position;
                else if (rand2 == 3) pos = this.transform.GetChild(2).position;

                GameObject sa = Instantiate(spawnAnimator, pos, transform.rotation);
                sa.GetComponent<SpawnAnimator>().AnimateSpawn(selectedEnemy, pos);
            }
        }
    }
}
