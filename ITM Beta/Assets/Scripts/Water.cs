using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public GameObject entity;
    public static float depthLevel; // must be between 0 and 1 inc.
    private bool isUnderwater;

    private void FixedUpdate() {
        if (isUnderwater) {
            PlayerMovement.movementType = PlayerMovement.MovementType.Underwater;
            if (depthLevel > 0.5f) { Oxygen.oxygen -= 0.3f; }
        }

        RaycastHit2D hit = Physics2D.Raycast(entity.transform.position, Vector2.zero);
        if (hit.collider != null) {
            if (hit.collider.gameObject != null) {
                if (hit.collider.gameObject.CompareTag("WATER")) {
                    // do water thingz
                }
            }
        }
    }
}
