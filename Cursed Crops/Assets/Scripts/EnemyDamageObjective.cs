using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyDamageObjective : MonoBehaviour
{
    public GameObject enemies;
    public GameObject mainObjective;
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
        //Once the objective's health reaches zero destroy it and change the scene to the game over
        if(houseHealth <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
    
    //OnCollision Enter doesn't work for some reason but on trigger will do for now to do damage to the house
    // Need to find a way to make damage stack if multiple enemies are in it
    private void OnTriggerStay(Collider other)
    {
        while (other.gameObject.tag == "Enemy" && !isItHit)
        {
            //houseHealth -= 5;
            //StartCoroutine(iframes());
            Collider[] enemies = Physics.OverlapBox(transform.position, transform.localScale / 2,
                                                            transform.rotation, LayerMask.GetMask("Enemies"));
            multiDamage(enemies);

        }
    }

    private void multiDamage (Collider[] enemies)
    {
        foreach(Collider c in enemies)
        {
            houseHealth -= 5;
            StartCoroutine(iframes());
        }
    }
    // IEnumarator so doesn't freaking get one 1 shotted in 1 second
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
