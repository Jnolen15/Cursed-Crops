using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bullet;
    public Transform enemies;
    public Transform[] listOfenemies;
    private Vector3 direction;
    private Vector3 flipDirection;
    private Transform enemyPosition;
    public bool shooting = false;
    private bool flipped = false;
    private SpriteRenderer turretSprite;
    // Start is called before the first frame update
    void Start()
    {
        turretSprite = this.transform.GetChild(0).GetChild(0).gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        SpriteFlip();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && !shooting)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            listOfenemies = new Transform[enemies.Length];
            for (int i = 0; i < listOfenemies.Length; ++i)
            {
                listOfenemies[i] = enemies[i].transform;
            }
            Debug.Log("enemies entering radius");
            enemiesInRange(listOfenemies);
            shooting = true;
            StartCoroutine(shoot());

        }
    }

    private void enemiesInRange(Transform[] enemies)
    {
        foreach (Transform c in enemies)
        {
            direction = new Vector3(c.position.x - transform.position.x, 0, c.position.z - transform.position.z);
            enemyPosition = c.transform;
            
        }
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
        GameObject bul = Instantiate(bullet, transform.position, transform.rotation);
        // Send bullet in correct direction
        //Debug.Log(direction);
        bul.GetComponent<turretBullet>().movement = direction.normalized;
        yield return new WaitForSeconds(1.0f);
        shooting = false;
    }
}
