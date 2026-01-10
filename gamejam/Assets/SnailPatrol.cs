using UnityEngine;

public class SnailPatrol : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 1.5f;

    [Header("Colliders")]
    [Tooltip("Non-trigger collider used for ground, walls, ledges")]
    public CapsuleCollider2D bodyCollider;

    [Tooltip("Trigger collider used ONLY for player detection")]
    public Collider2D playerTrigger;

    [Header("Detection")]
    public float wallCheckDistance = 0.05f;
    public float ledgeCheckDistance = 0.3f;
    public float groundedCheckDistance = 0.05f;
    public LayerMask groundLayer;

    [Header("Player Damage")]
    public int damage = 1;
    public float damageCooldown = 0.5f;

    private Rigidbody2D rb;
    private int direction = -1;
    private float lastDamageTime;

    private RaycastHit2D[] hits = new RaycastHit2D[4];

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (bodyCollider == null)
            Debug.LogError("BodyCollider not assigned on SnailPatrol");

        if (playerTrigger == null)
            Debug.LogError("PlayerTrigger not assigned on SnailPatrol");
    }

    void FixedUpdate()
    {
        if (!IsGrounded())
            return;

        Move();

        if (WallAhead() || AtLedge())
        {
            Flip();
        }
    }

    // ================= MOVEMENT =================

    void Move()
    {
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
    }

    // ================= CHECKS =================

    bool IsGrounded()
    {
        return bodyCollider.Cast(Vector2.down, hits, groundedCheckDistance, true) > 0;
    }

    bool WallAhead()
    {
        int hitCount = bodyCollider.Cast(
            Vector2.right * direction,
            hits,
            wallCheckDistance,
            true
        );

        for (int i = 0; i < hitCount; i++)
        {
            if (((1 << hits[i].collider.gameObject.layer) & groundLayer) != 0)
                return true;
        }

        return false;
    }

    bool AtLedge()
    {
        Bounds b = bodyCollider.bounds;

        Vector2 origin = new Vector2(
            b.center.x + b.extents.x * direction,
            b.min.y + 0.02f
        );

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            Vector2.down,
            ledgeCheckDistance,
            groundLayer
        );

        return hit.collider == null;
    }

    // ================= FLIP =================

    void Flip()
    {
        direction *= -1;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // ================= PLAYER DAMAGE =================

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other != playerTrigger)
            TryDamagePlayer(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other != playerTrigger)
            TryDamagePlayer(other);
    }

    void TryDamagePlayer(Collider2D other)
    {
        if (Time.time - lastDamageTime < damageCooldown)
            return;

        Player player = other.GetComponent<Player>();
        if (player == null)
            return;

        lastDamageTime = Time.time;
        player.TakeDamage(damage);
    }

    // ================= GIZMOS =================

    void OnDrawGizmosSelected()
    {
        if (bodyCollider == null)
            return;

        Bounds b = bodyCollider.bounds;

        // Wall
        Gizmos.color = Color.red;
        Vector3 wallStart = b.center;
        wallStart.x += b.extents.x * direction;
        Gizmos.DrawLine(
            wallStart,
            wallStart + Vector3.right * direction * wallCheckDistance
        );

        // Ledge
        Gizmos.color = Color.blue;
        Vector3 ledgeStart = new Vector3(
            b.center.x + b.extents.x * direction,
            b.min.y + 0.02f,
            0
        );
        Gizmos.DrawLine(
            ledgeStart,
            ledgeStart + Vector3.down * ledgeCheckDistance
        );

        // Grounded
        Gizmos.color = Color.green;
        Gizmos.DrawLine(
            b.center,
            b.center + Vector3.down * groundedCheckDistance
        );
    }
}
