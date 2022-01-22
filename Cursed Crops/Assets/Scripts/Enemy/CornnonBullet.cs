using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornnonBullet : MonoBehaviour
{

    public GameObject bullet;
    public Transform mainTarget;

    private Vector3 direction;
    private bool shooting = false;
    // Start is called before the first frame update
    void Start()
    {
        mainTarget = GameObject.FindGameObjectWithTag("MainObjective").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.GetComponent<EnemyControler>().health > 0)
        {
            if (!shooting)
            {
                shooting = true;
                direction = new Vector3 (Random.Range(-10, 10), 0, Random.Range(-10, 10));
                StopCoroutine("shoot");
                StartCoroutine("shoot");
            }
        }
        else
        {
            StopCoroutine("shoot");
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "MainObjective")
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }

    IEnumerator shoot()
    {
        //gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        yield return new WaitForSeconds(0.65f);
        //gameObject.GetComponent<Renderer>().material.color = Color.green;
        GameObject bul = Instantiate(bullet, transform.position, transform.rotation);
        // Send bullet in correct direction
        //Debug.Log(direction);
        bul.GetComponent<Bullet>().movement = direction.normalized;
        //enemySpeed = 0f;
        yield return new WaitForSeconds(0.00000001f);
        //enemySpeed = originalSpeed;
        //gameObject.GetComponent<Renderer>().material.color = prev;
        shooting = false;

    }
}
