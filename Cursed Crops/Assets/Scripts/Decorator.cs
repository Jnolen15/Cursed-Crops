using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decorator : MonoBehaviour
{
    public int NumDecor;

    public GameObject checker;
    public float xbounds = 18f;
    public float ybounds = 18f;
    public float gridSize = 1;
    public float gridOffsetX = 0.5f;
    public float gridOffsetZ = 0.5f;

    private void Start()
    {
        SpawnDecor();
    }

    public void SpawnDecor()
    {
        StartCoroutine(PlaceDecor());
    }

    IEnumerator PlaceDecor()
    {
        for (int i = 0; i < NumDecor; i++)
        {
            // Pick a random pos
            checker.transform.position = new Vector3(Random.Range(-xbounds / 2, xbounds / 2), 0, Random.Range(-ybounds / 2, ybounds / 2));
            alignToGrid(checker.transform);

            // Wait for placer to check its tile
            yield return new WaitForFixedUpdate();

            //Debug.Log("On Tile: " + checker.GetComponent<GridPlacementChecker>().tileType + " at " + checker.transform.position);

            // Place decor
            Vector3 pos = new Vector3(checker.transform.position.x, 0.5f, checker.transform.position.z);
            float rand = Random.Range(1, 10);
            string deco = "Decor/Dirt1";
            switch (checker.GetComponent<GridPlacementChecker>().tileType)
            {
                case "TilePlantable":
                    rand = Random.Range(1, 10);
                    if (rand < 6) deco = "Decor/Dirt1";
                    else deco = "Decor/Dirt2";
                    Instantiate(Resources.Load<GameObject>(deco), pos, transform.rotation, transform);
                    break;
                case "TileBuildable":
                    rand = Random.Range(1, 10);
                    if (rand < 3) deco = "Decor/Grass1";
                    else if (rand > 3 && rand < 6) deco = "Decor/Grass2";
                    else deco = "Decor/Grass3";
                    Instantiate(Resources.Load<GameObject>(deco), pos, transform.rotation, transform);
                    break;
                case "TileUnplaceable":
                    Instantiate(Resources.Load<GameObject>("Decor/Rock1"), pos, transform.rotation, transform);
                    break;
                case "TileObjective":
                    Instantiate(Resources.Load<GameObject>("Decor/Rock1"), pos, transform.rotation, transform);
                    break;
            }
        }

        checker.SetActive(false);
    }

    private void alignToGrid(Transform trans)
    {
        // Get player position
        Vector3 selectedPos = trans.position;

        // align it to grid
        float xPos = Mathf.Round(selectedPos.x);
        xPos -= (xPos % gridSize);
        float zPos = Mathf.Round(selectedPos.z);
        zPos -= (zPos % gridSize);

        // Add offset
        xPos += gridOffsetX;
        zPos += gridOffsetZ;

        // Create and return position
        Vector3 placePos = new Vector3(xPos, selectedPos.y, zPos);

        trans.position = placePos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(xbounds, 1, ybounds));
    }
}
