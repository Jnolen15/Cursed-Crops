using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToEnemy : MonoBehaviour
{
    public Transform plane;
    public Transform randomObject;
    public bool gettingBack;
    float minx;
    float minz;
    private float TickSpeed = 10f;
    private float Timer = 10f;
    private Vector3 randomPosition;

    const float minPathupdateTime = .2f;
    const float pathUpdateMoveThreshhold = .5f;
    //Setting up changable variables for enemies speeds
    public float enemySpeed = 1f;
    public float originalSpeed = 1f;
    public Transform Player;
    public Transform mainTarget;
    Rigidbody rb;
    private bool targetChange = false;
    public Transform[] listOfEnemies;
    public bool aPlayerIsStun = true;
    Vector3[] path;
    int targetIndex = 0;
    private bool playerinbound = true;
    PathFinding pathFinder;
    public Transform closestPlayer;
    public Transform oldTarget;
    public AudioClip spawnsound;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //minx = plane..x;
        //minz = plane.localScale.z;
        
        //randomPosition = new Vector3(Random.Range(-plane.position.x, plane.position.x), 0f, Random.Range(-plane.position.z, plane.position.z));

        mainTarget = GameObject.FindGameObjectWithTag("MainObjective").GetComponent<Transform>();
        gameObject.GetComponent<AudioPlayer>().PlaySound(spawnsound);
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
        closestPlayer = FindLowHealthEnemy(listOfEnemies);
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
        //Might use this to make mediberries go to random locations if there are no targets
        //if (Timer <= 0)
        //{
        //randomPosition = new Vector3(Random.Range(-minx, minx), 0f, Random.Range(-minz, minz));
        //    Timer = TickSpeed;
        //}
        //else
        //{
        //    Timer -= Time.deltaTime;
        //}
        // new multiplayer chase code
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        listOfEnemies = new Transform[enemies.Length];

        for (int i = 0; i < listOfEnemies.Length; ++i)
            listOfEnemies[i] = enemies[i].transform;

        if (!gettingBack)
        {
            closestPlayer = FindLowHealthEnemy(listOfEnemies);
        }

        foreach (GameObject c in enemies)
        {
            if (c.GetComponent<GoToEnemy>() != null )
            {
                if (Vector3.Distance(c.transform.position, this.gameObject.transform.position) <= 5)
                {
                    if (c != this.gameObject)
                    {
                        this.gameObject.GetComponent<EnemyControler>().healing = true;
                    }//c.gameObject.GetComponent<EnemyControler>().healing = true;
                }
                else
                {
                    if (c != this.gameObject)
                    {
                        //c.gameObject.GetComponent<EnemyControler>().healing = false;
                        this.gameObject.GetComponent<EnemyControler>().healing = false;

                    }
                }

                if (Vector3.Distance(c.transform.position, closestPlayer.transform.position) <= 1)
                {
                    enemySpeed = 0;
                }
                else
                {
                    enemySpeed = originalSpeed;
                }

                
            }  
        }
        

        
        //Debug.Log("Number of enemies" + listOfEnemies.Length);

        if (oldTarget != closestPlayer && oldTarget != null && closestPlayer != null)
        {

            oldTarget = closestPlayer;
            StartCoroutine("UpdatePath");
        }
        if (!this.gameObject.activeInHierarchy)
        {

            StopAllCoroutines();
            //Destroy(gameObject);
        }
        if (this.gameObject.GetComponent<EnemyControler>().takingDamage)
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

    Transform FindLowHealthEnemy(Transform[] enemies)
    {
        Transform bestTarget = mainTarget.transform;

        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        //float damage = 0;

        foreach (Transform potentialTarget in enemies)
        {

            //Debug.Log(potentialTarget + " did " + playerDamage.overAllPlayerDamage);
            //damage += playerDamage.overAllPlayerDamage;
            //higherDamage = playerDamage.overAllPlayerDamage;


            if ((potentialTarget.GetComponent<EnemyControler>().health < potentialTarget.GetComponent<EnemyControler>().maxHealth) && potentialTarget.GetComponent<EnemyControler>().health > 0)
            {

                Vector3 directionToTarget = potentialTarget.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
            
            
                if (dSqrToTarget < closestDistanceSqr)
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
