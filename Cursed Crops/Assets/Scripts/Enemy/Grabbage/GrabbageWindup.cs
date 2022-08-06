using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbageWindup : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 preAttackPosition;
    Vector3 attackPosition;
    public Vector3 enemyPosition;
    Vector3 newPosition;
    Vector3 direction;
    public bool attacking = false;
    bool getPosition = false;
    public bool windupStarting = false;
    public bool startWalking = false;
    private bool hitFence = false;
    private float attackTimer = 1;
    private float attackTickSpeed = 1;
    private float actualSpeed = 0;
    Rigidbody childRB;
    private SpriteRenderer sr;
    Transform targetToAttack;
    GameObject theTarget;
    private BoxCollider hurtBox;
    private MeshRenderer daAttack;
    Color prev;

    public LayerMask maskToIgnore;
    public AudioClip grabSound;

    IEnumerator inst = null;
    void Start()
    {
        //hurtBox = this.gameObject.transform.GetChild(1).GetComponent<BoxCollider>();
        actualSpeed = gameObject.GetComponent<GrabbageToPlayers>().originalSpeed;
        //daAttack = this.gameObject.transform.GetChild(1).GetComponent<MeshRenderer>();
        sr = this.transform.GetComponentInChildren<SpriteRenderer>();
        prev = sr.color;
        inst = attack();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        targetToAttack = gameObject.GetComponent<GrabbageToPlayers>().closestPlayer;
        direction = new Vector3(targetToAttack.position.x - transform.position.x, 0, targetToAttack.position.z - transform.position.z);
        // Raycast to target to see if it can be hit
        RaycastHit hit;
        Debug.DrawRay(transform.position, direction, Color.red);
        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, ~maskToIgnore))
        {
            if (hit.collider.gameObject.tag == "Player" || hit.collider.gameObject.tag == "MainObjective")
            {
                if (Vector3.Distance(gameObject.transform.position, targetToAttack.transform.position) <= 6f && !hitFence)
                {
                    //StopCoroutine("stun");
                    if (!windupStarting && gameObject.GetComponent<GrabbageAI>().noMoreGrabs)
                    {
                        //StopCoroutine("attack");
                        sr.color = Color.yellow;
                        windupStarting = true;
                        gameObject.GetComponent<AudioPlayer>().PlaySound(grabSound);
                        startWalking = true;
                        //preAttackPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
                        attackPosition = new Vector3(targetToAttack.transform.position.x, targetToAttack.transform.position.y, targetToAttack.transform.position.z);
                        enemyPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                        newPosition = (attackPosition - enemyPosition) - (attackPosition - enemyPosition).normalized * 0;

                        newPosition += enemyPosition;
                    }
                    //transform.position = Vector3.MoveTowards(transform.position, targetToAttack.position, 10f * Time.deltaTime);
                }
            }
        }
        if (startWalking)
        {
            gameObject.GetComponent<GrabbageToPlayers>().originalSpeed = 0;
        }
        else
        {
            sr.color = prev;
            gameObject.GetComponent<GrabbageToPlayers>().originalSpeed = actualSpeed;
        }
        if (windupStarting)
        {
            
            
            if (!attacking)
            {
                
                if (!gameObject.activeInHierarchy)
                {
                    StopCoroutine("attack");
                }
                else
                {
                    StartCoroutine("attack");
                }
            }
        }


        if (hitFence)
        {
            StopCoroutine("attack");
            if (attackTimer <= 0)
            {
                attackTimer = attackTickSpeed;
                windupStarting = false;
                attacking = false;
                hitFence = false;


            }
            else
            {
                attackTimer -= Time.deltaTime;
            }
        }

        if (Vector3.Distance(gameObject.transform.position, targetToAttack.transform.position) > 6f && !windupStarting)
        {
            StopCoroutine("attack");


            sr.color = prev;
            gameObject.GetComponent<GrabbageToPlayers>().originalSpeed = actualSpeed;



        }

    }

    IEnumerator attack()
    {

        //sr.color = Color.yellow;
        //childRB.MovePosition(transform.position + direction.normalized);
        if (transform.position != newPosition)
        {
            yield return new WaitForSeconds(0.45f);


            //sr.color = Color.green;
            //1 0.92 0.016 1
            transform.position = Vector3.MoveTowards(transform.position, newPosition, (actualSpeed * 10) * Time.deltaTime);

            attacking = true;
            //gameObject.GetComponent<EnemyToPlayer>().enemySpeed = 0;
        }

        yield return new WaitForSeconds(1f);
        if (Vector3.Distance(gameObject.transform.position, targetToAttack.transform.position) > 6f && !windupStarting)
        {
            //StopCoroutine("attack");


            sr.color = prev;
            gameObject.GetComponent<GrabbageToPlayers>().originalSpeed = actualSpeed;



        }
        startWalking = false;
        //hurtBox.enabled = false;
        //daAttack.enabled = false;
        yield return new WaitForSeconds(1f);
        windupStarting = false;
        attacking = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Border")
        {
            hitFence = true;
        }
    }
}
