using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlacementChecker : MonoBehaviour
{
    public bool acceptablePos;
    public bool colChecked;
    public string tileType;

    private void OnTriggerEnter(Collider other)
    {
        colChecked = true;
        if (other.gameObject.tag == "TilePlantable")
        {
            tileType = "TilePlantable";
            acceptablePos = true;
            //Debug.Log("Spawned on TilePlantable");
        }
        else if (other.gameObject.tag == "TileBuildable")
        {
            tileType = "TileBuildable";
            acceptablePos = false;
            //Debug.Log("Spawned on TileBuildable");
            //Destroy(this.gameObject);
        }
        else if (other.gameObject.tag == "TileUnplaceable")
        {
            tileType = "TileUnplaceable";
            acceptablePos = false;
            //Debug.Log("Spawned on TileUnplaceable");
            //Destroy(this.gameObject);
        }
        else if (other.gameObject.tag == "TileObjective")
        {
            tileType = "TileObjective";
            acceptablePos = false;
            //Debug.Log("Spawned on TileUnplaceable");
            //Destroy(this.gameObject);
        }
    }

    public void AddSelf(Dictionary<Vector3, string> dict)
    {
        if (colChecked)
        {
            if (acceptablePos)
            {
                dict.Add(transform.position, "Empty");
                gameObject.SetActive(false);
            }
            else if (!acceptablePos)
            {
                //Debug.Log("Unacceptable position, deleting");
                Destroy(gameObject);
            }
        } else
        {
            Debug.LogError("AddSelf called before colisions were checked.");
        }

    }
}
