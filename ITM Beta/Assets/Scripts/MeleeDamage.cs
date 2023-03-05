using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    [SerializeField] private int damage;
    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "entity") {
            collider.gameObject.GetComponent<HealthPoolEnemy>().TakeDamage(damage);
        } 
    }
}
