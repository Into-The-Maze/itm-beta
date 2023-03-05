using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCollider : MonoBehaviour {

    [SerializeField] private PhysicsMaterial2D material;
    [SerializeField] private bool isTrigger;

    void Awake() {
        PolygonCollider2D collider = gameObject.AddComponent<PolygonCollider2D>();
        collider.sharedMaterial = material;
        collider.isTrigger = isTrigger;
    }
}