using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

public class GenerateMazeLayer3 : MonoBehaviour
{

    const int mazeSize = 31;
    const int mazeHeight = mazeSize;
    const int mazeWidth = mazeSize;

    public static char[,] GenerateMaze() {
        Room[] roomlist = new Room[0];
        char[,] maze = initMaze();
        mazeify(ref maze);
        PlaceDefaultAndRandomRooms(ref maze, ref roomlist);
        printMaze(maze);
        return maze;
    }
    static void printMaze(char[,] maze) {
        for (int y = 0; y < maze.GetLength(0); y++) {
            for (int x = 0; x < maze.GetLength(1); x++) {
                Console.Write(maze[y, x]);
            }
            Console.WriteLine();
        }
    }
    static void mazeify(ref char[,] maze) {
        Random r = new();
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
        Random r = new();
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
    public static void PlaceDefaultAndRandomRooms(ref char[,] maze, ref Room[] roomList) {
        //PlaceRooms(ref maze, MainRooms(), ref roomList);
        PlaceRooms(ref maze, RandomRooms(), ref roomList);
    }
    private static RoomType[] MainRooms() {
        RoomType[] rooms = new RoomType[5];
        rooms[0].Name = "giant Room";
        rooms[0].Size = 11;
        rooms[0].Width = 12;
        rooms[1].Name = "long room";
        rooms[1].Size = 10;
        rooms[1].Width = 7;
        rooms[2].Name = "wide room";
        rooms[2].Size = 7;
        rooms[2].Width = 10;
        rooms[3].Name = "thin room";
        rooms[3].Size = 10;
        rooms[3].Width = 5;
        rooms[4].Name = "cube room";
        rooms[4].Size = 6;
        rooms[4].Width = 6;
        return rooms;
    }
    private static RoomType[] RandomRooms() {
        Random r = new Random();
        int numRooms = r.Next(15, 21);
        RoomType[] rooms = new RoomType[numRooms];
        for (int i = 0; i < numRooms; i++) {
            rooms[i].Name = Convert.ToString(i) + " random room";
            rooms[i].Size = r.Next(2, 5);
            rooms[i].Width = r.Next(2, 5);
        }
        return rooms;
    }
    private static void PlaceRooms(ref char[,] maze, RoomType[] rooms, ref Room[] roomList) {
        Room[] oldRoomList = roomList;
        Room[] newRoomList = new Room[rooms.Length];
        Random RandomNumber = new Random();
        bool valid;
        char orientation = ' ';
        int row = 0;
        int column = 0;
        int HorV = 0;
        int i = 0;
        int tries;
        foreach (var room in rooms) {
            newRoomList[i].Tile = room.Name[0];
            newRoomList[i].Visited = false;
            valid = false;
            tries = 0;
            while (valid == false && tries < 80) {
                row = RandomNumber.Next(3, mazeSize - 3);
                column = RandomNumber.Next(3, mazeSize - 3);
                HorV = RandomNumber.Next(0, 2);
                if (HorV == 0) {
                    orientation = 'v';
                }
                else {
                    orientation = 'h';
                }
                valid = ValidateRoomPosition(maze, room, row, column, orientation);
                tries++;
            }
            if (orientation == 'v') {
                newRoomList[i].Coordinates = ((row, column), (row + room.Size - 1, column + room.Width - 1));
            }
            else {
                newRoomList[i].Coordinates = ((row, column), (row + room.Width - 1, column + room.Size - 1));
            }
            i++;
            if (valid) {
                PlaceRoom(ref maze, room, row, column, orientation);
            }
        }
        roomList = new Room[newRoomList.Length + oldRoomList.Length];
        oldRoomList.CopyTo(roomList, 0);
        newRoomList.CopyTo(roomList, oldRoomList.Length);
    }
    private static void PlaceRoom(ref char[,] maze, RoomType room, int row, int column, char orientation) {
        if (orientation == 'v') {
            for (int scanColumn = 0; scanColumn < room.Width; scanColumn++) {
                for (int scanRow = 0; scanRow < room.Size; scanRow++) {
                    if ((scanRow == 0 || scanRow == room.Size - 1) && (scanColumn == 0 || scanColumn == room.Width - 1)) {
                        maze[row + scanRow, column + scanColumn] = 'b';
                    }
                    else if (scanRow == 0 || scanRow == room.Size - 1 || scanColumn == 0 || scanColumn == room.Width - 1) {
                        maze[row + scanRow, column + scanColumn] = 'b';
                    }
                    else {
                        maze[row + scanRow, column + scanColumn] = 'b';
                    }
                }
            }
        }
        else if (orientation == 'h') {
            for (int scanColumn = 0; scanColumn < room.Width; scanColumn++) {
                for (int scanRow = 0; scanRow < room.Size; scanRow++) {
                    if ((scanRow == 0 || scanRow == room.Size - 1) && (scanColumn == 0 || scanColumn == room.Width - 1)) {
                        maze[row + scanColumn, column + scanRow] = 'b';
                    }
                    else if (scanRow == 0 || scanRow == room.Size - 1 || scanColumn == 0 || scanColumn == room.Width - 1) {
                        maze[row + scanColumn, column + scanRow] = 'b';
                    }
                    else {
                        maze[row + scanColumn, column + scanRow] = 'b';
                    }
                }
            }
        }
    }
    private static bool ValidateRoomPosition(char[,] maze, RoomType ship, int row, int column, char orientation) {
        if (orientation == 'v' && row + ship.Size + 2 > mazeSize - 1 || column + ship.Width + 2 > mazeSize - 1) {
            return false;
        }
        else if (orientation == 'h' && column + ship.Size + 2 > mazeSize - 1 || row + ship.Width + 2 > mazeSize - 1) {
            return false;
        }
        if (orientation == 'v') {
            for (int scanColumn = -2; scanColumn < ship.Width + 2; scanColumn++) {
                for (int scanRow = -2; scanRow < ship.Size + 2; scanRow++) {
                    if (maze[row + scanRow, column + scanColumn] == 'b') {
                        return false;
                    }
                }

            }   
        }
        else if (orientation == 'h') {
            for (int scanColumn = -2; scanColumn < ship.Width + 2; scanColumn++) {
                for (int scanRow = -2; scanRow < ship.Size + 2; scanRow++) {
                    if (maze[row + scanColumn, column + scanRow] == 'b') {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public struct RoomType {
        public string Name;
        public int Size;
        public int Width;
    }
    public struct Room {
        public char Tile;
        public bool Visited;
        public ((int, int), (int, int)) Coordinates;
    }
}
