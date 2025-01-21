using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private PlayerStats playerStats;

    [HideInInspector]
    public string type;

    void Start()
    {
        type = playerStats.metaPowerUps[Random.Range(0, playerStats.metaPowerUps.Count - 1)].powerUpName;
        Debug.Log("PowerUp type: " + type);
    }
}
