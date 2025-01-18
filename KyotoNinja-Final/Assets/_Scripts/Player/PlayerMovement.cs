using KyotoNinja;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KyotoNinja
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float dashForce = 10f; // Fuerza del dash
        [SerializeField] private float maxDragDistance = 5f; // Distancia máxima de arrastre

        private InputMapping inputMapping; // Clase generada del Input Action Asset
        private Vector2 startPosition;
        private Vector2 dragDirection;

        // Estados del jugador
        private enum PlayerState
        {
            IDLE,
            AIMING,
            ATTACHED
        }

        private PlayerState currentState = PlayerState.IDLE;

        private void Awake()
        {
            // Inicializar la instancia de InputMapping
            inputMapping = new InputMapping();
        }

        private void OnEnable()
        {
            // Suscribirse a los eventos de las acciones Touch y Aim
            inputMapping.Player.Touch.started += OnTouchStarted;
            inputMapping.Player.Touch.canceled += OnTouchCanceled;

            inputMapping.Player.Aim.performed += OnAimPerformed;

            // Habilitar el mapa de acciones
            inputMapping.Player.Enable();
        }

        private void OnDisable()
        {
            // Desuscribirse de los eventos
            inputMapping.Player.Touch.started -= OnTouchStarted;
            inputMapping.Player.Touch.canceled -= OnTouchCanceled;

            inputMapping.Player.Aim.performed -= OnAimPerformed;

            // Deshabilitar el mapa de acciones
            inputMapping.Player.Disable();
        }

        private void OnTouchStarted(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (currentState == PlayerState.ATTACHED)
            {
                // Liberar el enganche para permitir el salto
                DetachFromWall();
            }

            // Cambiar el estado a AIMING solo si está en ATTACHED o IDLE
            if (currentState == PlayerState.IDLE || currentState == PlayerState.ATTACHED)
            {
                currentState = PlayerState.AIMING;
            }

            // Leer la posición inicial desde la acción Aim (posición del ratón o toque)
            Vector2 screenPosition = inputMapping.Player.Aim.ReadValue<Vector2>();
            startPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        }

        private void OnAimPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (currentState == PlayerState.AIMING)
            {
                // Actualizar la posición actual mientras se arrastra
                Vector2 screenPosition = context.ReadValue<Vector2>();
                Vector2 currentPosition = Camera.main.ScreenToWorldPoint(screenPosition);

                // Calcular la dirección de arrastre
                dragDirection = currentPosition - startPosition;
            }
        }

        private void OnTouchCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (currentState == PlayerState.AIMING)
            {
                // Calcular la dirección final del dash
                dragDirection = Vector2.ClampMagnitude(dragDirection, maxDragDistance);

                // Ejecutar el dash y volver a IDLE
                Dash(dragDirection);
                currentState = PlayerState.IDLE;
            }
        }

        private void Dash(Vector2 direction)
        {
            // Asegurarse de que el Rigidbody no acumule fuerzas anteriores
            rb.velocity = Vector2.zero;

            // Aplicar la fuerza del dash en la dirección calculada
            rb.AddForce(direction.normalized * dashForce, ForceMode2D.Impulse);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Comprobar si el objeto tiene el tag "Wall"
            if (collision.gameObject.CompareTag("Wall") && currentState != PlayerState.AIMING)
            {
                // Cambiar el estado a ATTACHED
                currentState = PlayerState.ATTACHED;

                // Enganchar al punto de colisión
                Vector2 contactPoint = collision.contacts[0].point; // Obtener el primer punto de contacto
                rb.velocity = Vector2.zero; // Detener el movimiento
                rb.gravityScale = 0f; // Desactivar la gravedad
                transform.position = contactPoint; // Mover el jugador al punto de contacto
            }
        }

        private void DetachFromWall()
        {
            // Reactivar la gravedad para permitir el movimiento
            rb.gravityScale = 1f;
            currentState = PlayerState.IDLE;
        }

        private void OnDrawGizmos()
        {
            // Dibujar el vector de arrastre para depuración
            if (currentState == PlayerState.AIMING)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(startPosition, startPosition + dragDirection);
                Gizmos.DrawWireSphere(startPosition, 0.2f);
            }
        }
    }

}
