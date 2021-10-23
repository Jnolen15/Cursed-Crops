using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public bool makeCameraStatic = false;
    public float camHeight = 10f;
    public float camOffset = 10f;
    public float camAngel = 0f;
    public float followSpeed = 1f;
    public float minDistToCenter = 4f;
    public float maxCameraDistMultiply = 2f; //min multply will always be 1x

    public List<Transform> playerTransformsList;

    void Start()
    {
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(new Vector3(camAngel, 0, 0)));
    }

    private void FixedUpdate()
    {
        if (playerTransformsList.Count > 0 && !makeCameraStatic)
        {
            // Store ther Cam's current position
            Vector3 currCamPos = transform.position;

            // Calculate where Cam should move to
            Vector3 sum = new Vector3(0, 0, 0);
            foreach (Transform pT in playerTransformsList)
            {
                sum += pT.position;
            }
            sum /= playerTransformsList.Count;

            float distToCenter = Vector3.Distance(playerTransformsList[0].position, sum);
            float distPercentage = distToCenter / minDistToCenter;

            print("dist to center: " + distToCenter);

            Vector3 currCamTarget;
            if (distPercentage <= 1)
            {
                currCamTarget = sum - new Vector3(0, 0, camOffset);
                currCamTarget = new Vector3(currCamTarget.x, camHeight, currCamTarget.z);
            }
            else
            {
                float distMultiply = 1 + ( (distPercentage-1)*maxCameraDistMultiply );

                currCamTarget = sum - new Vector3(0, 0, camOffset * distMultiply);
                currCamTarget = new Vector3(currCamTarget.x, camHeight * distMultiply, currCamTarget.z);
            }

            // Slerp Cam to the decided location
            transform.position = Vector3.Slerp(currCamPos, currCamTarget, followSpeed * Time.fixedDeltaTime);
        }
    }
}

