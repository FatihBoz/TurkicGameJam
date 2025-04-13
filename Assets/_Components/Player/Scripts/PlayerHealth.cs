using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public static float health = 60f;
    public static Action OnPlayerDeath;
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            OnPlayerDeath?.Invoke();
            health = 60f;
        }
    }
}
