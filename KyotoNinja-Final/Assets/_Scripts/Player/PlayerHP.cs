using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public float health = 100f;  // Salud del jugador

    // M�todo para infligir da�o
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Die();
        }
    }

    // M�todo para manejar la muerte del jugador
    void Die()
    {
        // Aqu� puedes agregar lo que suceda cuando el jugador muera (como reiniciar la escena)
        Debug.Log("El jugador ha muerto");
    }
}
