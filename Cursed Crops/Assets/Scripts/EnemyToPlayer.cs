using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class EnemyToPlayer : MonoBehaviour
{
    //Setting up changable variables for enemies speeds
    public float enemySpeed = 1f;
    public Transform Player;
    public Transform mainTarget;
    Rigidbody rb;
    private bool targetChange = false;
    public Transform[] listOfPlayers;

    //[SerializeField] private GameObject _EnemyPlayerDamage;
    private EnemyPlayerDamage script;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        script = FindObjectOfType<EnemyPlayerDamage>();
        //Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        listOfPlayers = new Transform[players.Length];
        for (int i = 0; i < listOfPlayers.Length; ++i)
            listOfPlayers[i] = players[i].transform;

        mainTarget = GameObject.FindGameObjectWithTag("MainObjective").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // new multiplayer chase code
        Transform closestPlayer = FindClosestPlayer(listOfPlayers);

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControler>().enabled)
        {
            Vector3 position = Vector3.MoveTowards(transform.position, closestPlayer.position, enemySpeed * Time.fixedDeltaTime);
            rb.MovePosition(position);
            
            if (targetChange)
            {
                Vector3 objectivePosition = Vector3.MoveTowards(transform.position, mainTarget.position, enemySpeed * Time.fixedDeltaTime);
                rb.MovePosition(objectivePosition);
            }
            
            //transform.LookAt(Player);
        }

        /*
        else if (!GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControler>().enabled)
        {
            if (mainTarget != null)
            {
                //targetChange = true;
                Vector3 objectivePosition = Vector3.MoveTowards(transform.position, mainTarget.position, enemySpeed * Time.fixedDeltaTime);
                rb.MovePosition(objectivePosition);
                //transform.LookAt(mainTarget);
            }
        }
        /*
        //old one player chase code
        /*
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControler>().enabled)
        {
            Vector3 position = Vector3.MoveTowards(transform.position, Player.position, enemySpeed * Time.fixedDeltaTime);
            rb.MovePosition(position);
            if (targetChange)
            {
                Vector3 objectivePosition = Vector3.MoveTowards(transform.position, mainTarget.position, enemySpeed * Time.fixedDeltaTime);
                rb.MovePosition(objectivePosition);
            }
            //transform.LookAt(Player);
        }

        else if (!GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControler>().enabled)
        {
            if (mainTarget != null)
            {
                //targetChange = true;
                Vector3 objectivePosition = Vector3.MoveTowards(transform.position, mainTarget.position, enemySpeed * Time.fixedDeltaTime);
                rb.MovePosition(objectivePosition);
                //transform.LookAt(mainTarget);
            }
        }
        */

    }

    Transform FindClosestPlayer(Transform[] players)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in players)
        {
            

            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "MainObjective")
        {
            targetChange = true;

        }
    }
}
