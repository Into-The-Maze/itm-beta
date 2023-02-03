using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerSpriteFaceTowardsMouse : MonoBehaviour
{
    private Camera cam;
    
    float turnSpeed = 200f;
    

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (ToggleInventory.invIsOpen == false) {
            Quaternion thing = Quaternion.LookRotation(Vector3.forward, transform.position - cam.ScreenToWorldPoint(Input.mousePosition));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, thing, turnSpeed * Time.deltaTime);
        }
    }
}
