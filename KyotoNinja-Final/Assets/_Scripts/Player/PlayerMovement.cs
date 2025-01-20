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
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private float dashForce = 10f;
        [SerializeField] private float maxDragDistance = 5f;

        private InputMapping inputMapping;
        private Vector2 startPosition;
        private Vector2 dragDirection;

        private int currentDashes;
        private float dashTimeRemaining;
        private float timeSlowIntensity;

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

            InitializePlayer();
        }

        private void OnDisable()
        {
            inputMapping.Player.Touch.started -= OnTouchStarted;
            inputMapping.Player.Touch.canceled -= OnTouchCanceled;
            inputMapping.Player.Aim.performed -= OnAimPerformed;
            inputMapping.Player.Disable();
        }

        private void InitializePlayer()
        {
            currentDashes = playerStats.initialDashes;
            dashTimeRemaining = playerStats.dashTime;
            timeSlowIntensity = playerStats.timeSlowIntensity;
        }

        private void Update()
        {
            if (currentState == PlayerState.AIMING)
            {
                dashTimeRemaining -= Time.unscaledDeltaTime;
                if (dashTimeRemaining <= 0)
                {
                    LoseDashOnAimTimeout();
                    CancelAiming();
                }
            }
        }

        private void OnTouchStarted(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (currentDashes <= 0)
            {
                return;
            }

            if (currentState == PlayerState.ATTACHED)
            {
                DetachFromWall();
            }

            if (currentState == PlayerState.IDLE || currentState == PlayerState.ATTACHED)
            {
                currentState = PlayerState.AIMING;
                SlowTimeSpeed();
                dashTimeRemaining = playerStats.dashTime;
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
            if (currentState == PlayerState.AIMING && currentDashes > 0)
            {
                dragDirection = Vector2.ClampMagnitude(dragDirection, maxDragDistance);
                PerformDash(dragDirection);
                currentState = PlayerState.IDLE;
                currentDashes--;
            }

            if (currentDashes <= 0)
            {
                currentState = PlayerState.IDLE;
            }

            ResumeTimeSpeed();
        }

        private void PerformDash(Vector2 direction)
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
                currentDashes = playerStats.initialDashes;
            }
        }

        private void DetachFromWall()
        {
            rb.gravityScale = 1f;
            currentState = PlayerState.IDLE;
        }

        private void CancelAiming()
        {
            currentState = PlayerState.IDLE;
            ResumeTimeSpeed();
            dashTimeRemaining = 0f;
        }

        private void LoseDashOnAimTimeout()
        {
            if (currentDashes > 0)
            {
                currentDashes--;
            }
        }

        private void ResumeTimeSpeed()
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }

        private void SlowTimeSpeed()
        {
            Time.timeScale = timeSlowIntensity;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
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
