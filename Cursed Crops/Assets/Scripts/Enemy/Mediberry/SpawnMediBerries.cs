using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMediBerries : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mediberry;

    void Start()
    {
        for(int i = 0; i < 2; i++)
        {
            GameObject child = Instantiate(mediberry, transform.position, transform.rotation);
            child.transform.parent = gameObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
