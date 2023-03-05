using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "entity" && Attack.CurrentlySwinging) {
            collider.gameObject.GetComponent<HealthPoolEnemy>().TakeDamage(Attack.damage);
        } 
    }
}
