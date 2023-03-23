using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CrateController : MonoBehaviour
{
    
    public static CrateController c;
    

    public GameObject fragment1;
    public GameObject fragment2;
    public GameObject fragment3;


    
    public GameObject droppedItem;

    private void Awake() {
        c = this;
    }

    public void breakOpen(Dictionary<int, float> dropTable, float rarityLevel, Vector3 posOfCrate) {
        Vector3 position = posOfCrate;
        int itemElement = getRandomItemDrop(dropTable, rarityLevel);

        Fragment(position);

        droppedItem.GetComponent<SpriteRenderer>().sprite = InvController.itemDrops.items[itemElement].itemIcon;
        droppedItem.GetComponent<ItemDataDump>().itemDataElementReference = itemElement;
        Instantiate(droppedItem, position, Quaternion.identity);
    }

    void Fragment(Vector3 pos) {
        Instantiate(fragment1, pos, Quaternion.identity);
        Instantiate(fragment2, pos, Quaternion.identity);
        Instantiate(fragment3, pos, Quaternion.identity);
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
