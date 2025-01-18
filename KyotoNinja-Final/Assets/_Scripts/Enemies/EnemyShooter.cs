using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    // Referencia al jugador
    public GameObject player;

    // Prefab del proyectil
    public GameObject projectilePrefab;

    // Velocidad del proyectil
    public float projectileSpeed = 5f;

    // Tiempo entre disparos
    public float shootInterval = 2f;

    // Temporizador para controlar el intervalo de disparo
    private float shootTimer;

    private void Start()
    {
        // Inicializamos el temporizador
        shootTimer = shootInterval;
    }

    private void Update()
    {
        // Decrementamos el temporizador
        shootTimer -= Time.deltaTime;

        // Si el temporizador llega a cero, dispara
        if (shootTimer <= 0f)
        {
            ShootProjectile();
            // Reiniciamos el temporizador
            shootTimer = shootInterval;
        }
    }

    private void ShootProjectile()
    {
        // Aseguramos que el jugador esté asignado
        if (player != null)
        {
            // Calcular dirección hacia el jugador
            Vector2 direction = (player.transform.position - transform.position).normalized;

            // Crear el proyectil
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            // Aseguramos que el proyectil tenga un Rigidbody2D
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Aplicamos la velocidad al proyectil en la dirección hacia el jugador
                rb.velocity = direction * projectileSpeed;
            }
        }
    }
}
