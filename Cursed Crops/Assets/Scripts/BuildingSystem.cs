using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingSystem : MonoBehaviour
{
    // Private variables
    [SerializeField] private PlaceableSO activePlaceable;
    private PlayerControler pc;
    private GameObject PlaceableHighlight;
    private bool buildmodeActive = false;

    // Public variables
    public float gridSize = 2;
    public float gridOffsetX = 0.5f;
    public float gridOffsetZ = 0.5f;

    private void Awake()
    {
        PlaceableHighlight = this.transform.Find("PlaceableHighlight").gameObject;
        pc = this.GetComponent<PlayerControler>();
    }


    private void Update()
    {
        if (buildmodeActive)
        {
            alignToGrid(PlaceableHighlight.transform);
        }
    }

    // Enter build Mode
    public void BuildMode_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (buildmodeActive)
            {
                buildmodeActive = false;
                if (PlaceableHighlight != null)
                {
                    PlaceableHighlight.SetActive(false);
                }
            }
            else
            {
                buildmodeActive = true;
                if (PlaceableHighlight != null)
                {
                    PlaceableHighlight.SetActive(true);

                    // Set the higlight to match the prefab
                    PlaceableHighlight.transform.localScale = activePlaceable.prefab.transform.localScale;
                }
            }
        }
    }

    // Rotate Placeable 90 degrees
    public void Rotate_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (buildmodeActive)
            {
                if (PlaceableHighlight != null)
                {
                    Quaternion rotationAmmount = Quaternion.Euler(0, 90, 0);
                    PlaceableHighlight.transform.rotation *= rotationAmmount;
                }
            }
        }
    }

    // Instantiate Placeeable
    public void Place_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (buildmodeActive)
            {
                if (PlaceableHighlight != null)
                {
                    Instantiate(activePlaceable.prefab, PlaceableHighlight.transform.position, PlaceableHighlight.transform.rotation);
                }
            }
        }
    }

    private void alignToGrid(Transform trans)
    {
        // Get player position
        Vector3 playerPos = new Vector3(transform.position.x, 0, transform.position.z);

        // align it to grid
        float xPos = Mathf.Round(playerPos.x);
        xPos -= (xPos % gridSize);
        float zPos = Mathf.Round(playerPos.z);
        zPos -= (zPos % gridSize);

        // Add offset
        xPos += gridOffsetX;
        zPos += gridOffsetZ;

        // Add to the X so its in front of the player
        if (pc.flipped)
            xPos -= (gridSize * 2);
        if(!pc.flipped)
            xPos += (gridSize);

        // Create and return position
        Vector3 placePos = new Vector3(xPos, 0, zPos);

        trans.position = placePos;
    }

    /*public void Build_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Instantiate
            Instantiate(activePlaceable.prefab, alignToGrid(), transform.rotation);
        }
    }*/

    /*private Vector3 alignToGrid()
    {
        // Get player position
        Vector3 playerPos = new Vector3(transform.position.x, 0, transform.position.z);

        //Debug.Log("Player pos: " + transform.position.x + " " + transform.position.z);

        // align it to grid
        float xPos = Mathf.Round(transform.position.x);
        xPos -= (xPos % gridSize);
        float zPos = Mathf.Round(transform.position.z);
        zPos -= (zPos % gridSize);

        //Debug.Log("Rounded pos: " + xPos + " " + zPos);

        // Create and return position
        Vector3 placePos = new Vector3(xPos, 0, zPos);
        return placePos;

    }*/
}
