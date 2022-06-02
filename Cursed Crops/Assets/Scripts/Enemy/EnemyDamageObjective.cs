using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EnemyDamageObjective : MonoBehaviour
{
    public GameObject enemies;
    public GameObject mainObjective;
    public int houseHealth = 500;
    public int startingHouseHealth = 500;

    public GameObject uiCavas;
    public GameObject damageNotif;
    public GameObject healthNotif;
    public GameObject barn;
    public GameObject destroyedBarn;

    private float iframesTime = 0.2f;
    private float damageNotifCooldown = 6f;
    private float damageNotifTimer = 6f;
    private bool showingNotif = false;
    private bool isItHit = false;
    private bool shownHalfWarning = false;
    private bool shownQuarterWarning = false;
    private bool lostLevel = false;

    void Start()
    {
        houseHealth = startingHouseHealth;

        uiCavas = GameObject.Find("UI Canvas");
        damageNotif = uiCavas.transform.Find("UI Overlay/General UI/AttackWarning").gameObject;
        healthNotif = uiCavas.transform.Find("UI Overlay/General UI/HealthWarning").gameObject;
        healthNotif.SetActive(false);

        barn = this.transform.Find("FarmHouse").gameObject;
        destroyedBarn = this.transform.Find("FarmHouseRuined").gameObject;
    }

    void Update()
    {
        //Once the objective's health reaches zero destroy it and change the scene to the game over
        if(houseHealth <= 0 && !lostLevel)
        {
            StartCoroutine(FailLevel());
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

        // Alert players when house health is at half or quarter
        if (houseHealth <= startingHouseHealth/2 && !shownHalfWarning)
        {
            shownHalfWarning = true;
            StartCoroutine(ShowHalfWarning());
        }

        if (houseHealth <= startingHouseHealth / 4 && !shownQuarterWarning)
        {
            shownQuarterWarning = true;
            healthNotif.GetComponent<TextMeshProUGUI>().SetText("Barn at One Quarter Health");
            StartCoroutine(ShowQuarterWarning());
        }
    }

    public IEnumerator ShowHalfWarning()
    {
        healthNotif.SetActive(true);
        yield return new WaitForSeconds(4f);
        healthNotif.SetActive(false);
    }

    public IEnumerator ShowQuarterWarning()
    {
        healthNotif.SetActive(true);
        yield return new WaitForSeconds(4f);
        healthNotif.SetActive(false);
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

    public IEnumerator FailLevel()
    {
        lostLevel = true;
        barn.SetActive(false);
        destroyedBarn.SetActive(true);
        healthNotif.GetComponent<TextMeshProUGUI>().SetText("The Barn Has Been Destroyed!");
        healthNotif.SetActive(true);
        var cam = GameObject.FindGameObjectWithTag("MainCamera");
        cam.GetComponent<CameraMover>().hasEnded = true;
        yield return new WaitForSeconds(8);
        SceneManager.LoadScene("GameOver");
    }
}
