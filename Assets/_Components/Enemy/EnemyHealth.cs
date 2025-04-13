using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageReceiver
{
    [SerializeField]
    private float health = 100f;
    public static Action OnEnemyDeath;


    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            OnEnemyDeath?.Invoke();
            Die();
        }
    }

    void Die()
    {
        SkillUnlocker.Instance.IncreaseSlayedMonsterCount();
        Destroy(gameObject);
    }
}
