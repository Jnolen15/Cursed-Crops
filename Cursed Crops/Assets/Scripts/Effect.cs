using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    // ================= Public variables =================
    public float aliveTime = 1f;
    public int damageAmmount = 1;

    // ================= Private variables =================

    /* NOTE:
     * Effects are created by traps
     * I'm trying to build this script to be useable for all
     * 'trap' type buildables. That way we have 1 versitile
     * script instead of a single script for each trap.
     * Especially since many traps will have similar code.
     */

    // NOTE 2: RN it just works for the healing radio. But make it more versitile in future when needed

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, aliveTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && other.gameObject.name != "cornnonBullet")
        {

        } else if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<EnemyPlayerDamage>().Heal(damageAmmount);
            other.gameObject.GetComponent<EnemyPlayerDamage>().ApplyEffect("DamageBoost", 6f);
        }
    }
}
