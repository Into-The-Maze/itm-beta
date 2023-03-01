using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = System.Random;

public class InstantiateMaze : MonoBehaviour {

    private static Random r = new Random();
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject mazeParent;
    [SerializeField] private GameObject shadowCasterParent;

    [SerializeField] private GameObject globalLightObj;
    [SerializeField] private GameObject globalWallLightObj;
    private Light2D globalLight;
    private Light2D globalWallLight;

    #region layer0
    [SerializeField] private GameObject layer0Wall;
    [SerializeField] private GameObject layer0Floor;
    [SerializeField] private GameObject layer0Door;
    #endregion

    #region layer1
    [SerializeField] private GameObject layer1Wall;
    [SerializeField] private GameObject layer1Floor;
    #endregion

    #region layer2
    [SerializeField] private GameObject layer2Wall;
    [SerializeField] private GameObject layer2WaterFloor;
    [SerializeField] private GameObject layer2Floor;
    [SerializeField] private GameObject layer2IllusionFloor;
    #endregion

    #region layer3
    [SerializeField] private GameObject layer3Wall;
    [SerializeField] private GameObject layer3Floor;
    [SerializeField] private GameObject layer3RoomFloor;
    [SerializeField] private GameObject layer3RoomEdgeRight;
    [SerializeField] private GameObject layer3RoomEdgeLeft;
    [SerializeField] private GameObject layer3RoomEdgeTop;
    [SerializeField] private GameObject layer3RoomEdgeBottom;
    [SerializeField] private GameObject layer3Blood1;
    [SerializeField] private GameObject layer3Blood2;
    [SerializeField] private GameObject layer3Blood3;
    [SerializeField] private GameObject layer3Skull1;
    [SerializeField] private GameObject layer3Skull2;
    #endregion

    #region layer4
    [SerializeField] private GameObject layer4WallHorizontal;
    [SerializeField] private GameObject layer4WallVertical;
    [SerializeField] private GameObject layer4Floor;
    [SerializeField] private GameObject layer4Corner;
    [SerializeField] private GameObject layer4VerticalBlock;
    [SerializeField] private GameObject layer4HorizontalBlock;
    #endregion

    #region lights
    [SerializeField] private GameObject candle;
    [SerializeField] private GameObject lampPost;
    //[SerializeField] private GameObject lampPostFlicker;
    #endregion  

    private void Start() {
        Vector3 spawn = SetPlayerSpawnPos();
        player.transform.position = spawn;
    }
    private void Awake() {  
        //InstantiateMazeLayer0(InitialiseLayer0());
        InstantiateMazeLayer1(initialiseMazeLayer1());
        //InstantiateMazeLayer2(initialiseMazeLayer2());
        //InstantiateMazeLayer3(initialiseMazeLayer3());
        //InstantiateLayer4Pyramid(InitialiseLayer4Pyramid());
    }

    private void InstantiateMazeLayer0(char[,] maze) {
        globalLight = globalLightObj.GetComponent<Light2D>();
        globalWallLight = globalWallLightObj.GetComponent<Light2D>();
        globalLight.intensity = 1f;
        globalWallLight.intensity= 0.375f;
        for (int y = 0; y < maze.GetLength(0); y ++ ) {
            for (int x = 0; x < maze.GetLength(1); x++ ) {
                if (maze[y, x] == ' ') {
                    GameObject floor = Instantiate(layer0Floor, new Vector3(4 * x, 4 * y, 0), Quaternion.identity);
                    floor.transform.parent = mazeParent.transform;
                }
                else if (maze[y, x] == '.') {
                    GameObject door = Instantiate(layer0Door, new Vector3(4 * x, 4 * y, 0), Quaternion.identity);
                    door.transform.parent = mazeParent.transform;
                }
                else {
                    GameObject wall = Instantiate(layer0Wall, new Vector3(4 * x, 4 * y, 0), Quaternion.identity);
                    wall.transform.parent = mazeParent.transform;
                }
            }
        }
    }
    private void InstantiateMazeLayer1(char[,] maze) {
        int layer1Upscale = 8;
        string setting = SetLayer1Setting();
        string weather = SetLayer1Weather();
        Debug.Log($"{setting}, {weather}");
        for (int y = 0; y < maze.GetLength(0); y++ ) {
            for (int x = 0; x < maze.GetLength(1); x++) {
                if (maze[y, x] == ' ') {
                    Instantiate(layer1Floor, new Vector3(layer1Upscale * x, layer1Upscale * y, 0), Quaternion.identity, mazeParent.transform);
                    if (r.Next(1, 3) == 1) {
                        int side = r.Next(1, 5); // 1:top 2:right 3:bottom 4:left
                        if (side == 1 && maze[y + 1, x] != ' ') {
                            Instantiate(lampPost, new Vector3(layer1Upscale * x, layer1Upscale * y + 2.5f, 0), Quaternion.Euler(0f, 0, 180f), mazeParent.transform);
                        }
                        else if (side == 2 && maze[y, x + 1] != ' ') {
                            Instantiate(lampPost, new Vector3(layer1Upscale * x + 2.5f, layer1Upscale * y, 0), Quaternion.Euler(0f, 0, 90f), mazeParent.transform);
                        }
                        else if (side == 3 && maze[y - 1, x] != ' ') {
                            Instantiate(lampPost, new Vector3(layer1Upscale * x, layer1Upscale * y - 2.5f, 0), Quaternion.Euler(0f, 0f, 0f), mazeParent.transform);
                        }
                        else if (side == 4 && maze[y, x - 1] != ' ') {
                            Instantiate(lampPost, new Vector3(layer1Upscale * x - 2.5f, layer1Upscale * y, 0), Quaternion.Euler(0f, 0, 270f), mazeParent.transform);
                        }
                    }
                }
                else {
                    Instantiate(layer1Wall, new Vector3(layer1Upscale * x, layer1Upscale * y, 0), Quaternion.identity, shadowCasterParent.transform);
                }
            }
        }
    }
    private void InstantiateMazeLayer2(char[,] maze) {

        globalLight = globalLightObj.GetComponent<Light2D>();
        globalWallLight = globalWallLightObj.GetComponent<Light2D>();
        globalLight.intensity = 0f;
        globalWallLight.intensity = 0.125f;
        for (int y = 0; y < maze.GetLength(0); y++) {
            for (int x = 0; x < maze.GetLength(1); x++) {
                if (maze[y, x] == '#') {
                    GameObject wall = Instantiate(layer2Wall, new Vector3(4 * x, 4 * y, 0), Quaternion.identity);
                    wall.transform.parent = mazeParent.transform;
                }
                else {
                    System.Random r = new();
                    string floorPicker = Convert.ToString(r.Next(0, 10));
                    if ("01234".Contains(floorPicker)) {
                        GameObject floor = Instantiate(layer2Floor, new Vector3(4 * x, 4 * y, 0), Quaternion.identity);
                        floor.transform.parent = mazeParent.transform;
                    }
                    else if ("56789".Contains(floorPicker)) {
                        GameObject floor = Instantiate(layer2WaterFloor, new Vector3(4 * x, 4 * y, 0), Quaternion.identity);
                        floor.transform.parent = mazeParent.transform;
                    }
                    //ADD ILLUSION FLOOR FUNCTIONALITY LATER BECAUSE THIS METHOD MAKES TOO MANY ILLUSION FLOORS AND THEYRE ALL UNAVOIDABLE

                    //else {
                    //    Instantiate(layer2IllusionFloor, new Vector3(3 * x, 3 * y, 0), Quaternion.identity);
                    //}
                }
            }
        }
    }
    private void InstantiateMazeLayer3(char[,] maze) {
        globalLight = globalLightObj.GetComponent<Light2D>();
        globalWallLight = globalWallLightObj.GetComponent<Light2D>();
        globalLight.intensity = 0f;
        globalWallLight.intensity = 0.125f;
        for (int y = 0; y < maze.GetLength(0); y++) {
            for (int x = 0; x < maze.GetLength(1); x++) {
                if (maze[x, y] == ' ') {
                    GameObject floor = Instantiate(layer3Floor, new Vector3(2 * x, 2 * y, 0), Quaternion.identity);
                    floor.transform.parent = mazeParent.transform;
                    //blood
                    if (r.Next(0, 100) < 15) {
                        GameObject blood = Instantiate(layer3Blood1, new Vector3((2 * x), (2 * y)), Quaternion.identity);
                        blood.transform.parent = mazeParent.transform;
                    }
                    if (r.Next(0, 100) < 15) {
                        GameObject blood = Instantiate(layer3Blood2, new Vector3((2 * x), (2 * y)), Quaternion.identity);
                        blood.transform.parent = mazeParent.transform;
                    }
                    if (r.Next(0, 100) < 15) {
                        GameObject blood = Instantiate(layer3Blood2, new Vector3((2 * x), (2 * y)), Quaternion.identity);
                        blood.transform.parent = mazeParent.transform;
                    }
                    //candles
                    if (r.Next(0, 100) < 25) {
                        GameObject item = Instantiate(candle, new Vector3((2 * x) + (r.Next(-80, 80) / 100f), (2 * y) + (r.Next(-80, 80) / 100f)), Quaternion.identity);
                        item.transform.parent = mazeParent.transform;
                    }
                }
                else if (maze[x, y] == 'b') {
                    GameObject floor = Instantiate(layer3RoomFloor, new Vector3(2 * x, 2 * y, 0), Quaternion.identity);
                    floor.transform.parent = mazeParent.transform;
                    if (maze[x + 1, y] == ' ') {
                        GameObject edge = Instantiate(layer3RoomEdgeRight, new Vector3((2 * x) + 2, 2 * y, 0), Quaternion.identity);
                        edge.transform.parent = mazeParent.transform;
                    }
                    if (maze[x, y + 1] == ' ') {
                        GameObject edge = Instantiate(layer3RoomEdgeTop, new Vector3(2 * x, (2 * y) + 2, 0), Quaternion.identity);
                        edge.transform.parent = mazeParent.transform;
                    }
                    if (maze[x - 1, y] == ' ') {
                        GameObject edge = Instantiate(layer3RoomEdgeLeft, new Vector3((2 * x) - 2, 2 * y, 0), Quaternion.identity);
                        edge.transform.parent = mazeParent.transform;
                    }
                    if (maze[x, y - 1] == ' ') {
                        GameObject edge = Instantiate(layer3RoomEdgeBottom, new Vector3(2 * x, (2 * y) - 2, 0), Quaternion.identity);
                        edge.transform.parent = mazeParent.transform;
                    }
                    //blood
                    if (r.Next(0, 100) < 25) {
                        GameObject blood = Instantiate(layer3Blood1, new Vector3((2 * x) + (r.Next(-80, 80) / 100f), (2 * y) + (r.Next(-80, 80) / 100f)), Quaternion.identity);
                        blood.transform.parent = mazeParent.transform;
                    }
                    if (r.Next(0, 100) < 25) {
                        GameObject blood = Instantiate(layer3Blood2, new Vector3((2 * x) + (r.Next(-80, 80) / 100f), (2 * y) + (r.Next(-80, 80) / 100f)), Quaternion.identity);
                        blood.transform.parent = mazeParent.transform;
                    }
                    if (r.Next(0, 100) < 25) {
                        GameObject blood = Instantiate(layer3Blood2, new Vector3((2 * x) + (r.Next(-80, 80) / 100f), (2 * y) + (r.Next(-80, 80) / 100f)), Quaternion.identity);
                        blood.transform.parent = mazeParent.transform;  
                    }
                    //skulls
                    if (r.Next(0, 100) < 30) {
                        GameObject skull = Instantiate(layer3Skull1, new Vector3((2 * x) + (r.Next(-80, 80) / 100f), (2 * y) + (r.Next(-80, 80) / 100f)), Quaternion.identity);
                        skull.transform.parent = mazeParent.transform;
                    }
                    if (r.Next(0, 100) < 30) {
                        GameObject skull = Instantiate(layer3Skull2, new Vector3((2 * x) + (r.Next(-80, 80) / 100f), (2 * y) + (r.Next(-80, 80) / 100f)), Quaternion.identity);
                        skull.transform.parent = mazeParent.transform;
                    }
                }
                else {
                    GameObject wall = Instantiate(layer3Wall, new Vector3(2 * x, 2 * y, 0), Quaternion.identity);
                    wall.transform.parent = mazeParent.transform;
                }
            }
        }
    }
    // actual layer4 (desert)
    private void InstantiateLayer4Pyramid(GenerateMazeLayer4.Tile[,] maze) {
        globalLight = globalLightObj.GetComponent<Light2D>();
        globalWallLight = globalWallLightObj.GetComponent<Light2D>();
        globalLight.intensity = 0f;
        globalWallLight.intensity = 0;
        for (int y = 0; y < maze.GetLength(0); y ++) {
            for (int x = 0; x < maze.GetLength(1); x ++) {
                var floor = Instantiate(layer4Floor, new Vector3((8 * x), (8 * y), 0), Quaternion.identity);
                var corner1 = Instantiate(layer4Corner, new Vector3((8 * x) + 3.75f, (8 * y) + 3.75f, 0), Quaternion.identity);
                var corner2 = Instantiate(layer4Corner, new Vector3((8 * x) - 3.75f, (8 * y) + 3.75f, 0), Quaternion.identity);
                var corner3 = Instantiate(layer4Corner, new Vector3((8 * x) + 3.75f, (8 * y) - 3.75f, 0), Quaternion.identity);
                var corner4 = Instantiate(layer4Corner, new Vector3((8 * x) - 3.75f, (8 * y) - 3.75f, 0), Quaternion.identity);
                floor.transform.parent = mazeParent.transform;
                if (r.Next(0, 100) < 25) {
                    GameObject item = Instantiate(candle, new Vector3((8 * x) + (r.Next(-80, 80) / 25f), (8 * y) + (r.Next(-80, 80) / 25f)), Quaternion.identity);
                    item.transform.parent = mazeParent.transform;
                }
                corner1.transform.parent = mazeParent.transform;
                corner2.transform.parent = mazeParent.transform;
                corner3.transform.parent = mazeParent.transform;
                corner4.transform.parent = mazeParent.transform;
                if (maze[x, y].TopWall == true) {
                    var topwall = Instantiate(layer4WallHorizontal, new Vector3((8 * x), (8 * y) + 3.75f, 0), Quaternion.identity);
                    var topblock = Instantiate(layer4HorizontalBlock, new Vector3((8 * x), (8 * y) + 4f, 0), Quaternion.identity);
                    topwall.transform.parent = mazeParent.transform;
                    topblock.transform.parent = mazeParent.transform;
                }   
                if (maze[x, y].RightWall == true) {
                    var rightwall = Instantiate(layer4WallVertical, new Vector3((8 * x) + 3.75f, (8 * y), 0), Quaternion.identity);
                    var rightblock = Instantiate(layer4VerticalBlock, new Vector3((8 * x) + 4f, (8 * y), 0), Quaternion.identity);
                    rightwall.transform.parent = mazeParent.transform;
                    rightblock.transform.parent = mazeParent.transform;
                }
                if (maze[x, y].BottomWall == true) {
                    var bottomwall = Instantiate(layer4WallHorizontal, new Vector3((8 * x), (8 * y) - 3.75f, 0), Quaternion.identity);
                    var bottomblock = Instantiate(layer4HorizontalBlock, new Vector3((8 * x), (8 * y) - 4f, 0), Quaternion.identity);
                    bottomwall.transform.parent = mazeParent.transform;
                    bottomblock.transform.parent = mazeParent.transform;
                }
                if (maze[x, y].LeftWall == true) {
                    var leftwall = Instantiate(layer4WallVertical, new Vector3((8 * x) - 3.75f, (8 * y), 0), Quaternion.identity);
                    var leftblock = Instantiate(layer4VerticalBlock, new Vector3((8 * x) - 4f, (8 * y), 0), Quaternion.identity);
                    leftwall.transform.parent = mazeParent.transform;
                    leftblock.transform.parent = mazeParent.transform;
                }
            }
        }
    }

    private char[,] InitialiseLayer0() {
        char[,] maze = GenerateMazeLayer0.CreateArray();
        GenerateMazeLayer0.Room[] roomList = new GenerateMazeLayer0.Room[0];
        GenerateMazeLayer0.PlaceDefaultAndRandomRooms(ref maze, ref roomList);

        int currentRoom = GenerateMazeLayer0.SetStartRoom(ref roomList);
        (char, int, int, int) start;
        return GenerateMazeLayer0.Mazeify(ref maze, ref roomList, ref currentRoom, out start);
    }
    private char[,] initialiseMazeLayer1() {
        return GenerateMazeLayer1.generatePerfectMaze();
    }
    private char[,] initialiseMazeLayer2() {
        char[,] maze = GenerateMazeLayer2.makeBinaryTreeMaze();
        return maze;
    }
    private char[,] initialiseMazeLayer3() {
        char[,] maze = GenerateMazeLayer3.GenerateMaze();
        return maze;
    }
    private GenerateMazeLayer4.Tile[,] InitialiseLayer4Pyramid() {
        GenerateMazeLayer4.Tile[,] maze = GenerateMazeLayer4.CreateHuntKillMaze();
        return maze;
    }

    public static Vector3 SetPlayerSpawnPos() {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        int attempts = 0;
        while (true) {
            if (attempts == 100) {
                Debug.Log("No Floor found within 100 attempts!");
                return new Vector3(10, 10, 0); 
            }
            GameObject spawnPos = allObjects[(int)Math.Truncate((decimal)UnityEngine.Random.Range(0, allObjects.Length-1))];
            if (spawnPos.CompareTag("FLOOR")) {
                return new Vector3(spawnPos.transform.position.x, spawnPos.transform.position.y, 0);
            }
            attempts++;
        }
    }

    // layer 0 setting
    private string SetLayer1Setting() {
        int i;
        int random = r.Next(1, 101);
        string[] lightColours = { "0xFFFFFF", "0x2222FF", "0x5555FF", "0xFF3333" };
        float[] lightLevels = { 0.25f, 0.875f, 1.125f, 1.125f };
        string[] settingNames = { "starlight", "moonlight", "full moon", "blood moon" };
        globalLight = globalLightObj.GetComponent<Light2D>();   
        globalWallLight = globalWallLightObj.GetComponent<Light2D>();
        if (random <= 26) {
            i = 0; // starlight
        }
        else if (random <= 91) {
            i = 1; // moonlight
        }
        else if (random <= 99){
            i = 2; // full moon
        }
        else {
            i = 3; // blood moon
        }
        globalLight.intensity = lightLevels[i];
        globalWallLight.intensity = lightLevels[i] + 0.1f ;
        globalLight.color = hexToColor(lightColours[i]);
        globalWallLight.color = hexToColor(lightColours[i]);
        return settingNames[i];
    }
    private string SetLayer1Weather() {
        int random = r.Next(1, 5);
        string[] weathers = { "clear", "storm" };
        if (random <= 3) {
            return weathers[0];     
        }
        else {
            return weathers[1];
        }
    }
    // layer 2 setting
    // layer 3 setting
    // layer 4 setting

    /// <summary>
    /// Method to convert hex RGB string of format "0xFFFFFF" into a UnityEngine.Color value.
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    private UnityEngine.Color hexToColor(string hex) {
        hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
        byte a = 255;//assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8) {
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }
}