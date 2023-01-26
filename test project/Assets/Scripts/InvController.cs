using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvController : MonoBehaviour
{
    [HideInInspector] //avoids confusion with assigning script
    public ItemGrid selectedItemGrid;

    //holds item currently being dragged
    InventoryItem selectedItem;
    RectTransform rectTransform;

    private void Update() {
        //stops grid interaction if not hovering over it
        if (selectedItem != null) {
            rectTransform.position = Input.mousePosition;
        }
        
        if (selectedItemGrid == null) { 
            return; 
        }

        if (Input.GetMouseButtonDown(0)) {
            Vector2Int tileGridPosition = selectedItemGrid.GetTileGridPos(Input.mousePosition);

            if (selectedItem == null) {
                selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
                if (selectedItem != null ) {
                    rectTransform = selectedItem.GetComponent<RectTransform>();
                }
            }
            else {
                selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y);
                selectedItem = null;
            }
        }
    }
}
