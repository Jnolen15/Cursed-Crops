using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    public float moveSpeed;

    private Rigidbody rb;                  // The player's Rigidbody
    private GameObject meleeAttack;

    private bool flipped = false;

    [SerializeField] private LayerMask layermask;


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
        FaceMouse();
    }

    // Called every fixed frame-rate frame. Better for physics stuff
    private void FixedUpdate()
    {
        Move();
    }

    // Aiming ========================================================

    private void FaceMouse() // Aiming with the mouse
    {
        // Cast a ray from the camera to the ground plane where the mouse is.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layermask))
        {
            // Get the mouse position relative to the player
            Vector3 mousePosition = raycastHit.point;
            Vector3 direction = new Vector3(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

            // Flip sprite to face the mouse position
            if (direction.x > 0 && !flipped)
            {
                flipped = true;
                transform.localScale += new Vector3(-2, 0, 0); // Sprite is facing right
            }
            else if (direction.x < 0 && flipped)
            {
                flipped = false;
                transform.localScale += new Vector3(2, 0, 0); // Sprite is facing Left
            }
        }
    }

    // Move =================================
    private void Move() // Movement code, Uses Vector3, input from Horazontal and Vertical Axis, and the RB to move
    {
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        rb.MovePosition(transform.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // Attack =================================
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
