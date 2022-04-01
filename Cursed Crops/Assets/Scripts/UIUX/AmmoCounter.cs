using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCounter : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider Bullet1;
    public Slider Bullet2;
    public Slider Bullet3;

    // takes in the number of bullets (x), and the recharge % of the next bullet
    public void SetBullets(int x, float r)
    {

        // using a switch statement cause I'm too lazy to figure out a better way
        switch(x)
        {
            case 0:
                Bullet1.value = r;
                Bullet2.value = 0;
                Bullet3.value = 0;
                break;

            case 1:
                Bullet1.value = 1;
                Bullet2.value = r;
                Bullet3.value = 0;
                break;

            case 2:
                Bullet1.value = 1;
                Bullet2.value = 1;
                Bullet3.value = r;
                break;

            case 3:
                Bullet1.value = 1;
                Bullet2.value = 1;
                Bullet3.value = 1;
                break;
        }
    }
}
