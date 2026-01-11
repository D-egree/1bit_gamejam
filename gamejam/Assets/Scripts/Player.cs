using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class Player : MonoBehaviour
{
    public PlayerState currentState;
    public PlayerIdleState idleState;
    public PlayerJumpState jumpState;
    public PlayerRunState runState;
    public PlayerAttackState attackState;
    public SpriteRenderer spriteRenderer;
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

    [Header("Attack Settings")]
    public int damage;
    public float attackRadius = 0.5f;
    public Transform attackPoint;
    public LayerMask enemyLayer;
    private bool isAttacking;

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float staminaDrainRate = 20f;
    public float staminaRegenRate = 15f;
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

    public bool IsGrounded { get; private set; }
    public bool IsMoving => Mathf.Abs(moveInput.x) > 0.01f;

    private void Awake()
    {
        health = maxHealth;
        jumpsRemaining = maxJumps;
        currentStamina = maxStamina;

        idleState = new PlayerIdleState(this);
        jumpState = new PlayerJumpState(this);
        runState = new PlayerRunState(this);
    }

    private void Start()
    {
        ChangeState(idleState);
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
            if (KnockFromRight)
                rb.velocity = new Vector2(-KBForce, KBForce);
            else
                rb.velocity = new Vector2(KBForce, KBForce);

            KBCounter -= Time.fixedDeltaTime;
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
        IsGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (IsMoving)
        {
            spriteRenderer.flipX = moveInput.x > 0 ? true : false;
        }

        if (IsGrounded)
            jumpsRemaining = maxJumps;

        currentState?.Update();

        // Passive stamina regen
        if (!isRunning && currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            if (currentStamina >= maxStamina * 0.2f)
                canRun = true;

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
        if (!context.performed)
            return;

        if (isAttacking)
            return;

        StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        anim.SetBool("IsAttacking", true);

        // Wait until the "hit frame" (adjust to match animation)
        yield return new WaitForSeconds(0.2f);

        Collider2D enemy = Physics2D.OverlapCircle(attackPoint.position, attackRadius, enemyLayer);
        if (enemy != null)
        {
            enemy.GetComponent<Health>()?.ChangeHealth(-damage);
            Debug.Log("Attacked");
        }

        // Wait for the rest of the animation
        yield return new WaitForSeconds(0.3f);

        anim.SetBool("IsAttacking", false);
        isAttacking = false;
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
        anim.SetBool("IsJumping", true);
    }

    public void ChangeState(PlayerState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void TakeDamage(int amount)
    {
        StartCoroutine(FlashWhite());

        if (amount != 0)
            GetComponent<AudioSource>()?.Play();

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
