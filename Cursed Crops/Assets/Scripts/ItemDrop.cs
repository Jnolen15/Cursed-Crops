using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public int value = 1;
    public float destroyTime = 10f;
    private float unavailableTime = 0.5f;
    public float timeAlive = 0.0f;

    public bool hasBeenMerged = false;
    public bool canPickUp = false;

    public SpriteRenderer itemSpriteRenderer;
    public Sprite stage1CropSprite;
    public Sprite stage2CropSprite;
    public Sprite stage3CropSprite;
    public Sprite stage4CropSprite;
    public Sprite stage5CropSprite;

    private void Update()
    {
        timeAlive += Time.deltaTime;

        // Can't pick up for the first half second or so
        if (timeAlive > unavailableTime)
            canPickUp = true;

        // Pulse if close to being destroyed
        if (timeAlive >= (destroyTime * 0.7))
        {
            itemSpriteRenderer.color = new Color(1, 1, 1, Mathf.PingPong(Time.time, 0.5f));
        } else
        {
            itemSpriteRenderer.color = new Color(1, 1, 1, 1);
        }

        // Destroy item if its been alive longer than destroy time
        if (timeAlive > destroyTime)
            Destroy(this.gameObject);

        // Change sprite depending on value
        if (value == 1) // 1
        {
            itemSpriteRenderer.sprite = stage1CropSprite;
        } else if (value > 1 && value <= 4) // 2-4
        {
            itemSpriteRenderer.sprite = stage2CropSprite;
        } else if (value > 4 && value <= 15) // 5-15
        {
            itemSpriteRenderer.sprite = stage3CropSprite;
        } else if (value > 15 && value <= 30) // 16-30
        {
            itemSpriteRenderer.sprite = stage4CropSprite;
        } else if (value > 30) // 31 and above
        {
            itemSpriteRenderer.sprite = stage5CropSprite;
        }
    }

    public void ReduceValue(int val)
    {
        value -= val;
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
                itemDropOther.destroyTime += destroyTime;
                //Debug.Log("THIS VEGGIE HAS BEEN ASSIMILATED. NEW VALUE: " + itemDropOther.value);
                Destroy(this.gameObject);
            }
        }
    }
}
