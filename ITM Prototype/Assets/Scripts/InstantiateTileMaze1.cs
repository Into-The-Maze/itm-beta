using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

public class InstantiateTileMaze : MonoBehaviour {
    public GameObject layer4Wall;
    public GameObject layer4Floor;
    public GameObject layer4Door;

    public GameObject player;

    private void Start() {
        player.transform.position = SetPlayerSpawnPos();
    }
    private void Awake() {
        //InstantiateMazeLayer4Pyramid(InitialisePyramid());
    }

    public void InstantiateMazeLayer4Pyramid(Tile[,] maze) {
        for (int y = 0; y < maze.GetLength(0); y++) {
            for (int x = 0; x < maze.GetLength(1); x++) {
                Instantiate(layer4Floor, new Vector3(3 * x, 3 * y, 0), Quaternion.identity);
                if (maze[y, x].TopWall == true) {
                    Instantiate(layer4WallTop, new Vector3(3 * x, 3 * y, 0), Quaternion.identity);
                }
                if (maze[y, x].RightWall == true) {
                    Instantiate(layer4WallRight, new Vector3(3 * x, 3 * y, 0), Quaternion.identity);
                }
            }
        }
    }
 
    public Tile [,] InitialisePyramid() {
        
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