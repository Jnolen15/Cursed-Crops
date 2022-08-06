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
    public bool aPlayerIsAlready = false;
    public bool noMoreGrabs = true;
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
        if (!gameObject.activeInHierarchy)
        {
            StopAllCoroutines();
        }
        if (trappedPlayer != null)
        {

            if (!lettingGo && alreadyGrabbing)
            {
                
                    gameObject.GetComponent<GrabbageToPlayers>().enemySpeed = 0;
                    gameObject.GetComponent<GrabbageToPlayers>().originalSpeed = 0;
                    gameObject.transform.position = trappedPlayer.gameObject.transform.position + new Vector3(0, 0, -0.1f);
                    gameObject.GetComponent<GrabbageWindup>().enabled = false;


                
                StartCoroutine("LetGo");
            }
            if (lettingGo && alreadyGrabbing)
            {
                
                StartCoroutine("takeRest");
            }
            if (lettingGo)
            {

                StopCoroutine("LetGo");
                alreadyGrabbing = false;
                StopCoroutine("cheapDamage");
                trappedPlayer.GetComponent<PlayerControler>().trapped = false;
                aPlayerIsAlready = false;
                trappedPlayer.transform.position = Vector3.MoveTowards(trappedPlayer.transform.position, gameObject.GetComponent<GrabbageWindup>().enemyPosition + new Vector3(0, 0, -0.1f), 0.4f);

            }

            if (!isItHit)
            {

                isItHit = true;
                if (trappedPlayer.gameObject.GetComponent<EnemyPlayerDamage>().playerHealth > 1 && gameObject.activeInHierarchy)
                {
                    StartCoroutine("cheapDamage");
                }
                else if (!gameObject.activeInHierarchy || trappedPlayer.gameObject.GetComponent<EnemyPlayerDamage>().playerHealth <= 0)
                {
                    isItHit = false;
                    StopCoroutine("cheapDamage");
                }
            }
            if(trappedPlayer.gameObject.GetComponent<PlayerControler>().state == PlayerControler.State.Downed)
            {
                StopAllCoroutines();
            }
        }
        Debug.Log(lettingGo);
    }

    private void OnTriggerStay(Collider other)
    {
        if (noMoreGrabs)
        {
            if (other.gameObject.tag == "Player" && (gameObject.GetComponent<EnemyControler>().health > 0) && other.gameObject.GetComponent<PlayerControler>().state != PlayerControler.State.Rolling)
            {

                if (!other.gameObject.GetComponent<PlayerControler>().trapped && !alreadyGrabbing)
                {
                    trappedPlayer = other.gameObject;

                    StopCoroutine("takeRest");
                    alreadyGrabbing = true;
                    isItHit = false;
                    gameObject.GetComponent<GrabbageWindup>().enabled = false;


                    trappedPlayer.GetComponent<PlayerControler>().moveSpeed = 0;
                    //trappedPlayer.transform.position = gameObject.transform.position;
                    trappedPlayer.gameObject.GetComponent<PlayerControler>().trapped = true;

                }
                






            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (trappedPlayer!= null && other.gameObject == trappedPlayer)
        {
            //alreadyGrabbing = false;
            StopCoroutine("cheapDamage");
            //StopCoroutine("LetGo");

        }
    }



    IEnumerator cheapDamage()
    {
        if (trappedPlayer.GetComponent<EnemyPlayerDamage>().playerHealth > 1)
        {
            trappedPlayer.GetComponent<EnemyPlayerDamage>().Damage(1);
            yield return new WaitForSeconds(2f);

        }
        isItHit = false;

    }
    IEnumerator LetGo()
    {

        yield return new WaitForSeconds(6f);
        noMoreGrabs = false;
        lettingGo = true;
        //alreadyGrabbing = false;
        

    }
    IEnumerator takeRest()
    {
        yield return new WaitForSeconds(0.2f);
        alreadyGrabbing = false;
        lettingGo = false;
        animator.SetFloat("Speed", gTP.enemySpeed);
        yield return new WaitForSeconds(1.5f);
        //gameObject.GetComponent<GrabbageToPlayers>().enemySpeed = actualSpeed;
        //gameObject.GetComponent<GrabbageToPlayers>().originalSpeed = actualSpeed;
        gameObject.GetComponent<GrabbageWindup>().enabled = true;
        yield return new WaitForSeconds(2f);
        noMoreGrabs = true;



    }
}
