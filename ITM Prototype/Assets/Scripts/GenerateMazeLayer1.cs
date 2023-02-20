using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMazeLayer1 : MonoBehaviour
{
    //must be odd
    const int mazeHeight = 35;
    const int mazeWidth = 35;

    public static char[,] generatePerfectMaze() {
        char[,] maze = initMaze();
        mazeify(ref maze);
        return maze;
    }

    static void mazeify(ref char[,] maze) {
        System.Random r = new();
        int cellsInMaze = (mazeHeight / 2) * (mazeWidth / 2);
        int cellsVisited = 1;
        (int x, int y) constructorPos = getStartPos(maze);
        ArrayList visitedCells = new() { constructorPos };

        while (cellsVisited < cellsInMaze) {
            ArrayList possibleDirections = checkDirections(maze, constructorPos.x, constructorPos.y);
            if (possibleDirections.Count > 0) {
                switch (possibleDirections[r.Next(0, possibleDirections.Count)]) {
                    case "north":
                        maze[constructorPos.y - 1, constructorPos.x] = ' ';
                        constructorPos = (constructorPos.x, constructorPos.y - 2);
                        break;
                    case "south":
                        maze[constructorPos.y + 1, constructorPos.x] = ' ';
                        constructorPos = (constructorPos.x, constructorPos.y + 2);
                        break;
                    case "east":
                        maze[constructorPos.y, constructorPos.x + 1] = ' ';
                        constructorPos = (constructorPos.x + 2, constructorPos.y);
                        break;
                    case "west":
                        maze[constructorPos.y, constructorPos.x - 1] = ' ';
                        constructorPos = (constructorPos.x - 2, constructorPos.y);
                        break;
                }
                visitedCells.Add(constructorPos);
                cellsVisited++;
            }
            else {
                constructorPos = ((int, int))visitedCells[visitedCells.Count - 1];
                visitedCells.RemoveAt(visitedCells.Count - 1);
            }
        }
    }

    static ArrayList checkDirections(char[,] maze, int x, int y) {
        ArrayList possibleDirections = new();
        try { if (checkIfCellVisited(maze, x, y - 2)) { possibleDirections.Add("north"); } } catch (IndexOutOfRangeException) { }
        try { if (checkIfCellVisited(maze, x, y + 2)) { possibleDirections.Add("south"); } } catch (IndexOutOfRangeException) { }
        try { if (checkIfCellVisited(maze, x + 2, y)) { possibleDirections.Add("east"); } } catch (IndexOutOfRangeException) { }
        try { if (checkIfCellVisited(maze, x - 2, y)) { possibleDirections.Add("west"); } } catch (IndexOutOfRangeException) { }

        return possibleDirections;
    }

    static bool checkIfCellVisited(char[,] maze, int x, int y) {
        bool hasNorthWall = false, hasSouthWall = false, hasWestWall = false, hasEastWall = false;
        if (maze[y - 1, x] == '#') { hasNorthWall = true; }
        if (maze[y + 1, x] == '#') { hasSouthWall = true; }
        if (maze[y, x - 1] == '#') { hasWestWall = true; }
        if (maze[y, x + 1] == '#') { hasEastWall = true; }
        return (hasNorthWall && hasSouthWall && hasWestWall && hasEastWall) ? true : false;
    }

    static (int, int) getStartPos(char[,] maze) {
        System.Random r = new();
        bool valid = false;
        int x = 0, y = 0;

        while (!valid) {
            x = r.Next(0, maze.GetLength(1));
            y = r.Next(0, maze.GetLength(0));
            if (maze[y, x] == ' ') {
                valid = true;
            }
        }

        return (x, y);
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
}
