using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public int value = 1;
    public float timeAlive = 0.0f;

    public bool hasBeenMerged = false;

    public SpriteRenderer itemSpriteRenderer;
    public Sprite stage1CropSprite;
    public Sprite stage2CropSprite;
    public Sprite stage3CropSprite;
    public Sprite stage4CropSprite;
    public Sprite stage5CropSprite;

    private void Update()
    {
        timeAlive += Time.deltaTime;

        if (value == 1)
        {
            itemSpriteRenderer.sprite = stage1CropSprite;
        } else if (value > 1 && value < 10)
        {
            itemSpriteRenderer.sprite = stage2CropSprite;
        } else if (value > 9 && value < 25)
        {
            itemSpriteRenderer.sprite = stage3CropSprite;
        } else if (value > 24 && value < 50)
        {
            itemSpriteRenderer.sprite = stage4CropSprite;
        } else if (value > 49)
        {
            itemSpriteRenderer.sprite = stage5CropSprite;
        }
    }

    public void GetPickedUp()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "MergeRadius")
        {
            var itemDropOther = other.gameObject.GetComponentInParent<ItemDrop>();
            if (!hasBeenMerged && itemDropOther.timeAlive > timeAlive)
            {
                hasBeenMerged = true;
                itemDropOther.value += value;
                //Debug.Log("THIS VEGGIE HAS BEEN ASSIMILATED. NEW VALUE: " + itemDropOther.value);
                Destroy(this.gameObject);
            }
        }
    }
}
