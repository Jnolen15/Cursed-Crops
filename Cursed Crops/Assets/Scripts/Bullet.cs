using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // ================= Public variables =================
    public Vector3 movement;
    public float range = 1f;
    public int damage = 5;
    public int bulletSpeed = 15;
    public bool piercing = false;

    [Header("Payloads: damage when reach destination")]
    public bool isPayload = false;
    public Vector3 destination;

    // ================= Private variables =================
    private Rigidbody rb;
    private SpriteRenderer sr;
    private bool exploded = false; // Used for Payload type bullets

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sr = GetComponent<SpriteRenderer>();
        // GameObject.FindObjectsOfType<SpriteLeaner>();
    }

    private void Update()
    {
        // If payload is in range of destination, explode
        if (isPayload)
        {
            if (((this.transform.position.x <= destination.x + 1) && (this.transform.position.x >= destination.x - 1)) 
                && ((this.transform.position.z <= destination.z + 1) && (this.transform.position.z >= destination.z - 1)))
            {
                Debug.Log("BOOM BITCH");
                exploded = true;
            }
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + movement * bulletSpeed * Time.fixedDeltaTime);
        Destroy(this.gameObject, range);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Enemy bullets
        if (gameObject.tag == "enemyBullet")
        {
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<EnemyPlayerDamage>().playerHealth -= 5;
            }
            else if (other.gameObject.tag == "MainObjective")
            {
                other.gameObject.GetComponent<EnemyDamageObjective>().houseHealth -= 5;
            }
        }

        // Player or Turret Bullets
        else
        {
            // Normal Bullets
            if (!isPayload && (other.gameObject.tag == "Enemy" || other.gameObject.name == "cornnonBullet(Clone)"))
            {
                EnemyControler enemyControler = other.gameObject.GetComponent<EnemyControler>();
                enemyControler.takeDamage(damage, "Range");
                if (!piercing)
                    Destroy(gameObject);
                //Debug.Log("Hit " + other.gameObject.tag);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Payload Bullets
        if (isPayload && exploded)
        {
            if (other.gameObject.tag == "Enemy" || other.gameObject.name == "cornnonBullet(Clone)")
            {
                EnemyControler enemyControler = other.gameObject.GetComponent<EnemyControler>();
                enemyControler.ApplyEffect("Burning", 4f);
                enemyControler.takeDamage(damage, "Range");
            }
            Destroy(this.gameObject);
        }
    }
}
