using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbageAI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject trappedPlayer;
    private float actualSpeed = 4;
    public bool boostedHealthActivate = false;
    private bool stop = false;
    private bool isItHit = false;
    public bool lettingGo = false;
    public bool alreadyGrabbing = false;
    private GrabbageToPlayers gTP;
    public AudioClip spawnSound;
    private Animator animator;
    void Start()
    {
        animator = this.gameObject.transform.GetChild(0).GetComponent<Animator>();
        gTP = this.GetComponentInParent<GrabbageToPlayers>();
        gameObject.GetComponent<AudioPlayer>().PlaySound(spawnSound);
        actualSpeed = gameObject.GetComponent<GrabbageToPlayers>().originalSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        if(trappedPlayer != null && gameObject.GetComponent<EnemyControler>().health <= 0)
        {
            Debug.Log("do we check if the cabbage dies");
            boostedHealthActivate = false;
            StopAllCoroutines();
            trappedPlayer.GetComponent<PlayerControler>().trapped = false;
            gameObject.SetActive(false);
        }
        if(gameObject.GetComponent<EnemyControler>().health <= 0)
        {
            boostedHealthActivate = false;
            StopAllCoroutines();
            gameObject.SetActive(false);
        }*/
        if (!gameObject.activeInHierarchy)
        {
            StopAllCoroutines();
        }
        if (trappedPlayer != null)
        {
            if (!lettingGo && alreadyGrabbing)
            {

                StartCoroutine("LetGo");
            }
            if (lettingGo && alreadyGrabbing)
            {

                StartCoroutine("takeRest");
            }
            if (lettingGo)
            {


                trappedPlayer.GetComponent<PlayerControler>().trapped = false;
                trappedPlayer.transform.position = Vector3.MoveTowards(trappedPlayer.transform.position, gameObject.GetComponent<GrabbageWindup>().enemyPosition + new Vector3(0, 0, -0.1f), 0.4f);


                StopCoroutine("cheapDamage");


            }

            if (!isItHit)
            {

                isItHit = true;
                if (trappedPlayer.gameObject.GetComponent<EnemyPlayerDamage>().playerHealth > 0 && gameObject.activeInHierarchy)
                {
                    StartCoroutine("cheapDamage");
                }
                else if (!gameObject.activeInHierarchy || trappedPlayer.gameObject.GetComponent<EnemyPlayerDamage>().playerHealth <= 0)
                {
                    isItHit = false;
                    StopCoroutine("cheapDamage");
                }
            }
        }
        Debug.Log(lettingGo);

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && gameObject.GetComponent<EnemyControler>().health > 0 && other.gameObject.GetComponent<PlayerControler>().state != PlayerControler.State.Rolling && !alreadyGrabbing)
        {
            if (!lettingGo)
            {
                gameObject.GetComponent<GrabbageToPlayers>().enemySpeed = 0;
                gameObject.GetComponent<GrabbageToPlayers>().originalSpeed = 0;
                gameObject.transform.position = other.gameObject.transform.position + new Vector3(0, 0, -0.1f);
                gameObject.GetComponent<GrabbageWindup>().enabled = false;

            }
            if (!alreadyGrabbing)
            {

                StopCoroutine("takeRest");
                alreadyGrabbing = true;
                isItHit = false;
                gameObject.GetComponent<GrabbageWindup>().enabled = false;

                trappedPlayer = other.gameObject;

                trappedPlayer.gameObject.GetComponent<PlayerControler>().trapped = true;

            }
            



        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //alreadyGrabbing = false;
            
            StopCoroutine("LetGo");
        }
    }



    IEnumerator cheapDamage()
    {
        if (trappedPlayer.GetComponent<EnemyPlayerDamage>().playerHealth > 0)
        {
            trappedPlayer.GetComponent<EnemyPlayerDamage>().Damage(1);
            yield return new WaitForSeconds(2f);

        }
        isItHit = false;

    }
    IEnumerator LetGo()
    {

        yield return new WaitForSeconds(6f);
        
        lettingGo = true;
        

    }
    IEnumerator takeRest()
    {
        yield return new WaitForSeconds(0.2f);
        alreadyGrabbing = false;
        lettingGo = false;
        animator.SetFloat("Speed", gTP.enemySpeed);
        yield return new WaitForSeconds(1.5f);
        gameObject.GetComponent<GrabbageToPlayers>().enemySpeed = actualSpeed;
        gameObject.GetComponent<GrabbageToPlayers>().originalSpeed = actualSpeed;
        gameObject.GetComponent<GrabbageWindup>().enabled = true;



    }
}
