using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    // ================= Public variables =================
    public bool singleUse = false;
    public bool sabotaged = false;
    public GameObject effect;
    public GameObject trapChild;
    public float cdTime = 2f;
    public int cost = 0;
    public AudioClip soundClip;

    // ================= Private variables =================
    private delegate void Callback();
    private SpriteRenderer trapSprite;
    private bool playonce = false;
    private Color prev;
    public bool onCooldown = false;

    /* NOTE:
     * I'm trying to build this script to be useable for all
     * 'trap' type buildables. That way we have 1 versitile
     * script instead of a single script for each trap.
     * Especially since many traps will have similar code.
     */

    void Start()
    {
        trapSprite = this.transform.GetChild(0).GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        prev = trapSprite.color;
        gameObject.GetComponent<AudioPlayer>().SetAudioSource(soundClip);
        trapChild = this.transform.GetChild(1).gameObject;
    }

    void Update()
    {
        if (!sabotaged)
        {
            trapSprite.color = prev;
            if (singleUse)
            {

            }
            else if (!onCooldown)
            {
                spawnEffect();
                onCooldown = true;
                StartCoroutine(cooldown(() => { onCooldown = false; }, cdTime));
            }
        }
        else
        {
            trapSprite.color = Color.red;
            StopCoroutine(cooldown(() => { onCooldown = false; }, cdTime));
            playonce = true;
            gameObject.GetComponent<AudioPlayer>().StopSound();
        }

        if (trapChild.GetComponent<TurretSabotager>().theSabotager != null && !trapChild.GetComponent<TurretSabotager>().theSabotager.activeInHierarchy)
        {
            sabotaged = false;
            playonce = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && other.gameObject.name != "cornnonBullet")
        {

        }
        if(other.gameObject.tag == "Player" && !playonce && !sabotaged)
        {
            playonce = true;
            gameObject.GetComponent<AudioPlayer>().PlayTheSetClip();
            //gameObject.GetComponent<AudioPlayer>().LoopSound();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && other.gameObject.name != "cornnonBullet")
        {

        }
        if (other.gameObject.tag == "Player" && playonce && !sabotaged)
        {
            playonce = false;
            gameObject.GetComponent<AudioPlayer>().StopSound();
        }
    }

    private void spawnEffect()
    {
        GameObject eff = Instantiate(effect, transform.position, transform.rotation);
    }

    IEnumerator cooldown(Callback callback, float time)
    {
        yield return new WaitForSeconds(time);
        callback?.Invoke();
    }
}
