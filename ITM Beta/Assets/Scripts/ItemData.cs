using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Populate with data to make more item types

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public int width = 1;
    public int height = 1;
    public Sprite itemIcon;

    public bool isWeapon = false;
    public bool isArmor = false;
    public bool isHeadgear = false;
    public bool isRing = false;

    public string flavourText;


}
