using MyNavigationSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public float health = 3f;

    public void TakeDamage(float damage)
    {
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
