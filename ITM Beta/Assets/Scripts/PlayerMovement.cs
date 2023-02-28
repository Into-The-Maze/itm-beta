using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerMovement : MonoBehaviour {
    
    public Slider staminaBar;
    public GameObject playerSprite;
    public Rigidbody2D rb2d;

    //private Rigidbody2D playerBody;
    //float currentSpeed = 0f;
    //float weight = 10f;
    //float acceleration = 1f;
    //(bool, float) movement;
    //Vector2 moveVector;
    //Vector2 lastVector;


    #region movement
    public static float lastRadiansFromNorth = 0f;
    float speedModifier = 1f;
    float moveSpeed = 1f;
    public static float stamina = maxStamina;
    static float maxStamina = 50f;
    private Vector2 moveInput;
    #endregion

    #region dodge
    private float activeMoveSpeed;
    private float dodgeSpeed, dodgeLength = 0.5f;//, dodgeCooldown = 1f;
    private float dodgeCounter, dodgeCooldownCounter;
    #endregion

    public static MovementType movementType = MovementType.Walking;

    public LayerMask wallMask;
    //public LayerMask playerMask;
    //public LayerMask itemMask;
    //public LayerMask vaultMask;

    void Start() {
        ChangeMovementType(ref movementType, ref stamina, ref speedModifier, dodgeSpeed, maxStamina);
        activeMoveSpeed = moveSpeed;
        staminaBar.maxValue = maxStamina;
    }

    void Update(){
        ChangeMovementType(ref movementType, ref stamina, ref speedModifier, dodgeSpeed, maxStamina);


        if (movementType != MovementType.Dodging) {
            simpleMovement(speedModifier, activeMoveSpeed);
        }
        
        staminaBar.value = stamina;
    }


    private void simpleMovement(float speedModifier, float activeMoveSpeed) {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        //moveInput.Normalize();
        rb2d.velocity = moveInput * speedModifier * activeMoveSpeed; 

    }

    private void doDodge() {
        if (dodgeCooldownCounter <= 0 && dodgeCounter <= 0) {
            activeMoveSpeed = dodgeSpeed;
            dodgeCounter = dodgeLength;
        }
    }
    

    //NOT redundant
    public static float GetRotation(float lastRadiansFromNorth) {
        //gets rotation of player and if they are moving

        int horizontal = Convert.ToInt16(Input.GetAxisRaw("Horizontal"));
        int vertical = Convert.ToInt16(Input.GetAxisRaw("Vertical"));
        (int, int) hv = (horizontal, vertical);
        switch (hv) {
            case (0, -1):
                return 0;
            case (1, -1):
                return 45;
            case (1, 0):
                return 90;
            case (1, 1):
                return 135;
            case (0, 1):
                return 180;
            case (-1, 1):
                return 225;
            case (-1, 0):
                return 270;
            case (-1, -1):
                return 315;
            default:
                return (float)((lastRadiansFromNorth * 180) / Math.PI);
        }
    }


    void ChangeMovementType(ref MovementType movementType, ref float stamina, ref float speedModifier, float dodgeSpeed, float maxStamina) {
        //changes movement speed and stamina depending on wether play is running, sneaking or walking

        if (Input.GetKeyDown(KeyCode.LeftShift) && stamina > 10) {
            movementType = MovementType.Running;
            //ToggleInventory.shutFOVRun();
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl)) {
            movementType = MovementType.Sneaking;
            //ToggleInventory.openFOVRun();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && stamina >= 25f) {
            movementType = MovementType.Dodging;
            //Dodge();
        }
        else if ((Input.GetKeyUp(KeyCode.LeftControl) && movementType == MovementType.Sneaking) || 
            (Input.GetKeyUp(KeyCode.LeftShift) && movementType == MovementType.Running) ||
            stamina == 0) {
            movementType = MovementType.Walking;
        }   

        if (movementType == MovementType.Running) {
            speedModifier = 6f;
            stamina = Mathf.Clamp(stamina - (10 * Time.deltaTime), 0, maxStamina);
        }
        else if (movementType == MovementType.Sneaking) {
            speedModifier = 1.4f;
            stamina = Mathf.Clamp(stamina + (10 * Time.deltaTime), 0, maxStamina);
        }
        else {
            speedModifier = 4f;
            stamina = Mathf.Clamp(stamina + (5 * Time.deltaTime), 0, maxStamina);
        }
    }


    public enum MovementType {
        //enum for movement type
        Walking,
        Running,
        Sneaking,
        Dodging
    }
}
