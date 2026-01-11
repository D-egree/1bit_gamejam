using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Floaty : MonoBehaviour
{
    [Header("Movement")]
    public float sinkDistance = 0.5f;      // How far down it sinks
    public float sinkSpeed = 0.5f;         // Speed while being stood on
    public float returnSpeed = 1.0f;       // Speed when returning
    [SerializeField] Animator animator;

    [Header("Audio")]
    public AudioClip clip;

    [Header("Timing (seconds)")]
    public float minDelay = 15f;
    public float maxDelay = 30f;

    private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();

        source.playOnAwake = false;
        source.loop = false;
    }



    [Header("Timing")]
    public float returnDelay = 0.5f;        // Delay before returning after player leaves

    private Vector3 startPosition;
    private bool isBeingStoodOn;
    private float leaveTimer;

    void Start()
    {
        startPosition = transform.position;
        animator.SetBool("FloatyAnim", true);
        if (clip != null)
            StartCoroutine(PlayRoutine());
    }

    void Update()
    {
        if (isBeingStoodOn)
        {
            leaveTimer = 0f;

            Vector3 target = startPosition + Vector3.down * sinkDistance;
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                sinkSpeed * Time.deltaTime
            );
        }
        else
        {
            leaveTimer += Time.deltaTime;

            if (leaveTimer >= returnDelay)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    startPosition,
                    returnSpeed * Time.deltaTime
                );
            }
        }
    }
    void OnEnable()
    {
        if (clip != null)
            StartCoroutine(PlayRoutine());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && IsAbove(collision))
        {
            isBeingStoodOn = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isBeingStoodOn = false;
        }
    }

    bool IsAbove(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y < -0.5f)
                return true;
        }
        return false;
    }
    void OnDrawGizmosSelected()
{
    // Use current position in editor, runtime startPosition in play mode
    Vector3 basePos = Application.isPlaying ? startPosition : transform.position;
    Vector3 sinkPos = basePos + Vector3.down * sinkDistance;

    Gizmos.color = Color.cyan;

    // Draw sink path
    Gizmos.DrawLine(basePos, sinkPos);

    // Draw endpoint
    Gizmos.DrawWireSphere(sinkPos, 0.05f);
}

IEnumerator PlayRoutine()
    {
        while (true)
        {
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            source.PlayOneShot(clip);
        }
    }
}
