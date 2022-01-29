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
    public int damage = 1;
    public bool isItHit = false;
    private bool hitAgain = false;
    private bool alphaChekcer = false;
    public bool playerIsStun = false;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        playerSprite = gameObject.transform.Find("PlayerPivot").gameObject;
        animator = this.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // This is a temp fix. Player should not be able to take damage when downed
        if(playerIsStun)
            playerHealth = reviveHealth;

        if (playerHealth <= 0)
        {
            //alphaChekcer = true;
            StartCoroutine(downed());
            playerHealth = reviveHealth;
            Debug.Log(playerHealth);
            //gameOver();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<WindUpAttackMelee>() != null && other.gameObject.GetComponent<GrabbageAI>() == null)
        {
            if (other.gameObject.tag == "Enemy" && !isItHit && !hitAgain && other.gameObject.GetComponent<WindUpAttackMelee>().attacking)
            {
                StartCoroutine(iframes(damage));
            }
        }
    }
    public IEnumerator iframes(int damages)
    {
        if (playerHealth > 0)
        {
            isItHit = true;
            playerHealth -= damages;

            // Flash red and play hurt anim
            playerSprite.transform.Find("PlayerSprite").GetComponent<SpriteRenderer>().color = Color.red;
            animator.SetTrigger("Hurt");
            yield return new WaitForSeconds(0.1f);
            playerSprite.transform.Find("PlayerSprite").GetComponent<SpriteRenderer>().color = Color.white;
            
            // Finish Iframes
            yield return new WaitForSeconds(0.9f);
            isItHit = false;
        }
    }

    private IEnumerator downed()
    {
        playerIsStun = true;
        yield return new WaitForSeconds(10.0f);
        playerIsStun = false;
    }

    // Now unused
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
