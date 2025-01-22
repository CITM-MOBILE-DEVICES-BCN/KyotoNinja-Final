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

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        int random = Random.Range(0, playerStats.metaPowerUps.Count - 1);
        spriteRenderer = GetComponent<SpriteRenderer>();
        type = playerStats.metaPowerUps[random].powerUpName;
        spriteRenderer.sprite = playerStats.metaPowerUps[random].icon;

        Debug.Log("PowerUp type: " + type);
    }
}
