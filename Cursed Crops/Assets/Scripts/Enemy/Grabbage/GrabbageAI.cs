using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbageAI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject trappedPlayer;
    public bool boostedHealthActivate = false;
    private bool isItHit = false;
    public bool alreadyGrabbing = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && gameObject.GetComponent<EnemyControler>().health > 0)
        {
            gameObject.GetComponent<GrabbageToPlayers>().enemySpeed = 0;
            gameObject.GetComponent<GrabbageToPlayers>().originalSpeed = 0;
            gameObject.transform.position = other.gameObject.transform.position + new Vector3(0, 0, -0.1f);

            if (!alreadyGrabbing)
            {
                gameObject.GetComponent<GrabbageWindup>().enabled = false;
                alreadyGrabbing = true;
                trappedPlayer = other.gameObject;
                
                other.gameObject.GetComponent<PlayerControler>().trapped = true;
                if (!isItHit)
                {

                    isItHit = true;
                    if (other.gameObject.GetComponent<EnemyPlayerDamage>().playerHealth > 0)
                    {
                        StartCoroutine("cheapDamage");
                    }
                    else
                    {
                        isItHit = false;
                        StopCoroutine("cheapDamage");
                    }
                }
            }
            
            
        }
    }

    

    IEnumerator cheapDamage()
    {
        if (trappedPlayer.GetComponent<EnemyPlayerDamage>().playerHealth > 0)
        {
            trappedPlayer.GetComponent<EnemyPlayerDamage>().Damage(1);
            yield return new WaitForSeconds(1f);
            
        }
        isItHit = false;

    }
}
