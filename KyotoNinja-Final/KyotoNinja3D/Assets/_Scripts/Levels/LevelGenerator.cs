using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject initialRoomPrefab;  // Prefab de la habitación inicial
    public GameObject[] roomSet1;  // Lista de habitaciones para el intervalo 1
    public GameObject[] roomSet2;  // Lista de habitaciones para el intervalo 2
    public GameObject[] roomSet3;  // Lista de habitaciones para el intervalo 3
    public GameObject[] roomSet4;  // Lista de habitaciones para el intervalo 4
    public GameObject[] roomSet5;  // Lista de habitaciones para el intervalo 5
    public GameObject[] roomSet6;  // Lista de habitaciones para el intervalo 6
    public float roomHeight = 10f;  // Altura de cada habitación
    public Transform player;  // Referencia al jugador (para obtener su posición Y)
    public float destroyThreshold = 15f;  // Distancia en la que las habitaciones se destruyen
    public TMP_Text heightText;  

    private float nextSpawnHeight;  // Altura para el próximo bloque
    private int currentInterval = 0;  // Intervalo actual (0 a 5, para 6 intervalos)
    private const int intervalCount = 6;  // Número de intervalos de generación
    private const float intervalHeight = 200f;  // Altura de cada intervalo

    private List<GameObject> spawnedRooms = new List<GameObject>();  // Lista de habitaciones generadas

    void Start()
    {
        nextSpawnHeight = 0f;  // Inicia en el nivel 0
        SpawnInitialRoom();  // Spawnea la habitación inicial
    }

    void Update()
    {
        // Verifica si el jugador se acerca al próximo intervalo de generación
        if (player.position.y + 40f >= nextSpawnHeight && currentInterval < intervalCount)
        {
            SpawnRoomsForInterval();
        }

        if (heightText != null)
        {
            heightText.text = player.position.y.ToString("F0") + "m";
        }

        // Destruye habitaciones que estén por debajo de la distancia de destrucción
        DestroyRooms();
    }

    void SpawnInitialRoom()
    {
        // Genera la habitación inicial
        Vector3 spawnPosition = new Vector3(transform.position.x, nextSpawnHeight, transform.position.z);
        GameObject initialRoom = Instantiate(initialRoomPrefab, spawnPosition, Quaternion.identity);
        spawnedRooms.Add(initialRoom);
        nextSpawnHeight += roomHeight;  // Avanza la altura para la siguiente habitación
    }

    void SpawnRoomsForInterval()
    {
        if (currentInterval >= intervalCount) return;  // No genera más si ya se completaron los intervalos

        GameObject[] currentRoomSet = GetRoomSetForInterval(currentInterval);

        for (int i = 0; i < 6; i++)
        {
            int randomIndex = Random.Range(0, currentRoomSet.Length);  // Selecciona un prefab aleatorio de la lista correspondiente
            Vector3 spawnPosition = new Vector3(transform.position.x, nextSpawnHeight, transform.position.z);
            GameObject newRoom = Instantiate(currentRoomSet[randomIndex], spawnPosition, Quaternion.identity);
            spawnedRooms.Add(newRoom);  // Agrega la habitación a la lista
            nextSpawnHeight += roomHeight;  // Avanza la altura para la siguiente habitación
        }

        currentInterval++;  // Avanza al siguiente intervalo
    }

    GameObject[] GetRoomSetForInterval(int interval)
    {
        // Devuelve la lista de habitaciones correspondiente al intervalo actual
        switch (interval)
        {
            case 0: return roomSet1;
            case 1: return roomSet2;
            case 2: return roomSet3;
            case 3: return roomSet4;
            case 4: return roomSet5;
            case 5: return roomSet6;
            default: return new GameObject[0];
        }
    }

    void DestroyRooms()
    {
        // Recorre todas las habitaciones generadas y destruye las que ya no están cerca del jugador
        for (int i = spawnedRooms.Count - 1; i >= 0; i--)
        {
            GameObject room = spawnedRooms[i];
            if (room.transform.position.y < player.position.y - destroyThreshold)
            {
                Destroy(room);  // Destruye la habitación
                spawnedRooms.RemoveAt(i);  // La elimina de la lista
            }
        }
    }
}
