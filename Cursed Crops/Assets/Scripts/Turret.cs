using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    // ================= Public variables =================
    public GameObject bullet;
    public bool shooting = false;
    public float cooldown = 2f;

    public Sprite innitialSprite;
    public Sprite shootingSprite;
    public Sprite reloadingSprite;

    // ================= Private variables =================
    private Vector3 direction;
    private Vector3 flipDirection;
    private Transform enemyPosition;
    private GameObject targetedEnemy;
    private SpriteRenderer turretSprite;
    private bool flipped = false;


    void Start()
    {
        turretSprite = this.transform.GetChild(0).GetChild(0).gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        SpriteFlip();

        if (targetedEnemy != null && !shooting)
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
        direction = new Vector3(enemy.position.x - transform.position.x, 0, enemy.position.z - transform.position.z);
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
        shooting = true;
        turretSprite.sprite = shootingSprite;
        GameObject bul = Instantiate(bullet, transform.position, transform.rotation);
        // Send bullet in correct direction
        bul.GetComponent<Bullet>().movement = direction.normalized;

        // Change when animations are in (Use keyframes to signal shooting event)
        yield return new WaitForSeconds(cooldown/2f);
        turretSprite.sprite = reloadingSprite;
        yield return new WaitForSeconds(cooldown/2f);
        turretSprite.sprite = innitialSprite;
        shooting = false;
    }
}
