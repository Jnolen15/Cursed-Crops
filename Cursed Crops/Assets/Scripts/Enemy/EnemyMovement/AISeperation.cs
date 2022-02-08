using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISeperation : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject[] AI;
    public float SpaceBetween = 5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AI = GameObject.FindGameObjectsWithTag("Enemy");
       // Debug.Log(AI.Length);
        foreach(GameObject go in AI)
        {
            if(go != gameObject)
            {
                float distance = Vector3.Distance(go.transform.position, this.transform.position);
                if(distance <= SpaceBetween)
                {
                    Vector3 direction = transform.position - go.transform.position;
                    direction.y = 0;
                    transform.Translate(direction * Time.deltaTime);
                }
            }
        }
    }
}
