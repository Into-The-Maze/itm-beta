using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Attack : MonoBehaviour
{
    public static InventoryItem attackItem;

    public static float damage;
    private GameObject weapon;
    public Light2D muzzleFlashPistol;

    [HideInInspector] public static bool CurrentlyAttacking = false;
    public static bool CurrentlySwinging = false;
    WaitForSeconds swingTime = new WaitForSeconds(0.5f);

    [HideInInspector] public static bool CurrentlyAiming = false;

    public static ItemData.WeaponType[] meleeWeapons = { ItemData.WeaponType.ShortSword, ItemData.WeaponType.LongSword, ItemData.WeaponType.GreatSword, ItemData.WeaponType.Axe, ItemData.WeaponType.Spear };
    public static ItemData.WeaponType[] rangedWeapons = { ItemData.WeaponType.Pistol, ItemData.WeaponType.Rifle };

    [SerializeField] GameObject attackPrefab;
    [SerializeField] GameObject playerSprite;
    [SerializeField] GameObject playerLocation;

    private void Update() {

        if (Input.GetMouseButtonDown(0) && PlayerMovement.stamina > 25 && attackItem != null && meleeWeapons.Contains(attackItem.itemData.weaponType) && !ToggleEquipMenu.invIsOpen && !ToggleHealthScreen.invIsOpen && !ToggleInventory.invIsOpen && !CurrentlyAttacking){
            damage = (attackItem == null) ? 0 : attackItem.Damage;
            PlayerMovement.movementType = PlayerMovement.MovementType.ChargingAttack;
            CurrentlyAttacking = true;
            showWeapon();
            StartCoroutine(chargeAttack());
        }

        //replace f with Input.GetMouseButtonDown(1) later. My laptop cant handle simultaneous lmb & rmb so i cant test properly.
        else if (Input.GetKeyDown(KeyCode.F) && !CurrentlyAiming && attackItem != null && rangedWeapons.Contains(attackItem.itemData.weaponType) && !ToggleEquipMenu.invIsOpen && !ToggleHealthScreen.invIsOpen && !ToggleInventory.invIsOpen && !CurrentlyAttacking) {
            Debug.Log("Aiming successful");
            damage = (attackItem == null) ? 0 : attackItem.Damage;
            StartCoroutine(aim());
        }
    }

    IEnumerator aim() {
        attackPrefab = attackItem.itemData.model_Gun;
        weapon = Instantiate(attackPrefab, playerLocation.transform.TransformPoint(playerSprite.transform.up * -1f), playerSprite.transform.rotation, playerSprite.transform);

        PlayerMovement.movementType = PlayerMovement.MovementType.ChargingAttack;
        CurrentlyAiming = true;
        while (CurrentlyAiming) {
            //replace f with Input.GetMouseButtonUp(1) later. My laptop cant handle simultaneous lmb & rmb so i cant test properly.
            if (Input.GetKeyUp(KeyCode.F)) { break; }
            if (Input.GetMouseButtonDown(0) && CurrentlyAiming && attackItem.itemData.currentMagazineCapacity > 0) {
                fire();
            }
            else if (Input.GetMouseButtonDown(0) && CurrentlyAiming && attackItem.itemData.currentMagazineCapacity <= 0) {
                Debug.Log("Gun Empty");
            }
            yield return null;
        }
        CurrentlyAiming = false;
        Destroy(weapon);
        PlayerMovement.movementType = PlayerMovement.MovementType.Walking;
        
        StopCoroutine(aim());
    }

    void fire() {
        Debug.Log($"Shooting gun: current ammo {attackItem.itemData.currentMagazineCapacity}; capacity after shot {attackItem.itemData.currentMagazineCapacity - 1}");

        raycastBullet();
        StartCoroutine(handleMuzzleFlash());


        
        --attackItem.itemData.currentMagazineCapacity;
        //updates inventory image to show whether the gun is still usable due to ammo
        attackItem.GetComponent<UnityEngine.UI.Image>().sprite = (attackItem.itemData.currentMagazineCapacity <= 0) ? attackItem.itemData.itemIcon_GunEmpty : attackItem.itemData.itemIcon_GunLoaded;

    }

    void raycastBullet() {

        //z + 10f should account for the position of the camera
        RaycastHit2D hit = Physics2D.Raycast(weapon.transform.position, (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z+10f)) - weapon.transform.position).normalized, 1000f);
        Debug.DrawRay(weapon.transform.position, (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z + 10f)) - weapon.transform.position).normalized * 1000f, Color.red, 1f);
        if (hit.collider != null) {
            if (hit.collider.gameObject != null) {
                Debug.Log("hit something that is also not nothing");
                if (hit.collider.gameObject.CompareTag("entity")) {
                    Debug.Log($"trying to damage enemy for {damage} damage");
                    hit.collider.gameObject.GetComponent<HealthPoolEnemy>().TakeDamage(damage);
                }
            }
        }
    }

    IEnumerator handleMuzzleFlash() {
        
        var flash = Instantiate(muzzleFlashPistol, weapon.transform.position, Quaternion.identity, weapon.transform);

        yield return new WaitForSeconds(0.3f);

        Destroy(flash.gameObject);

        StopCoroutine(handleMuzzleFlash());
    }

    #region meleeHandlers
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
    #endregion
}
