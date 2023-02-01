using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleInventory : MonoBehaviour
{
    RectTransform rectTransform;
    bool invIsOpen = false;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            ToggleInv(ref invIsOpen);
        }
    }
    void ToggleInvSmooth(ref bool invIsOpen) {
        while (true) {
            rectTransform = GetComponent<RectTransform>();
            Vector3 currentPos = new Vector3(rectTransform.position.x, rectTransform.position.y, rectTransform.position.z);
            if (!invIsOpen) {
                rectTransform.transform.position = Vector3.MoveTowards(currentPos, new Vector3(0, 1080, 0), 50 * Time.deltaTime);
                invIsOpen = true;
                break;
            }
            else {
                rectTransform.transform.position = Vector3.MoveTowards(currentPos, new Vector3(-640, 1080, 0), 50 * Time.deltaTime);
                invIsOpen = false;
                break;
            }
        }
    } //dont know why this doesnt work, something to do with frames
    void ToggleInv(ref bool invIsOpen) {
        rectTransform = GetComponent<RectTransform>();
        if (!invIsOpen) {
            rectTransform.transform.position = new Vector3(0, 1080, 0);
            invIsOpen = true;
        }
        else {
            rectTransform.transform.position = new Vector3(-640, 1080, 0);
            invIsOpen = false;
        }
    } //works but kind of ugly
}
