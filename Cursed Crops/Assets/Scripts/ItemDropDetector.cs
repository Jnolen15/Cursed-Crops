using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropDetector : MonoBehaviour
{
    private PlayerResourceManager resources;

    private void Start()
    {
        resources = GetComponent<PlayerResourceManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DroppedItem")
        {
            ItemDrop itemDrop = other.GetComponent<ItemDrop>();
            resources.addCrops(itemDrop.value);
            itemDrop.GetPickedUp();
        }
    }
}
