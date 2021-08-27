using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    public float moveSpeed;

    private Rigidbody rb;                  // The player's Rigidbody
    private GameObject meleeAttack;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        meleeAttack = this.transform.GetChild(0).gameObject;
        meleeAttack.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    private void FixedUpdate()
    {
        Move();
    }

    // Move
    private void Move() // Movement code, Uses Vector3, input from Horazontal and Vertical Axis, and the RB to move
    {
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        rb.MovePosition(transform.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // Attack
    private void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(doAttack());
        }
    }

    IEnumerator doAttack()
    {
        meleeAttack.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        meleeAttack.SetActive(false);
    }
}
