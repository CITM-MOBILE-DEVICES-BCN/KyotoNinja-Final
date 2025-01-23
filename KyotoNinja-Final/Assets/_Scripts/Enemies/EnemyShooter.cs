using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject player;
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;
    public float shootInterval = 2f;
    public float shootRadius = 20f;
    private float shootTimer;
    private Animator enemyAnimator;

    private void Start()
    {
        shootTimer = shootInterval;
        enemyAnimator = GetComponent<Animator>();

        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            if (player == null)
            {
                Debug.LogError("Player object not found! Make sure the Player has the 'Player' tag.");
            }
        }
    }

    private void Update()
    {
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0f && IsPlayerInRange())
        {
            ShootProjectile();
            shootTimer = shootInterval;
        }
    }

    private bool IsPlayerInRange()
    {
        if (player == null) return false;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        return distanceToPlayer <= shootRadius;
    }

    private void ShootProjectile()
    {
        if (player != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            // Aseguramos que el proyectil tenga un Rigidbody2D
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * projectileSpeed;
            }
            enemyAnimator.SetTrigger("Attack");
        }
    }
}