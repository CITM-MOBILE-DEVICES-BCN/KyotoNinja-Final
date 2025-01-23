using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float maxDistanceFromPlayer = 5f;
    [SerializeField] private float minDistanceFromPlayer = 2f;

    private void Update()
    {
        MoveSpikes();
    }

    private void MoveSpikes()
    {
        float playerY = player.position.y;
        float spikesY = transform.position.y;

        if (playerY - spikesY > maxDistanceFromPlayer)
        {
            transform.position = new Vector3(transform.position.x, playerY - maxDistanceFromPlayer, transform.position.z);
        }

        transform.position += Vector3.up * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHP playerHP = collision.GetComponent<PlayerHP>();
            playerHP.TakeDamage(3);
        }
    }
}
