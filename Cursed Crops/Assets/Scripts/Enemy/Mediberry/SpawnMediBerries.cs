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

    }
}
