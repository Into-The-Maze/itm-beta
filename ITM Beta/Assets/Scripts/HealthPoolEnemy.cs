using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPoolEnemy : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount) {
        currentHealth -= amount;
        if (currentHealth < maxHealth / 2) { GetWounded(); }
        if (currentHealth <= 0) { Die(); }
    }

    private void Die() {
        Destroy(gameObject);
        // death animation/effect
        // loot
    }

    private void GetWounded() {
        Debug.Log("Enemy wounded");
        // blood effects
        // speed reduction
    }
}
