using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaboAI : MonoBehaviour
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
    public Transform[] listOfTurrets;
    public bool aPlayerIsStun = true;
    Vector3[] path;
    int targetIndex = 0;
    private bool playerinbound = true;
    PathFinding pathFinder;
    public Transform closestTurret;
    public Transform oldTarget;
    private EnemyControler ec;

    // Start is called before the first frame update
    void Start()
    {
        ec = this.gameObject.GetComponent<EnemyControler>();
        rb = GetComponent<Rigidbody>();

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

        PathRequestManager.RequestPath(new PathRequest(transform.position, closestTurret.position, OnPathFound));

        float sqrMoveThreshhold = pathUpdateMoveThreshhold * pathUpdateMoveThreshhold;
        Vector3 targetPosOld = closestTurret.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathupdateTime);
            if ((closestTurret.position - targetPosOld).sqrMagnitude > sqrMoveThreshhold)
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, closestTurret.position, OnPathFound));
                targetPosOld = closestTurret.position;
            }
        }
    }
    void FixedUpdate()
    {
        // new multiplayer chase code

        GameObject[] turrets = GameObject.FindGameObjectsWithTag("Turret");
        listOfTurrets = new Transform[turrets.Length];

        for (int i = 0; i < listOfTurrets.Length; ++i)
            listOfTurrets[i] = turrets[i].transform;


        // Using the old code so the Sabomato will keep changing to each turret there is
        closestTurret = FindClosestTurret(listOfTurrets);
        if (oldTarget != closestTurret && oldTarget != null && closestTurret != null)
        {

           oldTarget = closestTurret;
           StartCoroutine("UpdatePath");
        }
        if (!gameObject.activeInHierarchy)
        {

            StopAllCoroutines();
            //Destroy(gameObject);
        }

    }

    Transform FindClosestTurret(Transform[] turrets)
    {
        Vector3 randomPosition = Vector3.zero;
        Transform bestTarget = mainTarget.transform;

        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        //float higherDamage = 0;

        //float damage = 0;

        foreach (Transform potentialTarget in turrets)
        {
            Turret stopped = potentialTarget.GetComponent<Turret>();
            Trap trapStopped = potentialTarget.GetComponent<Trap>();
            //Debug.Log(potentialTarget + " did " + playerDamage.overAllPlayerDamage);
            //damage += playerDamage.overAllPlayerDamage;
            //higherDamage = playerDamage.overAllPlayerDamage;

            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            Vector3 directionToMain = mainTarget.position - currentPosition;
            float distanceToMain = directionToMain.sqrMagnitude;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (distanceToMain > dSqrToTarget)
            {
                /*
                if (playerDamage.overAllPlayerDamage > higherDamage && !playerStun.playerIsStun)
                {
                    higherDamage = playerDamage.overAllPlayerDamage;
                    bestTarget = potentialTarget;
                    //Debug.Log(potentialTarget + "Has the highest amount of damage = " + higherDamage);
                }
                && higherDamage == 0
                */


                if (dSqrToTarget < closestDistanceSqr && (stopped != null && !stopped.sabotaged || trapStopped != null && !trapStopped.sabotaged))
                {

                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;
                }
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
            if (!ec.stunned) transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, enemySpeed * Time.deltaTime);
            yield return null;
            //Vector3 positioning = transform.position;
            //rb.MovePosition(positioning);

        }
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