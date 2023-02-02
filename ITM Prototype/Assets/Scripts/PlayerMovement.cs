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

    private Rigidbody2D playerBody;
    public Slider staminaBar;

    float currentSpeed = 0f;
    //float weight = 10f;
    float maxSpeed = 5f;
    float acceleration = 0.6f;
    static float maxStamina = 50f;
    float stamina = maxStamina;
    float speedModifier = 1f;
    (bool, float) movement;
    float lastRadiansFromNorth = 0f;
    Vector2 moveVector;
    Vector2 lastVector;

    MovementType movementType = MovementType.Walking;

    public LayerMask wallMask;
    //public LayerMask playerMask;
    //public LayerMask itemMask;
    //public LayerMask vaultMask;

    void Start() {
        playerBody = GetComponent<Rigidbody2D>();
        ChangeMovementType(ref movementType, ref stamina, ref speedModifier, maxStamina);
        movement = GetRotation(ref lastRadiansFromNorth);
        staminaBar.maxValue = maxStamina;
    }

    void Update(){
        ChangeMovementType(ref movementType, ref stamina, ref speedModifier, maxStamina);
        movement = GetRotation(ref lastRadiansFromNorth);
        staminaBar.value = stamina;

    }
    void FixedUpdate() {
        Movement(ref moveVector, ref lastVector, speedModifier, movement.Item1, ref lastRadiansFromNorth, movement.Item2, ref currentSpeed, maxSpeed, playerBody, acceleration);
    }

    static (bool, float) GetRotation(ref float lastRadiansFromNorth) {
        //gets rotation of player and if they are moving

        int horizontal = Convert.ToInt16(Input.GetAxisRaw("Horizontal"));
        int vertical = Convert.ToInt16(Input.GetAxisRaw("Vertical"));
        (int, int) hv = (horizontal, vertical);
        switch (hv)
        {
            case (0, 1):
                return (true, 0);
            case (1, 1):
                return (true, Convert.ToSingle(0.25 * Math.PI));
            case (1, 0):
                return (true, Convert.ToSingle(0.5 * Math.PI));
            case (1, -1):
                return (true, Convert.ToSingle(0.75 * Math.PI));
            case (0, -1):
                return (true, Convert.ToSingle(Math.PI));
            case (-1, -1):
                return (true, Convert.ToSingle(1.25 * Math.PI));
            case (-1, 0):
                return (true, Convert.ToSingle(1.5 * Math.PI));
            case (-1, 1):
                return (true, Convert.ToSingle(1.75 * Math.PI));
            default:
                return (false, lastRadiansFromNorth);
        }
    }

    static void Movement(ref Vector2 moveVector, ref Vector2 lastVector, float speedModifier, bool isMoving, ref float lastRadiansFromNorth, float radiansFromNorth, ref float currentSpeed, float maxSpeed, Rigidbody2D playerBody, float acceleration) {
        //moves the player with wasd at the correct speeds

        moveVector.y = Mathf.Cos(radiansFromNorth);
        moveVector.x = Mathf.Sin(radiansFromNorth);
        if (isMoving 
            && ((Math.Abs(radiansFromNorth - lastRadiansFromNorth) <= (Math.PI/4 + 0.2f))
            || playerBody.velocity.magnitude == 0 
            || (radiansFromNorth == 0f && lastRadiansFromNorth == Convert.ToSingle(1.75 * Math.PI)) 
            || (lastRadiansFromNorth == 0f && radiansFromNorth == Convert.ToSingle(1.75 * Math.PI))))
            {
            currentSpeed = Mathf.Clamp((currentSpeed + acceleration), 0, ((maxSpeed - 1) * speedModifier));
            playerBody.velocity = moveVector * currentSpeed;
            lastRadiansFromNorth = radiansFromNorth;
            lastVector = moveVector;
        }
        else {
            currentSpeed = Mathf.Clamp((currentSpeed - acceleration), 0, ((maxSpeed - 1) * speedModifier));
            
            playerBody.velocity = lastVector * currentSpeed;
        }
    }

    static void ChangeMovementType(ref MovementType movementType, ref float stamina, ref float speedModifier, float maxStamina) {
        //changes movement speed and stamina depending on wether play is running, sneaking or walking

        if (Input.GetKeyDown("left shift") && stamina > 10) {
            movementType = MovementType.Running;
        }
        else if (Input.GetKeyDown("left ctrl")) {
            movementType = MovementType.Sneaking;
        }
        else if ((Input.GetKeyUp("left ctrl") && movementType == MovementType.Sneaking) || 
            (Input.GetKeyUp("left shift") && movementType == MovementType.Running) ||
            stamina == 0) {
            movementType = MovementType.Walking;
        }   

        if (movementType == MovementType.Running) {
            speedModifier = 2f;
            stamina = Mathf.Clamp(stamina - (10 * Time.deltaTime), 0, maxStamina);
        }
        else if (movementType == MovementType.Sneaking) {
            speedModifier = 0.5f;
            stamina = Mathf.Clamp(stamina + (10 * Time.deltaTime), 0, maxStamina);
        }
        else {
            speedModifier = 1f;
            stamina = Mathf.Clamp(stamina + (5 * Time.deltaTime), 0, maxStamina);
        }
    }

    public enum MovementType {
        //enum for movement type
        Walking,
        Running,
        Sneaking
    }
}
