using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMaze : MonoBehaviour
{
    public GameObject wall;
    [SerializeField] public int mazeWidth;
    [SerializeField] public int mazeHeight;
    [SerializeField] public int mazeOriginX;
    [SerializeField] public int mazeOriginY;

    

    void Start() {
        createWaffle();
        MakeMaze();
    }

    void createWaffle() {
        for (int y = mazeOriginY; y < mazeHeight+1; y++) {
            if (y%2== 0) {
                for (int x = mazeOriginX; x < mazeWidth+1; x++) {
                    Instantiate(wall, new Vector3(x, y, 1), Quaternion.identity);
                }
            }
            else {
                for (int x = mazeOriginX; x < mazeWidth+1; x+=2) {
                    Instantiate(wall, new Vector3(x, y, 1), Quaternion.identity);
                }
            }
        }
    }

    void destroyObject(Vector3 location) {
        Collider[] hitColliders = Physics.OverlapSphere(location, 0.1f);

        for (int i = 0; i < hitColliders.Length; i++) {
            Destroy(hitColliders[i].gameObject);
            i++;
        }
    }

    bool checkWalls(Vector3 location) {
        Collider[] nExists = Physics.OverlapSphere(new Vector3(location.x, location.y + 1, 1), 0.1f);
        Collider[] eExists = Physics.OverlapSphere(new Vector3(location.x + 1, location.y, 1), 0.1f);
        Collider[] sExists = Physics.OverlapSphere(new Vector3(location.x, location.y - 1, 1), 0.1f);
        Collider[] wExists = Physics.OverlapSphere(new Vector3(location.x - 1, location.y, 1), 0.1f);

        return (nExists.Length > 0 && eExists.Length > 0 && sExists.Length > 0 && wExists.Length > 0) ? true : false;
    }

    Vector3 getStartPos() {
        System.Random r = new();
        bool valid = false;
        Vector3 pos;
        do {
            pos = new Vector3(r.Next(mazeOriginX, mazeWidth), r.Next(mazeOriginY, mazeHeight), 1);
            Collider[] posEmpty = Physics.OverlapSphere(pos, 0.1f);
            if (posEmpty.Length == 0) { valid = true; }
        } while (!valid);

        return pos;
    }

    void MakeMaze() {
        System.Random r = new();
        int cellsInMaze = (mazeWidth / 2) * (mazeHeight / 2);
        int cellsVisited = 1;
        ArrayList cellsVisitedVectors = new ArrayList();
        Vector3 constructorPos = getStartPos();
        cellsVisitedVectors.Add(constructorPos);

        while (cellsVisited < cellsInMaze) {
            ArrayList possibleDirections = CheckPossibleDirections(constructorPos);
            if (possibleDirections.Count != 0) {
                switch (possibleDirections[r.Next(0, possibleDirections.Count)]) {
                    case "left":
                        destroyObject(new Vector3(constructorPos.x - 1, constructorPos.y, 1));
                        constructorPos.x -= 2;
                        break;
                    case "right":
                        destroyObject(new Vector3(constructorPos.x + 1, constructorPos.y, 1));
                        constructorPos.x += 2;
                        break;
                    case "up":
                        destroyObject(new Vector3(constructorPos.x, constructorPos.y + 1, 1));
                        constructorPos.y += 2;
                        break;
                    case "down":
                        destroyObject(new Vector3(constructorPos.x, constructorPos.y - 1, 1));
                        constructorPos.y -= 2;
                        break;
                }   
                cellsVisited++;
                cellsVisitedVectors.Add(constructorPos);
            }
            else {
                Vector3 retrace = (Vector3)cellsVisitedVectors[cellsVisitedVectors.Count - 1]; //causes OOB error
                cellsVisitedVectors.RemoveAt(cellsVisitedVectors.Count - 1);
                constructorPos = retrace;
            }
        }
    }

    private ArrayList CheckPossibleDirections(Vector3 constructorPos) {
        ArrayList possibleDirections = new ArrayList();
        if (checkWalls(new Vector3(constructorPos.x - 2, constructorPos.y, 1))) {
            possibleDirections.Add("left");
        }
        if (checkWalls(new Vector3(constructorPos.x + 2, constructorPos.y, 1))) {
            possibleDirections.Add("right");
        }
        if (checkWalls(new Vector3(constructorPos.x, constructorPos.y + 2, 1))) {
            possibleDirections.Add("up");
        }
        if (checkWalls(new Vector3(constructorPos.x, constructorPos.y - 2, 1))) {
            possibleDirections.Add("down");
        }
        return possibleDirections;
    }
}
