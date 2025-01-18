using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] roomPrefabs;  // Prefabs de las habitaciones
    public float roomHeight = 10f;  // Altura de cada habitación
    public Transform player;  // Referencia al jugador (para obtener su posición Y)

    private float nextSpawnHeight;  // Altura para el próximo bloque
    private bool isSpecialRoomSpawned = false;  // Para asegurarse de que solo haya una habitación especial
    private bool hasSpawnedFirstRoom = false;  // Verifica si ya se ha spawneado la primera habitación

    void Start()
    {
        nextSpawnHeight = 0f;  // Inicia en el nivel 0
        SpawnFirstRoom();  // Spawnea primero la habitación 0
    }

    void Update()
    {
        // Verifica la posición Y del jugador
        if (player.position.y + 40f >= nextSpawnHeight)  // Si el jugador está cerca de la altura de spawn
        {
            SpawnRoom();
        }
    }

    void SpawnRoom()
    {
        
            // Después de la primera habitación, genera aleatoriamente el resto de las habitaciones (sin el elemento 0)
            if (hasSpawnedFirstRoom)
            {
                int randomIndex = Random.Range(1, roomPrefabs.Length);  // Genera aleatoriamente, pero no el 0
                Instantiate(roomPrefabs[randomIndex], new Vector3(transform.position.x, nextSpawnHeight, transform.position.z), Quaternion.identity);
                nextSpawnHeight += roomHeight;  // Avanza la altura para la siguiente habitación
            }
        
    }

    void SpawnFirstRoom()
    {
        if (!hasSpawnedFirstRoom)
        {
            // Spawnea la primera habitación siempre como el elemento 0
            Instantiate(roomPrefabs[0], new Vector3(transform.position.x, nextSpawnHeight, transform.position.z), Quaternion.identity);
            nextSpawnHeight += roomHeight;  // Avanza la altura para la siguiente habitación
            hasSpawnedFirstRoom = true;  // Marca que la primera habitación ya fue spawneada
        }
    }

}
