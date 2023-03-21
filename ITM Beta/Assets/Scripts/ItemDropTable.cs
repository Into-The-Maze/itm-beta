using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropTable : MonoBehaviour
{
    public static Dictionary<int, string> masterItemDatabase = new() { //for referencing
        {000, "Greatsword" },
        {001, "Pipe" },
        {002, "Medkit" },
        {003, "Stick" }
    };


    public static Dictionary<int, float> layer0DropTable = new() { //store items as their reference number followed by their (rarity)

    };

    public static Dictionary<int, float> layer1DropTable = new() { 

    };

    public static Dictionary<int, float> layer2DropTable = new() {

    };

    public static Dictionary<int, float> layer3DropTable = new() { 

    };

    public static Dictionary<int, float> layer4DesertDropTable = new() { 

    };

    public static Dictionary<int, float> layer4PyramidDropTable = new() { 

    };

    public static Dictionary<int, float> layer5DropTable = new() { 

    };

    
}
