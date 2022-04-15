using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyDamageObjective : MonoBehaviour
{
    public GameObject enemies;
    public GameObject mainObjective;
    public int houseHealth = 500;
    public int startingHouseHealth = 500;
    public GameObject damageNotif;

    private float iframesTime = 0.2f;
    private float damageNotifCooldown = 6f;
    private float damageNotifTimer = 6f;
    private bool showingNotif = false;
    private bool isItHit = false;

    void Start()
    {
        houseHealth = startingHouseHealth;

        damageNotif = this.transform.GetChild(0).gameObject;
    }

    void Update()
    {
        //Once the objective's health reaches zero destroy it and change the scene to the game over
        if(houseHealth <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }

        // Notification Cooldown
        if (damageNotifTimer < damageNotifCooldown)
        {
            showingNotif = true;
            damageNotifTimer += Time.deltaTime;
        }
        else showingNotif = false;

        // Show Notification
        if(showingNotif)
        {
            // Display notification
            damageNotif.SetActive(true);
        } else
        {
            // Close notification
            damageNotif.SetActive(false);
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
            damageNotifTimer = 0f;
        }
    }

    public void takeDamageIgnoreIFrames(int damage)
    {
        houseHealth -= damage;
        damageNotifTimer = 0f;
    }

    // IEnumarator so doesn't freaking get one 1 shotted in 1 second
    public IEnumerator iframes()
    {
        isItHit = true;
        // process pre-yield
        //Debug.Log(houseHealth);
        yield return new WaitForSeconds(iframesTime);
        // process post-yield
        isItHit = false;
    }
}
