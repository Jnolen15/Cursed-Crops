using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildChecker : MonoBehaviour
{
    public bool acceptablePos;
    public bool intersectingBuildable;
    public string mode;
    public BoxCollider boxCol;
    private void Start()
    {
        boxCol = GetComponent<BoxCollider>();
    }

    private void OnDisable()
    {
        intersectingBuildable = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (mode == "Build")
        {
            if (other.gameObject.tag == "Buildable")
            {
                intersectingBuildable = true;
            }
            else if (other.gameObject.tag == "TilePlantable")
                acceptablePos = false;
            else if (other.gameObject.tag == "TileBuildable")
                acceptablePos = true;
            else if (other.gameObject.tag == "TileUnplaceable")
                acceptablePos = false;
        } else if (mode == "Plant")
        {
            if (other.gameObject.tag == "Spawner")
            {
                intersectingBuildable = true;
            }
            else if (other.gameObject.tag == "TilePlantable")
                acceptablePos = true;
            else if (other.gameObject.tag == "TileBuildable")
                acceptablePos = false;
            else if (other.gameObject.tag == "TileUnplaceable")
                acceptablePos = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (mode == "Build")
        {
            if (other.gameObject.tag == "Buildable")
            {
                intersectingBuildable = false;
            }
        } else if (mode == "Plant")
        {
            if (other.gameObject.tag == "Spawner")
            {
                intersectingBuildable = false;
            }
        }
    }
}
