using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bullet;
    public Transform enemies;
    public Transform[] listOfenemies;
    private Vector3 direction;
    public bool shooting = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {

        
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
