using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    // ================= Public variables =================
    public string appliedEffect;
    public float effectduration = 3f;
    public float aliveTime = 1f;
    public int damageAmmount = 1;
    public bool targetPlayer = false;
    

    // ================= Private variables =================

    /* NOTE:
     * Effects are created by traps
     * I'm trying to build this script to be useable for all
     * 'trap' type buildables. That way we have 1 versitile
     * script instead of a single script for each trap.
     * Especially since many traps will have similar code.
     */

    // NOTE 2: RN it just works for the healing radio. But make it more versitile in future when needed

    void Start()
    {
        Destroy(this.gameObject, aliveTime);
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && !targetPlayer)
        {
            // Effects: Burning, Healing
            //Debug.Log("Hit: " + other.gameObject.name);
            other.GetComponent<EnemyControler>().ApplyEffect(appliedEffect, effectduration);
        } else if (other.gameObject.tag == "Player" && targetPlayer)
        {
            if (!other.gameObject.GetComponent<EnemyPlayerDamage>().damageBuffed)
            {
                Debug.Log("Applied Buff");
                other.gameObject.GetComponent<EnemyPlayerDamage>().Heal(damageAmmount);
                other.gameObject.GetComponent<EnemyPlayerDamage>().ApplyEffect(appliedEffect, effectduration);
            }
        }
    }
}
