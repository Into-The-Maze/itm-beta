using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;
using Random = System.Random;

public class InstantiateMaze : MonoBehaviour {


    public GameObject player;   

    public static Random r = new Random();

    public GameObject mazeParent;

    public GameObject layer0Wall;
    public GameObject layer0Floor;
    public GameObject layer0Door;

    public GameObject layer1Wall;
    public GameObject layer1Floor;
    
    public GameObject layer2Wall;
    public GameObject layer2WaterFloor;
    public GameObject layer2Floor;
    public GameObject layer2IllusionFloor;

    public GameObject layer3Wall;
    public GameObject layer3Floor;
    public GameObject layer3RoomFloor;
    public GameObject layer3RoomEdgeRight;
    public GameObject layer3RoomEdgeLeft;
    public GameObject layer3RoomEdgeTop;
    public GameObject layer3RoomEdgeBottom;
    public GameObject layer3Blood1;
    public GameObject layer3Blood2;
    public GameObject layer3Blood3;
    public GameObject layer3Skull1;
    public GameObject layer3Skull2;

    public GameObject layer4WallHorizontal;
    public GameObject layer4WallVertical;
    public GameObject layer4Floor;
    public GameObject layer4Corner;
    public GameObject layer4VerticalBlock;
    public GameObject layer4HorizontalBlock;

    public GameObject candle;


    private void Start() {
        Vector3 spawn = SetPlayerSpawnPos();
        player.transform.position = spawn;
    }
    private void Awake() {  
        //InstantiateMazeLayer0(InitialiseLabs());
        //InstantiateMazeLayer1(initialiseMazeLayer1());
        //InstantiateMazeLayer2(initialiseMazeLayer2());
        InstantiateMazeLayer3(initialiseMazeLayer3());
        //InstantiatePyramid(InitialisePyramid());
    }

    public void InstantiateMazeLayer0(char[,] maze) {
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
    public void InstantiateMazeLayer1(char[,] maze) {
        for (int y = 0; y < maze.GetLength(0); y++ ) {
            for (int x = 0; x < maze.GetLength(1); x++) {
                if (maze[y, x] == ' ') {
                    GameObject floor = Instantiate(layer1Floor, new Vector3(8 * x, 8 * y, 0), Quaternion.identity);
                    floor.transform.parent = mazeParent.transform;
                }
                else {
                    GameObject wall = Instantiate(layer1Wall, new Vector3(8 * x, 8 * y, 0), Quaternion.identity);
                    wall.transform.parent = mazeParent.transform;
                }
            }
        }
    }                                   
    public void InstantiateMazeLayer2(char[,] maze) {
        for (int y = 0; y < maze.GetLength(0); y++) {
            for (int x = 0; x < maze.GetLength(1); x++) {
                if (maze[y, x] == '#') {
                    GameObject wall = Instantiate(layer2Wall, new Vector3(3 * x, 3 * y, 0), Quaternion.identity);
                    wall.transform.parent = mazeParent.transform;
                }
                else {
                    System.Random r = new();
                    string floorPicker = Convert.ToString(r.Next(0, 10));
                    if ("01234".Contains(floorPicker)) {
                        GameObject floor = Instantiate(layer2Floor, new Vector3(3 * x, 3 * y, 0), Quaternion.identity);
                        floor.transform.parent = mazeParent.transform;
                    }
                    else if ("56789".Contains(floorPicker)) {
                        GameObject floor = Instantiate(layer2WaterFloor, new Vector3(3 * x, 3 * y, 0), Quaternion.identity);
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
    public void InstantiateMazeLayer3(char[,] maze) {
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
    public void InstantiatePyramid(GenerateMazeLayer4.Tile[,] maze) {
        for (int y = 0; y < maze.GetLength(0); y ++) {
            for (int x = 0; x < maze.GetLength(1); x ++) {
                var floor = Instantiate(layer4Floor, new Vector3((8 * x), (8 * y), 0), Quaternion.identity);
                var corner1 = Instantiate(layer4Corner, new Vector3((8 * x) + 3.75f, (8 * y) + 3.75f, 0), Quaternion.identity);
                var corner2 = Instantiate(layer4Corner, new Vector3((8 * x) - 3.75f, (8 * y) + 3.75f, 0), Quaternion.identity);
                var corner3 = Instantiate(layer4Corner, new Vector3((8 * x) + 3.75f, (8 * y) - 3.75f, 0), Quaternion.identity);
                var corner4 = Instantiate(layer4Corner, new Vector3((8 * x) - 3.75f, (8 * y) - 3.75f, 0), Quaternion.identity);
                floor.transform.parent = mazeParent.transform;
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

    public char[,] initialiseMazeLayer1() {
        return GenerateMazeLayer1.generatePerfectMaze();
    }
    public char[,] initialiseMazeLayer2() {
        char[,] maze = GenerateMazeLayer2.makeBinaryTreeMaze();
        return maze;
    }
    public char[,] initialiseMazeLayer3() {
        char[,] maze = GenerateMazeLayer3.GenerateMaze();
        return maze;
    }
    public char[,] InitialiseLabs() {
        char[,] maze = GenerateMazeLayer0.CreateArray();
        GenerateMazeLayer0.Room[] roomList = new GenerateMazeLayer0.Room[0];
        GenerateMazeLayer0.PlaceDefaultAndRandomRooms(ref maze, ref roomList);
        
        int currentRoom = GenerateMazeLayer0.SetStartRoom(ref roomList);
        (char, int, int, int) start;
        return GenerateMazeLayer0.Mazeify(ref maze, ref roomList, ref currentRoom, out start);
    }
    public GenerateMazeLayer4.Tile[,] InitialisePyramid() {
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
}