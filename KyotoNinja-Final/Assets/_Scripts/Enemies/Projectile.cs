using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Daño que el proyectil causará al jugador
    public int damage = 1;

    // Distancia máxima que puede recorrer el proyectil
    public float maxDistance = 10f;

    // Distancia recorrida por el proyectil
    private Vector2 startPosition;

    // Collider para detectar las colisiones
    private void Start()
    {
        // Guardamos la posición inicial del proyectil
        startPosition = transform.position;
    }

    private void Update()
    {
        // Calculamos la distancia recorrida
        float distanceTraveled = Vector2.Distance(startPosition, transform.position);

        // Si el proyectil ha recorrido la distancia máxima, lo destruimos
        if (distanceTraveled >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Verificamos si el proyectil colisiona con el jugador
        if (collider.CompareTag("Player"))
        {
            // Llamamos al método TakeDamage() del script PlayerHP
            PlayerHP playerHP = collider.GetComponent<PlayerHP>();
            if (playerHP != null)
            {
                playerHP.TakeDamage(damage);
            }

            // Destruir el proyectil después de colisionar
            Destroy(gameObject);
        }

        
    }

}
