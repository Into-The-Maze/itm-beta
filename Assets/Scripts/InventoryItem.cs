using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour {
    public ItemData itemData;
    public int itemDataElementReference;
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

    public bool isWeapon { get { return itemData.isWeapon; } }
    public bool isArmor { get { return itemData.isArmor; } }
    public bool isHeadgear { get { return itemData.isHeadgear; } }
    public bool isRing { get { return itemData.isRing; } }
    public bool isHeavy { get { return itemData.isHeavy; } }

    public float Damage {
        get {
            return itemData.damage;
        }
    }




    internal void Rotate() {
        rotated = !rotated;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0, 0, (rotated == true) ? 90f : 0f);
    }

    internal void Set(ItemData itemData) {
        this.itemData = itemData;
        if (Attack.rangedWeapons.Contains(itemData.weaponType)) {
            itemData.currentMagazineCapacity = UnityEngine.Random.Range(0, itemData.maxMagazineCapacity + 1);
        }
        if (itemData.weaponType == ItemData.WeaponType.Pistol || itemData.weaponType == ItemData.WeaponType.Rifle) {
            GetComponent<Image>().sprite = (itemData.currentMagazineCapacity <= 0) ? itemData.itemIcon_GunEmpty : itemData.itemIcon_GunLoaded;
        }
        else {
            GetComponent<Image>().sprite = itemData.itemIcon;
        }
        


        Vector2 size = new Vector2();
        size.x = itemData.width * ItemGrid.tileWidth;
        size.y = itemData.height * ItemGrid.tileHeight;
        GetComponent<RectTransform>().sizeDelta = size;
    }

    
}
