using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    // ================= Public variables =================
    public bool doesSomething = true;
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
    public bool onCooldown = false;
    private GameObject vines;
    private ParticleSystem ps;

    /* NOTE:
     * I'm trying to build this script to be useable for all
     * 'trap' type buildables. That way we have 1 versitile
     * script instead of a single script for each trap.
     * Especially since many traps will have similar code.
     */

    void Start()
    {
        trapSprite = this.transform.GetChild(0).GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        gameObject.GetComponent<AudioPlayer>().SetAudioSource(soundClip);
        trapChild = this.transform.GetChild(1).gameObject;

        ps = this.transform.Find("Notes").gameObject.GetComponent<ParticleSystem>();
        ps.Pause();
    }

    void Update()
    {
        if (doesSomething)
        {
            if (!sabotaged)
            {
                if (!onCooldown)
                {
                    spawnEffect();
                    onCooldown = true;
                    StartCoroutine(cooldown(() => { onCooldown = false; }, cdTime));
                }
            }

            if (sabotaged && trapChild.GetComponent<TurretSabotager>().theSabotager != null && !trapChild.GetComponent<TurretSabotager>().theSabotager.activeInHierarchy)
            {
                Sabotage();
            }
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
        if (ps != null)
            ps.Emit(8);
        GameObject eff = Instantiate(effect, transform.position, transform.rotation);
    }

    IEnumerator cooldown(Callback callback, float time)
    {
        yield return new WaitForSeconds(time);
        callback?.Invoke();
    }

    public bool Sabotage()
    {
        // If already sabotaged
        if (sabotaged)
        {
            if (vines != null)
                Destroy(vines);

            sabotaged = false;
            onCooldown = false;
        }
        // If no longer sabotaged
        else
        {
            sabotaged = true;
            if (vines == null)
                vines = Instantiate(Resources.Load<GameObject>("Effects/Vines"), transform.position, transform.rotation, transform);
            StopCoroutine(cooldown(() => { onCooldown = false; }, cdTime));
            playonce = true;
            gameObject.GetComponent<AudioPlayer>().StopSound();
        }

        return sabotaged;
    }
}
