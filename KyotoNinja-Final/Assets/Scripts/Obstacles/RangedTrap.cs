using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedTrap : MonoBehaviour
{
    // Prefab del proyectil
    public GameObject projectilePrefab;

    // Punto desde donde se disparan los proyectiles
    public Transform shootPoint;

    // Velocidad del proyectil
    public float projectileSpeed = 10f;

    // Intervalo entre disparos
    public float fireInterval = 2f;

    // Temporizador interno
    private float fireTimer;

    void Update()
    {
        // Incrementar el temporizador
        fireTimer += Time.deltaTime;

        // Disparar si se alcanza el intervalo
        if (fireTimer >= fireInterval)
        {
            FireProjectile();
            fireTimer = 0f; // Reiniciar el temporizador
        }
    }

    void FireProjectile()
    {
        // Instanciar el proyectil
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

        // Añadir velocidad al proyectil
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = shootPoint.right * projectileSpeed;
        }

        // Opcional: Destruir el proyectil después de un tiempo
        Destroy(projectile, 0.75f);
    }
}
