using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSabotager : MonoBehaviour
{
    public GameObject theSabotager;
    public bool isSabotaged = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<SaboAI>() && other.gameObject.tag == "Enemy" && !isSabotaged)
        {
            theSabotager = other.gameObject;
            var sAI = theSabotager.GetComponent<SaboAI>();
            if (sAI.closestTurret == this.transform.parent)
            {
                sAI.StartCoroutine(sAI.Sabotage());
                if (gameObject.transform.parent.gameObject.GetComponent<Trap>() == null && gameObject.transform.parent.gameObject.GetComponent<EnemyDamageObjective>() == null)
                {
                    isSabotaged = gameObject.transform.parent.gameObject.GetComponent<Turret>().Sabotage();
                }
                else if (gameObject.transform.parent.gameObject.GetComponent<Turret>() == null && gameObject.transform.parent.gameObject.GetComponent<EnemyDamageObjective>() == null)
                {
                    isSabotaged = gameObject.transform.parent.gameObject.GetComponent<Trap>().Sabotage();
                }
                else if(gameObject.transform.parent.gameObject.GetComponent<Turret>() == null && gameObject.transform.parent.gameObject.GetComponent<Trap>() == null)
                {
                    isSabotaged = gameObject.transform.parent.gameObject.GetComponent<EnemyDamageObjective>().Sabotage();
                }
            }
        }
    }
}
