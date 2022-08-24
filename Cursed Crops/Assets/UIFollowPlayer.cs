using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowPlayer : MonoBehaviour
{
    public Transform lookat;
    public Vector3 offset;

    public Camera cam;

    void Start()
    {
        cam = Camera.main;
        lookat = transform.parent.parent.parent;
    }

    void Update()
    {
        if (cam != null)
        {
            Vector3 pos = cam.WorldToScreenPoint(lookat.position + offset);

            if (transform.position != pos)
                transform.position = pos;
        }

    }
}
