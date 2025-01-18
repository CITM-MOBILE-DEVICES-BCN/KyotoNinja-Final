using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] roomPrefabs;  // Prefabs de las habitaciones
    public float roomHeight = 10f;  // Altura de cada habitación
    public Transform player;  // Referencia al jugador (para obtener su posición Y)
    public float destroyThreshold = 15f;  // Distancia en la que las habitaciones se destruyen (ajustar según sea necesario)

    private float nextSpawnHeight;  // Altura para el próximo bloque
    private bool hasSpawnedFirstRoom = false;  // Verifica si ya se ha spawneado la primera habitación

    private List<GameObject> spawnedRooms = new List<GameObject>();  // Lista de habitaciones generadas

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

        // Destruye habitaciones que estén por debajo de la distancia de destrucción
        DestroyRooms();
    }

    void SpawnRoom()
    {
       
            // Después de la primera habitación, genera aleatoriamente el resto de las habitaciones (sin el elemento 0)
            if (hasSpawnedFirstRoom)
            {
                int randomIndex = Random.Range(1, roomPrefabs.Length);  // Genera aleatoriamente, pero no el 0
                GameObject newRoom = Instantiate(roomPrefabs[randomIndex], new Vector3(transform.position.x, nextSpawnHeight, transform.position.z), Quaternion.identity);
                spawnedRooms.Add(newRoom);  // Agrega la habitación a la lista
                nextSpawnHeight += roomHeight;  // Avanza la altura para la siguiente habitación
            }
        
    }

    void SpawnFirstRoom()
    {
        if (!hasSpawnedFirstRoom)
        {
            // Spawnea la primera habitación siempre como el elemento 0
            GameObject firstRoom = Instantiate(roomPrefabs[0], new Vector3(transform.position.x, nextSpawnHeight, transform.position.z), Quaternion.identity);
            spawnedRooms.Add(firstRoom);  // Agrega la primera habitación a la lista
            nextSpawnHeight += roomHeight;  // Avanza la altura para la siguiente habitación
            hasSpawnedFirstRoom = true;  // Marca que la primera habitación ya fue spawneada
        }
    }

    void DestroyRooms()
    {
        // Recorre todas las habitaciones generadas y destruye las que ya no están cerca del jugador
        for (int i = spawnedRooms.Count - 1; i >= 0; i--)
        {
            GameObject room = spawnedRooms[i];
            // Si la habitación está por debajo de la distancia de destrucción (no está cerca del jugador)
            if (room.transform.position.y < player.position.y - destroyThreshold)
            {
                Destroy(room);  // Destruye la habitación
                spawnedRooms.RemoveAt(i);  // La elimina de la lista
            }
        }
    }

}
