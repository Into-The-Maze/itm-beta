using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroEnemyTest : MonoBehaviour 
{
    private CircleCollider2D aggro;
    private float aggroRadius = 10f;
    private void Awake() {
        aggro = GetComponent<CircleCollider2D>();
        aggro.radius = aggroRadius;
    }
    private void OnTriggerStay2D(Collider2D collider) {
        EnemyTestAI AI = gameObject.transform.parent.GetChild(1).gameObject.GetComponent<EnemyTestAI>();
        AI.Trigger(collider);
    }
}
