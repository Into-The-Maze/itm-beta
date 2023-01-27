using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    //iventory
    public const float tileWidth = 160f;
    public const float tileHeight = 160f;
    InventoryItem[,] inventoryItemSlot;
    [SerializeField] int gridSizeWidth;
    [SerializeField] int gridSizeHeight;

    //getting mouse position
    RectTransform rectTransform;
    Vector2 posOnGrid = new Vector2();
    Vector2Int tileGridPosition = new Vector2Int();

    
    private void Start() {
        rectTransform = GetComponent<RectTransform>();
        Init(gridSizeWidth, gridSizeHeight);
        
    }

    private void Init(int width, int height) {
        inventoryItemSlot = new InventoryItem[width, height];
        Vector2 size = new Vector2(width * tileWidth, height * tileHeight);
        rectTransform.sizeDelta = size;
    }

    public Vector2Int GetTileGridPos(Vector2 mousePos) {
        posOnGrid.x = mousePos.x - rectTransform.position.x;
        posOnGrid.y = rectTransform.position.y - mousePos.y;
        tileGridPosition.x = (int)(posOnGrid.x / tileWidth);
        tileGridPosition.y = (int)(posOnGrid.y / tileHeight);
        return tileGridPosition;
    }

    public bool PlaceItem(InventoryItem inventoryItem, int posX, int posY, ref InventoryItem overlapItem) {
        if (!BoundaryCheck(posX, posY, inventoryItem.itemData.width, inventoryItem.itemData.height)) {
            return false;
        }
        if (!OverlapCheck(posX, posY, inventoryItem.itemData.width, inventoryItem.itemData.height, ref overlapItem)) {
            overlapItem = null;
            return false;
        }
        if (overlapItem != null) {
            CleanGridReference(overlapItem);
        }

        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

        for (int i = 0; i < inventoryItem.itemData.width; i++) {
            for (int j = 0; j < inventoryItem.itemData.height; j++) {
                inventoryItemSlot[posX + i, posY + j] = inventoryItem;
            }
        }

        inventoryItem.onGridPosX = posX;
        inventoryItem.onGridPosY = posY;
        Vector2 position = CalculatePositionOnGrid(inventoryItem, posX, posY);

        rectTransform.localPosition = position;

        return true;
    }

    public Vector2 CalculatePositionOnGrid(InventoryItem inventoryItem, int posX, int posY) {
        Vector2 position = new Vector2();
        position.x = posX * tileWidth + tileWidth * inventoryItem.itemData.width / 2;
        position.y = -(posY * tileHeight + tileHeight * inventoryItem.itemData.height / 2);
        return position;
    }

    private bool OverlapCheck(int posX, int posY, int width, int height, ref InventoryItem overlapItem) {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (inventoryItemSlot[posX + i, posY + j] != null) {
                    if (overlapItem == null) {
                        overlapItem = inventoryItemSlot[posX + i, posY + j];
                    }
                    else if (overlapItem != inventoryItemSlot[posX+i, posY+j]){
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public InventoryItem PickUpItem(int x, int y) {
        InventoryItem toReturn = inventoryItemSlot[x, y];

        if (toReturn == null) { return null; }

        CleanGridReference(toReturn);
        return toReturn;
    }

    private void CleanGridReference(InventoryItem item) {
        for (int i = 0; i < item.itemData.width; i++) {
            for (int j = 0; j < item.itemData.height; j++) {
                inventoryItemSlot[item.onGridPosX + i, item.onGridPosY + j] = null;
            }
        }
    }

    bool PositionCheck(int posX, int posY) {
        if (posX < 0 || posY < 0) {
            return false;
        }
        if (posX >= gridSizeWidth || posY >= gridSizeHeight) {
            return false;
        }
        return true;
    }

    public bool BoundaryCheck(int posX, int posY, int width, int height) {
        if (!PositionCheck(posX, posY)) { return false; }

        posX += width-1;
        posY += height-1;

        if (!PositionCheck(posX, posY)) { return false; }

        return true;
    }

    internal InventoryItem GetItem(int x, int y) {
        return inventoryItemSlot[x, y];
    }
}
