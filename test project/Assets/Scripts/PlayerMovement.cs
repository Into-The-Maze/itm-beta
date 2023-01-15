using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float weight = 10f;
    public float maxSpeed = 10f;
    public float maxStamina = 50f;
    float stamina;
    string movementType = "walking";

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
    }

    static void Movement(ref float speed)
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float moveDirection = horizontal * 90;
        if (vertical == 1)
        {
            moveDirection = moveDirection / 2;
        }
        else if (vertical == -1)
        {
            moveDirection = 180;
            moveDirection -= (horizontal * 45);
        }
        //Debug.Log($"{moveDirection}");

        
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
