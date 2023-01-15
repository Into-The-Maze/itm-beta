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
    string movement = "walking";

    public LayerMask wallMask;
    //public LayerMask itemMask;
    //public LayerMask vaultMask;
    //public LayerMask slowMask;

    void Start()
    {
        maxStamina--;
        stamina = maxStamina;
    }

    void Update()
    {
        movementType(ref movement, ref stamina, speed, maxStamina);
    }

    //changes movement speed and stamina depending on wether play is running, sneaking or walking
    static void movementType(ref string movement, ref float stamina, float speed, float maxStamina)
    {
        if (Input.GetKeyDown("left shift") && stamina > 10)
        {
            movement = "running";
        }
        else if (Input.GetKeyDown("left ctrl"))
        {
            movement = "sneaking";
        }
        else if ((Input.GetKeyUp("left ctrl") && movement == "sneaking") || 
            (Input.GetKeyUp("left shift") && movement == "running") ||
            stamina == 0)
        {
            movement = "walking";
        }   

        if (movement == "running")
        {
            speed = 10f;
            stamina = Mathf.Clamp(stamina - (10 * Time.deltaTime), 0, maxStamina + 1);
            //Debug.Log($"running {stamina}");
        }
        else if (movement == "sneaking")
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
