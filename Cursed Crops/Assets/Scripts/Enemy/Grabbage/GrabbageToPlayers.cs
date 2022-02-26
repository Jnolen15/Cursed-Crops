using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbageToPlayers : MonoBehaviour
{
    const float minPathupdateTime = .2f;
    const float pathUpdateMoveThreshhold = .5f;
    //Setting up changable variables for enemies speeds
    public float enemySpeed = 1f;
    public float originalSpeed = 1f;
    public Transform Player;
    public Transform mainTarget;
    Rigidbody rb;
    private bool targetChange = false;
    public Transform[] listOfPlayers;
    public bool aPlayerIsStun = true;
    Vector3[] path;
    int targetIndex = 0;
    private bool playerinbound = true;
    PathFinding pathFinder;
    public Transform closestPlayer;
    public Transform oldTarget;
    private float healingTickSpeed = 1f;
    private float healingTimer = 5f;
    public bool angered = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        listOfPlayers = new Transform[players.Length];
        pathFinder = GetComponent<PathFinding>();

        for (int i = 0; i < listOfPlayers.Length; ++i)
            listOfPlayers[i] = players[i].transform;

        mainTarget = GameObject.FindGameObjectWithTag("MainObjective").GetComponent<Transform>();
        //Transform closestPlayer = FindClosestPlayer(listOfPlayers);
        //pathFinder.StartFindPath(transform.position, closestPlayer.position);
        //PathRequestManager.RequestPath(transform.position, closestPlayer.position, OnPathFound);
        closestPlayer = FindClosestPlayer(listOfPlayers);
        oldTarget = closestPlayer;
        StartCoroutine("UpdatePath");


    }

    // Update is called once per frame
    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }

        PathRequestManager.RequestPath(new PathRequest(transform.position, closestPlayer.position, OnPathFound));

        float sqrMoveThreshhold = pathUpdateMoveThreshhold * pathUpdateMoveThreshhold;
        Vector3 targetPosOld = closestPlayer.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathupdateTime);
            if ((closestPlayer.position - targetPosOld).sqrMagnitude > sqrMoveThreshhold)
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, closestPlayer.position, OnPathFound));
                targetPosOld = closestPlayer.position;
            }
        }
    }
    void FixedUpdate()
    {
        // new multiplayer chase code

        closestPlayer = FindClosestPlayer(listOfPlayers);
        if (oldTarget != closestPlayer && oldTarget != null && closestPlayer != null)
        {

           oldTarget = closestPlayer;
           StartCoroutine("UpdatePath");
        }
        if (!gameObject.activeInHierarchy)
        {

            StopAllCoroutines();
            //Destroy(gameObject);
        }
        if (gameObject.GetComponent<EnemyControler>().takingDamage)
        {
            enemySpeed = 0;
            StopCoroutine("stun");
            StartCoroutine("stun");
        }
        else
        {
            enemySpeed = originalSpeed;

        }
        
        //PathRequestManager.RequestPath(this.transform.position, closestPlayer.position, OnPathFound);
        /*if (targetChange)
        {
            //Vector3 objectivePosition = Vector3.MoveTowards(transform.position, mainTarget.position, enemySpeed * Time.fixedDeltaTime);
            //rb.MovePosition(objectivePosition);
            PathRequestManager.RequestPath(transform.position, mainTarget.position, OnPathFound);
        }
        */
        /*
        Vector3 position = Vector3.MoveTowards(transform.position, closestPlayer.position, enemySpeed * Time.fixedDeltaTime);
        rb.MovePosition(position);
        
        */

    }

    Transform FindClosestPlayer(Transform[] players)
    {
        Vector3 randomPosition = Vector3.zero;
        Transform bestTarget = mainTarget.transform;

        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        //float higherDamage = 0;

        //float damage = 0;

        foreach (Transform potentialTarget in players)
        {
            EnemyPlayerDamage playerStun = potentialTarget.GetComponent<EnemyPlayerDamage>();
            PlayerControler playerDamage = potentialTarget.GetComponent<PlayerControler>();
            //Debug.Log(potentialTarget + " did " + playerDamage.overAllPlayerDamage);
            //damage += playerDamage.overAllPlayerDamage;
            //higherDamage = playerDamage.overAllPlayerDamage;

            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            //if (playerDamage.overAllPlayerDamage > higherDamage && !playerStun.playerIsStun)
            //{
            //    higherDamage = playerDamage.overAllPlayerDamage;
            //    bestTarget = potentialTarget;
                //Debug.Log(potentialTarget + "Has the highest amount of damage = " + higherDamage);
            //}


            if (dSqrToTarget < closestDistanceSqr && !playerStun.playerIsStun && !playerDamage.trapped)
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
        if (other.gameObject.name == "Player")
        {
            playerinbound = false;
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            if (path.Length > 0)
            {
                if (gameObject != null)
                {
                    StopCoroutine("FollowPath");
                    if (gameObject.activeInHierarchy)
                    {
                        StartCoroutine("FollowPath");
                    }
                    else
                    {
                        StopCoroutine("FollowPath");
                    }
                }

            }

        }
    }



    IEnumerator FollowPath()
    {

        Vector3 currentWaypoint = path[0];
        targetIndex = 0;
        //print(lengthOfCurrent);
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    targetIndex = 0;
                    path = new Vector3[0];
                    yield break;
                }
                //path[targetIndex].y = 0;

                currentWaypoint = path[targetIndex];
            }

            //print(currentWaypoint);

            //transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            //Debug.Log("currentWaypoint y = " + currentWaypoint.y);
            currentWaypoint.y = 1;
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, enemySpeed * Time.deltaTime);
            yield return null;
            //Vector3 positioning = transform.position;
            //rb.MovePosition(positioning);

        }
    }

    IEnumerator stun()
    {
        Debug.Log("getting stun");
        //gameObject.GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        //gameObject.GetComponent<Renderer>().material.color = prev;
        gameObject.GetComponent<EnemyControler>().takingDamage = false;
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
