using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMediBerries : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mediberry;
    List<GameObject> mediberries = new List<GameObject>();
    public Transform mainTarget;
    bool oneHasFallen = false;

    void Start()
    {
        mainTarget = GameObject.FindGameObjectWithTag("MainObjective").GetComponent<Transform>();
        /*
        for (int i = 0; i < 2; i++)
        {
            Vector3 newPos = new Vector3(Random.Range(-5, 5), 0f, Random.Range(-5, 5));
            newPos = newPos + transform.position;
            GameObject child = Instantiate(mediberry, newPos, transform.rotation);
            child.transform.parent = gameObject.transform;
            mediberries.Add(child);
        }
        */
        mediberries.Add(this.gameObject.transform.GetChild(0).gameObject);
        mediberries.Add(this.gameObject.transform.GetChild(1).gameObject);
        //Debug.Log(mediberries.Count);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((mediberries[0].GetComponent<EnemyControler>().health <= 0 || mediberries[1].GetComponent<EnemyControler>().health <= 0) && !oneHasFallen)
        {
            oneHasFallen = true;
            Debug.Log("too far");
            mediberries[0].GetComponent<GoToEnemy>().gettingBack = false;
            mediberries[1].GetComponent<GoToEnemy>().gettingBack = false;
            mediberries[0].GetComponent<GoToEnemy>().closestPlayer = mainTarget;
            mediberries[1].GetComponent<GoToEnemy>().closestPlayer = mainTarget;
        }
        
        if ((Vector3.Distance(mediberries[0].transform.position, mediberries[1].transform.position) >= 10) && !oneHasFallen)
        {
            
            mediberries[0].GetComponent<GoToEnemy>().closestPlayer = mediberries[1].transform;
            mediberries[1].GetComponent<GoToEnemy>().closestPlayer = mediberries[0].transform;
            mediberries[0].GetComponent<GoToEnemy>().gettingBack = true;
            mediberries[1].GetComponent<GoToEnemy>().gettingBack = true;
        }
        else if((Vector3.Distance(mediberries[0].transform.position, mediberries[1].transform.position) <= 10) || oneHasFallen)
        {
            mediberries[0].GetComponent<GoToEnemy>().gettingBack = false;
            mediberries[1].GetComponent<GoToEnemy>().gettingBack = false;
        }

        
        /*
        if (c != this.gameObject)
        {
            gettingBack = false;

        }
    }
    else
    {
        if (c != this.gameObject)
        {
            gettingBack = true;
            closestPlayer = c.transform;

            //c.gameObject.GetComponent<GoToEnemy>().closestPlayer = this.gameObject.transform;
        }
        */


    }
}
