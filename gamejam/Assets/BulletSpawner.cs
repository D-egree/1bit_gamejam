using UnityEngine;

public class CeilingBlob : MonoBehaviour
{
    private Animator animator;

    public GameObject bullet;
    public Transform firePoint;

    void Awake()
    {
        animator = GetComponent<Animator>();
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
}
