using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Calculation of item size for highlighting can be optimised

public class InvController : MonoBehaviour
{
    [HideInInspector] //avoids confusion with assigning script
    private ItemGrid selectedItemGrid;

    //holds item currently being dragged
    InventoryItem selectedItem;
    InventoryItem overlapItem;
    RectTransform rectTransform;

    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform canvasTransform;

    InventoryHighlight inventoryHighlight;
    Vector2Int oldPosition;
    InventoryItem itemToHighlight;

    public ItemGrid SelectedItemGrid {
        get => selectedItemGrid;
        set {
            selectedItemGrid = value;
            inventoryHighlight.SetParent(value);
        }
    }

    private void Awake() {
        inventoryHighlight = GetComponent<InventoryHighlight>();
    }

    private void Update() {
        //stops grid interaction if not hovering over it
        ItemIconDrag();
        

        if(Input.GetKeyDown(KeyCode.Q)) {
            
            if (selectedItem == null) {
                CreateRandomItem();
            }
        }//test script

        if (Input.GetKeyDown(KeyCode.E)) {
            InsertRandomItem();
        }//test script

        if (Input.GetKeyDown(KeyCode.R)) {
            RotateItem();
            //inventoryHighlight.SetSize(selectedItem); this needs bugfixing for best looks
        }

        if (selectedItemGrid == null) {
            inventoryHighlight.Show(false);
            return;
        }

        HandleHighlight();

        if (Input.GetMouseButtonDown(0)) {
            MoveItem();
        }
    }

    private void RotateItem() {
        if (selectedItem == null) { return; }

        selectedItem.Rotate();
    }

    private void InsertRandomItem() {
        if (selectedItemGrid == null) { return; }
        CreateRandomItem();
        InventoryItem itemToInsert = selectedItem;
        selectedItem = null;
        InsertItem(itemToInsert);
    }//test script

    private void InsertItem(InventoryItem itemToInsert) {

        
        Vector2Int? posOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);
        if (posOnGrid == null) { return; }

        selectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }

    private void HandleHighlight() {
        Vector2Int positionOnGrid = GetTileGridPosition();
        if (oldPosition == positionOnGrid) { return; }
        oldPosition = positionOnGrid;

        if (selectedItem == null) {
            itemToHighlight = selectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);

            if (itemToHighlight != null) {
                inventoryHighlight.Show(true);
                inventoryHighlight.SetSize(itemToHighlight);
                inventoryHighlight.SetPosition(selectedItemGrid, itemToHighlight);
            }
            else {
                inventoryHighlight.Show(false);
            }
        }
        else {
            inventoryHighlight.Show(selectedItemGrid.BoundaryCheck(positionOnGrid.x, positionOnGrid.y, selectedItem.rotateWidth, selectedItem.rotateHeight));
            inventoryHighlight.SetSize(selectedItem);
            inventoryHighlight.SetPosition(selectedItemGrid, selectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }

    private void CreateRandomItem() {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;

        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);
        rectTransform.SetAsLastSibling();

        int selectedItemID = UnityEngine.Random.Range(0, items.Count);
        inventoryItem.Set(items[selectedItemID]);
    }//for testing: generates item on mouse

    private void MoveItem() {
        Vector2Int tileGridPosition = GetTileGridPosition();

        if (selectedItem == null) {
            PickUpItem(tileGridPosition);
        }
        else {
            DropItem(tileGridPosition);
        }
    }

    private Vector2Int GetTileGridPosition() {
        Vector2 position = Input.mousePosition;

        if (selectedItem != null) {
            position.x -= (selectedItem.rotateWidth - 1) * ItemGrid.tileWidth / 2;
            position.y += (selectedItem.rotateHeight - 1) * ItemGrid.tileHeight / 2;
        }

        Vector2Int tileGridPosition = selectedItemGrid.GetTileGridPos(position);
        return tileGridPosition;
    }

    private void DropItem(Vector2Int tileGridPosition) {
        bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);
        if (complete) {
            selectedItem = null;
            if (overlapItem != null) {
                selectedItem = overlapItem;
                overlapItem = null;
                rectTransform = selectedItem.GetComponent<RectTransform>();
                rectTransform.SetAsLastSibling();
            }
        }
    }

    private void PickUpItem(Vector2Int tileGridPosition) {
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (selectedItem != null) {
            rectTransform = selectedItem.GetComponent<RectTransform>();
        }
    }

    private void ItemIconDrag() {
        if (selectedItem != null) {
            rectTransform.position = Input.mousePosition;
        }
    }


}
