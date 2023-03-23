using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPoolEnemy : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount) {
        currentHealth -= amount;
        if (currentHealth < maxHealth / 2) { GetWounded(); }
        if (currentHealth <= 0) { Die(); }
    }

    private void Die() {
        if (gameObject.CompareTag("CRATE")) {
            CrateController.c.breakOpen(ItemDropTable.layer0DropTable, 10f, transform.position);
        }
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
