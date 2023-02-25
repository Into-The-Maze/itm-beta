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

    //private Rigidbody2D playerBody;
    //float currentSpeed = 0f;
    //float weight = 10f;
    //float acceleration = 1f;
    //(bool, float) movement;
    //Vector2 moveVector;
    //Vector2 lastVector;

    float maxSpeed = 100f;
    public static float lastRadiansFromNorth = 0f;
    float speedModifier = 4f;
    public static float stamina = maxStamina;
    static float maxStamina = 50f;
    public static Vector3 movementDirection;

    public float dodgeSpeed;
    
    public static MovementType movementType = MovementType.Walking;

    public LayerMask wallMask;
    //public LayerMask playerMask;
    //public LayerMask itemMask;
    //public LayerMask vaultMask;

    void Start() {
        //playerBody = GetComponent<Rigidbody2D>();
        ChangeMovementType(ref movementType, ref stamina, ref speedModifier, dodgeSpeed, maxStamina);
        //movement = GetRotation(ref lastRadiansFromNorth);
        staminaBar.maxValue = maxStamina;
    }

    void Update(){
        ChangeMovementType(ref movementType, ref stamina, ref speedModifier, dodgeSpeed, maxStamina);

        //much simpler and more efficient, allows features to be added easily,
        //but you bounce weirdly against walls if you try to walk into them

        if (movementType != MovementType.Dodging) {
            simpleMovement(maxSpeed, speedModifier);
        }
        

        //movement = GetRotation(ref lastRadiansFromNorth);
        staminaBar.value = stamina;
        //fieldOfView.SetOrigin(transform.position);
    }

    void FixedUpdate() {
        //Movement(ref moveVector, ref lastVector, speedModifier, movement.Item1, ref lastRadiansFromNorth, movement.Item2, ref currentSpeed, maxSpeed, playerBody, acceleration);

    }

    private void simpleMovement(float maxSpeed, float speedModifier) {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(x, y, 0);
        movement = Vector3.ClampMagnitude(movement, maxSpeed);
        movementDirection = movement;
        transform.Translate(speedModifier * Time.deltaTime * movement);
    }

    //redundant
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

    //redundant
    static void Movement(ref Vector2 moveVector, ref Vector2 lastVector, float speedModifier, bool isMoving, ref float lastRadiansFromNorth, float radiansFromNorth, ref float currentSpeed, float maxSpeed, Rigidbody2D playerBody, float acceleration) {
        //moves the player with wasd at the correct speeds

        moveVector.y = Mathf.Cos(radiansFromNorth);
        moveVector.x = Mathf.Sin(radiansFromNorth);
        if (isMoving 
            && ((Math.Abs(radiansFromNorth - lastRadiansFromNorth) <= (Math.PI/4 + 0.2f))
            || playerBody.velocity.magnitude == 0 
            || (radiansFromNorth == 0f && lastRadiansFromNorth == Convert.ToSingle(1.75 * Math.PI)) 
            || (lastRadiansFromNorth == 0f && radiansFromNorth == Convert.ToSingle(1.75 * Math.PI))) 
            && ToggleInventory.invIsOpen == false)
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
            Dodge(dodgeSpeed);
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

    private void Dodge(float dodgeSpeed) {
        transform.Translate(dodgeSpeed * Time.deltaTime * movementDirection.normalized);
        stamina -= 25;
        movementType = MovementType.Walking;
    }

    public enum MovementType {
        //enum for movement type
        Walking,
        Running,
        Sneaking,
        Dodging
    }
}
