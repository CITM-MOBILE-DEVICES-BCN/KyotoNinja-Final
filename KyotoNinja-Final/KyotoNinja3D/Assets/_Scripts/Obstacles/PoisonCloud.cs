using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloud : MonoBehaviour
{
    // Velocidad del movimiento
    public float speed = 5f;

    // Límites izquierdo y derecho
    public float leftLimit = -5f;
    public float rightLimit = 5f;

    // Dirección actual del movimiento (1 = derecha, -1 = izquierda)
    private int direction = 1;

    void Update()
    {        
        // Mover el obstáculo
        transform.Translate(Vector2.right * speed * direction * Time.deltaTime);

        // Cambiar la dirección si se alcanza un límite
        if (transform.position.x >= rightLimit)
        {
            direction = -1; // Mover hacia la izquierda
        }
        else if (transform.position.x <= leftLimit)
        {
            direction = 1; // Mover hacia la derecha
        }
    }
}