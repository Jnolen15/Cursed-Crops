using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public enum DropType
    {
        none,
        red,
        purple
    };

    public SpriteRenderer itemSpriteRenderer;
    public Sprite redCropSprite;
    public Sprite purpleCropSprite;

    private DropType itemDropType = DropType.none;

    public DropType GetItemDropType() { return itemDropType; }

    public void SetDropType(DropType type)
    {
        itemDropType = type;

        if (type == DropType.red)
        {
            itemSpriteRenderer.sprite = redCropSprite;
        }
        else if (type == DropType.purple) 
        {
            itemSpriteRenderer.sprite = purpleCropSprite;
        }
        
    }

    public void GetPickedUp()
    {
        Destroy(gameObject);
    }
}