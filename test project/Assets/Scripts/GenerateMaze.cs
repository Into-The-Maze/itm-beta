using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMaze : MonoBehaviour
{
    public GameObject wall;
    public GameObject floor;

    private void Start() {
        //stick your maze array into this
        //InstantiateMaze();
    }

    public void InstantiateMaze(char[,] maze) {
        for (int y = 0; y < maze.GetLength(0); y++) {
            for (int x = 0; x < maze.GetLength(1); x++) {
                if (maze[y, x] != ' ') {
                    Instantiate(wall, new Vector3(x, y, 0), Quaternion.identity);
                }
                else {
                    Instantiate(floor, new Vector3(x, y, 0), Quaternion.identity);
                }
            }
        }
    } //instantiates maze at 0,0 world coords
    public void InstantiateMaze(char[,] maze, int oX, int oY) {
        for (int y = oY; y < maze.GetLength(0) + oY; y++) {
            for (int x = oX; x < maze.GetLength(1) + oX; x++) {
                if (maze[y, x] != ' ') {
                    Instantiate(wall, new Vector3(x, y, 0), Quaternion.identity);
                }
                else {
                    Instantiate(floor, new Vector3(x, y, 0), Quaternion.identity);
                }
            }
        }
    } //instantiates maze at given overload parameters
}
