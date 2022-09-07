using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindupWithStun : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 preAttackPosition;
    Vector3 attackPosition;
    Vector3 enemyPosition;
    Vector3 newPosition;
    Vector3 direction;
    Vector3 knocknewPosition;
    public bool attacking = false;
    public bool windupFinish = false;
    private bool inPosition = false; 
    bool getPosition = false;
    bool windupStarting = false;
    private bool hitFence = false;
    Rigidbody childRB;
    Rigidbody rb;
    public SpriteRenderer sr;
    public Transform targetToAttack;
    GameObject theTarget;
    private BoxCollider hurtBox;
    private MeshRenderer daAttack;
    private EnemyControler ec;
    private attackBoxDamage attackBoxBools;
    public Color prev;
    IEnumerator inst = null;
    private float attackTimer = 1;
    private float attackTickSpeed = 1;

    public LayerMask maskToIgnore;

    void Start()
    {
        childRB = this.gameObject.transform.GetChild(1).GetComponent<Rigidbody>();
        rb = gameObject.GetComponent<Rigidbody>();
        hurtBox = this.gameObject.transform.GetChild(1).GetComponent<BoxCollider>();
        attackBoxBools = this.gameObject.transform.GetChild(1).GetComponent<attackBoxDamage>();
        Debug.Log(hurtBox);
        daAttack = this.gameObject.transform.GetChild(1).GetComponent<MeshRenderer>();
        sr = this.transform.GetComponentInChildren<SpriteRenderer>();
        ec = this.gameObject.GetComponent<EnemyControler>();
        prev = sr.color;
        inst = attack();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (ec.stunned)
        {
            hurtBox.enabled = false;
            attacking = false;
        }
        
        
        targetToAttack = gameObject.GetComponent<EnemyToPlayer>().closestPlayer;
        if (targetToAttack != null)
        {
            direction = new Vector3(targetToAttack.position.x - transform.position.x, 0, targetToAttack.position.z - transform.position.z);
            // Raycast to target to see if it can be hit
            RaycastHit hit;
            Debug.DrawRay(transform.position, direction, Color.red);
            if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, ~maskToIgnore))
            {
                if (hit.collider.gameObject.tag == "Player" || hit.collider.gameObject.tag == "MainObjective")
                {
                    //Debug.Log("Raycast Hit!");
                    if (Vector3.Distance(gameObject.transform.position, targetToAttack.transform.position) <= 6f && !hitFence)
                    {
                        //StopCoroutine("stun");

                        if (!ec.takingDamage && !ec.stunned)
                        {
                            if (!windupStarting)
                            {
                                //StopCoroutine("attack");
                                sr.color = Color.yellow;
                                windupStarting = true;
                                attackBoxBools.playOnce = false;
                                attackPosition = new Vector3(targetToAttack.transform.position.x, targetToAttack.transform.position.y, targetToAttack.transform.position.z);
                                enemyPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                                newPosition = (attackPosition - enemyPosition) - (attackPosition - enemyPosition).normalized * 1.5f;

                                newPosition += enemyPosition;
                            }
                            else
                            {
                                gameObject.GetComponent<EnemyToPlayer>().enemySpeed = 0;
                                if (!attacking && !ec.stunned)
                                {
                                    sr.color = Color.yellow;
                                    if (!gameObject.activeInHierarchy)
                                    {
                                        StopCoroutine("attack");
                                    }
                                    else
                                    {
                                        StartCoroutine("attack");
                                    }
                                    if (ec.takingDamage && ec.lastDamageType == "Melee")
                                    {
                                        StopCoroutine("attack");
                                    }
                                }
                            }
                        }
                        //transform.position = Vector3.MoveTowards(transform.position, targetToAttack.position, 10f * Time.deltaTime);
                    }
                }
                else
                {
                    //Debug.Log("Raycast blocked by: " + hit.collider.gameObject.name);
                }
            }
            else
            {
                //Debug.Log("Raycast hit nothing");
            }

            if (ec.takingDamage && ec.lastDamageType == "Melee" && !ec.stunned)
            {
                attacking = false;
                windupStarting = false;
                StopCoroutine("attack");
                ec.Stun();
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
        }
        
        if (Vector3.Distance(gameObject.transform.position, targetToAttack.transform.position) > 6f && !windupStarting)
        {
            StopCoroutine("attack");


            sr.color = prev;
            gameObject.GetComponent<EnemyToPlayer>().enemySpeed = gameObject.GetComponent<EnemyToPlayer>().originalSpeed;



        }

        

    }

    IEnumerator attack()
    {

        
        childRB.MovePosition(transform.position + direction.normalized);
        //if (transform.position != newPosition)
        //{
            yield return new WaitForSeconds(0.75f);


            
            //1 0.92 0.016 1
            transform.position = Vector3.MoveTowards(transform.position, newPosition, (gameObject.GetComponent<EnemyToPlayer>().originalSpeed * 20) * Time.deltaTime);

            attacking = true;
            
            //gameObject.GetComponent<EnemyToPlayer>().enemySpeed = 0;
        //}
        if (transform.position == newPosition)
        {
            hurtBox.enabled = true;
            sr.color = Color.green;
            //daAttack.enabled = true;
        }

        yield return new WaitForSeconds(1f);
        
        
        hurtBox.enabled = false;
        //daAttack.enabled = false;
        windupStarting = false;
        windupFinish = true;
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
