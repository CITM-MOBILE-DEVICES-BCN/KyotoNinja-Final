using MyNavigationSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public float health = 3f;
    private Animator playerAnimator;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Die();
        }
        playerAnimator.SetTrigger("Hurt");
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
