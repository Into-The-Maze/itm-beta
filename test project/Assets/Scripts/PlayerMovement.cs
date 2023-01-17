using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D playerBody;

    public float speed = 10f;
    public float weight = 10f;
    public float maxSpeed = 10f;
    public float maxStamina = 50f;
    public float stamina;
    public MovementType movementType = MovementType.Walking;
    public float moveDirection = 0;

    public LayerMask wallMask;
    //public LayerMask playerMask;
    //public LayerMask itemMask;
    //public LayerMask vaultMask;

    void Start()
    {
        maxStamina--;
        stamina = maxStamina;
    }

    void Update()
    {
        MovementType(ref movementType, ref stamina, speed, maxStamina);
        (bool, float) movement = GetRotation();
        Movement(movement.Item1, movement.Item2, speed, maxSpeed, playerBody);
    }


    //gets rotation of player and if they are moving
    static (bool, float) GetRotation()
    {
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
        }
    }

    //moves the player with wasd at the correct speeds
    static void Movement(bool isMoving, float radiansFromNorth, float speed, float maxSpeed, Rigidbody2D playerBody)
    {
        if (isMoving)
        {
            Vector2 moveVector;
            moveVector.y = Mathf.Cos(radiansFromNorth); 
            moveVector.x = Mathf.Sin(radiansFromNorth);
            Debug.Log(radiansFromNorth);
            Debug.Log(moveVector);
            Debug.Log(playerBody.velocity);
            playerBody.velocity += moveVector * Time.deltaTime * speed;
            // clamp to max speed
        }
        else
        {
            playerBody.velocity = Vector2.zero;

            //slow down to stop when not moving
        }
    }

    //changes movement speed and stamina depending on wether play is running, sneaking or walking
    static void MovementType(ref string movementType, ref float stamina, float speed, float maxStamina)
    {
        if (Input.GetKeyDown("left shift") && stamina > 10)
        {
            movementType = MovementType.Walking;
        }
        else if (Input.GetKeyDown("left ctrl"))
        {
            movementType = MovementType.Sneaking;
        }
        else if ((Input.GetKeyUp("left ctrl") && movementType == MovementType.Sneaking) || 
            (Input.GetKeyUp("left shift") && movementType == MovementType.Running) ||
            stamina == 0)
        {
            movementType = MovementType.Walking;
        }   

        if (movementType == MovementType.Running)
        {
            speed = 10f;
            stamina = Mathf.Clamp(stamina - (10 * Time.deltaTime), 0, maxStamina + 1);
            //Debug.Log($"running {stamina}");
        }
        else if (movementType == MovementType.Sneaking)
        {
            speed = 3f;
            stamina = Mathf.Clamp(stamina + (10 * Time.deltaTime), 0, maxStamina + 1);
            //Debug.Log($"sneaking {stamina}");
        }
        else
        {
            speed = 6f;
            stamina = Mathf.Clamp(stamina + (5 * Time.deltaTime), 0, maxStamina + 1);
            //Debug.Log($"walking {stamina}");
        }
    }
    
    private enum MovementType {
        Walking,
        Running,
        Sneaking
    }
}
