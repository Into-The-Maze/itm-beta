using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Attack : MonoBehaviour
{
    public static InventoryItem attackItem;
    public static float damage;
    private GameObject weapon;
    [HideInInspector] public static bool CurrentlyAttacking = false;
    public static bool CurrentlySwinging = false;
    WaitForSeconds swingTime = new WaitForSeconds(0.5f);

    [SerializeField] GameObject attackPrefab;
    [SerializeField] GameObject playerSprite;
    [SerializeField] GameObject playerLocation;

    private void Update() {
        if (Input.GetMouseButtonDown(0) && PlayerMovement.stamina > 25 && attackItem != null && !ToggleEquipMenu.invIsOpen && !ToggleHealthScreen.invIsOpen && !ToggleInventory.invIsOpen && !CurrentlyAttacking){
            damage = (attackItem == null) ? 0 : attackItem.Damage;
            PlayerMovement.movementType = PlayerMovement.MovementType.ChargingAttack;
            CurrentlyAttacking = true;
            showWeapon();
            StartCoroutine(chargeAttack());
        }
    }

    IEnumerator chargeAttack() {
        while (!Input.GetKeyUp(KeyCode.Mouse0)) {
            damage += Time.deltaTime;
            yield return null;
        }
        CurrentlySwinging = true;
        StartCoroutine(attack());
        StopCoroutine(chargeAttack());
    }

    IEnumerator attack() {
        PlayerMovement.movementType = PlayerMovement.MovementType.Attacking;
        PlayerMovement.stamina -= 25;
        StartCoroutine(manageSwingTime());
        StartCoroutine(lunge());
        while (CurrentlySwinging) {
            weapon.transform.RotateAround(playerLocation.transform.position, Vector3.forward, 360 * Time.deltaTime);
            yield return null;
        }
        
        Destroy(weapon);
        CurrentlyAttacking = false;
        playerLocation.GetComponent<Rigidbody2D>().mass = 1f;
        damage = attackItem.Damage;
        PlayerMovement.movementType = PlayerMovement.MovementType.Walking;
        
        StopAllCoroutines();
    }

    IEnumerator lunge() {
        playerLocation.GetComponent<Rigidbody2D>().mass = 0.01f;
        while (true) {
            playerLocation.GetComponent<Rigidbody2D>().AddForce(playerSprite.transform.up * -1f, ForceMode2D.Impulse);
            playerLocation.GetComponent<Rigidbody2D>().mass += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator manageSwingTime() {
        yield return swingTime;
        CurrentlySwinging = false;
    }

    private void showWeapon() {
        attackPrefab.GetComponent<SpriteRenderer>().sprite = attackItem.itemData.itemIcon;
        weapon = Instantiate(attackPrefab, playerLocation.transform.TransformPoint(playerSprite.transform.right * -2f), playerSprite.transform.rotation * new Quaternion(Mathf.Cos(-90 / 2), Mathf.Sin(-90 / 2), 0, 0));
        weapon.transform.SetParent(playerSprite.transform, true);
        weapon.transform.SetAsLastSibling();
    }
}
