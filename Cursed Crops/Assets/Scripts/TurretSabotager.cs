using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSabotager : MonoBehaviour
{
    public GameObject theSabotager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<SaboAI>() && other.gameObject.tag == "Enemy")
        {
            theSabotager = other.gameObject;
            var sAI = theSabotager.GetComponent<SaboAI>();
            sAI.StartCoroutine(sAI.Sabotage());
            if (gameObject.transform.parent.gameObject.GetComponent<Trap>() == null)
            {
                gameObject.transform.parent.gameObject.GetComponent<Turret>().sabotaged = true;
            }
            if (gameObject.transform.parent.gameObject.GetComponent<Turret>() == null)
            {
                gameObject.transform.parent.gameObject.GetComponent<Trap>().sabotaged = true;
            }
        }
    }
}
