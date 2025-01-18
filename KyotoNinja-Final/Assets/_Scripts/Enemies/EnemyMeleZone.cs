using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class EnemyMeleZone : MonoBehaviour
{
    // Referencia al enemigo (asegúrate de asignarlo en el Inspector)
    public GameObject enemy;

    // Daño que el enemigo causará al jugador
    public int damage = 1;

    // Referencia al Animator del enemigo
    private Animator enemyAnimator;

    private void Start()
    {
        // Obtener el componente Animator del enemigo
        if (enemy != null)
        {
            enemyAnimator = enemy.GetComponent<Animator>();
        }
    }

    // Función que se llama cuando algo entra en el trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificamos si el objeto que entró en el trigger es el jugador
        if (other.CompareTag("Player"))
        {
            // Llamamos a la función TakeDamage() del script PlayerHP
            PlayerHP playerHP = other.GetComponent<PlayerHP>();
            if (playerHP != null)
            {
                playerHP.TakeDamage(damage);
            }

            // Activar la animación de ataque del enemigo
            if (enemyAnimator != null)
            {
                Debug.Log("El enemigo ha atacado al jugador");
                //enemyAnimator.SetTrigger("Attack"); // "Attack" es el nombre del trigger en el Animator
            }
        }
    }
}
