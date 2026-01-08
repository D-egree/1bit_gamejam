using UnityEngine;

public class Bird : MonoBehaviour
{
    public Player playerScript;
    [Header("References")]
    public Animator anim;
    public AudioSource audioSource;
    public AudioClip clip;

    [Header("Animator")]
    public string isAttackingBool = "IsAttacking";
    public string wantsToAttackBool = "WantsToAttack";

    private int playersInRange = 0;
    private bool waitingForAttackToFinish = false;

    void Start()
    {
        Debug.Log("[Bird] Started (2D). Forcing idle.");
        anim.SetBool(isAttackingBool, false);
        anim.SetBool(wantsToAttackBool, false);
    }

    // ================= PLAYER DETECTION =================
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playersInRange++;
        Debug.Log("[Bird] Player ENTER range. Count: " + playersInRange);

        anim.SetBool(wantsToAttackBool, true);
        anim.SetBool(isAttackingBool, true);
        waitingForAttackToFinish = false;
      
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playersInRange--;
        Debug.Log("[Bird] Player EXIT range. Count: " + playersInRange);

        if (playersInRange <= 0)
        {
            playersInRange = 0;
            anim.SetBool(wantsToAttackBool, false);
            waitingForAttackToFinish = true;
        }
    }

    void Update()
    {
        // If player left and we're waiting to finish the current attack
        if (!waitingForAttackToFinish) return;

        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

        // Are we currently in the attack state?
        if (!state.IsName("BirdStaticAttack")) return;

        // Has the current loop finished?
        if (state.normalizedTime >= 1f)
        {
            Debug.Log("[Bird] Attack finished → going IDLE");
            anim.SetBool(isAttackingBool, false);
            waitingForAttackToFinish = false;
        }
    }

    // ================= AUDIO =================
    // Called from Animation Event
    public void PlaySound()
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    public void BirdAttack()
    {
        if (playersInRange > 0)
        { playerScript.TakeDamage(1);
            playerScript.KBCounter = playerScript.KBTotalTime;
            if (GetComponent<Collider2D>().transform.position.x <= transform.position.x)
            {
                playerScript.KnockFromRight = true;
            }
            if (GetComponent<Collider2D>().transform.position.x > transform.position.x)
            {
                playerScript.KnockFromRight = false;
            }
        }
    }
}
