using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyPlayerDamage : MonoBehaviour
{
    private GameObject playerSprite;
    private GameObject mainObjective;
    public int playerHealth = 10;
    public int reviveHealth = 10;
    private bool isItHit = false;
    private bool hitAgain = false;
    private bool alphaChekcer = false;
    public bool playerIsStun = false;

    // Start is called before the first frame update
    void Start()
    {
        playerSprite = gameObject.transform.Find("PlayerPivot").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth <= 0)
        {
            alphaChekcer = true;
            StartCoroutine(stun());
            playerHealth += reviveHealth;
            Debug.Log(playerHealth);
            //gameOver();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && !isItHit && !hitAgain)
        {
            StartCoroutine(iframes());
        }
    }
    private IEnumerator iframes()
    {
        if (playerHealth > 0)
        {
            var trans = 0.5f;
            var col = playerSprite.transform.Find("PlayerSprite").GetComponent<SpriteRenderer>().color;
            col.a = trans;
            playerSprite.transform.Find("PlayerSprite").GetComponent<SpriteRenderer>().color = col;
            isItHit = true;
            playerHealth -= 5;
            // process pre-yield
            Debug.Log(playerHealth);
            yield return new WaitForSeconds(5.0f);
            // process post-yield
            if (!alphaChekcer)
            {
                col.a = 1f;
                playerSprite.transform.Find("PlayerSprite").GetComponent<SpriteRenderer>().color = col;
            }
            isItHit = false;
        }
    }

    private IEnumerator stun()
    {
        playerIsStun = true;

        //Debug.Log(gameObject + "is stun");

        //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControler>().enabled = false;
        gameObject.GetComponent<PlayerControler>().enabled = false;
        hitAgain = true;
        var trans = 0.2f;
        //var col = GameObject.Find("PlayerSprite").GetComponent<SpriteRenderer>().color;
        //GameObject playerSprite = gameObject.transform.Find("PlayerPivot").gameObject;
        var col = playerSprite.transform.Find("PlayerSprite").GetComponent<SpriteRenderer>().color;
        col.a = trans;
        //GameObject.Find("PlayerSprite").GetComponent<SpriteRenderer>().color = col;
        playerSprite.transform.Find("PlayerSprite").GetComponent<SpriteRenderer>().color = col;
        //GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyToPlayer>().enabled = false;
        //GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyToObjective>().enabled = true;
        yield return new WaitForSeconds(15.0f);
        // process post-yield
        playerIsStun = false;

        col.a = 0.5f;
        //GameObject.Find("PlayerSprite").GetComponent<SpriteRenderer>().color = col;
        playerSprite.transform.Find("PlayerSprite").GetComponent<SpriteRenderer>().color = col;
        //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControler>().enabled = true;
        gameObject.GetComponent<PlayerControler>().enabled = true;
        yield return new WaitForSeconds(3.0f);
        hitAgain = false;
        alphaChekcer = false;
        col.a = 1f;
        //GameObject.Find("PlayerSprite").GetComponent<SpriteRenderer>().color = col;
        playerSprite.transform.Find("PlayerSprite").GetComponent<SpriteRenderer>().color = col;
        Debug.Log("Checking if double wait works");


    }

    private void gameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); ;
    }
}
