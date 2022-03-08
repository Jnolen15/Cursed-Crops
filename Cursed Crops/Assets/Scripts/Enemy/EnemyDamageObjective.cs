using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyDamageObjective : MonoBehaviour
{
    public GameObject enemies;
    public GameObject mainObjective;
    public int houseHealth = 5000;
    public int startingHouseHealth = 5000;

    private bool isItHit = false;
    // Start is called before the first frame update
    void Start()
    {
        houseHealth = startingHouseHealth;
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
    
    //Don't want to erase this code yet 
    /*private void OnTriggerEnter(Collider other)
    {
        while (other.gameObject.tag == "attackBox" && !isItHit)
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
    */

    //Function for the house to take damage
    public void takeDamage(int damage)
    {
        if (!isItHit)
        {
            houseHealth -= damage;
            StartCoroutine(iframes());
        }
    }
    // IEnumarator so doesn't freaking get one 1 shotted in 1 second
    public IEnumerator iframes()
    {
        isItHit = true;
        // process pre-yield
        //Debug.Log(houseHealth);
        yield return new WaitForSeconds(5.0f);
        // process post-yield
        isItHit = false;
    }
}
