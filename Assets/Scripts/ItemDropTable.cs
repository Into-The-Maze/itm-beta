using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropTable : MonoBehaviour
{
    public static Dictionary<int, string> masterItemDatabase = new() { //for referencing
        {000, "Shortsword" },
        {001, "Pipe" },
        {002, "Medkit" },
        {003, "Stick" },
        {004, "Woodaxe" },
        {005, "Zweihander" },
        {006, "Pistol" }
    };


    public static Dictionary<int, float> layer0DropTable = new() { //store items as their reference number followed by their (rarity)
        {000, 1f },
        {001, 1f },
        {002, 1f },
        {003, 0.5f },
        {004, 2f }
    };

    public static Dictionary<int, float> layer1DropTable = new() {
        {000, 1f },
        {001, 1f },
        {002, 1f },
        {003, 1f }
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
