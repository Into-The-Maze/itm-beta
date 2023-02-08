using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMazeLayer2 : MonoBehaviour {
    //MUST BE ODD
    [HideInInspector] const int mazeHeight = 41, mazeWidth = 41;

    public static char[,] makeBinaryTreeMaze() {
        char[,] maze = initMaze();
        mazeify(ref maze);
        return maze;
    }

    static char[,] initMaze() {
        char[,] maze = new char[mazeHeight, mazeWidth];
        for (int y = 0; y < maze.GetLength(0); y++) {
            for (int x = 0; x < maze.GetLength(1); x++) {
                if (y % 2 != 0 && x % 2 != 0) {
                    maze[y, x] = ' ';
                }
                else {
                    maze[y, x] = '#';
                }
            }
        }
        return maze;
    }

    static void mazeify(ref char[,] maze) {
        for (int y = 0; y < maze.GetLength(0); y++) {
            for (int x = 0; x < maze.GetLength(1); x++) {
                if (y % 2 != 0 && x % 2 != 0) {
                    destroyWall(ref maze, x, y);
                }
            }
        }
    }

    static void destroyWall(ref char[,] maze, int x, int y) {
        System.Random r = new();
        if (x != 1 && y != 1) {
            if (r.Next(0, 2) == 0) {
                maze[y - 1, x] = ' ';
                return;
            }
            else {
                maze[y, x - 1] = ' ';
                return;
            }
        }
        else if (x != 1 && y == 1) {
            maze[y, x - 1] = ' ';
            return;
        }
        else if (y != 1 && x == 1) {
            maze[y - 1, x] = ' ';
            return;
        }
        else {
            return;
        }
    }
}
