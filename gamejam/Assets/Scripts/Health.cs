using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action OnDamaged;
    public event Action OnDeath;

    public int maxHealth = 5;
    public int health;

    public int orbsToSpawn = 1;
    public GameObject orb;

    void Start()
    {
        health = maxHealth;
    }

    public void ChangeHealth(int amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);

        if (health <= 0)
        {
            Die();
        }
        else if (amount < 0)
        {
            OnDamaged?.Invoke();
        }
    }

    void Die()
    {
        // Spawn orbs FIRST
        for (int i = 0; i < orbsToSpawn; i++)
        {
            Instantiate(orb, transform.position, Quaternion.identity);
        }

        OnDeath?.Invoke();

        Destroy(gameObject);
    }
}
