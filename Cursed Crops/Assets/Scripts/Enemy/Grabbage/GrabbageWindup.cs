using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbageWindup : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 preAttackPosition;
    Vector3 attackPosition;
    Vector3 enemyPosition;
    Vector3 newPosition;
    Vector3 direction;
    public bool attacking = false;
    bool getPosition = false;
    bool windupStarting = false;
    Rigidbody childRB;
    private SpriteRenderer sr;
    Transform targetToAttack;
    GameObject theTarget;
    private BoxCollider hurtBox;
    private MeshRenderer daAttack;
    Color prev;
    IEnumerator inst = null;
    void Start()
    {
        //hurtBox = this.gameObject.transform.GetChild(1).GetComponent<BoxCollider>();
        //daAttack = this.gameObject.transform.GetChild(1).GetComponent<MeshRenderer>();
        sr = this.transform.GetComponentInChildren<SpriteRenderer>();
        prev = sr.color;
        inst = attack();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        targetToAttack = gameObject.GetComponent<EnemyToPlayer>().oldTarget;
        //direction = new Vector3(targetToAttack.position.x - transform.position.x, 0, targetToAttack.position.z - transform.position.z);
        if (Vector3.Distance(gameObject.transform.position, targetToAttack.transform.position) <= 6f)
        {

            if (!windupStarting)
            {
                StopCoroutine("attack");
                windupStarting = true;

                //preAttackPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
                attackPosition = new Vector3(targetToAttack.transform.position.x, targetToAttack.transform.position.y, targetToAttack.transform.position.z);
                enemyPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                newPosition = (attackPosition - enemyPosition) - (attackPosition - enemyPosition).normalized * 0;

                newPosition += enemyPosition;




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

        sr.color = Color.yellow;
        //childRB.MovePosition(transform.position + direction.normalized);
        if (transform.position != newPosition)
        {
            yield return new WaitForSeconds(0.45f);


            sr.color = Color.green;
            //1 0.92 0.016 1
            transform.position = Vector3.MoveTowards(transform.position, newPosition, (gameObject.GetComponent<EnemyToPlayer>().originalSpeed * 20) * Time.deltaTime);

            attacking = true;
            //gameObject.GetComponent<EnemyToPlayer>().enemySpeed = 0;
        }
        /*
        if (transform.position == newPosition)
        {
            hurtBox.enabled = true;
            //daAttack.enabled = true;
        }
        */

        yield return new WaitForSeconds(1f);
        //hurtBox.enabled = false;
        //daAttack.enabled = false;
        windupStarting = false;
        attacking = false;






    }
}
