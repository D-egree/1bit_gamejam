using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements.Experimental;
using System;
using UnityEditor;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public PlayerState currentState;
    public PlayerIdleState idleState;
    public PlayerJumpState jumpState;
    public PlayerRunState runState;
    public SpriteRenderer spriteRenderer;





    public PlayerAttackState attackState;
    public Animator anim;
    [Header("Health")]
    public int health;
    public int maxHealth = 6;
    [Header("Movement")]
    public float speed = 5f;
    public float runSpeed = 8f;
    public Rigidbody2D rb;
    public Vector2 moveInput;
    public bool isRunning;
    public bool jumpPressed;

        [Header ("Attack Settings")]
    public int damage;
    public float attackRadius = .5f;
    public Transform attackPoint;
    public LayerMask enemyLayer;

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float staminaDrainRate = 20f;   // per second
    public float staminaRegenRate = 15f;    // per second
    private float currentStamina;
    private bool canRun = true;

    [Header("Jump")]
    public float jumpForce = 7f;
    public int maxJumps = 2;
    private int jumpsRemaining;
    public float KBForce;
    public float KBCounter;
    public float KBTotalTime;

    public bool KnockFromRight;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
    
    public bool attackPressed;

    public bool IsGrounded { get; private set; }
    public bool IsMoving => Mathf.Abs(moveInput.x) > 0.01f;



    private void Awake()
    {
        health = maxHealth;
        jumpsRemaining = maxJumps;
        currentStamina = maxStamina;
        //attackState = new PlayerAttackState(this);
        idleState = new PlayerIdleState(this);
        jumpState = new PlayerJumpState(this);
        runState = new PlayerRunState(this);
    }

    private void FixedUpdate()
    {
        if (KBCounter <= 0)
        {
            // Movement
            float currentSpeed = (isRunning && canRun) ? runSpeed : speed;
            rb.velocity = new Vector2(moveInput.x * currentSpeed, rb.velocity.y);
        }
        else
        { 
        if(KnockFromRight == true)
            {
                rb.velocity = new Vector2(-KBForce, KBForce);
            }
            if (KnockFromRight == false)
            {
                rb.velocity = new Vector2(KBForce, KBForce);
            }
            KBCounter -= Time.deltaTime;
        }

        // Drain stamina while running
        if (isRunning && canRun)
        {
            currentStamina -= staminaDrainRate * Time.fixedDeltaTime;

            if (currentStamina <= 0f)
            {
                currentStamina = 0f;
                canRun = false;
                isRunning = false; // force stop running
            }
        }
    }

 private void Update()
{
    IsGrounded = Physics2D.OverlapCircle(
        groundCheck.position,
        groundCheckRadius,
        groundLayer
    );

    if (IsMoving)
{
    if (moveInput.x > 0)
        spriteRenderer.flipX = true;
    else if (moveInput.x < 0)
        spriteRenderer.flipX = false;
}

    if (IsGrounded)
        jumpsRemaining = maxJumps;
    currentState?.Update();


        // Passive stamina regen
        if (!isRunning && currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;

            if (currentStamina >= maxStamina * 0.2f)
            {
                canRun = true; // allow running again after some regen
            }

            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (!canRun)
            return;

        isRunning = context.ReadValueAsButton();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
     //   attackPressed = value.isPressed;
        Collider2D enemy = Physics2D.OverlapCircle(attackPoint.position, attackRadius, enemyLayer);
        if (enemy != null)
        {
        enemy.gameObject.GetComponent<Health>().ChangeHealth(-damage);
        Debug.Log("Attacked");
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (jumpsRemaining <= 0)
            return;

        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpsRemaining--;
        jumpPressed = true;
        anim.SetBool ("IsJumping", true);
    }
    public void Start()
    {
        ChangeState(idleState);
    }
   public void ChangeState(PlayerState newState)
    {
        if (currentState !=null)
        currentState.Exit();

        currentState = newState;
        currentState.Enter();
    }
    public void TakeDamage(int amount)
    {
          StartCoroutine(FlashWhite());
        if (amount != 0) GetComponent<AudioSource>().Play();
        health -= amount;
      
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
        }
    }
    private System.Collections.IEnumerator FlashWhite()
{
        spriteRenderer.enabled = false;
    yield return null;
    yield return null;
    yield return null;
    yield return null;
        yield return null;
    yield return null;
    yield return null;
    yield return null;
        yield return null;
    yield return null;
    yield return null;
    yield return null;
    spriteRenderer.enabled = true;
        yield return null;
    yield return null;
    yield return null;
    yield return null;
        yield return null;
    yield return null;
    yield return null;
    yield return null;
        yield return null;
    yield return null;
    yield return null;
    yield return null;
            spriteRenderer.enabled = false;
    yield return null;
    yield return null;
    yield return null;
    yield return null;
        yield return null;
    yield return null;
    yield return null;
    yield return null;
        yield return null;
    yield return null;
    yield return null;
    yield return null;
    spriteRenderer.enabled = true;
}
}
