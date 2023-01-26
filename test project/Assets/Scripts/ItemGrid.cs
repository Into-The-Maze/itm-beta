using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    //iventory
    const float tileWidth = 160f;
    const float tileHeight = 160f;
    InventoryItem[,] inventoryItemSlot;
    [SerializeField] int gridSizeWidth;
    [SerializeField] int gridSizeHeight;

    //getting mouse position
    RectTransform rectTransform;
    Vector2 posOnGrid = new Vector2();
    Vector2Int tileGridPosition = new Vector2Int();

    //test stuff
    [SerializeField] GameObject ItemPrefab;
    
    private void Start() {
        rectTransform = GetComponent<RectTransform>();
        Init(gridSizeWidth, gridSizeHeight);
        
        //test
        InventoryItem inventoryItem = Instantiate(ItemPrefab).GetComponent<InventoryItem>();
        PlaceItem(inventoryItem, 3, 2);

        inventoryItem = Instantiate(ItemPrefab).GetComponent<InventoryItem>();
        PlaceItem(inventoryItem, 0, 0);
        inventoryItem = Instantiate(ItemPrefab).GetComponent<InventoryItem>();
        PlaceItem(inventoryItem, 2, 1);
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
    public void PlaceItem(InventoryItem inventoryItem, int posX, int posY) {
        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);
        inventoryItemSlot[posX, posY] = inventoryItem;

        Vector2 position = new Vector2();
        position.x = posX * tileWidth + tileWidth / 2;
        position.y = -(posY * tileHeight + tileHeight / 2);

        rectTransform.localPosition = position;
    }
    public InventoryItem PickUpItem(int x, int y) {
        InventoryItem toReturn = inventoryItemSlot[x, y];
        inventoryItemSlot[x, y] = null;
        return toReturn;
    }

}
