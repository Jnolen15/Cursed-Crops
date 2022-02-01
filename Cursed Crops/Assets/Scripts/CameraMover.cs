using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public Transform playerTrans;
    public float camHeight = 10f;
    public float camOffsetX = 0f;
    public float camOffsetZ = 0f;
    public float camAngle = 45f;
    public float followSpeed = 1f;

    private PlayerManager PM;

    void Start()
    {
        PM = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(new Vector3(camAngle,0,0)));
    }

    private void FixedUpdate()
    {
        // Store ther Cam's current position
        Vector3 currCamPos = transform.position;
        Vector3 currCamTarget = currCamPos;
        float targetX = 0;
        float targetZ = 0;

        // set target to the average of all existing player's positions
        
        if (PM.players.Count > 0)
        {
            foreach (GameObject player in PM.players)
            {
                targetX += player.GetComponent<Transform>().position.x;
                targetZ += player.GetComponent<Transform>().position.z;
            }
            targetX /= PM.players.Count;
            targetZ /= PM.players.Count;
            currCamTarget = new Vector3(targetX + camOffsetX, camHeight, targetZ + camOffsetZ);
        }

        

        /* Calculate where Cam should move to
        Vector3 currCamTarget = playerTrans.position - new Vector3(0, 0, camOffset);
        currCamTarget = new Vector3(currCamTarget.x, camHeight, currCamTarget.z);
        */

        // Slerp Cam to the decided location
        transform.position = Vector3.Slerp(currCamPos, currCamTarget, followSpeed * Time.fixedDeltaTime);
    }
}
