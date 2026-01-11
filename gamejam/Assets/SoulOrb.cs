using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class SoulOrb : MonoBehaviour
{
    [Header("Idle Float")]
    public float verticalSpeed = 0.3f;
    public bool moveUp = true;
    public float bobAmplitude = 0.15f;
    public float bobFrequency = 1.5f;
    public float windStrength = 0.25f;
    public float idleSmoothness = 2f;

    [Header("Attraction")]
    public float attractionStrength = 6f;
    public float maxAttractionSpeed = 7f;
    public float attractionSmoothness = 6f;

    [Header("Absorption")]
    public float absorptionDistance = 0.25f;
    public float absorptionSmoothness = 12f;

    private Transform player;
    private Vector2 velocity;
    private Vector2 windDirection;
    private bool attracted;
    private bool absorbing;

    void Start()
    {
        CircleCollider2D col = GetComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = 1.5f;

        windDirection = Random.insideUnitCircle.normalized;
    }

    void Update()
    {
        if (absorbing && player != null)
        {
            Absorb();
        }
        else if (attracted && player != null)
        {
            AttractToPlayer();
        }
        else
        {
            IdleFloat();
        }
    }

    void IdleFloat()
    {
        float verticalDir = moveUp ? 1f : -1f;
        Vector2 idleVelocity = Vector2.up * verticalSpeed * verticalDir;

        float bob = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
        idleVelocity += Vector2.up * bob;

        windDirection = Vector2.Lerp(
            windDirection,
            Random.insideUnitCircle.normalized,
            Time.deltaTime * 0.3f
        );

        idleVelocity += windDirection * windStrength;

        velocity = Vector2.Lerp(
            velocity,
            idleVelocity,
            Time.deltaTime * idleSmoothness
        );

        transform.position += (Vector3)(velocity * Time.deltaTime);
    }

    void AttractToPlayer()
    {
        Vector2 toPlayer = (Vector2)player.position - (Vector2)transform.position;
        float distance = toPlayer.magnitude;

        if (distance <= absorptionDistance)
        {
            absorbing = true;
            return;
        }

        Vector2 targetVelocity = toPlayer.normalized *
                                 attractionStrength *
                                 Mathf.Clamp01(1f / distance) *
                                 maxAttractionSpeed;

        velocity = Vector2.Lerp(
            velocity,
            targetVelocity,
            Time.deltaTime * attractionSmoothness
        );

        transform.position += (Vector3)(velocity * Time.deltaTime);
    }

    void Absorb()
    {
        Vector2 toPlayer = (Vector2)player.position - (Vector2)transform.position;

        velocity = Vector2.Lerp(
            velocity,
            toPlayer * absorptionSmoothness,
            Time.deltaTime * absorptionSmoothness
        );

        transform.position += (Vector3)(velocity * Time.deltaTime);

        if (toPlayer.magnitude <= 0.05f)
        {
            // ===== PLAYER ABSORPTION HOOK =====
            // Call this from your player script later
            // Example:
            // player.GetComponent<PlayerSouls>().AddSouls(1);

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            attracted = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            attracted = false;
            player = null;
        }
    }
}
