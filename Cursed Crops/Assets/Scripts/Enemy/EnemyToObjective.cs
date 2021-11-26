using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyToObjective : MonoBehaviour
{
    //Setting up changable variables for enemies speeds
    public float enemySpeed = 1f;
    public Transform Objective;
    Rigidbody rb;
    Vector3[] path;
    int targetIndex;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Objective = GameObject.FindGameObjectWithTag("MainObjective").GetComponent<Transform>();
        PathRequestManager.RequestPath(transform.position, Objective.position, OnPathFound);


    }

    // Update is called once per frame
    void Update()
    {
        if(Objective != null){
            //Vector3 position = Vector3.MoveTowards(transform.position, Objective.position, enemySpeed * Time.fixedDeltaTime);
            //rb.MovePosition(position);
            //transform.LookAt(Objective);
                
        }
        
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];
        while (true)
        {
            if(transform.position == currentWaypoint)
            {
                targetIndex++;
                if(targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, enemySpeed * Time.deltaTime);
            //rb.MovePosition(position);
            yield return null;
        }
    }
}
