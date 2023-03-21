using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CrateController : MonoBehaviour
{
    
    public static CrateController c;

    public GameObject crate;
    public GameObject droppedItem;

    private void Awake() {
        c = this;
    }

    private void Start() {
        
    }
    public void breakOpen(Dictionary<int, float> dropTable, float rarityLevel) {
        Vector3 position = transform.position;
        int itemElement = getRandomItemDrop(dropTable, rarityLevel);
        
        //MUST IMPLEMENT FRAGMENTATION MYSELF

        droppedItem.GetComponent<SpriteRenderer>().sprite = InvController.itemDrops.items[itemElement].itemIcon;
        droppedItem.GetComponent<ItemDataDump>().itemDataElementReference = itemElement;
        Instantiate(droppedItem, position, Quaternion.identity);
    }

    private int getRandomItemDrop(Dictionary<int, float> dropTable, float rarityLevel) {
        System.Random r = new();
        int attempts = 0;
        int randomItem = r.Next(0, dropTable.Count);
        do {
            if (dropTable.ElementAt(randomItem).Value <= rarityLevel) {
                return dropTable.ElementAt(randomItem).Key;
            }
        } while (attempts < 1000);
        return 0;
    }
}
