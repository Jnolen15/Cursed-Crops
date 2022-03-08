using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackBoxDamage : MonoBehaviour
{
    // Start is called before the first frame update
    public int playerdamage = 1;
    public int houseDamage = 5;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            StartCoroutine(other.gameObject.GetComponent<EnemyPlayerDamage>().iframes(playerdamage));
        }
        else if (other.gameObject.tag == "MainObjective")
        {
            other.gameObject.GetComponent<EnemyDamageObjective>().takeDamage(houseDamage);
        }
    }
}
