using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarrotAttack : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 preAttackPosition;
    Vector3 attackPosition;
    Vector3 enemyPosition;
    Vector3 newPosition;
    public bool attacking = false;
    public int damage = 5;
    private bool getPosition = false;
    private bool windupStarting = false;
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
    }

    // Update is called once per frame
    void Update()
    {
        
        if(gameObject.GetComponent<EnemyControler>().health <= 0)
        {
            StopAllCoroutines();
            gameObject.GetComponent<EnemyControler>().death();

        }
        targetToAttack = gameObject.GetComponent<EnemyToPlayer>().oldTarget;

        if (Vector3.Distance(gameObject.transform.position, targetToAttack.transform.position) <= 6f)
        {

            if (!windupStarting)
            {
                StopCoroutine("attack");
                windupStarting = true;
                //preAttackPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
                chooseAttack = Random.Range(1, 3);
                randomTimer = Random.Range(0.45f, 1f);
                if (chooseAttack == 1)
                {
                    sr.color = Color.yellow;
                    
                }
                else if(chooseAttack == 2)
                {
                    sr.color = Color.magenta;
                    attackPosition = new Vector3(targetToAttack.transform.position.x, targetToAttack.transform.position.y, targetToAttack.transform.position.z);
                    enemyPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    newPosition = (attackPosition - enemyPosition) - (attackPosition - enemyPosition).normalized * 2;

                    newPosition += enemyPosition;
                }

            }
            //attacking = true;

            gameObject.GetComponent<EnemyToPlayer>().enemySpeed = 0;

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


            //transform.position = Vector3.MoveTowards(transform.position, targetToAttack.position, 10f * Time.deltaTime);
        }
        else if (Vector3.Distance(gameObject.transform.position, targetToAttack.transform.position) > 6f && !attacking)
        {
            //StopCoroutine("attack");


            gameObject.GetComponent<EnemyToPlayer>().enemySpeed = gameObject.GetComponent<EnemyToPlayer>().originalSpeed;
            if (!gameObject.GetComponent<EnemyControler>().takingDamage)
            {
                sr.color = prev;
            }

        }
        else if (attacking)
        {
            gameObject.GetComponent<EnemyToPlayer>().enemySpeed = 0;
        }


    }

    IEnumerator attack()
    {
        if (chooseAttack == 1)
        {
            if (transform.position != newPosition)
            {
                yield return new WaitForSeconds(randomTimer);
                if (!getPosition)
                {
                    getPosition = true;
                    attackPosition = new Vector3(targetToAttack.transform.position.x, targetToAttack.transform.position.y, targetToAttack.transform.position.z);
                    enemyPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    newPosition = (attackPosition - enemyPosition) + (attackPosition - enemyPosition).normalized * 6;

                    newPosition += enemyPosition;
                }
                sr.color = Color.green;
                //1 0.92 0.016 1
                transform.position = Vector3.MoveTowards(transform.position, newPosition, (gameObject.GetComponent<EnemyToPlayer>().originalSpeed * 4) * Time.deltaTime);
                attacking = true;
                //gameObject.GetComponent<EnemyToPlayer>().enemySpeed = 0;
            }
            yield return new WaitForSeconds(1f);
            getPosition = false;
            windupStarting = false;
            attacking = false;
            chooseAttack = 0;
        }
        if(chooseAttack == 2)
        {
            if (transform.position != newPosition)
            {
                yield return new WaitForSeconds(0.45f);
                Debug.Log("first wait");


                sr.color = Color.green;
                transform.position = Vector3.MoveTowards(transform.position, newPosition, (gameObject.GetComponent<EnemyToPlayer>().originalSpeed * 4) * Time.deltaTime);
            }
            AOE.SetActive(true);
            attacking = true;
            //gameObject.GetComponent<EnemyToPlayer>().enemySpeed = 0;
            yield return new WaitForSeconds(1f);
            AOE.SetActive(false);
            windupStarting = false;
            attacking = false;
            
            chooseAttack = 0;
        }






    }

    
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player" && attacking)
        {
            StartCoroutine(other.gameObject.GetComponent<EnemyPlayerDamage>().iframes(damage));
        }
        else if (other.gameObject.tag == "MainObjective" && attacking)
        {
            other.gameObject.GetComponent<EnemyDamageObjective>().houseHealth -= damage;
        }
    }
}
