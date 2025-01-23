using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class EnemyMeleZone : MonoBehaviour
{
    public GameObject enemy;
    public int damage = 1;
    private Animator enemyAnimator;

    private void Start()
    {
        if (enemy != null)
        {
            enemyAnimator = enemy.GetComponent<Animator>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHP playerHP = other.GetComponent<PlayerHP>();
            if (playerHP != null)
            {
                playerHP.TakeDamage(damage);
            }

            if (enemyAnimator != null)
            {
                Debug.Log("El enemigo ha atacado al jugador");
                enemyAnimator.SetTrigger("Attack");
            }
        }
    }
}
