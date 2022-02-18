using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    // ================= Public variables =================
    public GameObject bullet;
    public bool onCooldown = false;
    public float cooldown = 2f;

    // ================= Private variables =================
    private Vector3 direction;
    private Vector3 flipDirection;
    private Transform enemyPosition;
    private Transform firePosition;
    private GameObject targetedEnemy;
    private SpriteRenderer turretSprite;
    private TurretAnimator tAnimator;
    private bool flipped = false;


    void Start()
    {
        turretSprite = this.transform.GetChild(0).GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        tAnimator = this.GetComponentInChildren<TurretAnimator>();
        firePosition = this.transform.GetChild(2).transform;
        //this.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TurretAnimator>();
    }

    void Update()
    {
        SpriteFlip();

        if (targetedEnemy != null && !onCooldown)
        {
            if (!targetedEnemy.activeSelf)
            {
                targetedEnemy = null;
            } else
            {
                enemyInRange(targetedEnemy.transform);
                StartCoroutine(shoot());
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && other.gameObject.name != "cornnonBullet")
        {
            if (targetedEnemy == null)
            {
                targetedEnemy = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && other.gameObject.name != "cornnonBullet")
        {
            // Targeted enemy leaves range
            if (targetedEnemy == other.gameObject)
            {
                targetedEnemy = null;
            }
        }
    }

    private void enemyInRange(Transform enemy)
    {
        direction = new Vector3(enemy.position.x - firePosition.position.x, 0, enemy.position.z - firePosition.position.z);
        enemyPosition = enemy.transform;
    }

    private void SpriteFlip()
    {
        if(enemyPosition != null)
        {
            flipDirection = new Vector3(enemyPosition.position.x - transform.position.x, 0, enemyPosition.position.z - transform.position.z);
            if(flipDirection.x > 0 && flipped)
            {
                flipped = false;
                turretSprite.flipX = false;

            }
            else if(flipDirection.x <= 0 && !flipped)
            {
                flipped = true;
                turretSprite.flipX = true;
            }
        }
    }

    IEnumerator shoot()
    {
        onCooldown = true;
        tAnimator.playShoot();
        //MakeBullet();
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }

    public void MakeBullet()
    {
        GameObject bul = Instantiate(bullet, firePosition.position, firePosition.rotation);
        if (bul.GetComponent<Bullet>().isPayload)
        {
            bul.GetComponent<Bullet>().destination = enemyPosition.position;
        }
        // Send bullet in correct direction
        bul.GetComponent<Bullet>().movement = direction.normalized;
    }
}
