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
    public bool aPlayerIsStun = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
            

            Vector3 position = Vector3.MoveTowards(transform.position, closestPlayer.position, enemySpeed * Time.fixedDeltaTime);
            rb.MovePosition(position);
            if (targetChange)
            {
                Vector3 objectivePosition = Vector3.MoveTowards(transform.position, mainTarget.position, enemySpeed * Time.fixedDeltaTime);
                rb.MovePosition(objectivePosition);
            }
        

    }

    Transform FindClosestPlayer(Transform[] players)
    {
        Transform bestTarget = mainTarget.transform;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        float higherDamage = 0;
        float damage = 0;
        
        foreach (Transform potentialTarget in players)
        {
            EnemyPlayerDamage playerStun = potentialTarget.GetComponent<EnemyPlayerDamage>();
            PlayerControler playerDamage = potentialTarget.GetComponent<PlayerControler>();
            //Debug.Log(potentialTarget + " did " + playerDamage.overAllPlayerDamage);
            //damage += playerDamage.overAllPlayerDamage;
            
            if (playerDamage.overAllPlayerDamage < higherDamage)
            {

                
                higherDamage = playerDamage.overAllPlayerDamage;
                Debug.Log("the higher damage is: " + higherDamage);
                Debug.Log("the over damage is: " + playerDamage.overAllPlayerDamage);

            }
            Debug.Log("the higher damage is (out of loop): " + higherDamage);
            Debug.Log("the over damage is(out of loop): " + playerDamage.overAllPlayerDamage);
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if ((dSqrToTarget < closestDistanceSqr) && !playerStun.playerIsStun)
            {

                //if (higherDamage > playerDamage.overAllPlayerDamage)
                //{
                    closestDistanceSqr = dSqrToTarget;
                    
                //    higherDamage = playerDamage.overAllPlayerDamage;
                //}
                    
                bestTarget = potentialTarget;
                
            }
            
        }
        //Debug.Log(bestTarget + "is aggro base on damage: " + higherDamage);
        //Debug.Log("base on damage: " + bestTarget);

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
