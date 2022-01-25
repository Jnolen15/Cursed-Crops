using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbageAI : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject trappedPlayer;
    private bool boostedHealthActivate = false;
    private bool isItHit = false;
    private bool alreadyGrabbing = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(trappedPlayer != null && gameObject.GetComponent<EnemyControler>().health <= 0)
        {
            Debug.Log("do we check if the cabbage dies");
            boostedHealthActivate = false;
            trappedPlayer.GetComponent<PlayerControler>().trapped = false;
            gameObject.SetActive(false);
        }
        else if(gameObject.GetComponent<EnemyControler>().health <= 0)
        {
            boostedHealthActivate = false;
            gameObject.SetActive(false);
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && gameObject.GetComponent<EnemyControler>().health > 0)
        {
            gameObject.GetComponent<EnemyToPlayer>().enemySpeed = 0;
            if (!alreadyGrabbing)
            {
                gameObject.GetComponent<WindUpAttackMelee>().enabled = false;
                alreadyGrabbing = true;
                trappedPlayer = other.gameObject;

                
                gameObject.transform.position = other.gameObject.transform.position;

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
                        StopCoroutine("cheapDamage");
                    }
                }
            }
            
            
        }
    }

    IEnumerator cheapDamage()
    {
        trappedPlayer.GetComponent<EnemyPlayerDamage>().playerHealth -= 1;
        yield return new WaitForSeconds(1f);
        isItHit = false;

    }
}
