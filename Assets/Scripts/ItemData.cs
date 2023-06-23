using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
    
//Populate with data to make more item types

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    //fill for all items
    public int width = 1;
    public int height = 1;
    public Sprite itemIcon;
    public string flavourText;

    //for guns only, leave blank for other items
    public Sprite itemIcon_GunEmpty;
    public Sprite itemIcon_GunLoaded;
    public GameObject model_Gun;
    public int maxMagazineCapacity;
    [HideInInspector]public int currentMagazineCapacity = 0;

    //fill for all equippable items
    public bool isWeapon = false;
    public bool isArmor = false;
    public bool isHeadgear = false;
    public bool isRing = false;
    public bool isHeavy = false;

    //fill for all weapons, otherwise leave as None
    public WeaponType weaponType;
    public float damage;

    //fill for all relevant items
    public bool isHealing = false;

    //you can add more weaponTypes here if you need
    public enum WeaponType {
        None,
        ShortSword,
        LongSword,
        GreatSword,
        Axe,
        Spear,
        Pistol,
        Rifle
    }
}
