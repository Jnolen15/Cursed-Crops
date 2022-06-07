using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : MonoBehaviour
{
    // ================= Public variables =================
    public Transform Player;
    public Transform mainTarget;
    public Transform[] listOfPlayers;
    public GameObject bullet;
    public Vector3 direction;
    public float enemySpeed = 1f;
    public float originalSpeed = 1f;
    public float rangeDistance = 10f;
    public float cooldown = 3f;
    public bool gotHit = false;
    public bool aPlayerIsStun = true;
    public bool shooting = false;
    public bool onCooldown = false;
    public LayerMask maskToIgnore;

    // ================= Private variables =================
    private Transform closestPlayer;
    private Transform oldTarget;
    private Transform instantiatePoint;
    private Rigidbody rb;
    private EnemyControler ec;
    private Vector3[] path;
    private const float minPathupdateTime = .2f;
    private const float pathUpdateMoveThreshhold = .5f;
    private int targetIndex = 0;
    private bool targetChange = false;
    private float cooldownTimer = 0f;

    public AudioClip shootSound;


    void Start()
    {
        instantiatePoint = this.transform.GetChild(1).GetComponent<Transform>();
        ec = this.GetComponent<EnemyControler>();
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

    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }
        closestPlayer = FindClosestPlayer(listOfPlayers);
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
        // Cooldown stuffs
        if (cooldownTimer < cooldown) cooldownTimer += Time.deltaTime; // Face aim period
        else onCooldown = false;

        // new multiplayer chase code
        Transform closestPlayer = FindClosestPlayer(listOfPlayers);
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

        direction = new Vector3(closestPlayer.position.x - transform.position.x, 0, closestPlayer.position.z - transform.position.z);
        // Raycast to target to see if it can be hit
        RaycastHit hit;
        Debug.DrawRay(transform.position, direction, Color.red);
        if (Vector3.Distance(closestPlayer.position, transform.position) < rangeDistance && !ec.stunned && Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, ~maskToIgnore))
        {
            if (hit.collider.gameObject.tag == "Player" || hit.collider.gameObject.tag == "MainObjective")
            {
                enemySpeed = 0;

                if (!shooting && !onCooldown && !ec.stunned)
                {
                    gameObject.GetComponent<AudioPlayer>().PlaySound(shootSound);
                    shooting = true;
                    //StopCoroutine("shoot");
                    //StartCoroutine("shoot");
                }
            } else
            {
                enemySpeed = originalSpeed;
            }
        }
        else
        {
            enemySpeed = originalSpeed;
        }

        // Get stunned
        if (ec.takingDamage && ec.lastDamageType == "Melee" && !ec.stunned)
        {
            ec.Stun();
            cooldownTimer = 0f;
            shooting = false;
            onCooldown = true;
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
                //Debug.Log(potentialTarget + "Has the highest amount of damage = " + higherDamage);
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
            if(!ec.stunned) transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, enemySpeed * Time.deltaTime);
            yield return null;
            //Vector3 positioning = transform.position;
            //rb.MovePosition(positioning);

        }
    }

    public void Shoot()
    {
        if (!ec.stunned)
        {
            // Create bullet and send bullet in correct direction
            //Debug.Log("Start of shoot");
            GameObject bul = Instantiate(bullet, instantiatePoint.position, instantiatePoint.rotation);
            bul.GetComponent<Bullet>().movement = direction.normalized;
            onCooldown = true;
            cooldownTimer = 0f;
            shooting = false;
        }
    }
}
