using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{
    public int health = 10;
    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
    }

    public void takeDamage(int dmg)
    {
        // Subtract from health
        health -= dmg;

        // If health is below or equal to 0 die
        if (health <= 0)
        {
            death();
        } else
        {
            // In not dead flash red to show hit
            StartCoroutine(hit(rend));
        }
    }

    IEnumerator hit(Renderer renderer)
    {
        renderer.material.SetColor("_Color", Color.red);
        yield return new WaitForSeconds(0.2f);
        renderer.material.SetColor("_Color", Color.white);
    }

    private void death()
    {
        Destroy(gameObject);
    }
}
