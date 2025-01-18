using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] roomPrefabs;  // Prefabs de las habitaciones
    public float roomHeight = 10f;  // Altura de cada habitaci�n
    public Transform player;  // Referencia al jugador (para obtener su posici�n Y)
    public float destroyThreshold = 15f;  // Distancia en la que las habitaciones se destruyen (ajustar seg�n sea necesario)

    private float nextSpawnHeight;  // Altura para el pr�ximo bloque
    private bool hasSpawnedFirstRoom = false;  // Verifica si ya se ha spawneado la primera habitaci�n

    private List<GameObject> spawnedRooms = new List<GameObject>();  // Lista de habitaciones generadas

    void Start()
    {
        nextSpawnHeight = 0f;  // Inicia en el nivel 0
        SpawnFirstRoom();  // Spawnea primero la habitaci�n 0
    }

    void Update()
    {
        // Verifica la posici�n Y del jugador
        if (player.position.y + 40f >= nextSpawnHeight)  // Si el jugador est� cerca de la altura de spawn
        {
            SpawnRoom();
        }

        // Destruye habitaciones que est�n por debajo de la distancia de destrucci�n
        DestroyRooms();
    }

    void SpawnRoom()
    {
       
            // Despu�s de la primera habitaci�n, genera aleatoriamente el resto de las habitaciones (sin el elemento 0)
            if (hasSpawnedFirstRoom)
            {
                int randomIndex = Random.Range(1, roomPrefabs.Length);  // Genera aleatoriamente, pero no el 0
                GameObject newRoom = Instantiate(roomPrefabs[randomIndex], new Vector3(transform.position.x, nextSpawnHeight, transform.position.z), Quaternion.identity);
                spawnedRooms.Add(newRoom);  // Agrega la habitaci�n a la lista
                nextSpawnHeight += roomHeight;  // Avanza la altura para la siguiente habitaci�n
            }
        
    }

    void SpawnFirstRoom()
    {
        if (!hasSpawnedFirstRoom)
        {
            // Spawnea la primera habitaci�n siempre como el elemento 0
            GameObject firstRoom = Instantiate(roomPrefabs[0], new Vector3(transform.position.x, nextSpawnHeight, transform.position.z), Quaternion.identity);
            spawnedRooms.Add(firstRoom);  // Agrega la primera habitaci�n a la lista
            nextSpawnHeight += roomHeight;  // Avanza la altura para la siguiente habitaci�n
            hasSpawnedFirstRoom = true;  // Marca que la primera habitaci�n ya fue spawneada
        }
    }

    void DestroyRooms()
    {
        // Recorre todas las habitaciones generadas y destruye las que ya no est�n cerca del jugador
        for (int i = spawnedRooms.Count - 1; i >= 0; i--)
        {
            GameObject room = spawnedRooms[i];
            // Si la habitaci�n est� por debajo de la distancia de destrucci�n (no est� cerca del jugador)
            if (room.transform.position.y < player.position.y - destroyThreshold)
            {
                Destroy(room);  // Destruye la habitaci�n
                spawnedRooms.RemoveAt(i);  // La elimina de la lista
            }
        }
    }

}
