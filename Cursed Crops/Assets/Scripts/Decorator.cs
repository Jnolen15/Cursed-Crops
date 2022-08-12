using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decorator : MonoBehaviour
{
    public int NumDecor;

    public GameObject checker;
    public float xbounds;
    public float ybounds;
    public float xboundsOffset;
    public float yboundsOffset;
    public float gridSize = 1;
    public float gridOffsetX = 0.5f;
    public float gridOffsetZ = 0.5f;
    public bool gridPlace = false;
    public float randDegree = 4;

    private float prevNumDecor;
    private float prevrandDegree;

    private void Start()
    {
        SpawnDecor();

        prevNumDecor = NumDecor;
        prevrandDegree = randDegree;
    }

    private void Update()
    {
        if(prevNumDecor != NumDecor || prevrandDegree != randDegree)
        {
            prevNumDecor = NumDecor;
            prevrandDegree = randDegree;

            foreach (Transform child in this.transform)
            {
                if(child.gameObject.name != "GridPlacementChecker") GameObject.Destroy(child.gameObject);
            }

            checker.SetActive(true);
            SpawnDecor();
        }
    }

    public void SpawnDecor()
    {
        if(!gridPlace) StartCoroutine(RandomPlaceDecor());
        else StartCoroutine(GridPlaceDecor());
    }

    IEnumerator RandomPlaceDecor()
    {
        for (int i = 0; i < NumDecor; i++)
        {
            // Pick a random pos
            var randX = (Random.Range(-xbounds / 2, xbounds / 2) + xboundsOffset);
            var randY = (Random.Range(-ybounds / 2, ybounds / 2) + yboundsOffset);
            checker.transform.position = new Vector3(randX, 0, randY);
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
                    rand = Random.Range(1, 11);
                    if (rand < 3) deco = "Decor/Grass1";
                    else if (rand > 3 && rand < 6) deco = "Decor/Grass2";
                    else if (rand > 6 && rand < 10) deco = "Decor/Grass3";
                    else deco = "Decor/Rock1";
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

    IEnumerator GridPlaceDecor()
    {
        var spread = (int)((xbounds * ybounds) / NumDecor);
        var count = 0;
        var xMin = ((-xbounds / 2) + xboundsOffset);
        var xMax = ((xbounds / 2) + xboundsOffset);
        var yMin = ((-ybounds / 2) + yboundsOffset);
        var yMax = ((ybounds / 2) + yboundsOffset);

        for (float i = xMin; i < xMax; i+=1)
        {
            count++;
            for (float j = yMin; j < yMax; j += 1)
            {
                count++;
                if(count % spread == 0)
                {
                    // Adjust with a little randomness
                    var randX = Random.Range(-randDegree, randDegree);
                    var randY = Random.Range(-randDegree, randDegree);
                    checker.transform.position = new Vector3(i+ randX, 0, j + randY);
                    alignToGrid(checker.transform);

                    // Wait for placer to check its tile
                    yield return new WaitForFixedUpdate();

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
                            rand = Random.Range(1, 11);
                            if (rand < 3) deco = "Decor/Grass1";
                            else if (rand > 3 && rand < 6) deco = "Decor/Grass2";
                            else if (rand > 6 && rand < 10) deco = "Decor/Grass3";
                            else deco = "Decor/Rock1";
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
        Vector3 drawPos = new Vector3(transform.position.x + xboundsOffset, transform.position.y, transform.position.z + yboundsOffset);
        Gizmos.DrawWireCube(drawPos, new Vector3(xbounds, 1, ybounds));
    }
}
