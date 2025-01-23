using KyotoNinja;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        [SerializeField] private Vector2 dashColliderSize;
        [SerializeField] private GameObject jumpIndicator;

        private InputMapping inputMapping;
        private Vector2 startPosition;
        private Vector2 dragDirection;
        private Vector2 originalColliderSize;
        private CapsuleCollider2D playerCollider;
        private CircleCollider2D coinCollectionCollider;
        private PlayerHP playerHP;

        private int maxDashes;
        private int currentDashes;
        private float dashTimeMax;
        private float dashTimeRemaining;
        private float timeSlowIntensity;
        private float coinCollectionRadius;
        private float luckMultiplier;

        private Animator playerAnimator;

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
            maxDashes = playerStats.initialDashes;
            currentDashes = maxDashes;
            dashTimeMax = playerStats.dashTime;
            dashTimeRemaining = dashTimeMax;
            timeSlowIntensity = playerStats.timeSlowIntensity;
            coinCollectionRadius = playerStats.coinCollectionRadius;
            luckMultiplier = playerStats.luckMultiplier;
        }

        private void Start()
        {
            playerCollider = GetComponent<CapsuleCollider2D>();
            originalColliderSize = playerCollider.size;
            coinCollectionCollider = GetComponent<CircleCollider2D>();
            coinCollectionCollider.radius *= coinCollectionRadius;
            playerAnimator = GetComponent<Animator>();
            playerHP = GetComponent<PlayerHP>();
            playerHP.OnDamageTaken += DamageAnim;

            jumpIndicator.SetActive(false);
        }

        private void Update()
        {
            if (currentState == PlayerState.AIMING)
            {
                dashTimeRemaining -= Time.unscaledDeltaTime;
                UpdateJumpIndicator();
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
                dashTimeRemaining = dashTimeMax;
                playerCollider.size = dashColliderSize;
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
                jumpIndicator.SetActive(false);
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
                jumpIndicator.SetActive(false);
            }

            if (currentDashes <= 0)
            {
                currentState = PlayerState.IDLE;
            }

            ResumeTimeSpeed();
        }

        private void UpdateJumpIndicator()
        {
            jumpIndicator.SetActive(true);
            Vector2 clampedDirection = Vector2.ClampMagnitude(dragDirection, maxDragDistance);

            jumpIndicator.transform.position = (Vector2)transform.position + clampedDirection * 0.5f;
            jumpIndicator.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(clampedDirection.y, clampedDirection.x) * Mathf.Rad2Deg);
            jumpIndicator.transform.localScale = new Vector3(clampedDirection.magnitude * 0.65f, 0.2f, 1f); // Ajusta la escala según la distancia
        }


        private void PerformDash(Vector2 direction)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(direction.normalized * dashForce, ForceMode2D.Impulse);
            playerAnimator.SetBool("isDashing", true);
            playerAnimator.SetBool("isAttached", false);
            playerAnimator.SetBool("Idle", false);

           
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayJumpSound();
            }
            else
            {
                Debug.LogError("AudioManager instance is missing!");
            }
        }




        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Wall") && currentState != PlayerState.AIMING)
            {
                currentState = PlayerState.ATTACHED;
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0f;
                currentDashes = maxDashes;
                playerCollider.size = originalColliderSize;
                playerAnimator.SetBool("isAttached", true);
                playerAnimator.SetBool("isDashing", false);

                ContactPoint2D contactPoint = collision.contacts[0];
                if (contactPoint.point.x < transform.position.x)
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                }
                else
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                }
            }
            else if (collision.gameObject.CompareTag("Obstacle"))
            {
                playerHP.TakeDamage(1);
                playerAnimator.SetTrigger("Hurt");
            }
            else if (collision.gameObject.CompareTag("BottomSpikes"))
            {
                playerHP.TakeDamage(3);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.CompareTag("Coin"))
            {
                StartCoroutine(AtractCollectible(collision.gameObject));
                playerStats.AddCurrency((int)Random.Range(1, luckMultiplier));
            }
            else if(collision.gameObject.CompareTag("TemporalPowerUp"))
            {
                PowerUp powerUp = collision.GetComponent<PowerUp>();
                StartCoroutine(ActivateTemoPowerUp(powerUp));
            }
            else if (collision.gameObject.CompareTag("Obstacle"))
            {
                playerHP.TakeDamage(1);
                playerAnimator.SetTrigger("Hurt");
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
                playerAnimator.SetTrigger("Hurt");
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

        private void DamageAnim()
        {
            playerAnimator.SetTrigger("Hurt");
        }

        private void OnApplicationQuit()
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }

        private IEnumerator ActivateTemoPowerUp(PowerUp powerUp)
        {
            switch (powerUp.type)
            {
                case "Extra Dash":
                    maxDashes += (int)playerStats.metaPowerUps[0].amountPerLevel;
                    break;
                case "Dash Time":
                    dashTimeMax += playerStats.metaPowerUps[1].amountPerLevel;
                    break;
                case "Time-Slow":
                    timeSlowIntensity += playerStats.metaPowerUps[2].amountPerLevel;
                    break;
                case "Collection Range":
                    coinCollectionRadius += playerStats.metaPowerUps[3].amountPerLevel;
                    coinCollectionCollider.radius = 1;
                    coinCollectionCollider.radius *= coinCollectionRadius;
                    break;
                case "Luck":
                    luckMultiplier += playerStats.metaPowerUps[4].amountPerLevel;
                    break;
            }
            yield return new WaitForSeconds(playerStats.metaPowerUps[5].baseAmount + playerStats.metaPowerUps[5].amountPerLevel * playerStats.metaPowerUps[5].level);

            switch (powerUp.type)
            {
                case "Extra Dash":
                    maxDashes -= (int)playerStats.metaPowerUps[0].amountPerLevel;
                    break;
                case "Dash Time":
                    dashTimeMax -= playerStats.metaPowerUps[1].amountPerLevel;
                    break;
                case "Time-Slow":
                    timeSlowIntensity -= playerStats.metaPowerUps[2].amountPerLevel;
                    break;
                case "Collection Range":
                    coinCollectionRadius -= playerStats.metaPowerUps[3].amountPerLevel;
                    coinCollectionCollider.radius = 1;
                    coinCollectionCollider.radius *= coinCollectionRadius;
                    break;
                case "Luck":
                    luckMultiplier -= playerStats.metaPowerUps[4].amountPerLevel;
                    break;
            }
        }

        private IEnumerator AtractCollectible(GameObject gameObject)
        {
            Vector2 direction = transform.position - gameObject.transform.position;
            float speed = 5f;
            while (Vector2.Distance(transform.position, gameObject.transform.position) > 0.1f)
            {
                gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, transform.position, speed * Time.deltaTime);
                yield return null;
            }
            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            if (currentState == PlayerState.AIMING)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(startPosition, startPosition + dragDirection);
                Gizmos.DrawWireSphere(startPosition, 0.2f);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, dashColliderSize);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, coinCollectionRadius);
        }
    }
}
