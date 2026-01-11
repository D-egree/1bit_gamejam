using UnityEngine;

public class CeilingBlob : MonoBehaviour
{
    private Animator animator;

    public GameObject bullet;
    public Transform firePoint;
    private AudioSource source;
    public AudioClip clip;

    void Awake()
    {
        animator = GetComponent<Animator>();

    
        source = GetComponent<AudioSource>();

        source.playOnAwake = false;
        source.loop = false;
    
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.Play("CeilingBlobAttack");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.Play("CeilingBlobIdle");
        }
    }

    public void BlobAttack()
    {
        if (bullet != null && firePoint != null)
        {
            Instantiate(bullet, firePoint.position, firePoint.rotation);
        }
    }
    void SoundPlay()
    {
        source.PlayOneShot(clip);
    }
}
