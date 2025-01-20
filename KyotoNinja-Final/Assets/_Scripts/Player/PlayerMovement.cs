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
        [SerializeField] private float dashForce = 10f;
        [SerializeField] private float maxDragDistance = 5f;

        private InputMapping inputMapping;
        private Vector2 startPosition;
        private Vector2 dragDirection;

        private enum PlayerState
        {
            IDLE,
            AIMING,
            ATTACHED
        }

        private PlayerState currentState = PlayerState.IDLE;

        private void Awake()
        {
            inputMapping = new InputMapping();
        }

        private void OnEnable()
        {
            inputMapping.Player.Touch.started += OnTouchStarted;
            inputMapping.Player.Touch.canceled += OnTouchCanceled;
            inputMapping.Player.Aim.performed += OnAimPerformed;
            inputMapping.Player.Enable();
        }

        private void OnDisable()
        {
            inputMapping.Player.Touch.started -= OnTouchStarted;
            inputMapping.Player.Touch.canceled -= OnTouchCanceled;
            inputMapping.Player.Aim.performed -= OnAimPerformed;
            inputMapping.Player.Disable();
        }

        private void OnTouchStarted(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (currentState == PlayerState.ATTACHED)
            {
                DetachFromWall();
            }

            if (currentState == PlayerState.IDLE || currentState == PlayerState.ATTACHED)
            {
                currentState = PlayerState.AIMING;
            }

            Vector2 screenPosition = inputMapping.Player.Aim.ReadValue<Vector2>();
            startPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        }

        private void OnAimPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (currentState == PlayerState.AIMING)
            {
                Vector2 screenPosition = context.ReadValue<Vector2>();
                Vector2 currentPosition = Camera.main.ScreenToWorldPoint(screenPosition);
                dragDirection = currentPosition - startPosition;
            }
        }

        private void OnTouchCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (currentState == PlayerState.AIMING)
            {
                dragDirection = Vector2.ClampMagnitude(dragDirection, maxDragDistance);
                Dash(dragDirection);
                currentState = PlayerState.IDLE;
            }
        }

        private void Dash(Vector2 direction)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(direction.normalized * dashForce, ForceMode2D.Impulse);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Wall") && currentState != PlayerState.AIMING)
            {
                currentState = PlayerState.ATTACHED;
                Vector2 contactPoint = collision.contacts[0].point;
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0f;
                transform.position = contactPoint;
            }
        }

        private void DetachFromWall()
        {
            rb.gravityScale = 1f;
            currentState = PlayerState.IDLE;
        }

        private void OnDrawGizmos()
        {
            if (currentState == PlayerState.AIMING)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(startPosition, startPosition + dragDirection);
                Gizmos.DrawWireSphere(startPosition, 0.2f);
            }
        }
    }

}
