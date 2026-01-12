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

    // ✅ NEW: End game toggle
    public bool endGame = false;

    // ✅ NEW: UI element to enable when game ends
    public GameObject endGameUI;

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

        // ✅ NEW: End game check
        if (endGame && endGameUI != null)
        {
            endGameUI.SetActive(true);
        }

        OnDeath?.Invoke();

        Destroy(gameObject);
    }
}
