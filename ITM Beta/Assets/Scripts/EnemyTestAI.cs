using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

public class EnemyTestAI : MonoBehaviour
{
    private float aggroRadius;
    private Vector3 origin = new Vector3(0, 0, 0);
    private CircleCollider2D aggro;
    public Vector3 target = new Vector3(8, 8, 0);


    void Awake()
    {
        aggro = gameObject.transform.parent.GetChild(0).GetComponent<CircleCollider2D>();
        aggroRadius = aggro.radius;  
        gameObject.transform.parent.position = origin;
    }
    private void Update() {
    }

    public void Trigger(Collider2D collider) {
        if (collider.gameObject.tag == "Player") {
            RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.parent.GetChild(0).position, collider.transform.position - gameObject.transform.parent.GetChild(0).position,aggroRadius + 1, LayerMask.GetMask(new string[2] { "Player", "Wall" }));
            if (hit.collider != null) {
                if (hit.collider.gameObject != null) {
                    if (hit.collider.gameObject.CompareTag("Player")) {
                        target = collider.gameObject.transform.position;
                        gameObject.transform.position = target;
                    }
                }
            }
        }
        else {
            if (gameObject.transform.parent.GetChild(0).position.y > gameObject.transform.position.y - 0.2f && gameObject.transform.parent.GetChild(0).position.y < gameObject.transform.position.y + 0.2f
                && gameObject.transform.parent.GetChild(0).position.x > gameObject.transform.position.x - 0.2f && gameObject.transform.parent.GetChild(0).position.x < gameObject.transform.position.x + 0.2f) {
                gameObject.transform.position = InstantiateMaze.RandomFloorPoint();
            }
        }
    }
}