using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretBullet : MonoBehaviour
{
    private Rigidbody rb;
    private SpriteRenderer sr;
    private int bulletSpeed = 15;

    public Vector3 movement;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + movement * bulletSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Hit " + other.gameObject.tag);
        if (other.gameObject.tag == "Enemy")
        {
            EnemyControler enemyControler = other.gameObject.GetComponent<EnemyControler>();
            enemyControler.takeDamage(5);
            Destroy(gameObject);
            Debug.Log("Hit Enemy");
        }
    }
}
