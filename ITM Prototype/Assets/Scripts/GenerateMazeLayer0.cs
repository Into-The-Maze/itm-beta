using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

public class GenerateMazeLayer0 : MonoBehaviour
{
    public static int mazeSize = 40;
    public static Vector3 spawnPos;
    public GameObject player;

    public static char[,] CreateLabsMaze() {
        char[,] maze = CreateArray();
        Room[] roomList = new Room[0];
        PlaceDefaultAndRandomRooms(ref maze, ref roomList);
        int currentRoom = SetStartRoom(ref roomList);
        (char, int, int, int) start;
        return Mazeify(ref maze, ref roomList, ref currentRoom, out start);
    }

    public static char[,] Mazeify(ref char[,] maze, ref Room[] roomList, ref int currentRoom, out (char, int, int, int) start) {
        start = (' ', -1, -1, -1);
        while (!roomsVisited(roomList)) {
            start = PickStartTile(roomList, maze);
            while (start == ('!', 0, 0, 0)) {
                //Console.WriteLine("failed");
                maze = CreateArray();
                roomList = new Room[0];
                PlaceRooms(ref maze, MainRooms(), ref roomList);
                PlaceRooms(ref maze, RandomRooms(), ref roomList);
                currentRoom = SetStartRoom(ref roomList);
                start = PickStartTile(roomList, maze);
            }
            GenerateHallways(ref maze, ref roomList, start, ref currentRoom);
            //Console.WriteLine("done recursing");
        }
        //Console.WriteLine("Mazetasic!!");
        return (maze);
    }

    public static void PlaceDefaultAndRandomRooms(ref char[,] maze, ref Room[] roomList) {
        PlaceRooms(ref maze, MainRooms(), ref roomList);
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
        int numRooms = r.Next(2, 6);
        RoomType[] rooms = new RoomType[numRooms];
        for (int i = 0; i < numRooms; i++) {
            rooms[i].Name = Convert.ToString(i) + " random room";
            rooms[i].Size = r.Next(5, 7);
            rooms[i].Width = r.Next(5, 7);
        }
        return rooms;
    }
    public static char[,] CreateArray() {
        char[,] maze = new char[mazeSize, mazeSize];
        for (int row = 0; row < mazeSize; row++) {
            for (int column = 0; column < mazeSize; column++) {
                if (row == 0 || row == mazeSize - 1 || column == 0 || column == mazeSize - 1) {
                    maze[row, column] = 'x';
                }
                else {
                    maze[row, column] = '#';
                }
            }
        }
        return maze;
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
                //Console.WriteLine($"Placing {room.Name}");
            }
            else {
                //Console.WriteLine($"Failed to place {room.Name}");
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
                        maze[row + scanRow, column + scanColumn] = '-';
                    }
                    else if (scanRow == 0 || scanRow == room.Size - 1 || scanColumn == 0 || scanColumn == room.Width - 1) {
                        maze[row + scanRow, column + scanColumn] = room.Name[0];
                    }
                    else {
                        maze[row + scanRow, column + scanColumn] = ' ';
                    }
                }
            }
        }
        else if (orientation == 'h') {
            for (int scanColumn = 0; scanColumn < room.Width; scanColumn++) {
                for (int scanRow = 0; scanRow < room.Size; scanRow++) {
                    if ((scanRow == 0 || scanRow == room.Size - 1) && (scanColumn == 0 || scanColumn == room.Width - 1)) {
                        maze[row + scanColumn, column + scanRow] = '-';
                    }
                    else if (scanRow == 0 || scanRow == room.Size - 1 || scanColumn == 0 || scanColumn == room.Width - 1) {
                        maze[row + scanColumn, column + scanRow] = room.Name[0];
                    }
                    else {
                        maze[row + scanColumn, column + scanRow] = ' ';
                    }
                }
            }
        }
    }
    private static bool ValidateRoomPosition(char[,] maze, RoomType ship, int row, int column, char orientation) {
        if (orientation == 'v' && row + ship.Size > mazeSize - 1 || column + ship.Width > mazeSize - 1) {
            return false;
        }
        else if (orientation == 'h' && column + ship.Size > mazeSize - 1 || row + ship.Width > mazeSize - 1) {
            return false;
        }
        else {
            if (orientation == 'v') {
                for (int scanColumn = -1; scanColumn < ship.Width + 1; scanColumn++) {
                    for (int scanRow = -1; scanRow < ship.Size + 1; scanRow++) {
                        if (maze[row + scanRow, column + scanColumn] != '#') {
                            return false;
                        }
                    }

                }
            }
            else if (orientation == 'h') {
                for (int scanColumn = -1; scanColumn < ship.Width + 1; scanColumn++) {
                    for (int scanRow = -1; scanRow < ship.Size + 1; scanRow++) {
                        if (maze[row + scanColumn, column + scanRow] != '#') {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }
    //private static void ShowArray(char[,] maze) {
    //    Console.WriteLine();
    //    for (int i = 0; i < mazeSize; i++) {
    //        for (int x = 0; x < mazeSize; x++) {
    //            Console.Write($"{maze[x, i]} ");
    //        }
    //        Console.WriteLine();
    //    }
    //    Console.WriteLine();
    //}

    //private static void ShowRooms(Room[] roomList) {
    //    foreach (var room in roomList) {
    //        Console.WriteLine($"ID: {room.Tile}, Visited: {room.Visited}, Coordinates: {room.Coordinates}");
    //    }
    //}
    public static int SetStartRoom(ref Room[] roomList) {
        Random r = new Random();
        int currentRoom = r.Next(0, roomList.Length);
        roomList[currentRoom].Visited = true;
        return currentRoom;
    }
    private static void MarkRoom(ref Room[] roomList, ref char tile) {
        for (int i = 0; i < roomList.Length; i++) {
            if (roomList[i].Tile == tile) {
                roomList[i].Visited = true;
            }
        }
    }
    private static bool isRoom(Room[] roomList, char tile) {
        for (int i = 0; i < roomList.Length; i++) {
            if (roomList[i].Tile == tile) {
                return true;
            }
        }
        return false;
    }
    private static bool validMove(char tile) {
        if (tile == '#') {
            return true;
        }
        return false;
    }
    private static (char, int, int, int) PickStartTile(Room[] roomlist, char[,] maze) {
        Random r = new Random();
        Room startRoom;
        (int, int) nextPos;
        int direction = 1;
        bool valid = false;
        int i = 0;
        int tries = 0;
        //Console.WriteLine(startRoom.Tile);
        //Console.WriteLine(startRoom.Coordinates);
        char startTile;
        (int, int) coords;
        do {
            do {
                startRoom = roomlist[r.Next(0, roomlist.Length)];
            } while (!startRoom.Visited);
            startTile = startRoom.Tile;
            coords = (r.Next(startRoom.Coordinates.Item1.Item1, startRoom.Coordinates.Item2.Item1),
                r.Next(startRoom.Coordinates.Item1.Item2, startRoom.Coordinates.Item2.Item2));
            //Console.WriteLine($"{startRoom.Coordinates.Item1.Item1}, {startRoom.Coordinates.Item2.Item1}");
            //Console.WriteLine($"{startRoom.Coordinates.Item1.Item2}, {startRoom.Coordinates.Item2.Item2}");
            //Console.WriteLine(coords);
            i++;
            tries++;
            if (i == 5) {
                i = 1;
            }
            if (i == 1) {
                nextPos = (coords.Item1, coords.Item2 + 1);
            }
            else if (i == 2) {
                nextPos = (coords.Item1 + 1, coords.Item2);
            }
            else if (i == 3) {
                nextPos = (coords.Item1, coords.Item2 - 1);
            }
            else {
                nextPos = (coords.Item1 - 1, coords.Item2);
            }
            if ((maze[coords.Item1, coords.Item2] != '-' &&
                maze[coords.Item1, coords.Item2] != ' ') &&
                maze[coords.Item1, coords.Item2] != 'x' &&
                maze[coords.Item1, coords.Item2] != '.') {
                if (maze[nextPos.Item1, nextPos.Item2] == '#') {
                    direction = i;
                    valid = true;
                    return (startRoom.Tile, coords.Item1, coords.Item2, direction);
                }
            }
            if (tries > 1000) {
                return ('!', 0, 0, 0);
            }
        } while (!valid);
        return (startRoom.Tile, coords.Item1, coords.Item2, direction);
    }
    private static bool roomsVisited(Room[] roomList) {
        foreach (var room in roomList) {
            if (!room.Visited) {
                return false;
            }
        }
        return true;
    }
    public static int GetRoomIndex(Room[] roomList, char tile) {
        foreach (var room in roomList) {
            if (room.Tile == tile) {
                return Array.IndexOf(roomList, room);
            }
        }
        return 0;
    }
    private static void GenerateHallways(ref char[,] maze, ref Room[] roomList, (char, int, int, int) startPos, ref int currentRoom) {
        (int, int) currentPos = (startPos.Item2, startPos.Item3);
        (int, int) nextPos;
        Random r = new Random();
        int length = 0;
        bool stop = false;
        bool split = false;
        int nextStartDirection;
        do {
            if (startPos.Item4 == 1) {
                nextPos = (currentPos.Item1, currentPos.Item2 + 1);
            }
            else if (startPos.Item4 == 2) {
                nextPos = (currentPos.Item1 + 1, currentPos.Item2);
            }
            else if (startPos.Item4 == 3) {
                nextPos = (currentPos.Item1, currentPos.Item2 - 1);
            }
            else {
                nextPos = (currentPos.Item1 - 1, currentPos.Item2);
            }
            if (validMove(maze[nextPos.Item1, nextPos.Item2])) {
                //Console.WriteLine($"found a valid move");
                currentPos = nextPos;
                maze[currentPos.Item1, currentPos.Item2] = ' ';
                //ShowArray(maze);
                length++;
                if ((r.Next(2, 5) % 2 == 0) && (length % 3 == 0)) {
                    //Console.WriteLine($"if split chance");
                    split = true;
                }
                else {
                    //Console.WriteLine($"else split chance");
                    split = false;
                }
                if (split) {
                    //Console.WriteLine($"split");
                    if (startPos.Item4 == 1 || startPos.Item4 == 3) {
                        //Console.WriteLine($"is vertical");
                        if (r.Next(0, 2) == 0) {
                            nextStartDirection = 2;
                        }
                        else {
                            nextStartDirection = 4;
                        }
                    }
                    else {
                        //Console.WriteLine($"is horizontal");
                        if (r.Next(0, 2) == 0) {
                            nextStartDirection = 1;
                        }
                        else {
                            nextStartDirection = 3;
                        }
                    }
                    //Console.WriteLine($"new start direction: {nextStartDirection}");
                    GenerateHallways(ref maze, ref roomList, ((maze[currentPos.Item1, currentPos.Item2]), currentPos.Item1, currentPos.Item2, nextStartDirection), ref currentRoom);
                }
            }
            else {
                //Console.WriteLine($"not valid");
                if (isRoom(roomList, maze[nextPos.Item1, nextPos.Item2])) {
                    MarkRoom(ref roomList, ref maze[nextPos.Item1, nextPos.Item2]);
                    currentRoom = GetRoomIndex(roomList, maze[nextPos.Item1, nextPos.Item2]);
                    maze[nextPos.Item1, nextPos.Item2] = '.';
                }
                if (isRoom(roomList, maze[startPos.Item2, startPos.Item3])) {
                    MarkRoom(ref roomList, ref maze[startPos.Item2, startPos.Item3]);
                    maze[startPos.Item2, startPos.Item3] = '.';
                }
                //MarkRoom(ref roomList, ref maze[nextPos.Item1, nextPos.Item2]);
                //MarkRoom(ref roomList, ref maze[startPos.Item2, startPos.Item3]);
                stop = true;
            }
        } while (!stop);
        //Console.WriteLine("stopped, going back to previous function");
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
