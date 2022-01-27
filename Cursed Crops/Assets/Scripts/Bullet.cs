using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    private SpriteRenderer sr;
    

    public Vector3 movement;
    public float range = 1f;
    public int damage = 5;
    public int bulletSpeed = 15;
    public bool piercing = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + movement * bulletSpeed * Time.fixedDeltaTime);
        Destroy(this.gameObject, range);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Hit " + other.gameObject.tag);
        if ((other.gameObject.tag == "Enemy" || other.gameObject.name == "cornnonBullet(Clone)") && gameObject.tag != "enemyBullet")
        {
            EnemyControler enemyControler = other.gameObject.GetComponent<EnemyControler>();
            enemyControler.takeDamageRange(damage);
            if(!piercing)
                Destroy(gameObject);
            Debug.Log("Hit Enemy");
        }

        // adding line for enemy bullets
        else if(gameObject.tag == "enemyBullet" && other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<EnemyPlayerDamage>().playerHealth -= 5;
        }
        else if (gameObject.tag == "enemyBullet" && other.gameObject.tag == "MainObjective")
        {
            other.gameObject.GetComponent<EnemyDamageObjective>().houseHealth -= 5;
        }
    }
}
