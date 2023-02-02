using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InstantiateMaze : MonoBehaviour {
    public GameObject wall;
    public GameObject floor;
    public GameObject player;

    private void Start() {
        player.transform.position = SetPlayerSpawnPos();
    }
    private void Awake() {
        InstantiateTheMaze(InitialiseLabs());
    }

    public void InstantiateTheMaze(char[,] maze) {
        for (int y = 0; y < maze.GetLength(0); y++) {
            for (int x = 0; x < maze.GetLength(1); x++) {
                if (maze[y, x] == ' ') {
                    Instantiate(floor, new Vector3(2 * x, 2 * y, 0), Quaternion.identity);
                }
                else if (maze[y, x] == '.') {
                    Instantiate(floor, new Vector3(2 * x, 2 * y, 0), Quaternion.identity);
                }
                else {
                    Instantiate(wall, new Vector3(2 * x, 2 * y, 0), Quaternion.identity);
                }
            }
        }
    } //instantiates maze at 0,0 world coords
    public void InstantiateTheMaze(char[,] maze, int oX, int oY) {
        for (int y = oY; y < maze.GetLength(0) + oY; y++) {
            for (int x = oX; x < maze.GetLength(1) + oX; x++) {
                if (maze[y, x] == ' ') {
                    Instantiate(floor, new Vector3(2 * x, 2 * y, 0), Quaternion.identity);
                }
                else if (maze[y, x] == '.') {
                    _ = Instantiate(floor, new Vector3(2 * x, 2 * y, 0), Quaternion.identity);
                }
                else {
                    Instantiate(wall, new Vector3(2 * x, 2 * y, 0), Quaternion.identity);
                }
            }
        }
    } //instantiates maze at given overload parameters

    public char[,] InitialiseLabs() {
        char[,] maze = GenerateMazeLayer0.CreateArray();
        GenerateMazeLayer0.Room[] roomList = new GenerateMazeLayer0.Room[0];
        GenerateMazeLayer0.PlaceDefaultAndRandomRooms(ref maze, ref roomList);
        
        int currentRoom = GenerateMazeLayer0.SetStartRoom(ref roomList);
        (char, int, int, int) start;
        return GenerateMazeLayer0.Mazeify(ref maze, ref roomList, ref currentRoom, out start);
    }
    public static Vector3 SetPlayerSpawnPos() {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        while (true) {
            GameObject spawnPos = allObjects[(int)Math.Truncate((decimal)UnityEngine.Random.Range(0, allObjects.Length-1))];
            if (spawnPos.CompareTag("FLOOR_LAYER0")) {
                return new Vector3(spawnPos.transform.position.x, spawnPos.transform.position.y, 0);
            }
        }
    }
        

    
}