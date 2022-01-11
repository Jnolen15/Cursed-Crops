using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    public GameObject dropItemPrefab;
    public ItemDrop.DropType dropType = ItemDrop.DropType.red;
    public int numDrops = 3;
    public float randomOffsetX = 0f;
    public float randomOffsetZ = 0f;

    public void DropItem(Vector3 pos)
    {
        for (int i = 0; i < numDrops; i++)
        {
            float x = pos.x + Random.Range(-randomOffsetX, randomOffsetX);
            float z = pos.z + Random.Range(-randomOffsetZ, randomOffsetZ);
            float y = pos.y;

            Vector3 itemPos = new Vector3(x, y, z);
            ItemDrop droppedItem = Instantiate(dropItemPrefab, itemPos, Quaternion.identity).GetComponent<ItemDrop>();
            droppedItem.SetDropType(dropType);
        }
    }
}