using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindUpAttackMelee : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 preAttackPosition;
    Vector3 attackPosition;
    public bool attacking = false;
    bool getPosition = false;
    private SpriteRenderer sr;
    Transform targetToAttack;
    GameObject theTarget;
    Color prev;
    IEnumerator inst = null;
    void Start()
    {
        sr = this.transform.GetComponentInChildren<SpriteRenderer>();
        prev = sr.color;
        inst = attack();
    }

    // Update is called once per frame
    void Update()
    {
        targetToAttack = gameObject.GetComponent<EnemyToPlayer>().oldTarget;

        if (!attacking)
        {

            //preAttackPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            attackPosition = new Vector3(targetToAttack.transform.position.x, targetToAttack.transform.position.y, targetToAttack.transform.position.z);

        }
        if (Vector3.Distance(gameObject.transform.position, targetToAttack.transform.position) <= 6f && !attacking)
        {


            //attacking = true;

            
            
            gameObject.GetComponent<EnemyToPlayer>().enemySpeed = 0;
            StartCoroutine("attack");


            //transform.position = Vector3.MoveTowards(transform.position, targetToAttack.position, 10f * Time.deltaTime);
        }
        else if (Vector3.Distance(gameObject.transform.position, targetToAttack.transform.position) >= 6f && !attacking)
        {
            StopCoroutine("attack");
            if (!gameObject.GetComponent<EnemyControler>().takingDamage) { 
                sr.color = prev;
            }
            gameObject.GetComponent<EnemyToPlayer>().enemySpeed = gameObject.GetComponent<EnemyToPlayer>().originalSpeed;
        }

    }

    IEnumerator attack()
    {
        
            sr.color = Color.yellow;
            yield return new WaitForSeconds(1.5f);
            Debug.Log("first wait");
            attacking = true;
            
            transform.position = Vector3.MoveTowards(transform.position, attackPosition, (gameObject.GetComponent<EnemyToPlayer>().originalSpeed * 4) * Time.deltaTime);
            //gameObject.GetComponent<EnemyToPlayer>().enemySpeed = 0;
            yield return new WaitForSeconds(1.8f);
            if (transform.position == attackPosition)
            {
                sr.color = prev;
                attacking = false;
            }
        

        
        
    }
}