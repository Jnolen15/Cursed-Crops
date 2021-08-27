using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyToPlayer : MonoBehaviour
{
    //Setting up changable variables for enemies speeds
    public float enemySpeed = 1f;
    public Transform Player;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = Vector3.MoveTowards(transform.position, Player.position, enemySpeed * Time.fixedDeltaTime);
        rb.MovePosition(position);
        transform.LookAt(Player);
    }
}
