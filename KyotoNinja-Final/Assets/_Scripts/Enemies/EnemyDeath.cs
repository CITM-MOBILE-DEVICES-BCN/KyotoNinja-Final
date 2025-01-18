using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    // Referencia al Animator del enemigo
    private Animator enemyAnimator;

    // Duración de la animación de muerte
    public float deathAnimationDuration = 2f;

    private void Start()
    {
        // Obtener el componente Animator del enemigo
        enemyAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Verificamos si el objeto que entró en el trigger es el jugador
        if (collider.CompareTag("Player"))
        {
            // Activar la animación de muerte
            if (enemyAnimator != null)
            {
                Debug.Log("El enemigo ha muerto");
                //enemyAnimator.SetTrigger("Die"); // "Die" es el nombre del trigger en el Animator
            }

            // Destruir el enemigo después de la animación
            Destroy(gameObject, deathAnimationDuration);
        }
    }
}
