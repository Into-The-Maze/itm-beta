using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider) {
        if ((collider.gameObject.CompareTag("entity") || collider.gameObject.CompareTag("CRATE")) && Attack.CurrentlySwinging) {
            collider.gameObject.GetComponent<HealthPoolEnemy>().TakeDamage(Attack.damage);
        } 
    }
}
