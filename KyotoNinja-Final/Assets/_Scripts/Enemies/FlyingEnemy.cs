using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public int damage = 1;
    // Start is called before the first frame update
    

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Verificamos si el proyectil colisiona con el jugador
        if (collider.CompareTag("Player"))
        {
            // Llamamos al método TakeDamage() del script PlayerHP
            PlayerHP playerHP = collider.GetComponent<PlayerHP>();
            if (playerHP != null)
            {
                Debug.Log("El jugador ha sido dañado");
                playerHP.TakeDamage(damage);
            }
        }
    }
}
