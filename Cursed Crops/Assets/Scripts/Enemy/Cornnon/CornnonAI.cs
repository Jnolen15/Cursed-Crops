using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornnonAI : MonoBehaviour
{
    // ================= Public variables =================
    //Setting up changable variables for enemies speeds
    public float enemySpeed = 1f;
    public float originalSpeed = 1f;
    public float rangeDistance = 10f;
    public bool gotHit = false;
    public Transform Player;
    public Transform mainTarget;
    public Transform[] listOfPlayers;
    public bool aPlayerIsStun = true;
    public GameObject bullet;
    public bool spawning = false;
    public bool shooting = false;
    public LayerMask maskToIgnore;
    public AudioClip spawnSound;
    public AudioClip prepareShot;

    // ================= Private variables =================
    const float minPathupdateTime = .2f;
    const float pathUpdateMoveThreshhold = .5f;
    private Rigidbody rb;
    private bool targetChange = false;
    Vector3[] path;
    int targetIndex = 0;
    Transform closestPlayer;
    Transform oldTarget;
    private Vector3 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        listOfPlayers = new Transform[players.Length];

        for (int i = 0; i < listOfPlayers.Length; ++i)
            listOfPlayers[i] = players[i].transform;

        gameObject.GetComponent<AudioPlayer>().PlaySound(spawnSound);
        mainTarget = GameObject.FindGameObjectWithTag("MainObjective").GetComponent<Transform>();
        //Transform closestPlayer = FindClosestPlayer(listOfPlayers);
        //pathFinder.StartFindPath(transform.position, closestPlayer.position);
        //PathRequestManager.RequestPath(transform.position, closestPlayer.position, OnPathFound);
        //oldTarget = mainTarget;
        closestPlayer = mainTarget;
        StartCoroutine("UpdatePath");


    }

    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }
        closestPlayer = mainTarget;
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
        if (!gameObject.activeInHierarchy)
        {
            StopAllCoroutines();
            //Destroy(gameObject);
        }

        direction = new Vector3(closestPlayer.position.x - transform.position.x, 0, closestPlayer.position.z - transform.position.z);
        // Raycast to target to see if it can be hit
        RaycastHit hit;
        Debug.DrawRay(transform.position, direction, Color.red);

        if (Vector3.Distance(closestPlayer.position, transform.position) < rangeDistance && Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, ~maskToIgnore))
        {
            if (hit.collider.gameObject.tag == "Player" || hit.collider.gameObject.tag == "MainObjective")
            {
                
                enemySpeed = 0;
                if (!shooting)
                {
                    gameObject.GetComponent<AudioPlayer>().PlaySound(prepareShot);
                    shooting = true;
                    //shoot();
                }
            }
        }
        else
        {
            enemySpeed = originalSpeed;
        }
    }

    public void Shoot()
    {
        GameObject bul = Instantiate(bullet, transform.position, transform.rotation);
        bul.GetComponent<Bullet>().movement = direction.normalized;
        bul.GetComponent<EnemyControler>().health = this.gameObject.GetComponent<EnemyControler>().health;
        //shooting = false;
    }

    public void Die()
    {
        this.gameObject.SetActive(false);
    }
    
    /*IEnumerator shoot()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        yield return new WaitForSeconds(0.65f);
        gameObject.GetComponent<Renderer>().material.color = Color.green;
        GameObject bul = Instantiate(bullet, transform.position, transform.rotation);
        // Send bullet in correct direction
        //Debug.Log(direction);
        bul.GetComponent<Bullet>().movement = direction.normalized;
        //enemySpeed = 0f;
        yield return new WaitForSeconds(7.5f);
        //enemySpeed = originalSpeed;
        gameObject.GetComponent<Renderer>().material.color = prev;
        shooting = false;

    }*/

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

    /* private void OnTriggerEnter(Collider melee)
     {
         if(melee.gameObject.name == "MeleeAttackLeft" || melee.gameObject.name == "MeleeAttackRight")
         {
             Debug.Log("We hit the range enemy");
             gotHit = true;
             StopCoroutine("shoot");
             StartCoroutine("stun");
         }
     }
    */

    IEnumerator stun()
    {
        Debug.Log("getting stun");
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<EnemyControler>().takingDamage = false;
        shooting = false;
    }
}
