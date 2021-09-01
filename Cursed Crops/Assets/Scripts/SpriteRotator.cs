using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpriteRotator : MonoBehaviour
{
    public float xPivot = 43f;
    public bool manageSortLayer = false;

    private SpriteRenderer sRend;
    private Transform parentTrans;

    void Start()
    {
        sRend = GetComponent<SpriteRenderer>();
        parentTrans = GetComponentInParent<Transform>();
        
        // rotate around the x pivot
        // transform.Rotate(new Vector3(xPivot, 0, 0));
    }

    void Update()
    {
        // lean by the xPivot angle
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(new Vector3(xPivot, 0, 0)));

        // manage sort layer
        if (manageSortLayer)
        {
            sRend.sortingOrder = -(int)parentTrans.position.z;
        }
    }
}
