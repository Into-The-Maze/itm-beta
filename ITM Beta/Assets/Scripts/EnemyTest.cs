using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class EnemyTest : MonoBehaviour
{
    private Random r = new Random();

    [SerializeField] private float speed;
    [SerializeField] private float aggroRange;
    [SerializeField] private float kiteRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackDamage;

    private string mode = "wander";
    private bool triggered = false;
    private bool lineOfSight = false;
    private Vector3 targetPosition;
    private Vector3 targetVector;
    private Vector3 playerPosition;
    Rigidbody2D rb2d;

    private void Awake() {
        CircleCollider2D aggro = gameObject.AddComponent<CircleCollider2D>();
        aggro.radius = aggroRange;
        aggro.isTrigger = true;
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }
    private void Update() {
        mode = GetMode();
        DoAction(mode);
        Move();
    }
    private void OnTriggerStay2D(Collider2D collider) {
        if (collider.gameObject.tag == "Player") {
            triggered = true;
            playerPosition = collider.gameObject.transform.position;
            lineOfSight = Physics.Raycast(gameObject.transform.position, playerPosition, aggroRange);
            if (lineOfSight) {
                targetPosition = playerPosition;
            }
        }
    }

    private void Wander() {
        mode = "wander";
    }
    private void Track() {
        mode = "track";
        targetVector = targetPosition - gameObject.transform.position;
    }
    private void Chase() {
        mode = "chase";
        targetVector = playerPosition - gameObject.transform.position;
    }
    private void Kite() {
        mode = "kite";
    }
    private void Attack() {
        mode = "attack";

    }
    
    private string GetMode() {
        if (mode == "attack") {
            return "attack";
        }
        if (mode == "kite") {
            if (targetVector.magnitude > kiteRange) {
                return "chase";
            }
            // randomly attack if not on cooldown
        }
        if (triggered) { 
            triggered = false;
        }
        return "wander";
    }
    private void Move() {

    }
    private void DoAction(string mode) {
        switch (mode) {
            case "wander":
                Wander(); break;
            case "track":
                Track(); break;
            case "chase":
                Chase(); break;
            case "kite":
                Kite(); break;
            case "attack":
                Attack(); break;
            default:
                Wander(); break;
        }
    }
}
