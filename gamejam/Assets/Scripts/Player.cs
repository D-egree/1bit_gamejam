using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public Vector2 moveInput;
    public Rigidbody2D rb;

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
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * speed, rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
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

 private void Update()
{
    bool isGrounded = Physics2D.OverlapCircle(
        groundCheck.position,
        groundCheckRadius,
        groundLayer
    );

    if (isGrounded)
    {
        jumpsRemaining = maxJumps;
    }
}}
