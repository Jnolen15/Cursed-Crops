using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    // ================= Public variables =================
    public bool singleUse = false;
    public GameObject effect;
    public float cdTime = 2f;

    // ================= Private variables =================
    private delegate void Callback();
    private SpriteRenderer trapSprite;
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
    }

    void Update()
    {
        if (singleUse)
        {

        } else if(!onCooldown)
        {
            spawnEffect();
            onCooldown = true;
            StartCoroutine(cooldown(() => { onCooldown = false; }, cdTime));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && other.gameObject.name != "cornnonBullet")
        {

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && other.gameObject.name != "cornnonBullet")
        {

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
