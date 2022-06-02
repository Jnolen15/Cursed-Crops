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
    public float followSpeed = 5f;
    public float zoomSpeed = 0.1f;
    // offset b/c sprite x is calculated at bottom left of sprites
    public float spriteWidthOffest = 0f;
    public float xBuffer = 0f;
    public float yBuffer = 0f;

    // The camera position showing complete level
    public Vector3 finalCamPos;
    public bool hasEnded = false;

    private PlayerManager PM;
    private float playerHeight = 1;

    // variables revealing inner worrkings of the camera, can be set to private or removed later
    private float FovVertical;
    private float FovHorizontal;
    private float pmiX;
    private float pmaX;
    private float pmiY;
    private float pmaY;
    private float camAngMiX;
    private float camAngMaX;
    private float camAngMiY;
    private float camAngMaY;
    private float zoomBufferV = 4.5f;
    private bool onScreen = true;

    // objects for making the edge of the camera capture area
    public GameObject cube1;
    public GameObject cube2;
    public GameObject cube3;
    public GameObject cube4;

    void Start()
    {
        PM = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(new Vector3(camAngle,0,0)));
    }

    private void FixedUpdate()
    {
        // Camera moves to show all players
        if (!hasEnded)
        {
            CameraAdjust();
        } else
        {
            LostLevel();
        }
    }

    public void CameraAdjust()
    {
        // Store ther Cam's current position
        Vector3 currCamPos = transform.position;
        Vector3 currCamTarget = currCamPos;
        float targetX = 0;
        float targetZ = 0;


        // set target to the average of all existing player's positions

        if (PM.players.Count > 0)
        {
            float minX = 0;
            float minZ = 0;
            float maxX = 0;
            float maxZ = 0;

            // Calculating camera X/Z
            foreach (GameObject player in PM.players)
            {
                Transform t = player.transform;
                targetX += t.position.x;
                targetZ += t.position.z;
                if (t.position.x < minX || minX == 0) minX = t.position.x;
                if (t.position.z < minZ || minZ == 0) minZ = t.position.z;
                if (t.position.x > maxX || maxX == 0) maxX = t.position.x;
                if (t.position.z > maxZ || maxZ == 0) maxZ = t.position.z;
            }
            targetX /= PM.players.Count;
            targetZ /= PM.players.Count;


            // Calculating camera Y
            // weird shit beyond this point, figuring out how to check if players are on Screen
            Vector3 cameraPos = transform.position;
            Vector3 forward = transform.forward;

            // fov for x and y
            FovVertical = Camera.main.fieldOfView;
            FovHorizontal = Camera.VerticalToHorizontalFieldOfView(Camera.main.fieldOfView, Camera.main.aspect);
            // calculating the min/max to not zoom out the screen
            float maxCamAngleX = (FovHorizontal / 2);
            float minCamAngleX = -(FovHorizontal / 2);
            float maxCamAngleZ = (FovVertical / 2);
            float minCamAngleZ = -(FovVertical / 2);

            //
            Vector3 tempCamTarget = new Vector3(targetX, camHeight, targetZ);


            Vector3 minPlayerPosX = new Vector3(minX + spriteWidthOffest - xBuffer, playerHeight, targetZ);
            Vector3 maxPlayerPosX = new Vector3(maxX + spriteWidthOffest + xBuffer, playerHeight, targetZ);
            Vector3 minPlayerPosZ = new Vector3(targetX, playerHeight, minZ - yBuffer);
            Vector3 maxPlayerPosZ = new Vector3(targetX, playerHeight, maxZ + yBuffer);

            // cubes visualize boundries 
            cube1.transform.SetPositionAndRotation(minPlayerPosX, cube1.transform.rotation);
            cube2.transform.SetPositionAndRotation(maxPlayerPosX, cube2.transform.rotation);
            cube3.transform.SetPositionAndRotation(minPlayerPosZ, cube3.transform.rotation);
            cube4.transform.SetPositionAndRotation(maxPlayerPosZ, cube4.transform.rotation);

            // calculating the angles of the players relative to the camera
            // min/max X are swapped, because otherwise min X is larger than max X
            float minPlayerX = Vector3.SignedAngle(maxPlayerPosX - cameraPos, forward, Vector3.up);
            float maxPlayerX = Vector3.SignedAngle(minPlayerPosX - cameraPos, forward, Vector3.up);
            float minPlayerZ = Vector3.SignedAngle(minPlayerPosZ - cameraPos, forward, Vector3.right);
            float maxPlayerZ = Vector3.SignedAngle(maxPlayerPosZ - cameraPos, forward, Vector3.right);


            pmiX = minPlayerX;
            pmaX = maxPlayerX;
            pmiY = minPlayerZ;
            pmaY = maxPlayerZ;
            camAngMiX = minCamAngleX;
            camAngMaX = maxCamAngleX;
            camAngMiY = minCamAngleZ;
            camAngMaY = maxCamAngleZ;

            // if a player is out of frame, zoom out; else zoom in until at starting height
            if (PM.players.Count > 1 && minPlayerX < minCamAngleX || maxPlayerX > maxCamAngleX || minPlayerZ < minCamAngleZ || maxPlayerZ > maxCamAngleZ)
            {
                onScreen = false;
                camHeight += zoomSpeed;
                camOffsetZ -= zoomSpeed;
            }
            else
            {
                onScreen = true;
                // variable for ensuring camera doesn't stutter when zooming in/out
                float zoomBuffer = zoomBufferV;
                if (camHeight > 13 && camOffsetZ < -8 && (minPlayerZ - zoomBuffer > minCamAngleZ && maxPlayerZ + zoomBuffer < maxCamAngleZ &&
                    minPlayerX - zoomBuffer > minCamAngleX && maxPlayerX + zoomBuffer < maxCamAngleX))
                {
                    camHeight -= zoomSpeed;
                    camOffsetZ += zoomSpeed;
                }
            }

            // Setting camera pos to 
            currCamTarget = new Vector3(targetX + camOffsetX, camHeight, targetZ + camOffsetZ);
        }

        /* Calculate where Cam should move to
        Vector3 currCamTarget = playerTrans.position - new Vector3(0, 0, camOffset);
        currCamTarget = new Vector3(currCamTarget.x, camHeight, currCamTarget.z);
        */

        // Slerp Cam to the decided location
        transform.position = Vector3.Slerp(currCamPos, currCamTarget, followSpeed * Time.fixedDeltaTime);
    }

    public void LostLevel()
    {
        // Store ther Cam's current position
        Vector3 currCamPos = transform.position;

        // Slerp Cam to the decided location
        transform.position = Vector3.Slerp(currCamPos, finalCamPos, 0.5f * Time.fixedDeltaTime);
    }
}
