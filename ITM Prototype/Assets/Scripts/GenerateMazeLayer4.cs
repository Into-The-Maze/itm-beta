using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMazeLayer4 : MonoBehaviour
{
    public const int mazeSize = 20;
    public static System.Random r = new();
    public static Tile[,] CreateHuntKillMaze() {
        Tile[,] maze = CreateArray();
        (int, int) currentPos;
        (bool, int, int) hunt;
        string possibleDirs;

        hunt = Hunt(maze);
        currentPos = (hunt.Item2, hunt.Item3);
        possibleDirs = GetPossibleMoves(maze, currentPos);
        while (possibleDirs != "") {
            currentPos = MakeNextMove(ref maze, possibleDirs, currentPos);
            possibleDirs = GetPossibleMoves(maze, currentPos);
        }

        hunt = Hunt(maze);
        do {
            currentPos = (hunt.Item2, hunt.Item3);
            currentPos = GoToAdjacentCarved(maze, currentPos);
            possibleDirs = GetPossibleMoves(maze, currentPos);
            while (possibleDirs != "") {
                currentPos = MakeNextMove(ref maze, possibleDirs, currentPos);
                possibleDirs = GetPossibleMoves(maze, currentPos);
            }
            hunt = Hunt(maze);
        } while (hunt.Item1);
        return (maze);
    }

    private static Tile[,] CreateArray() {
        Tile[,] maze = new Tile[mazeSize, mazeSize];
        for (int j = 0; j < mazeSize; j++) {
            for (int i = 0; i < mazeSize; i++) {
                maze[i, j].Visited = false;
                maze[i, j].TopWall = true;
                maze[i, j].RightWall = true;
                maze[i, j].BottomWall = true;
                maze[i, j].LeftWall = true;
            }
        }
        return maze;
    }

    private static (bool, int, int) Hunt(Tile[,] maze) {
        for (int y = 0; y < mazeSize; y++) {
            for (int x = 0; x < mazeSize; x++) {
                if (!maze[x, y].Visited) {
                    return (true, x, y);
                }
            }
        }
        return (false, 0, 0);
    }

    private static string GetPossibleMoves(Tile[,] maze, (int, int) currentPos) {
        string possibleDirs = "";
        for (int moveDir = 0; moveDir < 4; moveDir++) {
            if (CheckValidMove(maze, GetNextPosition(currentPos, moveDir))) {
                possibleDirs += Convert.ToString(moveDir);
            }
        }
        return possibleDirs;
    }

    private static (int, int) GetNextPosition((int, int) currentPos, int moveDir) {
        if (moveDir == 0) { currentPos.Item2++; } // up
        else if (moveDir == 1) { currentPos.Item1++; } // right
        else if (moveDir == 2) { currentPos.Item2--; } // down
        else { currentPos.Item1--; } // left
        return currentPos;
    }

    private static bool CheckValidMove(Tile[,] maze, (int, int) currentPos) {
        if (currentPos.Item1 >= mazeSize || currentPos.Item2 >= mazeSize || currentPos.Item1 < 0 || currentPos.Item2 < 0) {
            return false;
        }
        else if (!maze[currentPos.Item1, currentPos.Item2].Visited) {
            return true;
        }
        return false;
    }

    private static bool CheckValidMoveCarved(Tile[,] maze, (int, int) position) {
        if (position.Item1 >= mazeSize || position.Item2 >= mazeSize || position.Item1 < 0 || position.Item2 < 0) {
            return false;
        }
        if (maze[position.Item1, position.Item2].Visited) {
            return true;
        }
        return false;
    }

    private static (int, int) MakeNextMove(ref Tile[,] maze, string possibleDirs, (int, int) currentPos) {
        int moveDir = Convert.ToInt32(Convert.ToString(possibleDirs[r.Next(0, possibleDirs.Length)]));
        Carve(ref maze, currentPos, moveDir);
        return GetNextPosition(currentPos, moveDir);
    }

    private static void Carve(ref Tile[,] maze, (int, int) currentPos, int moveDir) {
        (int, int) nextPos = GetNextPosition(currentPos, moveDir);
        if (moveDir == 0) {
            maze[currentPos.Item1, currentPos.Item2].TopWall = false;
            maze[nextPos.Item1, nextPos.Item2].BottomWall = false;
        } // up
        else if (moveDir == 1) {
            maze[currentPos.Item1, currentPos.Item2].RightWall = false;
            maze[nextPos.Item1, nextPos.Item2].LeftWall = false;
        } // right
        else if (moveDir == 2) {
            maze[currentPos.Item1, currentPos.Item2].BottomWall = false;
            maze[nextPos.Item1, nextPos.Item2].TopWall = false;
        } // down
        else {
            maze[currentPos.Item1, currentPos.Item2].LeftWall = false;
            maze[nextPos.Item1, nextPos.Item2].RightWall = false;
        } // left
        maze[nextPos.Item1, nextPos.Item2].Visited = true;
        maze[currentPos.Item1, currentPos.Item2].Visited = true;
    }

    private static (int, int) GoToAdjacentCarved(Tile[,] maze, (int, int) currentPos) {
        string possibleDirs = "";
        for (int dir = 0; dir < 4; dir++) {
            if (CheckValidMoveCarved(maze, GetNextPosition(currentPos, dir))) {
                possibleDirs += Convert.ToString(dir);
            }
        }
        int moveDir = Convert.ToInt32(Convert.ToString(possibleDirs[r.Next(0, possibleDirs.Length)]));
        return GetNextPosition(currentPos, moveDir);
    }

    public struct Tile {
        public bool Visited;
        public bool TopWall;
        public bool RightWall;
        public bool BottomWall;
        public bool LeftWall;
    }
}
