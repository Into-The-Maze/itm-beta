using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public ItemData itemData;
    public int onGridPosX;
    public int onGridPosY;

    public bool rotated = false;
    public int rotateHeight {
        get {
            if (rotated == false) {
                return itemData.height;
            }
            return itemData.width;
        }
    }
    public int rotateWidth {
        get {
            if (rotated == false) {
                return itemData.width;
            }
            return itemData.height;
        }
    }

    internal void Rotate() {
        rotated = !rotated;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0, 0, (rotated == true) ? 90f : 0f);
    }

    internal void Set(ItemData itemData) {
        this.itemData = itemData;

        GetComponent<Image>().sprite = itemData.itemIcon;

        Vector2 size = new Vector2();
        size.x = itemData.width * ItemGrid.tileWidth;
        size.y = itemData.height * ItemGrid.tileHeight;
        GetComponent<RectTransform>().sizeDelta = size;
    }
}
