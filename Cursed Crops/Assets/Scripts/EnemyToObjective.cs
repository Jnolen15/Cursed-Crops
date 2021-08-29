using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyToObjective : MonoBehaviour
{
    //Setting up changable variables for enemies speeds
    public float enemySpeed = 1f;
    public Transform Objective;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Objective = GameObject.FindGameObjectWithTag("MainObjective").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = Vector3.MoveTowards(transform.position, Objective.position, enemySpeed * Time.fixedDeltaTime);
        rb.MovePosition(position);
        transform.LookAt(Objective);
    }
}
