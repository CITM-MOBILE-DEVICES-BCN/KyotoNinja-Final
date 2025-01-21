using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public float health = 100f;  // Salud del jugador

    // Método para infligir daño
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Die();
        }
    }

    // Método para manejar la muerte del jugador
    void Die()
    {
        // Aquí puedes agregar lo que suceda cuando el jugador muera (como reiniciar la escena)
        Debug.Log("El jugador ha muerto");
    }
}
