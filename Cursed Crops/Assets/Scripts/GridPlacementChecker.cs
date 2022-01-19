using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlacementChecker : MonoBehaviour
{
    public bool acceptablePos = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TilePlantable")
        {
            acceptablePos = true;
            Debug.Log("Spawned on TilePlantable");
        }
        else if (other.gameObject.tag == "TileBuildable")
        {
            acceptablePos = false;
            Debug.Log("Spawned on TileBuildable");
            //Destroy(this.gameObject);
        }
        else if (other.gameObject.tag == "TileUnplaceable")
        {
            acceptablePos = false;
            Debug.Log("Spawned on TileUnplaceable");
            //Destroy(this.gameObject);
        }
    }
}
