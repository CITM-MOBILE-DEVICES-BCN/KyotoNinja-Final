using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    // Referencia al Animator del enemigo
    private Animator enemyAnimator;

    // Duraci�n de la animaci�n de muerte
    public float deathAnimationDuration = 2f;

    private void Start()
    {
        // Obtener el componente Animator del enemigo
        enemyAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Verificamos si el objeto que entr� en el trigger es el jugador
        if (collider.CompareTag("Player"))
        {
            // Activar la animaci�n de muerte
            if (enemyAnimator != null)
            {
                Debug.Log("El enemigo ha muerto");
                //enemyAnimator.SetTrigger("Die"); // "Die" es el nombre del trigger en el Animator
            }

            // Destruir el enemigo despu�s de la animaci�n
            Destroy(gameObject, deathAnimationDuration);
        }
    }
}
