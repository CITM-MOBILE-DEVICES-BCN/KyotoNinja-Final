using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    private Animator enemyAnimator;
    public float deathAnimationDuration = 2f;

    private void Start()
    {
        enemyAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (enemyAnimator != null)
            {
                Debug.Log("El enemigo ha muerto");
                enemyAnimator.SetTrigger("Die");
            }

            Destroy(gameObject, deathAnimationDuration);
        }
    }
}
