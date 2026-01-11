using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
 public Animator animator;
 public Health health;


    void Awake()
    {
    }
    private void OnEnable()
    {
        health.OnDamaged += HandleDamage;
    }

        private void OnDisable()
    {
        health.OnDamaged -= HandleDamage;
    }

    void HandleDamage()
    {
        animator.SetTrigger("isDamaged");
    }
}
