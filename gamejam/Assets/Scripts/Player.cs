using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float runSpeed = 8f;
    public Rigidbody2D rb;
    public Vector2 moveInput;
    public bool isRunning;

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

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    private void Awake()
    {
        jumpsRemaining = maxJumps;
        currentStamina = maxStamina;
    }

    private void FixedUpdate()
    {
        // Movement
        float currentSpeed = (isRunning && canRun) ? runSpeed : speed;
        rb.velocity = new Vector2(moveInput.x * currentSpeed, rb.velocity.y);

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
        // Ground check
        bool isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        if (isGrounded)
            jumpsRemaining = maxJumps;

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

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (jumpsRemaining <= 0)
            return;

        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpsRemaining--;
    }
}
