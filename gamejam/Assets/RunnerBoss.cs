using UnityEngine;

public class RunnerBoss : MonoBehaviour
{
    public enum State { Patrol, Charge, Stunned }
    public State currentState;

    [Header("Attack")]
public int damage = 1;
public float hitCooldown = 0.4f; 
private float hitTimer;

    [Header("Movement")]
    public float patrolSpeed = 2f;
    public float chargeSpeed = 7f;
    private float currentSpeed;

    [Header("Checks (Transforms)")]
    public Transform wallCheck;
    public Transform playerCheck;

    [Header("Check Distances")]
    public float wallCheckDistance = 0.4f;
    public float playerCheckDistance = 6f;

    [Header("Layers")]
    public LayerMask groundLayer;
    public LayerMask playerLayer;

    [Header("Stun")]
    public float stunDuration = 2f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip chargeClip;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    private int facingDirection = 1; // 1 = right, -1 = left
    private bool directionLocked;
    private bool hasPlayedChargeSound;
    private float stunTimer;

    private string currentAnim;
    private bool ignoreWallThisFrame;
    public float wallSkin = 0.02f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        currentState = State.Patrol;
    }

    void Update()
    {
        if (hitTimer > 0f)
    hitTimer -= Time.deltaTime;

        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                DetectPlayer();
                break;

            case State.Charge:
                Charge();
                break;

            case State.Stunned:
                Stunned();
                break;
                
        }
    }

    void OnTriggerEnter2D(Collider2D other)
{
    if (hitTimer > 0f) return;

    if (!other.CompareTag("Player")) return;

    // Optional: only damage while charging
    if (currentState != State.Charge) return;

    Player playerScript = other.GetComponent<Player>();
    if (playerScript == null) return;

    // Deal damage
    playerScript.TakeDamage(damage);
    playerScript.KBCounter = playerScript.KBTotalTime;

    // Knockback direction logic (matches your BirdAttack example)
    if (other.transform.position.x <= transform.position.x)
    {
        playerScript.KnockFromRight = true;
    }
    else
    {
        playerScript.KnockFromRight = false;
    }

    hitTimer = hitCooldown;
}

    void FixedUpdate()
{
    if (currentState == State.Stunned)
    {
        rb.velocity = new Vector2(0f, rb.velocity.y);
        return;
    }

    Vector2 velocity = rb.velocity;

    // Force horizontal movement ONLY
    velocity.x = facingDirection * currentSpeed;

    rb.velocity = velocity;
}


    // ================= STATES =================

    void Patrol()
    {
        currentSpeed = patrolSpeed;
        directionLocked = false;

        // Optional walk animation – remove if you don’t have one
        PlayAnim("Runner");

        if (DetectWall())
        {
            Flip();
        }
    }

    void Charge()
{
    currentSpeed = chargeSpeed;
    PlayAnim("RunnerCharge");

    if (ignoreWallThisFrame)
    {
        ignoreWallThisFrame = false;
        return;
    }

    if (DetectWall())
    {
        EnterStun();
    }
}

    void Stunned()
    {
        PlayAnim("Idle");

        stunTimer -= Time.deltaTime;
        if (stunTimer <= 0f)
        {
            currentState = State.Patrol;
            hasPlayedChargeSound = false;
            directionLocked = false;
        }
    }

    // ================= DETECTION =================

bool DetectWall()
{
    RaycastHit2D hit = Physics2D.Raycast(
        wallCheck.position,
        Vector2.right * facingDirection,
        wallCheckDistance + wallSkin,
        groundLayer
    );

    return hit.collider != null;
}

    void DetectPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            playerCheck.position,
            Vector2.right * facingDirection,
            playerCheckDistance,
            playerLayer
        );

        if (hit.collider != null)
        {
            EnterCharge();
        }
    }

    // ================= TRANSITIONS =================

    void EnterCharge()
{
    if (currentState != State.Patrol) return;

    currentState = State.Charge;
    directionLocked = true;
    currentSpeed = chargeSpeed; // 🔑 IMPORTANT

    // Force movement immediately
    rb.velocity = new Vector2(facingDirection * chargeSpeed, rb.velocity.y);
    rb.WakeUp();
    ignoreWallThisFrame = true;

    if (!hasPlayedChargeSound)
    {
        audioSource.PlayOneShot(chargeClip);
        hasPlayedChargeSound = true;
    }
}

    void EnterStun()
    {
        if (currentState == State.Stunned) return;

        currentState = State.Stunned;
        stunTimer = stunDuration;
    }

    // ================= UTIL =================

    void Flip()
    {
        if (directionLocked) return;

        facingDirection *= -1;
        sr.flipX = facingDirection == -1;

        // Flip check points
        wallCheck.localPosition = new Vector3(
            Mathf.Abs(wallCheck.localPosition.x) * facingDirection,
            wallCheck.localPosition.y,
            wallCheck.localPosition.z
        );

        playerCheck.localPosition = new Vector3(
            Mathf.Abs(playerCheck.localPosition.x) * facingDirection,
            playerCheck.localPosition.y,
            playerCheck.localPosition.z
        );
    }

    // ================= ANIMATION =================

    void PlayAnim(string animName)
    {
        if (anim == null) return;
        if (currentAnim == animName) return;

        currentAnim = animName;
        anim.Play(animName);
    }

    // ================= DEBUG =================

    void OnDrawGizmosSelected()
    {
        if (wallCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(
                wallCheck.position,
                wallCheck.position + Vector3.right * facingDirection * wallCheckDistance
            );
        }

        if (playerCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(
                playerCheck.position,
                playerCheck.position + Vector3.right * facingDirection * playerCheckDistance
            );
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(
            transform.position,
            transform.position + Vector3.right * facingDirection
        );
        
    }
}
