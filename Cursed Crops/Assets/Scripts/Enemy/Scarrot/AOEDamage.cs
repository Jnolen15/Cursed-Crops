using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEDamage : MonoBehaviour
{
    // Start is called before the first frame update
    public int damage = 5;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {

            other.gameObject.GetComponent<EnemyPlayerDamage>().Damage(damage);
        }
        else if (other.gameObject.tag == "MainObjective")
        {
            other.gameObject.GetComponent<EnemyDamageObjective>().takeDamage(damage);
        }
    }
}
