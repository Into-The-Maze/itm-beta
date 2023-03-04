using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Attack : MonoBehaviour
{
    public static InventoryItem attackItem;
    private float? damage = (attackItem == null) ? null : attackItem.Damage;
    private GameObject weapon;
    private bool CurrentlySwinging = false;
    private bool CurrentlySwingingManager;
    WaitForSeconds swingTime = new WaitForSeconds(0.5f);

    [SerializeField] GameObject attackPrefab;
    [SerializeField] GameObject playerSprite;
    [SerializeField] GameObject playerLocation;

    private void Update() {

        if (Input.GetMouseButtonDown(0) && PlayerMovement.stamina > 25 && attackItem != null && !ToggleEquipMenu.invIsOpen && !ToggleHealthScreen.invIsOpen && !ToggleInventory.invIsOpen && !CurrentlySwinging){
            PlayerMovement.movementType = PlayerMovement.MovementType.ChargingAttack;
            CurrentlySwinging = true;
            showWeapon();
            StartCoroutine(chargeAttack());
        }
    }

    IEnumerator chargeAttack() {
        while (!Input.GetKeyUp(KeyCode.Mouse0)) {
            damage += Time.deltaTime;
            yield return null;
        }
        CurrentlySwingingManager = true;
        StartCoroutine(attack());
        StopCoroutine(chargeAttack());
    }

    IEnumerator attack() {
        PlayerMovement.movementType = PlayerMovement.MovementType.Attacking;
        PlayerMovement.stamina -= 25;
        StartCoroutine(manageSwingTime());
        while (CurrentlySwingingManager) {
            weapon.transform.RotateAround(playerLocation.transform.position, Vector3.forward, 360 * Time.deltaTime);
            yield return null;
        }
        
        Destroy(weapon);
        CurrentlySwinging = false;
        PlayerMovement.movementType = PlayerMovement.MovementType.Walking;
        StopAllCoroutines();
    }

    IEnumerator manageSwingTime() {
        yield return swingTime;
        CurrentlySwingingManager = false;
    }

    private void showWeapon() {
        attackPrefab.GetComponent<SpriteRenderer>().sprite = attackItem.itemData.itemIcon;
        weapon = Instantiate(attackPrefab, playerLocation.transform.TransformPoint(playerSprite.transform.right * -1.5f), playerSprite.transform.rotation * new Quaternion(Mathf.Cos(-90 / 2), Mathf.Sin(-90 / 2), 0, 0));
        weapon.transform.SetParent(playerSprite.transform, true);
        weapon.transform.SetAsLastSibling();
    }
}
