using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeControl : MonoBehaviour
{
    public int damage = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {

            EnemyControler enemyControler = other.GetComponent<EnemyControler>();
            enemyControler.takeDamage(damage);
        }
    }
}
