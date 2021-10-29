using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public Transform playerTrans;
    public float camHeight = 10f;
    public float camOffset = 10f;
    public float camAngel = 0f;
    public float followSpeed = 1f;

    void Start()
    {
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(new Vector3(camAngel,0,0)));
    }

    private void FixedUpdate()
    {
        /*
        // Store ther Cam's current position
        Vector3 currCamPos = transform.position;
        
        // Calculate where Cam should move to
        Vector3 currCamTarget = playerTrans.position - new Vector3(0, 0, camOffset);
        currCamTarget = new Vector3(currCamTarget.x, camHeight, currCamTarget.z);

        // Slerp Cam to the decided location
        transform.position = Vector3.Slerp(currCamPos, currCamTarget, followSpeed * Time.fixedDeltaTime);
        */
    }
}
