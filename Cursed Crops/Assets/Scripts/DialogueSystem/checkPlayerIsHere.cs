using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPlayerIsHere : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Broc;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            Broc.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
