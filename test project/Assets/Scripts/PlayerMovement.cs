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
    public float speed = 5f;
    public float weight = 10f;
    public float maxSpeed = 10f;
    public float maxStamina = 50f;
    float stamina;
    string movementType = "walking";
    float moveDirection = 0;

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
        (bool, int) movement = GetRotation();
        Movement(movement.Item1, movement.Item2, speed);
    }


    //gets rotation of player and if they are moving
    static (bool isMoving, int degreesFromNorth) GetRotation()
    {
        int horizontal = Convert.ToInt16(Input.GetAxisRaw("Horizontal"));
        int vertical = Convert.ToInt16(Input.GetAxisRaw("Vertical"));
        (int, int) hv = (horizontal, vertical);
        switch (hv)
        {
            case (0, 1):
                return (true, 0);
            case (1, 1):
                return (true, 45);
            case (1, 0):
                return (true, 90);
            case (1, -1):
                return (true, 135);
            case (0, -1):
                return (true, 180);
            case (-1, -1):
                return (true, 225);
            case (-1, 0):
                return (true, 270);
            case (-1, 1):
                return (true, 315);
            default:
                return (false, 0);
        }
    }

    //moves the player with wasd at the correct speeds
    static void Movement(bool isMoving, int moveDirection, float speed)
    {
        if (isMoving)
        {
            //add speed to current direction up to max speed
            Vector2 moveVector;
            moveVector.x = Mathf.Cos(moveDirection)*speed; 
            moveVector.y = Mathf.Sin(moveDirection)*speed;
            
        }
        else
        {
            //slow down to stop when not moving
        }
    }

    //changes movement speed and stamina depending on wether play is running, sneaking or walking
    static void MovementType(ref string movementType, ref float stamina, float speed, float maxStamina)
    {
        if (Input.GetKeyDown("left shift") && stamina > 10)
        {
            movementType = "running";
        }
        else if (Input.GetKeyDown("left ctrl"))
        {
            movementType = "sneaking";
        }
        else if ((Input.GetKeyUp("left ctrl") && movementType == "sneaking") || 
            (Input.GetKeyUp("left shift") && movementType == "running") ||
            stamina == 0)
        {
            movementType = "walking";
        }   

        if (movementType == "running")
        {
            speed = 10f;
            stamina = Mathf.Clamp(stamina - (10 * Time.deltaTime), 0, maxStamina + 1);
            //Debug.Log($"running {stamina}");
        }
        else if (movementType == "sneaking")
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
}
