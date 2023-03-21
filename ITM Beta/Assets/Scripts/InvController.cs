using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

//Calculation of item size for highlighting can be optimised

public class InvController : MonoBehaviour
{
    public static InvController itemDrops;

    [HideInInspector] //avoids confusion with assigning script
    private ItemGrid selectedItemGrid;
    private long itemAutoNum = 0;

    //holds item currently being dragged
    InventoryItem selectedItem;
    InventoryItem overlapItem;
    RectTransform rectTransform;

    [SerializeField] GameObject ThrownItem;
    [SerializeField] GameObject playerSprite;
    [SerializeField] GameObject playerLocation;
    [SerializeField] public List<ItemData> items;
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
        itemDrops = this;
    }
    private void Update() {
        ItemIconDrag();

        if(Input.GetKeyDown(KeyCode.Q)) {
            
            if (selectedItem == null) {
                CreateRandomItem();
            }
        }//test script

        

        if (Input.GetKeyDown(KeyCode.R)) {
            RotateItem();
            //inventoryHighlight.SetSize(selectedItem); this needs bugfixing for best looks
        }

        if (Input.GetMouseButtonDown(1) && selectedItemGrid == null && selectedItem != null) {
            throwItem();
        }
        else if (Input.GetMouseButtonDown(1) && selectedItemGrid == null && selectedItem == null) {
            PickUpItemFromFloor();
        }

        if (selectedItemGrid == null) {
            inventoryHighlight.Show(false);
            return;
        }
        

        HandleHighlight();

        if (Input.GetMouseButtonDown(0) && selectedItemGrid != null) {
            MoveItem();
        }
    }

    private void PickUpItemFromFloor() {

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if(hit.collider != null) {
            if (hit.collider.gameObject != null) {
                if (hit.collider.gameObject.CompareTag("FLOORITEM")) {
                    if (Vector3.Distance(playerSprite.transform.position, hit.transform.gameObject.transform.position) < 4f) {
                        int index = hit.collider.gameObject.GetComponent<ItemDataDump>().itemDataElementReference;
                        Destroy(hit.collider.gameObject);
                        CreatePickedUpItem(index);
                    }
                }
            }
        }
    }
    private void CreatePickedUpItem(int index) {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        inventoryItem.name = $"inv{itemAutoNum}";
        selectedItem = inventoryItem;

        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);
        rectTransform.SetAsLastSibling();

        inventoryItem.Set(items[index]);
        inventoryItem.itemDataElementReference = index;
        itemAutoNum++;
    }
    private void throwItem() {
        ThrownItem.GetComponent<SpriteRenderer>().sprite = selectedItem.itemData.itemIcon;
        ThrownItem.GetComponent<ItemDataDump>().itemDataElementReference = selectedItem.itemDataElementReference;
        Destroy(GameObject.Find(selectedItem.name));
        selectedItem = null;
        Instantiate(ThrownItem, playerLocation.transform.TransformPoint(playerSprite.transform.up * -1.5f), playerSprite.transform.rotation);
    }
    private void RotateItem() {
        if (selectedItem == null) { return; }

        selectedItem.Rotate();
    }
    private void InsertItem(InventoryItem itemToInsert) {

        
        Vector2Int? posOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);
        if (posOnGrid == null) { return; }

        selectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }
    private void HandleHighlight() {
        if (selectedItemGrid == null) { return; }
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
        inventoryItem.name = $"inv{itemAutoNum}";
        selectedItem = inventoryItem;

        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);
        rectTransform.SetAsLastSibling();

        int selectedItemID = UnityEngine.Random.Range(0, items.Count);
        inventoryItem.Set(items[selectedItemID]);
        inventoryItem.itemDataElementReference = selectedItemID;
        itemAutoNum++;
    }//for testing: generates item on mouse
    private void MoveItem() {
        Vector2Int tileGridPosition = GetTileGridPosition();

        if (selectedItem == null) {
            //Debug.Log("picking up item");
            PickUpItem(tileGridPosition);
        }
        else {
            //Debug.Log("Placing item");
            switch (selectedItemGrid.tag) {
                case "WEAPONSLOT":
                    if (!selectedItem.isWeapon) { return; }
                    break;
                case "RINGSLOT":
                    if (!selectedItem.isRing) { return; }
                    break;
                case "CHESTSLOT":
                    if (!selectedItem.isArmor) { return; }
                    break;
                case "HEADSLOT":
                    if (!selectedItem.isHeadgear) { return; }
                    break;
            }
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
        if (selectedItemGrid.tag == "INVENTORY") {
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
        else {
            bool complete = selectedItemGrid.PlaceItemEquip(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);
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
        
    }
    private void PickUpItem(Vector2Int tileGridPosition) {
        if (selectedItemGrid.tag == "INVENTORY") {
            selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
            if (selectedItem != null) {
                rectTransform = selectedItem.GetComponent<RectTransform>();
            }
        }
        else {
            selectedItem = selectedItemGrid.PickUpItemEquip(tileGridPosition.x, tileGridPosition.y);
            if (selectedItem != null) {
                rectTransform = selectedItem.GetComponent<RectTransform>();
            }
        }
        
    }
    private void ItemIconDrag() {
        if (selectedItem != null) {
            rectTransform.position = Input.mousePosition;
        }
    }


}
