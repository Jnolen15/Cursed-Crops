using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAnimator : MonoBehaviour
{
    private float animTime = 2f;
    private SpriteRenderer sr;

    public GameObject enemyObj;

    public void AnimateSpawn(GameObject enemy, Vector3 pos)
    {
        sr = this.GetComponent<SpriteRenderer>();

        // Create enemy and set it to inactive
        enemyObj = Instantiate(enemy, pos, this.transform.rotation);
        enemyObj.SetActive(false);

        // Set this sprite and scale to match that of the enemy
        sr.sprite = enemy.GetComponentInChildren<SpriteRenderer>().sprite;
        this.transform.localScale = enemy.GetComponentInChildren<SpriteRenderer>().gameObject.transform.localScale;

        // Run animate corutine to lerp the sprite from underground to above
        Vector3 endpos = new Vector3(pos.x, pos.y - 1, pos.z);
        Vector3 startpos = new Vector3(endpos.x, endpos.y - 1, endpos.z);
        Instantiate(Resources.Load<GameObject>("Effects/EnemySpawnParticle"), endpos, transform.rotation);
        StartCoroutine(Animate(enemy, startpos, endpos));
    }

    IEnumerator Animate(GameObject enemy, Vector3 startPos, Vector3 endPos)
    {
        float time = 0;
        while (time < animTime)
        {
            this.transform.position = Vector3.Lerp(startPos, endPos, time / animTime);
            time += Time.deltaTime;
            yield return null;
        }
        this.transform.localPosition = endPos;
        enemyObj.SetActive(true);
        Destroy(this.gameObject);
    }
}
