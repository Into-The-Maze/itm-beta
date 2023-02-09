using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMazeLayer4 : MonoBehaviour
{
    public const int mazeSize = 30;
    public static System.Random r = new();
    public static Tile[,] CreateHuntKillMaze() {
        Tile[,] maze = CreateArray();
        (int, int) currentPos;
        (bool, int, int) hunt;
        string possibleDirs;

        hunt = Hunt(maze);
        //Console.WriteLine($"hunt = {hunt}");
        currentPos = (hunt.Item2, hunt.Item3);
        //Console.WriteLine($"currentPos = {currentPos}");
        possibleDirs = GetPossibleMoves(maze, currentPos);
        //Console.WriteLine($"possibleDirs = {possibleDirs}");
        //ShowArray(maze);
        while (possibleDirs != "") {
            currentPos = MakeNextMove(ref maze, possibleDirs, currentPos);
            //Console.WriteLine($"currentPos = {currentPos}");
            possibleDirs = GetPossibleMoves(maze, currentPos);
            //Console.WriteLine($"possibleDirs = {possibleDirs}");
            //ShowArray(maze);

        }
        //Console.WriteLine($"done first kill");

        hunt = Hunt(maze);
        //Console.WriteLine($"hunt = {hunt}");
        do {
            currentPos = (hunt.Item2, hunt.Item3);
            //Console.WriteLine($"currentPos = {currentPos}");
            currentPos = GetAdjacentCarved(maze, currentPos);
            //Console.WriteLine($"carvedPos = {currentPos}");
            possibleDirs = GetPossibleMoves(maze, currentPos);
            //Console.WriteLine($"possibleDirs = {possibleDirs}");
            while (possibleDirs != "") {
                currentPos = MakeNextMove(ref maze, possibleDirs, currentPos);
                //Console.WriteLine($"currentPos = {currentPos}");
                possibleDirs = GetPossibleMoves(maze, currentPos);
                //Console.WriteLine($"possibleDirs = {possibleDirs}");
            }
            hunt = Hunt(maze);
            //Console.WriteLine($"hunt = {hunt}");
        } while (hunt.Item1);
        return (maze);
    }

    private static Tile[,] CreateArray() {
        Tile[,] maze = new Tile[mazeSize, mazeSize];
        for (int i = 0; i < mazeSize; i++) {
            for (int j = 0; j < mazeSize; j++) {
                maze[i, j].Visited = false;
                maze[i, j].TopWall = true;
                maze[i, j].RightWall = true;
            }
        }
        return maze;
    }

    private static (bool, int, int) Hunt(Tile[,] maze) {
        for (int i = 0; i < mazeSize; i++) {
            for (int j = 0; j < mazeSize; j++) {
                if (!maze[i, j].Visited) {
                    return (true, i, j);
                }
            }
        }
        return (false, 0, 0);
    }

    private static string GetPossibleMoves(Tile[,] maze, (int, int) currentPos) {
        string possibleDirs = "";
        for (int moveDir = 0; moveDir < 4; moveDir++) {
            if (CheckValidMove(maze, GetNextPosition(currentPos, moveDir))) {
                //Console.WriteLine($"founv valid move {moveDir}");
                possibleDirs += Convert.ToString(moveDir);
            }
        }
        return possibleDirs;
    }

    private static (int, int) GetNextPosition((int, int) currentPos, int moveDir) {
        if (moveDir == 0) { currentPos.Item2--; } // up
        else if (moveDir == 1) { currentPos.Item1++; } // right
        else if (moveDir == 2) { currentPos.Item2++; } // down
        else { currentPos.Item1--; } // left
                                     //Console.WriteLine($"nextPos = {nextPos}");
        return currentPos;
    }

    private static bool CheckValidMove(Tile[,] maze, (int, int) currentPos) {
        //Console.WriteLine($"currentPos = {currentPos}");
        if (currentPos.Item1 >= mazeSize || currentPos.Item2 >= mazeSize || currentPos.Item1 < 0 || currentPos.Item2 < 0) {
            //Console.WriteLine($"False");
            return false;
        }
        else if (!maze[currentPos.Item1, currentPos.Item2].Visited) {
            return true;
        }
        //Console.WriteLine($"visited = {maze[currentPos.Item1, currentPos.Item2].Visited}");
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
        int index = r.Next(0, possibleDirs.Length);
        //Console.WriteLine($"max = {possibleDirs.Length}");
        //Console.WriteLine($"index = {index}");
        int moveDir = Convert.ToInt32(Convert.ToString(possibleDirs[index]));
        //Console.WriteLine($"moveDir = {moveDir}");
        Carve(ref maze, currentPos, moveDir);
        return GetNextPosition(currentPos, moveDir);

    }

    private static void Carve(ref Tile[,] maze, (int, int) currentPos, int moveDir) {
        (int, int) nextPos = GetNextPosition(currentPos, moveDir);
        //Console.WriteLine($"currentPos = {currentPos}");
        //Console.WriteLine($"nextPos = {nextPos}");
        //Console.WriteLine($"moveDir = {moveDir}");
        if (moveDir == 0) { maze[currentPos.Item1, currentPos.Item2].TopWall = false; /*Console.WriteLine($"carved up");*/ } // up
        else if (moveDir == 1) { maze[currentPos.Item1, currentPos.Item2].RightWall = false; /*Console.WriteLine($"carved right");*/ } // right
        else if (moveDir == 2) { maze[nextPos.Item1, nextPos.Item2].TopWall = false; /*Console.WriteLine($"carved down");*/ } // down
        else { maze[nextPos.Item1, nextPos.Item2].RightWall = false; /*Console.WriteLine($"carved left");*/ } // left
        //Console.WriteLine($"");
        maze[nextPos.Item1, nextPos.Item2].Visited = true;
        maze[currentPos.Item1, currentPos.Item2].Visited = true;
    }

    private static (int, int) GetAdjacentCarved(Tile[,] maze, (int, int) currentPos) {
        string possibleDirs = "";
        for (int dir = 0; dir < 4; dir++) {
            if (CheckValidMoveCarved(maze, GetNextPosition(currentPos, dir))) {
                possibleDirs += Convert.ToString(dir);
            }
        }
        int index = r.Next(0, possibleDirs.Length);
        //Console.WriteLine($"max = {possibleDirs.Length}");
        //Console.WriteLine($"index = {index}");
        int moveDir = Convert.ToInt32(Convert.ToString(possibleDirs[index]));
        //Console.WriteLine($"moveDir = {moveDir}");
        return GetNextPosition(currentPos, moveDir);
    }

    public struct Tile {
        public bool Visited;
        public bool TopWall;
        public bool RightWall;
    }
}
