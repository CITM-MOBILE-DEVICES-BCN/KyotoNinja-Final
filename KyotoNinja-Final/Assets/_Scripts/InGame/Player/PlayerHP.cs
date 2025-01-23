using MyNavigationSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public float health = 3f;

    public Action OnDamageTaken;

    public void TakeDamage(float damage)
    {
        OnDamageTaken?.Invoke();
        health -= damage;
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        NavigationManager.Instance.LoadScene("InGame");
    }
}
