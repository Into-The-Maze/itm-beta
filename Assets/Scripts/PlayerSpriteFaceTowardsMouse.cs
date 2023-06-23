using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class PlayerSpriteFaceTowardsMouse : MonoBehaviour
{
    //[SerializeField] private FieldOfView fieldOfView;
    private Camera cam;
    public static float turnSpeed = 200f;
    
    void Start()
    {
        cam = Camera.main;
    }
    void Update()
    {
        if (Attack.CurrentlySwinging) { return; }
        if (Attack.attackItem != null){
            if (Attack.attackItem.isHeavy && Attack.CurrentlyAttacking) { return; }
        }
        if (ToggleInventory.invIsOpen == false) {
            if (PlayerMovement.movementType != PlayerMovement.MovementType.Running) {
                Quaternion thing = Quaternion.LookRotation(Vector3.forward, transform.position - cam.ScreenToWorldPoint(Input.mousePosition));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, thing, turnSpeed * Time.deltaTime);
            }
            else {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, PlayerMovement.GetRotation(PlayerMovement.lastRadiansFromNorth)), turnSpeed * Time.deltaTime);
            }
            //fieldOfView.SetAimDirection(transform.right);
        }   
    }
}
