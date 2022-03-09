using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCounter : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Bullet1;
    public GameObject Bullet2;
    public GameObject Bullet3;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void SetBullets(int x)
    {
        if (x > 0)
        {
            Bullet1.SetActive(true);
        }
        else
        {
            Bullet1.SetActive(false);
        }

        if (x > 1)
        {
            Bullet2.SetActive(true);
        }
        else
        {
            Bullet2.SetActive(false);
        }

        if (x > 2)
        {
            Bullet3.SetActive(true);
        }
        else
        {
            Bullet3.SetActive(false);
        }
    }
}
