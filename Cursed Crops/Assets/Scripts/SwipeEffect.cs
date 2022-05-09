using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeEffect : MonoBehaviour
{
    public void AnimateSwipe(bool flip)
    {
        this.GetComponent<SpriteRenderer>().flipX = flip;
        Vector3 temp = Vector3.zero;
        if (flip)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x - 2, this.transform.localPosition.y + -0.5f, 0f);
            temp = new Vector3(this.transform.localPosition.x - 0.6f, 0, 0);
        }
        else
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x + 2, this.transform.localPosition.y + -0.5f, 0f);
            temp = new Vector3(this.transform.localPosition.x + 0.6f, 0, 0);
        }    

        StopCoroutine("swipeAnim");
        StartCoroutine(Swipe(this.transform.localPosition, temp));
    }

    IEnumerator Swipe(Vector3 startPos, Vector3 endPos)
    {
        // Show swipe, then slowly lerp it forward and fade it out
        float time = 0;
        Color temp = Color.white;
        while (time < 0.2f)
        {
            this.transform.localPosition = Vector3.Lerp(startPos, endPos, time / 0.2f);
            temp.a = Mathf.Lerp(1, 0, time / 0.2f);
            this.GetComponent<SpriteRenderer>().color = temp;
            time += Time.deltaTime;
            yield return null;
        }
        Destroy(this.gameObject);
        this.transform.localPosition = startPos;
    }
}
