using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NewAI : MonoBehaviour
{
    private (Vector2 direction, float weight)[] moveDirections = new (Vector2, float)[16];
    private Vector2 target;
    [SerializeField] private float aggroRadius = 10f;

    void Awake()
    {
        InitialiseMoveDirections();
    }
    void Update() 
    {
        DrawRays();
    }
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, collision.transform.position - gameObject.transform.parent.GetChild(0).position, aggroRadius + 1, LayerMask.GetMask(new string[2] { "Player", "Wall" }));
            if (hit.collider != null) {
                if (hit.collider.gameObject != null) {
                    if (hit.collider.gameObject.CompareTag("Player")) {
                        target = GetComponent<Collider>().gameObject.transform.position;
                        gameObject.transform.position = target;
                    }
                }
            }
        }
    }

    private void InitialiseMoveDirections() {
        int count = 0;
        for (float angle = 0f; angle < 360f; angle+=22.5f) {
            moveDirections[count].direction = Quaternion.Euler(0f, 0f, angle) * Vector2.up;
            moveDirections[count].weight = 1f;
            count++;
        }
    }

    private void DrawRays() {
        for (int i = 0; i < moveDirections.Length; i++) {
            Debug.DrawRay(transform.position, moveDirections[i].direction, Color.red, 0.1f);
        }
    }
}
