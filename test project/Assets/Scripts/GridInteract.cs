using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//makes the code work
[RequireComponent(typeof(ItemGrid))]
public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    InvController inventoryController;
    ItemGrid itemGrid;

    //interfaces which check if the cursor enters/leaves the grid.
    public void OnPointerEnter(PointerEventData eventData) {
        inventoryController.selectedItemGrid = itemGrid;
    }
    public void OnPointerExit(PointerEventData eventData) {
        inventoryController.selectedItemGrid = null;
    }

    //can be optimised out according to russian youtube man
    private void Awake() {
        inventoryController = FindObjectOfType(typeof(InvController)) as InvController;
        itemGrid = GetComponent<ItemGrid>();
    }
}
