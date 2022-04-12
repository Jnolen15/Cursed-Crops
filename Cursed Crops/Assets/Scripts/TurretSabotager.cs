using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSabotager : MonoBehaviour
{
    public GameObject theSabotager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<SaboAI>() && other.gameObject.tag == "Enemy")
        {
            theSabotager = other.gameObject;
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
