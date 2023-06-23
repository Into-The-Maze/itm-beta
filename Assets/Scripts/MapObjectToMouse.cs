using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectToMouse : MonoBehaviour
{

    private Camera cam2;
    // Start is called before the first frame update
    void Start()
    {
        cam2 = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = cam2.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos;
    }
}
