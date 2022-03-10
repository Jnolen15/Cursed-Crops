using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarrotAttack : MonoBehaviour
{
    // ================= Public variables =================
    public float aoeCoolDown = 9f;
    public float straightAttackCoolDown = 5f;
    public int playerdamage = 5;
    public int houseDamage = 5;
    public bool attacking = false;
    public bool attackAOE = false;
    public bool attackDash = false;
    public bool onCooldown = false;
    public AudioClip spawnSound;

    // ================= Private variables =================
    Vector3 preAttackPosition;
    Vector3 attackPosition;
    Vector3 enemyPosition;
    Vector3 newPosition;
    private float attackTimer = 2;
    private float attackTickSpeed = 2;
    private bool getPosition = false;
    private bool windupStarting = false;
    private bool hitFence = false;
    private bool attackMoving = false;
    private int chooseAttack;
    private float randomTimer = 0;
    private SpriteRenderer sr;
    Transform targetToAttack;
    GameObject AOE;

    Color prev;
    IEnumerator inst = null;
    void Start()
    {
        sr = this.transform.GetComponentInChildren<SpriteRenderer>();
        prev = sr.color;
        AOE = gameObject.transform.Find("AOE").gameObject;
        gameObject.GetComponent<AudioPlayer>().PlaySound(spawnSound);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        // Death Check
        if(gameObject.GetComponent<EnemyControler>().health <= 0)
        {
            StopAllCoroutines();
            gameObject.GetComponent<EnemyControler>().death();
        }
        
        // Get closest Player to attack
        targetToAttack = gameObject.GetComponent<EnemyToPlayer>().closestPlayer;

        // Attack if close to the player
        if (Vector3.Distance(gameObject.transform.position, targetToAttack.transform.position) <= 6f && !hitFence)
        {
            if (!windupStarting)
            {
                StopCoroutine("attack");
                
                windupStarting = true;
                //preAttackPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
                chooseAttack = Random.Range(1, 3);
                randomTimer = Random.Range(0.45f, 1f);
                // DASH
                if (chooseAttack == 1)
                {
                    //sr.color = Color.yellow;
                }
                // AOE
                else if(chooseAttack == 2)
                {
                    //sr.color = Color.magenta;
                    attackPosition = new Vector3(targetToAttack.transform.position.x, 1, targetToAttack.transform.position.z);
                    enemyPosition = new Vector3(transform.position.x, 1, transform.position.z);
                    newPosition = (attackPosition - enemyPosition) - (attackPosition - enemyPosition).normalized * 2;

                    newPosition += enemyPosition;
                }

            }

            if (!attacking)
            {
                gameObject.GetComponent<EnemyToPlayer>().enemySpeed = 0;
                if (!gameObject.activeInHierarchy)
                {
                    StopCoroutine("attack");
                }
                else
                {
                    StartCoroutine("attack");
                }
            }
            //transform.position = Vector3.MoveTowards(transform.position, targetToAttack.position, 10f * Time.deltaTime);
        }
        else if (Vector3.Distance(gameObject.transform.position, targetToAttack.transform.position) > 6f && !attacking)
        {
            //StopCoroutine("attack");
            gameObject.GetComponent<EnemyToPlayer>().enemySpeed = gameObject.GetComponent<EnemyToPlayer>().originalSpeed;
            if (!gameObject.GetComponent<EnemyControler>().takingDamage)
            {
                //sr.color = prev;
            }

        }
        else if (attacking)
        {
            gameObject.GetComponent<EnemyToPlayer>().enemySpeed = 0;
        }

        // Move twords player while attacking
        if (transform.position != newPosition && attackMoving && !hitFence)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPosition, (gameObject.GetComponent<EnemyToPlayer>().originalSpeed * 20) * Time.deltaTime);
        }
        else
        {
            attackMoving = false;
        }

        //If the scarrot hits the fence stop its attack and give it a bit to gets its cool down again
        if (hitFence)
        {
            StopCoroutine("attack");
            if (attackTimer <= 0)
            {
                attackTimer = attackTickSpeed;
                    AOE.SetActive(false);
                    getPosition = false;
                    windupStarting = false;
                    attacking = false;
                    chooseAttack = 0;
                hitFence = false;
                
            }
            else
            {
               attackTimer -= Time.deltaTime;
            }
        }

    }

    IEnumerator attack()
    {
        

        // DASH Attack
        if (chooseAttack == 1)
        {
            //if (transform.position != newPosition)
            //{
            yield return new WaitForSeconds(randomTimer);
            if (!getPosition)
            {
                getPosition = true;
                attackPosition = new Vector3(targetToAttack.transform.position.x, 1, targetToAttack.transform.position.z);
                enemyPosition = new Vector3(transform.position.x, 1, transform.position.z);
                newPosition = (attackPosition - enemyPosition) + (attackPosition - enemyPosition).normalized * 6;

                newPosition += enemyPosition;
            }
            //sr.color = Color.green;
            attackDash = true;
            //1 0.92 0.016 1
            attacking = true;
            //gameObject.GetComponent<EnemyToPlayer>().enemySpeed = 0;
            //}
            //yield return new WaitForSeconds(straightAttackCoolDown);
            //getPosition = false;
            //windupStarting = false;
            //attacking = false;
            //attackDash = false;
            //chooseAttack = 0;
        }

        // AOE Attack
        if(chooseAttack == 2)
        {
            if (transform.position != newPosition)
            {
                yield return new WaitForSeconds(0.45f);
                Debug.Log("first wait");


                //sr.color = Color.green;
                attackMoving = true;
                //transform.position = Vector3.MoveTowards(transform.position, newPosition, (gameObject.GetComponent<EnemyToPlayer>().originalSpeed * 20) * Time.deltaTime);
            }
            attackAOE = true;
            //AOE.SetActive(true);
            attacking = true;
            //gameObject.GetComponent<EnemyToPlayer>().enemySpeed = 0;
            //yield return new WaitForSeconds(0.5f);
            //AOE.SetActive(false);
            //yield return new WaitForSeconds(aoeCoolDown);
            //windupStarting = false;
            //attacking = false;
            //attackAOE = false;
            //chooseAttack = 0;
        }
    }


    // DASH ATTACK
    public void DashAttack()
    {
        attackMoving = true;
        StartCoroutine("DashAttackCooldown");
    }

    IEnumerator DashAttackCooldown()
    {
        attackDash = false;
        yield return new WaitForSeconds(0.2f);
        onCooldown = true;
        yield return new WaitForSeconds(straightAttackCoolDown);
        getPosition = false;
        windupStarting = false;
        attacking = false;
        onCooldown = false;
        chooseAttack = 0;
    }


    // AOE ATTACK
    public void AOEAttack()
    {
        AOE.SetActive(true);
        StartCoroutine("AOEAttackCooldown");
    }

    IEnumerator AOEAttackCooldown()
    {
        attackAOE = false;
        yield return new WaitForSeconds(0.2f);
        AOE.SetActive(false);
        onCooldown = true;
        yield return new WaitForSeconds(aoeCoolDown);
        windupStarting = false;
        attacking = false;
        onCooldown = false;
        chooseAttack = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Border")
        {
            hitFence = true;
        }

        if (other.gameObject.tag == "Player" && attacking)
        {
            other.gameObject.GetComponent<EnemyPlayerDamage>().Damage(playerdamage);
        }
        if (other.gameObject.tag == "MainObjective" && attacking)
        {
            other.gameObject.GetComponent<EnemyDamageObjective>().takeDamage(houseDamage);
        }
    }
    
}
