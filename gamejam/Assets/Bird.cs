using UnityEngine;

public class Bird : MonoBehaviour
{
    [Header("References")]
    public Animator anim;
    public AudioSource audioSource;
    public AudioClip clip;

    [Header("Animator")]
    public string attackBoolName = "IsAttacking";

    private int playersInRange = 0;

    void Start()
    {
        Debug.Log("[Bird] Started (2D). Forcing idle.");
        anim.SetBool(attackBoolName, false);
    }

    // ================= PLAYER DETECTION (2D) =================
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("[Bird] Trigger ENTER by: " + other.name);

        if (!other.CompareTag("Player")) return;

        playersInRange++;
        Debug.Log("[Bird] Player ENTER range. Count: " + playersInRange);

        anim.SetBool(attackBoolName, true);
        Debug.Log("[Bird] IsAttacking = TRUE");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("[Bird] Trigger EXIT by: " + other.name);

        if (!other.CompareTag("Player")) return;

        playersInRange--;
        Debug.Log("[Bird] Player EXIT range. Count: " + playersInRange);

        if (playersInRange <= 0)
        {
            playersInRange = 0;
            anim.SetBool(attackBoolName, false);
            Debug.Log("[Bird] IsAttacking = FALSE");
        }
    }

    // ================= AUDIO =================
    // Call this from an Animation Event
    public void PlaySound()
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
            Debug.Log("[Bird] Attack sound played");
        }
        else
        {
            Debug.LogWarning("[Bird] AudioSource or Clip missing!");
        }
    }
}
