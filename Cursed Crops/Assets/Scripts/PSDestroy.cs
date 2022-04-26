using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSDestroy : MonoBehaviour
{
    public float destroyTime = 1f;
    
    void Start()
    {
        Destroy(this.gameObject, destroyTime);
    }
}
