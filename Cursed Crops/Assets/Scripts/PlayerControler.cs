using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    // FLOATS / INTS ===========
    [SerializeField] private float attackCDTimer = 1;
    [SerializeField] private float attackDuration = 0.2f;
    [SerializeField] private float coolDownTimer;
    [SerializeField] private float rollSpeedMax = 100;
    private float rollSpeed;
    private int attackChain = 1;
    public int overAllPlayerDamage = 0;

    public float moveSpeed;

    // BOOLS ===========
    private bool flipped = false;
    private bool attackCD = false;
    private bool rangeCD = false;

    public bool useControler;               // If using controller changes aiming

    // STRINGS ===========
    // Strings are used for input mapping. This is done to make the creation of multiple players clean and simple.
    //[SerializeField] private string HorizontalMove = "Horizontal_P1";
    //[SerializeField] private string VerticalMove = "Vertical_P1";
    //[SerializeField] private string HorizontalAimController = "CHorizontal_P1";
    //[SerializeField] private string VerticalAimController = "CVertical_P1";
    //[SerializeField] private string AttackButton = "Attack_P1";
    //[SerializeField] private string RangedButton = "Ranged_P1";
    //[SerializeField] private string RollButton = "Roll_P1";

    // OTHER COMPONENTS ===========
    //private PlayerInputActions playerInputActions;  // The player input object script
    private Vector2 aimInputVector = Vector2.zero;
    private Vector2 moveInputVector = Vector2.zero;
    private Rigidbody rb;                  // The player's Rigidbody
    private SpriteRenderer playerSprite;
    private Animator animator;
    private GameObject meleeAttackLeft;
    private GameObject meleeAttackRight;
    private Vector3 movement;
    private Vector3 rollDir;
    [SerializeField] private LayerMask layermask;
    private delegate void Callback();
    private enum State
    {
        Normal,
        Rolling,
    }
    private State state;

    public GameObject bullet;


    // Start is called before the first frame update
    void Start()
    {
        state = State.Normal;

        rb = GetComponent<Rigidbody>();

        // Get the left facing attack hitbox and set it inactive
        meleeAttackLeft = this.transform.GetChild(0).gameObject;
        // Get the Right facing attack hitbox and set it inactive
        meleeAttackRight = this.transform.GetChild(1).gameObject;
        /* Note ^
         * Previously I just had one hitbox and would flip the
         * objects X scale when the player flipped directions.
         * However I got warnings that flipping the object
         * could cause the collider to behave not as intended.
         * So instead I just have one attack for each side that
         * are switched based on duirection faced.
         * Then I just flip the players sprite on the spriterender.
         */

        // Player Sprite
        playerSprite = this.transform.GetChild(2).GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        animator = this.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Animator>();
    }

    private void Awake()
    {
        /*// Get access to the player input actions asset
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        // Assign each action to a function
        playerInputActions.Player.Attack.performed += Attack_performed;
        playerInputActions.Player.Ranged.performed += Ranged_performed;
        playerInputActions.Player.Roll.performed += Roll_performed;
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Normal) // Not in rolling state
        {
            if (!useControler) FaceMouse();
            else FaceController();
        }

        if (coolDownTimer < attackCDTimer) coolDownTimer += Time.deltaTime; // Attack cooldown

        if (coolDownTimer > attackDuration) animator.SetBool("Melee", false); // Reset attack animation
    }

    // Called every fixed frame-rate frame. Better for physics stuff
    private void FixedUpdate()
    {
        switch (state)
        {
            case State.Normal:
                //if(coolDownTimer > attackDuration) Move(); //Do it this way if movement should be locked while attacking
                Move();
                rollDir = movement;
                break;
            case State.Rolling:
                DodgeRoll();
                float rollSpeedDropMultiplier = 3f;
                rollSpeed -= rollSpeed * rollSpeedDropMultiplier * Time.deltaTime;
                if (rollSpeed < moveSpeed) state = State.Normal;
                break;
        }
    }

    // Aiming ========================================================

    private void FaceMouse() // Aiming with the mouse
    {
        // Cast a ray from the camera to the ground plane where the mouse is.
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layermask))
        {
            // Get the mouse position relative to the player
            Vector3 mousePosition = raycastHit.point;
            Vector3 direction = new Vector3(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

            // Flip sprite to face the mouse position
            if (direction.x > 0 && !flipped)
            {
                flipped = true;
                playerSprite.flipX = true;
            }
            else if (direction.x < 0 && flipped)
            {
                flipped = false;
                playerSprite.flipX = false;
            }
        }
    }

    public void Aim_performed(InputAction.CallbackContext context)
    {
        //Debug.Log(context.action.ToString());
        if (context.action.ToString() == "Player/Aim[/Mouse/position]")
        {
            useControler = false;
        }
        else
        {
            useControler = true;
            aimInputVector = context.ReadValue<Vector2>();
        }
    }

    private void FaceController()
    {
        //Vector2 inputVector = playerInputActions.Player.Aim.ReadValue<Vector2>();
        Vector3 direction = Vector3.right * aimInputVector.x + Vector3.up * -aimInputVector.y;

        // Flip sprite to face the mouse position
        if (direction.x > 0 && !flipped)
        {
            flipped = true;
            playerSprite.flipX = true;
        }
        else if (direction.x < 0 && flipped)
        {
            flipped = false;
            playerSprite.flipX = false;
        }
    }

    // Move =================================
    public void Move_performed(InputAction.CallbackContext context)
    {
        moveInputVector = context.ReadValue<Vector2>();
    }

    private void Move() // Movement code, Uses Vector3, input from Horazontal and Vertical Axis, and the RB to move
    {
        //Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        movement = new Vector3(moveInputVector.x, 0, moveInputVector.y).normalized;
        rb.MovePosition(transform.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void Roll_performed(InputAction.CallbackContext context)
    {
        //Debug.Log(context);
        if (context.performed)
        {
            state = State.Rolling;
            rollSpeed = rollSpeedMax;
        }
    }

    private void DodgeRoll()
    {
        rb.MovePosition(transform.position + rollDir * rollSpeed * Time.fixedDeltaTime);
    }

    // Attack =================================
    public void Attack_performed(InputAction.CallbackContext context)
    {
        //Debug.Log(context);
        if (context.performed)
        {
            if (!attackCD && state == State.Normal)
            {
                if (coolDownTimer > attackCDTimer) // If attack isnt on cooldown
                {
                    coolDownTimer = 0;
                    attackChain = 1;
                    animator.SetBool("Melee", true);
                    DoAttack();
                    Debug.Log("Attack 1");

                }
                else if (coolDownTimer > attackDuration) // If attack is on cooldown, subsiquent atacks chain
                {
                    if (attackChain == 1)
                    {
                        coolDownTimer = 0;
                        attackChain = 2;
                        animator.SetBool("Melee", true);
                        DoAttack();
                        Debug.Log("Attack 2");
                    }
                    else if (attackChain == 2)
                    {
                        coolDownTimer = 0;
                        attackChain = 3;
                        animator.SetBool("Melee", true);
                        DoAttack();
                        Debug.Log("Attack 3");
                    }
                    else if (attackChain == 3)
                    {
                        Debug.Log("Attack chain over");
                    }
                }
            }
        }
    }

    private void DoAttack()
    {
        if (!flipped) // Left attack hit detection
        {
            Collider[] cols = Physics.OverlapBox(meleeAttackLeft.transform.position, meleeAttackLeft.transform.localScale / 2, 
                                                    meleeAttackLeft.transform.rotation, LayerMask.GetMask("Enemies"));
            DamageEnemies(cols);
        }
        else  // Right attack hit detection
        {
            Collider[] cols = Physics.OverlapBox(meleeAttackRight.transform.position, meleeAttackRight.transform.localScale / 2,
                                                            meleeAttackRight.transform.rotation, LayerMask.GetMask("Enemies"));
            DamageEnemies(cols);
        }
    }

    private void DamageEnemies(Collider[] cols)
    {
        foreach (Collider c in cols)
        {
            //Debug.Log(c.name);
            EnemyControler enemyControler = c.GetComponent<EnemyControler>();

            if (c.gameObject.tag == "Enemy")
            {
                if (attackChain == 1)
                {
                    enemyControler.takeDamage(2);
                    overAllPlayerDamage += 2;
                }
                else if (attackChain == 2)
                {
                    enemyControler.takeDamage(3);
                    overAllPlayerDamage += 3;
                }
                else if (attackChain == 3)
                {
                    enemyControler.takeDamage(6);
                    overAllPlayerDamage += 6;
                }
            }
        }
    }

    public void Ranged_performed(InputAction.CallbackContext context)
    {
        //Debug.Log(context);
        if (context.performed)
        {
            if (!rangeCD && state == State.Normal)
            {
                // Set cooldown true
                rangeCD = true;
                //Play animation
                animator.SetBool("Ranged", true);
                StartCoroutine(cooldown(() => { animator.SetBool("Ranged", false); }, 0.2f));
                // Create bullet
                GameObject bul = Instantiate(bullet, transform.position, transform.rotation);
                // Send bullet in correct direction
                if (flipped) bul.GetComponent<Bullet>().movement = new Vector3(1, 0, 0);
                else bul.GetComponent<Bullet>().movement = new Vector3(-1, 0, 0);
                // Start ranged attack cooldown
                StartCoroutine(cooldown(() => { rangeCD = false; }, 4f));

            }
            else if (rangeCD && state == State.Normal) // If on cooldown send dubug msg
            {
                Debug.Log("Range on Cooldown");
            }
        }
    }

    // Misc =================================

    IEnumerator cooldown(Callback callback, float time)
    {
        yield return new WaitForSeconds(time);
        if (callback != null)
        {
            callback();
        }
    }
}
