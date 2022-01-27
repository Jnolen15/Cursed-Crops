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
    bool windupStarting = false;
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
        
        if (Vector3.Distance(gameObject.transform.position, targetToAttack.transform.position) <= 6f)
        {

            if (!windupStarting)
            {
                windupStarting = true;
                //preAttackPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
                attackPosition = new Vector3(targetToAttack.transform.position.x, targetToAttack.transform.position.y, targetToAttack.transform.position.z);

            }
            //attacking = true;

            gameObject.GetComponent<EnemyToPlayer>().enemySpeed = 0;

            if (!attacking)
            {
                
                StartCoroutine("attack");
            }


            //transform.position = Vector3.MoveTowards(transform.position, targetToAttack.position, 10f * Time.deltaTime);
        }
        else if (Vector3.Distance(gameObject.transform.position, targetToAttack.transform.position) > 6f && !attacking)
        {
            //StopCoroutine("attack");


            gameObject.GetComponent<EnemyToPlayer>().enemySpeed = gameObject.GetComponent<EnemyToPlayer>().originalSpeed;
            if (!gameObject.GetComponent<EnemyControler>().takingDamage) { 
                sr.color = prev;
            }
            
        }
 

    }

    IEnumerator attack()
    {
        
        sr.color = Color.yellow;
        yield return new WaitForSeconds(0.45f);
        Debug.Log("first wait");

        
        sr.color = Color.green;
        //1 0.92 0.016 1
        transform.position = Vector3.MoveTowards(transform.position, attackPosition, (gameObject.GetComponent<EnemyToPlayer>().originalSpeed * 4) * Time.deltaTime);
        attacking = true;
        //gameObject.GetComponent<EnemyToPlayer>().enemySpeed = 0;
        yield return new WaitForSeconds(1f);

        windupStarting = false;        
        attacking = false;
        





    }
}
