using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

public class InstantiateMaze : MonoBehaviour {

    public GameObject mazeParent;
    public GameObject blocksParent;

    public GameObject layer0Wall;
    public GameObject layer0Floor;
    public GameObject layer0Door;

    public GameObject layer1Wall;
    public GameObject layer1Floor;
    
    public GameObject layer2Wall;
    public GameObject layer2WaterFloor;
    public GameObject layer2Floor;
    public GameObject layer2IllusionFloor;

    public GameObject layer4WallHorizontal;
    public GameObject layer4WallVertical;
    public GameObject layer4Floor;
    public GameObject layer4Corner;
    public GameObject layer4VerticalBlock;
    public GameObject layer4HorizontalBlock;

    public GameObject player;

    private void Start() {
        player.transform.position = SetPlayerSpawnPos();
    }
    private void Awake() {
        //InstantiateMazeLayer0(InitialiseLabs());
        //InstantiateMazeLayer1(initialiseMazeLayer1());
        //InstantiateMazeLayer2(initialiseMazeLayer2());
        InstantiateMazeLayer4(InitialisePyramid());
    }

    public void InstantiateMazeLayer0(char[,] maze) {
        for (int y = 0; y < maze.GetLength(0); y++) {
            for (int x = 0; x < maze.GetLength(1); x++) {
                if (maze[y, x] == ' ') {
                    Instantiate(layer0Floor, new Vector3(3 * x, 3 * y, 0), Quaternion.identity);
                }
                else if (maze[y, x] == '.') {
                    Instantiate(layer0Door, new Vector3(3 * x, 3 * y, 0), Quaternion.identity);
                }
                else {
                    Instantiate(layer0Wall, new Vector3(3 * x, 3 * y, 0), Quaternion.identity);
                }
            }
        }
    }
    public void InstantiateMazeLayer1(char[,] maze) {
        for (int y = 0; y < maze.GetLength(0); y++) {
            for (int x = 0; x < maze.GetLength(1); x++) {
                if (maze[y, x] == ' ') {
                    Instantiate(layer1Floor, new Vector3(3 * x, 3 * y, 0), Quaternion.identity);
                }
                else {
                    Instantiate(layer1Wall, new Vector3(3 * x, 3 * y, 0), Quaternion.identity);
                }
            }
        }
    }
    public void InstantiateMazeLayer2(char[,] maze) {
        for (int y = 0; y < maze.GetLength(0); y++) {
            for (int x = 0; x < maze.GetLength(1); x++) {
                if (maze[y, x] == '#') {
                    Instantiate(layer2Wall, new Vector3(3 * x, 3 * y, 0), Quaternion.identity);
                }
                else {
                    System.Random r = new();
                    string floorPicker = Convert.ToString(r.Next(0, 10));
                    if ("01234".Contains(floorPicker)) {
                        Instantiate(layer2Floor, new Vector3(3 * x, 3 * y, 0), Quaternion.identity);
                    }
                    else if ("56789".Contains(floorPicker)) {
                        Instantiate(layer2WaterFloor, new Vector3(3 * x, 3 * y, 0), Quaternion.identity);
                    }
                    //ADD ILLUSION FLOOR FUNCTIONALITY LATER BECAUSE THIS METHOD MAKES TOO MANY ILLUSION FLOORS AND THEYRE ALL UNAVOIDABLE

                    //else {
                    //    Instantiate(layer2IllusionFloor, new Vector3(3 * x, 3 * y, 0), Quaternion.identity);
                    //}
                }
            }
        }
    }
    public void InstantiateMazeLayer4(GenerateMazeLayer4.Tile[,] maze) {
        for (int y = 0; y < maze.GetLength(0); y++) {
            for (int x = 0; x < maze.GetLength(1); x++) {
                var floor = Instantiate(layer4Floor, new Vector3(3 * x, 3 * y, 0), Quaternion.identity);
                var corner1 = Instantiate(layer4Corner, new Vector3((3 * x) + 1.3125f, (3 * y) + 1.3125f, 0), Quaternion.identity);
                var corner2 = Instantiate(layer4Corner, new Vector3((3 * x) - 1.3125f, (3 * y) + 1.3125f, 0), Quaternion.identity);
                var corner3 = Instantiate(layer4Corner, new Vector3((3 * x) + 1.3125f, (3 * y) - 1.3125f, 0), Quaternion.identity);
                var corner4 = Instantiate(layer4Corner, new Vector3((3 * x) - 1.3125f, (3 * y) - 1.3125f, 0), Quaternion.identity);
                floor.transform.parent = mazeParent.transform;
                corner1.transform.parent = mazeParent.transform;
                corner2.transform.parent = mazeParent.transform;
                corner3.transform.parent = mazeParent.transform;
                corner4.transform.parent = mazeParent.transform;
                if (maze[x, y].TopWall == true) {
                    var topwall = Instantiate(layer4WallHorizontal, new Vector3(3 * x, (3 * y) + 1.3125f, 0), Quaternion.identity);
                    var topblock = Instantiate(layer4HorizontalBlock, new Vector3(3 * x, (3 * y) + 1.5f, 0), Quaternion.identity);
                    topwall.transform.parent = mazeParent.transform;
                    topblock.transform.parent = blocksParent.transform;
                }   
                if (maze[x, y].RightWall == true) {
                    var rightwall = Instantiate(layer4WallVertical, new Vector3((3 * x) + 1.3125f, (3 * y), 0), Quaternion.identity);
                    var rightblock = Instantiate(layer4VerticalBlock, new Vector3((3 * x) + 1.5f, (3 * y), 0), Quaternion.identity);
                    rightwall.transform.parent = mazeParent.transform;
                    rightblock.transform.parent = blocksParent.transform;
                }
                if (maze[x, y].BottomWall == true) {
                    var bottomwall = Instantiate(layer4WallHorizontal, new Vector3((3 * x), (3 * y) - 1.3125f, 0), Quaternion.identity);
                    var bottomblock = Instantiate(layer4HorizontalBlock, new Vector3((3 * x), (3 * y) - 1.5f, 0), Quaternion.identity);
                    bottomwall.transform.parent = mazeParent.transform;
                    bottomblock.transform.parent = blocksParent.transform;
                }
                if (maze[x, y].LeftWall == true) {
                    var leftwall = Instantiate(layer4WallVertical, new Vector3((3 * x) - 1.3125f, (3 * y), 0), Quaternion.identity);
                    var leftblock = Instantiate(layer4VerticalBlock, new Vector3((3 * x) - 1.5f, (3 * y), 0), Quaternion.identity);
                    leftwall.transform.parent = mazeParent.transform;
                    leftblock.transform.parent = blocksParent.transform;
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
                return new Vector3(0, 0, 0); 
            }
            GameObject spawnPos = allObjects[(int)Math.Truncate((decimal)UnityEngine.Random.Range(0, allObjects.Length-1))];
            if (spawnPos.CompareTag("FLOOR")) {
                return new Vector3(spawnPos.transform.position.x, spawnPos.transform.position.y, 0);
            }
            attempts++;
        }
    }   
}