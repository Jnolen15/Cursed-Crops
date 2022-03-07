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
    public int pierceAmmount = 3;

    [Header("Payloads: damage when reach destination")]
    public bool isPayload = false;
    public GameObject effect;
    public Vector3 destination;

    // ================= Private variables =================
    private Rigidbody rb;
    private SpriteRenderer sr;
    private bool exploded = false; // Used for Payload type bullets
    private int pierceCount;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sr = GetComponent<SpriteRenderer>();
        pierceCount = pierceAmmount;
    }

    private void Update()
    {
        // If payload is in range of destination, explode
        if (isPayload)
        {
            if (((this.transform.position.x <= destination.x + 2) && (this.transform.position.x >= destination.x - 2)) 
                && ((this.transform.position.z <= destination.z + 2) && (this.transform.position.z >= destination.z - 2)))
            {
                //Debug.Log("BOOM BITCH");
                //exploded = true;
                Vector3 pos = new Vector3(this.transform.position.x, 0f, this.transform.position.z);
                Instantiate(effect, pos, Quaternion.Euler(-90f, 0f, 0f));
                Destroy(this.gameObject);
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
                other.gameObject.GetComponent<EnemyPlayerDamage>().Damage(damage);
                Destroy(this.gameObject);
            }
            else if (other.gameObject.tag == "MainObjective")
            {
                other.gameObject.GetComponent<EnemyDamageObjective>().houseHealth -= damage;
                Destroy(this.gameObject);
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
                if (!piercing || pierceCount <= 0)
                    Destroy(gameObject);
                else pierceCount--;
                //Debug.Log("Hit " + other.gameObject.tag);
            }
        }
    }

    /*private void OnTriggerStay(Collider other)
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
    }*/
}
