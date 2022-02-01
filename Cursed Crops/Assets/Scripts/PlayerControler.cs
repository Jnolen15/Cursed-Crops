using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    // FLOATS / INTS ===========
    [SerializeField] private float attackCDTimer = 1.5f;
    [SerializeField] private float rollSpeedMax = 24;
    [SerializeField] private float rollSpeedFallofDelay = 0.3f;
    [SerializeField] private float rollCDTime = 0.3f;
    [SerializeField] private float rangeCDTime = 0.4f;
    [SerializeField] private float faceAimTimer = 2f;
    public int attackChain = 1;
    private float attackcoolDown;
    private float attackDuration;
    private float rollSpeed;
    private float rollSpeedDropMultiplier;
    private float faceAimTime = 0;
    private float lungeSpeed = 8;

    public int overAllPlayerDamage = 0;
    public float moveSpeed;
    public float originalSpeed;
    public int money = 0;
    public int health = 10;
    public int maxHealth = 10;

    // BOOLS ===========
    private bool rangeCD = false;
    private bool rollCD = false;
    public bool faceaim = false;
    public bool trapped = false;
    private bool startRollFallOff = false;
    private bool attackMove = false;
    private bool attackCancleable = false;
    public bool attackQueued = false;

    public bool flipped = false;
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
    private BuildingSystem bs;
    private EnemyPlayerDamage epd;
    private Rigidbody rb;                  // The player's Rigidbody
    private CapsuleCollider cc;
    private SpriteRenderer playerSprite;
    private Animator animator;
    private GameObject meleeAttack;
    public GameObject bullet;
    private Coroutine ap;               // Used to store and stop attack Coroutine
    private Vector2 aimInputVector = Vector2.zero;
    private Vector2 moveInputVector = Vector2.zero;
    private Vector3 movement;
    private Vector3 rollDir;
    private Vector3 direction;
    [SerializeField] private LayerMask groundLayermask;
    private delegate void Callback();
    public enum State
    {
        Normal,
        Attacking,
        Building,
        Rolling,
        Downed,
    }
    public State state;


    // reference to pause menu
    private Pause_Manager pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Normal;

        bs = GetComponent<BuildingSystem>();
        epd = GetComponent<EnemyPlayerDamage>();
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
        originalSpeed = moveSpeed;
        meleeAttack = this.transform.GetChild(0).gameObject;

        // Player Sprite
        playerSprite = this.transform.GetChild(1).GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        animator = this.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Animator>();

        // UI stuff for pausing 
        // hello - keenan
        pauseMenu = GameObject.Find("UI Canvas").GetComponentInChildren<Pause_Manager>();
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
        // Face mouse / conroller when not rolling
        if (state != State.Rolling)
        {
            if (!useControler) FaceMouse();
            else FaceController();
        }

        // Enter build state
        if (bs.buildmodeActive)
            state = State.Building;

        // Enter downed state if stunned
        if(epd.playerIsStun)
            state = State.Downed;
        else if (!epd.playerIsStun && state == State.Downed)
        {
            animator.SetBool("Downed", false);
            state = State.Normal;
        }

        // movement is stopped if player is trapped
        if (trapped)
            moveSpeed = 0;
        else
            moveSpeed = originalSpeed;

        // Slight move forward during attack
        if (attackMove)
            AutoMove(lungeSpeed);

        // Cooldown stuffs
        if (faceAimTime < faceAimTimer) faceAimTime += Time.deltaTime; // Face aim period
        else faceaim = false;

        if (attackcoolDown < attackCDTimer) attackcoolDown += Time.deltaTime; // Attack cooldown
        else attackChain = 0;

        //if (attackcoolDown > attackDuration) animator.SetInteger("MeleeStage", 0); // Reset attack animation

            // Move into the SpriteLeaner script! Placeholder for now
        playerSprite.sortingOrder = -(int)this.transform.position.z;
    }

    // Called every fixed frame-rate frame. Better for physics stuff
    private void FixedUpdate()
    {
        switch (state)
        {
            case State.Normal:
                // Can't move while attacking
                if (!animator.GetBool("Ranged") && attackcoolDown > attackDuration)
                {
                    Move();
                }
                rollDir = movement;
                break;
            case State.Building:
                // Exit building state if out of build mode
                if (!bs.buildmodeActive)
                    state = State.Normal;
                // Can't move while attacking
                if (!animator.GetBool("Ranged"))
                {
                    if (attackcoolDown > attackDuration)
                        Move();
                }
                rollDir = movement;
                break;
            /*case State.Attacking:
                movement = new Vector3(moveInputVector.x, 0, moveInputVector.y).normalized;
                rollDir = movement;
                break;*/
            case State.Rolling:
                cc.enabled = false;
                DodgeRoll();
                //animator.SetBool("Dodging", true);
                // Roll speed is constant at the start, then falls off untill just below move speed
                if (!startRollFallOff)
                {
                    startRollFallOff = true;
                    StartCoroutine(cooldown(() => { rollSpeedDropMultiplier = 4f; }, rollSpeedFallofDelay));
                }
                
                if (rollSpeedDropMultiplier > 0)
                {
                    rollSpeed -= rollSpeed * rollSpeedDropMultiplier * Time.deltaTime;
                    if (rollSpeed <= 3.5f)
                    {
                        cc.enabled = true;
                        state = State.Normal;
                        //animator.SetBool("Dodging", false);
                        startRollFallOff = false;
                        rollSpeedDropMultiplier = 0f;
                    }
                }
                break;
            case State.Downed:
                animator.SetBool("Downed", true);
                bs.CloseBuildMode();
                break;
        }
    }

    // Aiming ========================================================

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

    private void FaceMouse() // Aiming with the mouse
    {
        // Cast a ray from the camera to the ground plane where the mouse is.
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, groundLayermask))
        {
            // Get the mouse position relative to the player
            Vector3 mousePosition = raycastHit.point;
            direction = new Vector3(mousePosition.x - transform.position.x, 0, mousePosition.z - transform.position.z);

            // Flip sprite to face the mouse position
            if (faceaim)
            {
                if (direction.x > 0 && flipped)
                {
                    flipped = false;
                    playerSprite.flipX = false;
                    meleeAttack.transform.localPosition = new Vector3(-(meleeAttack.transform.localPosition.x), 0f, 0f);
                }
                else if (direction.x < 0 && !flipped)
                {
                    flipped = true;
                    playerSprite.flipX = true;
                    meleeAttack.transform.localPosition = new Vector3(-(meleeAttack.transform.localPosition.x), 0f, 0f);
                }
            } else
            {
                FaceMove();
            }
        }
    }

    private void FaceController()
    {
        //Vector2 inputVector = playerInputActions.Player.Aim.ReadValue<Vector2>();
        direction = Vector3.right * aimInputVector.x + Vector3.forward * aimInputVector.y;

        // Flip sprite to face the joystick position
        if (faceaim)
        {
            if (direction.x < 0 && !flipped)
            {
                flipped = true;
                playerSprite.flipX = true;
                meleeAttack.transform.localPosition = new Vector3(-(meleeAttack.transform.localPosition.x), 0f, 0f);
            }
            else if (direction.x > 0 && flipped)
            {
                flipped = false;
                playerSprite.flipX = false;
                meleeAttack.transform.localPosition = new Vector3(-(meleeAttack.transform.localPosition.x), 0f, 0f);
            }
        }
        else
        {
            FaceMove();
        }
    }

    private void FaceMove()
    {
        if (state != State.Downed)
        {
            // Flip sprite to face the direction the player is moving
            if (moveInputVector.x < -0.2 && !flipped)
            {
                flipped = true;
                playerSprite.flipX = true;
                meleeAttack.transform.localPosition = new Vector3(-(meleeAttack.transform.localPosition.x), 0f, 0f);
            }
            else if (moveInputVector.x > 0.2 && flipped)
            {
                flipped = false;
                playerSprite.flipX = false;
                meleeAttack.transform.localPosition = new Vector3(-(meleeAttack.transform.localPosition.x), 0f, 0f);
            }
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

        //run animation management
        animator.SetFloat("MovementMagnitude", movement.magnitude);

        if (flipped)
        {
            if(moveInputVector.x < 0)
                animator.SetFloat("Direction", 1);
            if (moveInputVector.x > 0)
                animator.SetFloat("Direction", -1);
        } else if (!flipped)
        {
            if (moveInputVector.x < 0)
                animator.SetFloat("Direction", -1);
            if (moveInputVector.x > 0)
                animator.SetFloat("Direction", 1);
        }

    }

    private void AutoMove(float speed) // Automatically move forward
    {
        /*if (flipped)
            movement = new Vector3(-1, 0, 0).normalized;
        else if (!flipped)
            movement = new Vector3(1, 0, 0).normalized;*/
        movement = new Vector3(moveInputVector.x, 0, moveInputVector.y).normalized;

        rb.MovePosition(transform.position + movement * speed * Time.fixedDeltaTime);
    }

    public void Roll_performed(InputAction.CallbackContext context)
    {
        //Debug.Log(context);
        if (context.performed && !trapped)
        {
            if (movement.magnitude == 1 && !rollCD && state == State.Normal) // Only roll if moving, not on CD, and in Normal state
            {
                state = State.Rolling;
                rollSpeed = rollSpeedMax;
                rollCD = true;
                animator.SetTrigger("Dodge");
                StartCoroutine(cooldown(() => { rollCD = false; }, rollCDTime));
            }
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
            AttackChain();
        }
    }

    private void AttackChain()
    {
        if (state == State.Normal || state == State.Attacking)
        {
            if (attackcoolDown > attackDuration /* || attackCancleable */)
            {
                switch (attackChain)
                {
                    case 0:
                        //Debug.Log("Attack 1");
                        attackcoolDown = 0;
                        //attackChain = 1;
                        attackDuration = 0.4f;
                        animator.SetTrigger("Melee1");
                        ap = StartCoroutine(attackPhase(0f, 0.2f, 0.2f, false));
                        break;
                    case 1:
                        //Debug.Log("Attack 2")
                        attackcoolDown = 0;
                        //attackChain = 2;
                        attackDuration = 0.4f;
                        animator.SetTrigger("Melee2");
                        if (ap != null)
                            StopCoroutine(ap);
                        ap = StartCoroutine(attackPhase(0f, 0.2f, 0.2f, false));
                        break;
                    case 2:
                        //Debug.Log("Attack 3");
                        attackcoolDown = 0;
                        //attackChain = 3;
                        attackDuration = 0.7f;
                        animator.SetTrigger("Melee3");
                        if (ap != null)
                            StopCoroutine(ap);
                        ap = StartCoroutine(attackPhase(0.3f, 0.1f, 0.3f, true)); // add to 0.7
                        break;
                    case 3:
                        attackChain = 0;
                        state = State.Normal;
                        break;
                }
            }
            else /*if (attackChain < 3)*/
            {
                attackQueued = true;
            }
        }
    }

    IEnumerator attackPhase(float windup, float strike, float recovery, bool lunge)
    {
        // Time the following to sync up with animation frames
        // IN FUTURE, WORK WITH ATTACK ANIMATORS: Make sure attacks across charecters are synced (Frame timing wise)
        // Windup:
        state = State.Attacking;
        attackCancleable = false;
        yield return new WaitForSeconds(windup);
        // Strike: Move if holding a direction
        if (lunge)
        {
            attackMove = true;
            StartCoroutine(cooldown(() => { attackMove = false; }, strike));
        }
        yield return new WaitForSeconds(strike);
        // Contact: activate hitbox check
        DoAttack();
        // Recovery: Cancleable only by another melee or ranged attack, a roll, or damage / downed
        attackCancleable = true;
        yield return new WaitForSeconds(recovery);
        attackCancleable = false;
        if (attackQueued)
        {
            attackQueued = false;
            AttackChain();
        } else
        {
            state = State.Normal;
        }
    }

    private void DoAttack()
    {
        Collider[] cols = Physics.OverlapBox(meleeAttack.transform.position, meleeAttack.transform.localScale / 2,
                                                    meleeAttack.transform.rotation, LayerMask.GetMask("Enemies"));
        if (cols.Length > 0) attackChain++;
        else attackChain = 0;
        DamageEnemies(cols);
    }

    private void DamageEnemies(Collider[] cols)
    {
        foreach (Collider c in cols)
        {
            EnemyControler enemyControler = c.GetComponent<EnemyControler>();

            if (c.gameObject.tag == "Enemy" || c.gameObject.name == "cornnonBullet(Clone)")
            {
                if (!trapped)
                {
                    if (attackChain == 1)
                    {
                        enemyControler.takeDamageMelee(4);
                        overAllPlayerDamage += 4;
                    }
                    else if (attackChain == 2)
                    {
                        enemyControler.takeDamageMelee(6);
                        overAllPlayerDamage += 6;
                    }
                    else if (attackChain == 3)
                    {
                        enemyControler.takeDamageMelee(10);
                        overAllPlayerDamage += 10;
                    }
                }
                else
                {
                    enemyControler.takeDamageMelee(1);
                    overAllPlayerDamage += 1;
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
                if(animator != null)
                {
                    animator.SetBool("Ranged", true);
                    StartCoroutine(cooldown(() => { animator.SetBool("Ranged", false); }, 0.4f));
                }
                if (direction != new Vector3(0,0,0))
                {
                    // Create bullet
                    GameObject bul = null;
                    StartCoroutine(cooldown(() => { bul = Instantiate(bullet, transform.position, transform.rotation);
                        bul.GetComponent<Bullet>().movement = direction.normalized; }, 0.15f));
                    // Send bullet in correct direction
                    //Debug.Log(direction);
                    //bul.GetComponent<Bullet>().movement = direction.normalized;
                }
                else
                {
                    Vector3 movDir = new Vector3(1, 0, 0);
                    // Flip sprite to face the direction the player is moving
                    if (!flipped)
                        movDir = new Vector3(1, 0, 0);
                    else if (flipped)
                        movDir = new Vector3(-1, 0, 0);

                    GameObject bul = null;
                    StartCoroutine(cooldown(() => { bul = Instantiate(bullet, transform.position, transform.rotation);
                        bul.GetComponent<Bullet>().movement = movDir.normalized; }, 0.15f));
                }
                // Start ranged attack cooldown
                StartCoroutine(cooldown(() => { rangeCD = false; }, rangeCDTime));
                // Set face aim period
                faceAimTime = 0;
                faceaim = true;
            }
            else if (rangeCD && state == State.Normal) // If on cooldown send dubug msg
            {
                //Debug.Log("Range on Cooldown");
            }
        }
    }

    // Misc =================================

    private void OnTriggerEnter(Collider other)
    {
        // if colliding with the farm house, bank items
        if (other.gameObject.tag == "MainObjective")
        {
            GetComponent<PlayerResourceManager>().BankItems();
        }
    }

    IEnumerator cooldown(Callback callback, float time)
    {
        yield return new WaitForSeconds(time);
        callback?.Invoke();
    }

    // method for UI pausing
    public void togglePause()
    {
        pauseMenu.TogglePause();
    }

}
