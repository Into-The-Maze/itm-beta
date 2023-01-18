using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;


//ouch
Vector2 lastVector;
lastVector.x = 0;
lastVector.y = 0;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody2D playerBody;

    float currentSpeed = 0f;
    //float weight = 10f;
    float maxSpeed = 6f;
    float acceleration = 0.1f;
    static float maxStamina = 50f;
    float stamina = maxStamina;
    float moveDirection = 0;
    float speedModifier = 1f;
    (bool, float) movement;
    
    

    MovementType movementType = MovementType.Walking;

    public LayerMask wallMask;
    //public LayerMask playerMask;
    //public LayerMask itemMask;
    //public LayerMask vaultMask;

    void Start() {
        playerBody = GetComponent<Rigidbody2D>();
        ChangeMovementType(ref movementType, ref stamina, ref speedModifier, maxStamina);
        movement = GetRotation();
    }

    void Update(){
        ChangeMovementType(ref movementType, ref stamina, ref speedModifier, maxStamina);
        movement = GetRotation();

    }
    void FixedUpdate() {
        Movement(speedModifier, movement.Item1, movement.Item2, ref currentSpeed, maxSpeed, playerBody, acceleration, lastVector);
    }


    static (bool, float) GetRotation() {
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
                return (false, 0);
                //0 unused
        }
    }

    static void Movement(float speedModifier, bool isMoving, float radiansFromNorth, ref float currentSpeed, float maxSpeed, Rigidbody2D playerBody, float acceleration) {
        //moves the player with wasd at the correct speeds

        Vector2 moveVector;
        moveVector.y = Mathf.Cos(radiansFromNorth);
        moveVector.x = Mathf.Sin(radiansFromNorth);
        
        

        if (isMoving) {
            currentSpeed = Mathf.Clamp((currentSpeed + acceleration), 0, ((maxSpeed - 1) * speedModifier));
            playerBody.velocity = moveVector * currentSpeed;
            lastVector = moveVector;
            //Debug.Log($"{playerBody.velocity.magnitude}");
        }
        else {
            currentSpeed = Mathf.Clamp((currentSpeed - acceleration), 0, ((maxSpeed - 1) * speedModifier));
            playerBody.velocity = lastVector * (maxSpeed - currentSpeed);
        }
        lastVector.x = moveVector.x;
        lastVector.y = moveVector.y;
        Debug.Log(//$"moveVector:{moveVector}, " +
        //    $"velocity:{playerBody.velocity}, " +
            $"currentSpeed:{currentSpeed}, " +
        //    $"velocity.magnitude:{playerBody.velocity.magnitude}, " +
            $"acceleration:{acceleration}");
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
            Debug.Log($"running {stamina}");
        }
        else if (movementType == MovementType.Sneaking) {
            speedModifier = 0.5f;
            stamina = Mathf.Clamp(stamina + (10 * Time.deltaTime), 0, maxStamina);
            Debug.Log($"sneaking {stamina}");
        }
        else {
            speedModifier = 1f;
            stamina = Mathf.Clamp(stamina + (5 * Time.deltaTime), 0, maxStamina);
            Debug.Log($"walking {stamina}");
        }
    }

    public enum MovementType {
        //enum for movement type
        Walking,
        Running,
        Sneaking
    }
}
