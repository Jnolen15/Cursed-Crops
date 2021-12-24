using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : MonoBehaviour
{
    const float minPathupdateTime = .2f;
    const float pathUpdateMoveThreshhold = .5f;
    //Setting up changable variables for enemies speeds
    public float enemySpeed = 1f;
    public float originalSpeed = 1f;
    public float rangeDistance = 10f;
    public Transform Player;
    public Transform mainTarget;
    Rigidbody rb;
    private bool targetChange = false;
    public Transform[] listOfPlayers;
    public bool aPlayerIsStun = true;
    Vector3[] path;
    int targetIndex = 0;
    bool shooting = false;
    Transform closestPlayer;
    Transform oldTarget;
    //all stuff for the range enemies
    public GameObject bullet;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        listOfPlayers = new Transform[players.Length];

        for (int i = 0; i < listOfPlayers.Length; ++i)
            listOfPlayers[i] = players[i].transform;

        mainTarget = GameObject.FindGameObjectWithTag("MainObjective").GetComponent<Transform>();
        //Transform closestPlayer = FindClosestPlayer(listOfPlayers);
        //pathFinder.StartFindPath(transform.position, closestPlayer.position);
        //PathRequestManager.RequestPath(transform.position, closestPlayer.position, OnPathFound);
        oldTarget = mainTarget;
        StartCoroutine("UpdatePath");


    }

    // Update is called once per frame
    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }
        closestPlayer = FindClosestPlayer(listOfPlayers);
        PathRequestManager.RequestPath(transform.position, closestPlayer.position, OnPathFound);

        float sqrMoveThreshhold = pathUpdateMoveThreshhold * pathUpdateMoveThreshhold;
        Vector3 targetPosOld = closestPlayer.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathupdateTime);
            if ((closestPlayer.position - targetPosOld).sqrMagnitude > sqrMoveThreshhold)
            {
                PathRequestManager.RequestPath(transform.position, closestPlayer.position, OnPathFound);
                targetPosOld = closestPlayer.position;
            }
        }
    }
    void FixedUpdate()
    {
        // new multiplayer chase code

        Transform closestPlayer = FindClosestPlayer(listOfPlayers);
        
        if (oldTarget != closestPlayer && oldTarget != null && closestPlayer != null)
        {

            oldTarget = closestPlayer;
            
            StartCoroutine("UpdatePath");
        }
        if (Vector3.Distance(closestPlayer.position, transform.position) < rangeDistance)
        {
            Debug.Log("hello are you shooting");
            
            enemySpeed = 0;
            
            if (!shooting)
            {
                shooting = true;
                direction = new Vector3(closestPlayer.position.x - transform.position.x, 0, closestPlayer.position.z - transform.position.z);
                StartCoroutine(shoot());
            }
        }
        else
        {
            enemySpeed = originalSpeed;
        }


    }

    Transform FindClosestPlayer(Transform[] players)
    {
        Transform bestTarget = mainTarget.transform;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        float higherDamage = 0;
        //float damage = 0;

        foreach (Transform potentialTarget in players)
        {
            EnemyPlayerDamage playerStun = potentialTarget.GetComponent<EnemyPlayerDamage>();
            PlayerControler playerDamage = potentialTarget.GetComponent<PlayerControler>();

            if (playerDamage.overAllPlayerDamage > higherDamage && !playerStun.playerIsStun)
            {
                higherDamage = playerDamage.overAllPlayerDamage;
                bestTarget = potentialTarget;
                Debug.Log(potentialTarget + "Has the highest amount of damage = " + higherDamage);
            }
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr && !playerStun.playerIsStun && higherDamage == 0)
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

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            if (path.Length > 0)
            {
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");

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

    IEnumerator shoot()
    {
        
        GameObject bul = Instantiate(bullet, transform.position, transform.rotation);
        // Send bullet in correct direction
        //Debug.Log(direction);
        bul.GetComponent<Bullet>().movement = direction.normalized;
        yield return new WaitForSeconds(1.0f);
        //enemySpeed = 0f;
        yield return new WaitForSeconds(1.0f);
        //enemySpeed = originalSpeed;
        shooting = false;
    }
}
