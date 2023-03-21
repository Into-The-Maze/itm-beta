using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dodge : MonoBehaviour
{
    public static Dodge d;
    public float dodgeSpeed = 10;
    public WaitForSeconds dodgeCooldown = new WaitForSeconds(1.5f);
    public WaitForSeconds dodgeLength = new WaitForSeconds(0.1f);
    static bool isDodging;
    public static bool canDodge_cooldown;
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] GameObject playerRotation;
    Vector2 moveInput;

    private void Awake() {
        isDodging = false;
        canDodge_cooldown = true;
        d = this;
    }

    void Update() {
        moveInput = Vector2.zero;
    }

    
    
    IEnumerator handleDodgeCooldown() {
        yield return dodgeCooldown;
        canDodge_cooldown = true;
    }

    IEnumerator handleDodgeLength() {
        yield return dodgeLength;
        isDodging = false;
    }

    public IEnumerator handleDodge() {
        PlayerMovement.movementType = PlayerMovement.MovementType.Dodging;
        
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        isDodging = true;
        canDodge_cooldown = false;
        rb2d.mass = 0.1f;
        StartCoroutine(handleDodgeLength());
        if (moveInput == Vector2.zero) {
            while (isDodging) {
                rb2d.AddForce(playerRotation.transform.up, ForceMode2D.Impulse);
                yield return null;
            }
        }
        else {
            while (isDodging) {
                rb2d.AddForce(moveInput, ForceMode2D.Impulse);
                yield return null;
            }
        }


        PlayerMovement.movementType = PlayerMovement.MovementType.Walking;
        StartCoroutine(handleDodgeCooldown());
    }
}
