using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageObjective : MonoBehaviour
{
    private GameObject enemies;
    private GameObject mainObjective;
    public int houseHealth = 50;
    private bool isItHit = false;
    // Start is called before the first frame update
    void Start()
    {
        //enemies = GameObject.FindGameObjectWithTag("Enemy").GetComponent<GameObject>();
        //mainObjective = GameObject.FindGameObjectWithTag("MainObjective").GetComponent<GameObject>();
        //gameObject.tag = "MainObjective";
    }

    // Update is called once per frame
    void Update()
    {
        if(houseHealth == 0)
        {
            Destroy(gameObject);
        }
    }
       
    private void OnTriggerStay(Collider other)
    {
        while (other.gameObject.tag == "Enemy" && !isItHit)
        {
            houseHealth -= 5;
            StartCoroutine(iframes());

        }
    }
    private IEnumerator iframes()
    {
        isItHit = true;
        // process pre-yield
        Debug.Log(houseHealth);
        yield return new WaitForSeconds(5.0f);
        // process post-yield
        isItHit = false;
    }
}
