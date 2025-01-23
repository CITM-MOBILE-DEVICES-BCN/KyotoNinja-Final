using MyNavigationSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public float health = 3f;
    public float invincibilityDuration = 1f;
    private float invincibilityTimer = 0f;
    private Animator playerAnimator;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    public Action OnDamageTaken;

    public void TakeDamage(float damage)
    {
        if(invincibilityTimer > 0f)
        {
            invincibilityTimer -= Time.deltaTime;
            return;
        }

        OnDamageTaken?.Invoke();
        health -= damage;
        if (health <= 0f)
        {
            Die();
        }
        playerAnimator.SetTrigger("Hurt");
        invincibilityTimer = invincibilityDuration;
    }

    private void Die()
    {
        NavigationManager.Instance.LoadScene("InGame");
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Obstacle"))
        {
            Die();
        }
    }
}
