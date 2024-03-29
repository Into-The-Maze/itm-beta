﻿using DG.Tweening;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using Random = System.Random;
using Pathfinding;
using UnityEditor.SceneManagement;
using System.Collections;

public class InstantiateMaze : MonoBehaviour {

    private Random r = new Random();
    [SerializeField] private GameObject NavMesh;
    [SerializeField] private GameObject player;
    [SerializeField] private Volume volume;
    [SerializeField] private GameObject mazeParent;
    [SerializeField] private GameObject cameraParent;
    [SerializeField] private GameObject shadowCasterParent;

    [SerializeField] private GameObject globalLightObj;
    [SerializeField] private GameObject globalWallLightObj;
    [SerializeField] private GameObject fogs;
    private Light2D globalLight;
    private Light2D globalWallLight;

    #region Crates
    public GameObject woodCrate;
    #endregion

    #region Enemies
    public GameObject crawler;
    #endregion

    #region layer0
    [SerializeField] private GameObject layer0Wall;
    [SerializeField] private GameObject layer0Floor;
    [SerializeField] private GameObject layer0Door;
    #endregion

    #region layer1

    [HideInInspector] public List<GameObject> lampPosts = new List<GameObject>();
    [SerializeField] private GameObject rainDrops;
    [SerializeField] private GameObject layer1Wall;
    [SerializeField] private GameObject layer1Floor;
    #endregion

    #region layer2
    [SerializeField] private GameObject layer2Wall;
    [SerializeField] private GameObject layer2WaterFloor;
    [SerializeField] private GameObject layer2Floor;
    [SerializeField] private GameObject layer2IllusionFloor;
    #endregion

    #region layer3
    [SerializeField] private GameObject layer3Wall;
    [SerializeField] private GameObject layer3Floor;
    [SerializeField] private GameObject layer3RoomFloor;
    [SerializeField] private GameObject layer3RoomEdgeRight;
    [SerializeField] private GameObject layer3RoomEdgeLeft;
    [SerializeField] private GameObject layer3RoomEdgeTop;
    [SerializeField] private GameObject layer3RoomEdgeBottom;
    [SerializeField] private GameObject layer3Blood1;
    [SerializeField] private GameObject layer3Blood2;
    [SerializeField] private GameObject layer3Blood3;
    [SerializeField] private GameObject layer3Skull1;
    [SerializeField] private GameObject layer3Skull2;
    #endregion

    #region layer4
    [SerializeField] private GameObject layer4WallHorizontal;
    [SerializeField] private GameObject layer4WallVertical;
    [SerializeField] private GameObject layer4Floor;
    [SerializeField] private GameObject layer4Corner;
    #endregion

    #region lights
    [SerializeField] private GameObject candle;
    [SerializeField] private GameObject lampPost;
    //[SerializeField] private GameObject lampPostFlicker;
    #endregion

    [HideInInspector] public static string weather;
    [HideInInspector] public static string power;
    [HideInInspector] public static string setting;

    private void Awake() {
        Volume volume = GetComponent<Volume>();
        StartCoroutine(instantiateSelectedMaze());
    }

    IEnumerator instantiateSelectedMaze() {
        bool mazeInputGot = false;
        while (!mazeInputGot) {
            if (Input.GetKeyDown(KeyCode.Alpha0)) {
                InstantiateMazeLayer0(InitialiseLayer0());
                mazeInputGot = true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1)) {
                InstantiateMazeLayer1(initialiseMazeLayer1());
                mazeInputGot = true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                InstantiateMazeLayer2(initialiseMazeLayer2());
                mazeInputGot = true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3)) {
                InstantiateMazeLayer3(initialiseMazeLayer3());
                mazeInputGot = true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4)) {
                InstantiateLayer4Pyramid(InitialiseLayer4Pyramid());
                mazeInputGot = true;
            }
            yield return null;
        }
        Vector3 spawn = RandomFloorPoint();
        player.transform.position = spawn;
        PopulateMaze();
        StopCoroutine(instantiateSelectedMaze());
    }

    private void PopulateMaze() {
        for (int i = 0; i < 15; i++) {
            Instantiate(crawler, RandomFloorPoint(), Quaternion.identity);
            Instantiate(woodCrate, RandomFloorPoint(), Quaternion.identity);
        }
    }

    private void InstantiateMazeLayer0(char[,] maze) {
        GameObject Navmesh = Instantiate(NavMesh, new Vector3(0, 0, 0), Quaternion.identity);
        AstarPath.active.data.gridGraph.SetDimensions(40, 40, 4f);
        AstarPath.active.data.gridGraph.center = new Vector3(78f, 78f, 0f);
        Bloom bloom;
        volume.profile.TryGet(out bloom);
        {
            bloom.dirtIntensity.value = 0f;
            bloom.intensity.value = 1f;
        }
        int layer0Upscale = 4;
        setting = SetLayer0Setting();
        power = SetLayer0Power();
        if (setting == "breach") {
            LabsBreachController breachController = gameObject.AddComponent<LabsBreachController>();
            breachController.GetLights(globalLight, globalWallLight);
        }
        Debug.Log($"{setting}, {power}");
        for (int y = 0; y < maze.GetLength(0); y ++ ) {
            for (int x = 0; x < maze.GetLength(1); x++ ) {
                if (maze[y, x] == ' ') {
                    GameObject floor = Instantiate(layer0Floor, new Vector3(layer0Upscale * x, layer0Upscale * y, 0), Quaternion.identity);
                    floor.transform.parent = mazeParent.transform;
                }
                else if (maze[y, x] == '.') {
                    GameObject door = Instantiate(layer0Door, new Vector3(layer0Upscale * x, layer0Upscale * y, 0), Quaternion.identity);
                    door.transform.parent = mazeParent.transform;
                }
                else {
                    GameObject wall = Instantiate(layer0Wall, new Vector3(layer0Upscale * x, layer0Upscale * y, 0), Quaternion.identity);
                    wall.transform.parent = mazeParent.transform;
                }
            }
        }
        AstarPath.active.Scan();
    }
    private void InstantiateMazeLayer1(char[,] maze) {
        GameObject Navmesh = Instantiate(NavMesh, new Vector3(0, 0, 0), Quaternion.identity);
        AstarPath.active.data.gridGraph.SetDimensions(35, 35, 8f);
        AstarPath.active.data.gridGraph.center = new Vector3(136f, 136f, 0f);
        Bloom bloom;
        volume.profile.TryGet(out bloom);
        {
            bloom.dirtIntensity.value = 15f;
            bloom.intensity.value = 2.5f;
        }
        int layer1Upscale = 8;
        int lampPostChance = 50;
        setting = SetLayer1Setting();
        weather = SetLayer1Weather(layer1Upscale);
        Debug.Log($"{setting}, {weather}");
        if (weather == "storm") {
            GameObject rain = Instantiate(rainDrops, new Vector3(0, 0, -5), Quaternion.Euler(0f, 0f, 0f), cameraParent.transform);
            if (setting == "blood moon") {
                ParticleSystem rainEmitter = rain.GetComponent<ParticleSystem>();
#pragma warning disable CS0618 // Type or member is obsolete
                rainEmitter.startColor = hexToColor("0xFF000065");
#pragma warning restore CS0618 // Type or member is obsolete
                lampPostChance = 30;
            }
        }
        for (int y = 0; y < maze.GetLength(0); y++ ) {
            for (int x = 0; x < maze.GetLength(1); x++) {
                GameObject lamp;
                if (maze[y, x] == ' ') {
                    Instantiate(layer1Floor, new Vector3(layer1Upscale * x, layer1Upscale * y, 0), Quaternion.identity, mazeParent.transform);
                    if (r.Next(1, 101) <= lampPostChance) {
                        int side = r.Next(1, 5); // 1:top 2:right 3:bottom 4:left
                        if (side == 1 && maze[y + 1, x] != ' ') {
                            lamp = Instantiate(lampPost, new Vector3(layer1Upscale * x, layer1Upscale * y + 2.5f, 0), Quaternion.Euler(0f, 0, 180f), mazeParent.transform);
                            lampPosts.Add(lamp);
                        }
                        else if (side == 2 && maze[y, x + 1] != ' ') {
                            lamp = Instantiate(lampPost, new Vector3(layer1Upscale * x + 2.5f, layer1Upscale * y, 0), Quaternion.Euler(0f, 0, 90f), mazeParent.transform);
                            lampPosts.Add(lamp);
                        }
                        else if (side == 3 && maze[y - 1, x] != ' ') {
                            lamp = Instantiate(lampPost, new Vector3(layer1Upscale * x, layer1Upscale * y - 2.5f, 0), Quaternion.Euler(0f, 0f, 0f), mazeParent.transform);
                            lampPosts.Add(lamp);
                        }
                        else if (side == 4 && maze[y, x - 1] != ' ') {
                            lamp = Instantiate(lampPost, new Vector3(layer1Upscale * x - 2.5f, layer1Upscale * y, 0), Quaternion.Euler(0f, 0, 270f), mazeParent.transform);
                            lampPosts.Add(lamp);
                        }
                    }
                }
                else {
                    Instantiate(layer1Wall, new Vector3(layer1Upscale * x, layer1Upscale * y, 0), Quaternion.identity, shadowCasterParent.transform);
                }
            }
        }
        AstarPath.active.Scan();
    }
    private void InstantiateMazeLayer2(char[,] maze) {
        GameObject Navmesh = Instantiate(NavMesh, new Vector3(0, 0, 0), Quaternion.identity);
        AstarPath.active.data.gridGraph.SetDimensions(41, 41, 4f);
        AstarPath.active.data.gridGraph.center = new Vector3(80f, 80f, 0f);
        globalLight = globalLightObj.GetComponent<Light2D>();
        globalWallLight = globalWallLightObj.GetComponent<Light2D>();
        globalLight.intensity = 0.625f;
        globalWallLight.intensity = 0.75f;
        int layer2Upscale = 4;
        for (int y = 0; y < maze.GetLength(0); y++) {
            for (int x = 0; x < maze.GetLength(1); x++) {
                if (maze[y, x] == '#') {
                    GameObject wall = Instantiate(layer2Wall, new Vector3(layer2Upscale * x, layer2Upscale * y, 0), Quaternion.identity);
                    wall.transform.parent = mazeParent.transform;
                }
                else {
                    
                    //string floorPicker = Convert.ToString(r.Next(0, 10));
                    //GameObject floor = Instantiate(layer2Floor, new Vector3(layer2Upscale * x, layer2Upscale * y, 0), Quaternion.identity);
                    //floor.transform.parent = mazeParent.transform;
                    //if ("56789".Contains(floorPicker)) {
                    //    GameObject water = Instantiate(layer2WaterFloor, new Vector3(layer2Upscale * x, layer2Upscale * y, 0), Quaternion.identity);
                    //    water.transform.parent = mazeParent.transform;
                    //}

                    GameObject floor = Instantiate((maze[y, x] == '0') ? layer2Floor : layer2WaterFloor, new Vector3(layer2Upscale * x, layer2Upscale * y, 0), Quaternion.identity, mazeParent.transform);
                    if (maze[y,x] != '0') { floor.GetComponent<WaterDepthContainer>().depth = (float)((maze[y, x] != '*') ? char.GetNumericValue(maze[y, x]) * 0.1f : 1); }

                    //ADD ILLUSION FLOOR FUNCTIONALITY LATER BECAUSE THIS METHOD MAKES TOO MANY ILLUSION FLOORS AND THEYRE ALL UNAVOIDABLE

                    //else {
                    //    Instantiate(layer2IllusionFloor, new Vector3(3 * x, 3 * y, 0), Quaternion.identity);
                    //}
                }
            }
        }
        AstarPath.active.Scan();
    }
    private void InstantiateMazeLayer3(char[,] maze) {
        globalLight = globalLightObj.GetComponent<Light2D>();
        globalWallLight = globalWallLightObj.GetComponent<Light2D>();
        globalLight.intensity = 0f;
        globalWallLight.intensity = 0.125f;
        for (int y = 0; y < maze.GetLength(0); y++) {
            for (int x = 0; x < maze.GetLength(1); x++) {
                if (maze[x, y] == ' ') {
                    GameObject floor = Instantiate(layer3Floor, new Vector3(2 * x, 2 * y, 0), Quaternion.identity);
                    floor.transform.parent = mazeParent.transform;
                    //blood
                    if (r.Next(0, 100) < 15) {
                        GameObject blood = Instantiate(layer3Blood1, new Vector3((2 * x), (2 * y)), Quaternion.identity);
                        blood.transform.parent = mazeParent.transform;
                    }
                    if (r.Next(0, 100) < 15) {
                        GameObject blood = Instantiate(layer3Blood2, new Vector3((2 * x), (2 * y)), Quaternion.identity);
                        blood.transform.parent = mazeParent.transform;
                    }
                    if (r.Next(0, 100) < 15) {
                        GameObject blood = Instantiate(layer3Blood2, new Vector3((2 * x), (2 * y)), Quaternion.identity);
                        blood.transform.parent = mazeParent.transform;
                    }
                    //candles
                    if (r.Next(0, 100) < 25) {
                        GameObject item = Instantiate(candle, new Vector3((2 * x) + (r.Next(-80, 80) / 100f), (2 * y) + (r.Next(-80, 80) / 100f)), Quaternion.identity);
                        item.transform.parent = mazeParent.transform;
                    }
                }
                else if (maze[x, y] == 'b') {
                    GameObject floor = Instantiate(layer3RoomFloor, new Vector3(2 * x, 2 * y, 0), Quaternion.identity);
                    floor.transform.parent = mazeParent.transform;
                    if (maze[x + 1, y] == ' ') {
                        GameObject edge = Instantiate(layer3RoomEdgeRight, new Vector3((2 * x) + 2, 2 * y, 0), Quaternion.identity);
                        edge.transform.parent = mazeParent.transform;
                    }
                    if (maze[x, y + 1] == ' ') {
                        GameObject edge = Instantiate(layer3RoomEdgeTop, new Vector3(2 * x, (2 * y) + 2, 0), Quaternion.identity);
                        edge.transform.parent = mazeParent.transform;
                    }
                    if (maze[x - 1, y] == ' ') {
                        GameObject edge = Instantiate(layer3RoomEdgeLeft, new Vector3((2 * x) - 2, 2 * y, 0), Quaternion.identity);
                        edge.transform.parent = mazeParent.transform;
                    }
                    if (maze[x, y - 1] == ' ') {
                        GameObject edge = Instantiate(layer3RoomEdgeBottom, new Vector3(2 * x, (2 * y) - 2, 0), Quaternion.identity);
                        edge.transform.parent = mazeParent.transform;
                    }
                    //blood
                    if (r.Next(0, 100) < 25) {
                        GameObject blood = Instantiate(layer3Blood1, new Vector3((2 * x) + (r.Next(-80, 80) / 100f), (2 * y) + (r.Next(-80, 80) / 100f)), Quaternion.identity);
                        blood.transform.parent = mazeParent.transform;
                    }
                    if (r.Next(0, 100) < 25) {
                        GameObject blood = Instantiate(layer3Blood2, new Vector3((2 * x) + (r.Next(-80, 80) / 100f), (2 * y) + (r.Next(-80, 80) / 100f)), Quaternion.identity);
                        blood.transform.parent = mazeParent.transform;
                    }
                    if (r.Next(0, 100) < 25) {
                        GameObject blood = Instantiate(layer3Blood2, new Vector3((2 * x) + (r.Next(-80, 80) / 100f), (2 * y) + (r.Next(-80, 80) / 100f)), Quaternion.identity);
                        blood.transform.parent = mazeParent.transform;  
                    }
                    //skulls
                    if (r.Next(0, 100) < 30) {
                        GameObject skull = Instantiate(layer3Skull1, new Vector3((2 * x) + (r.Next(-80, 80) / 100f), (2 * y) + (r.Next(-80, 80) / 100f)), Quaternion.identity);
                        skull.transform.parent = mazeParent.transform;
                    }
                    if (r.Next(0, 100) < 30) {
                        GameObject skull = Instantiate(layer3Skull2, new Vector3((2 * x) + (r.Next(-80, 80) / 100f), (2 * y) + (r.Next(-80, 80) / 100f)), Quaternion.identity);
                        skull.transform.parent = mazeParent.transform;
                    }
                }
                else {
                    GameObject wall = Instantiate(layer3Wall, new Vector3(2 * x, 2 * y, 0), Quaternion.identity);
                    wall.transform.parent = mazeParent.transform;
                }
            }
        }
    }
    // actual layer4 (desert)
    private void InstantiateLayer4Pyramid(GenerateMazeLayer4.Tile[,] maze) {
        globalLight = globalLightObj.GetComponent<Light2D>();
        globalWallLight = globalWallLightObj.GetComponent<Light2D>();
        globalLight.intensity = 0f;
        globalWallLight.intensity = 0;
        int layer4Upscale = 8;
        for (int y = 0; y < maze.GetLength(0); y ++) {
            for (int x = 0; x < maze.GetLength(1); x ++) {
                var floor = Instantiate(layer4Floor, new Vector3((layer4Upscale * x), (layer4Upscale * y), 0), Quaternion.identity);
                var corner1 = Instantiate(layer4Corner, new Vector3((layer4Upscale * x) + 3.75f, (layer4Upscale * y) + 3.75f, 0), Quaternion.identity);
                var corner2 = Instantiate(layer4Corner, new Vector3((layer4Upscale * x) - 3.75f, (layer4Upscale * y) + 3.75f, 0), Quaternion.identity);
                var corner3 = Instantiate(layer4Corner, new Vector3((layer4Upscale * x) + 3.75f, (layer4Upscale * y) - 3.75f, 0), Quaternion.identity);
                var corner4 = Instantiate(layer4Corner, new Vector3((layer4Upscale * x) - 3.75f, (layer4Upscale * y) - 3.75f, 0), Quaternion.identity);
                floor.transform.parent = mazeParent.transform;
                if (r.Next(0, 100) < 25) {
                    GameObject item = Instantiate(candle, new Vector3((layer4Upscale * x) + (r.Next(-80, 80) / 25f), (layer4Upscale * y) + (r.Next(-80, 80) / 25f)), Quaternion.identity);
                    item.transform.parent = mazeParent.transform;
                }
                corner1.transform.parent = mazeParent.transform;
                corner2.transform.parent = mazeParent.transform;
                corner3.transform.parent = mazeParent.transform;
                corner4.transform.parent = mazeParent.transform;
                if (maze[x, y].TopWall == true) {
                    var topwall = Instantiate(layer4WallHorizontal, new Vector3((layer4Upscale * x), (layer4Upscale * y) + 3.75f, 0), Quaternion.identity);
                    topwall.transform.parent = mazeParent.transform;
                }   
                if (maze[x, y].RightWall == true) {
                    var rightwall = Instantiate(layer4WallVertical, new Vector3((layer4Upscale * x) + 3.75f, (layer4Upscale * y), 0), Quaternion.identity);
                    rightwall.transform.parent = mazeParent.transform;
                }
                if (maze[x, y].BottomWall == true) {
                    var bottomwall = Instantiate(layer4WallHorizontal, new Vector3((layer4Upscale * x), (layer4Upscale * y) - 3.75f, 0), Quaternion.identity);
                    bottomwall.transform.parent = mazeParent.transform;
                }
                if (maze[x, y].LeftWall == true) {
                    var leftwall = Instantiate(layer4WallVertical, new Vector3((layer4Upscale * x) - 3.75f, (layer4Upscale * y), 0), Quaternion.identity);
                    leftwall.transform.parent = mazeParent.transform;
                }
            }
        }
    }

    private char[,] InitialiseLayer0() {
        char[,] maze = GenerateMazeLayer0.CreateArray();
        GenerateMazeLayer0.Room[] roomList = new GenerateMazeLayer0.Room[0];
        GenerateMazeLayer0.PlaceDefaultAndRandomRooms(ref maze, ref roomList);

        int currentRoom = GenerateMazeLayer0.SetStartRoom(ref roomList);
        (char, int, int, int) start;
        return GenerateMazeLayer0.Mazeify(ref maze, ref roomList, ref currentRoom, out start);
    }
    private char[,] initialiseMazeLayer1() {
        return GenerateMazeLayer1.generatePerfectMaze();
    }
    private char[,] initialiseMazeLayer2() {
        char[,] maze = GenerateMazeLayer2.makeBinaryTreeMaze();
        return maze;
    }
    private char[,] initialiseMazeLayer3() {
        char[,] maze = GenerateMazeLayer3.GenerateMaze();
        return maze;
    }
    private GenerateMazeLayer4.Tile[,] InitialiseLayer4Pyramid() {
        GenerateMazeLayer4.Tile[,] maze = GenerateMazeLayer4.CreateHuntKillMaze();
        return maze;
    }

    public static Vector3 RandomFloorPoint() {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        int attempts = 0;
        while (true) {
            if (attempts == 100) {
                Debug.Log("No Floor found within 100 attempts!");
                return new Vector3(10, 10, 0); 
            }
            GameObject spawnPos = allObjects[(int)Math.Truncate((decimal)UnityEngine.Random.Range(0, allObjects.Length-1))];
            if (spawnPos.CompareTag("FLOOR")) {
                return new Vector3(spawnPos.transform.position.x, spawnPos.transform.position.y, 0);
            }
            attempts++;
        }
    }
    private string SetLayer0Setting() {
        int i;
        int random = r.Next(1, 101);
        string[] lightColours = { "FFFFFF", "9A2E1C" };
        string[] settingNames = { "normal", "breach" };
        globalLight = globalLightObj.GetComponent<Light2D>();
        globalWallLight = globalWallLightObj.GetComponent<Light2D>();
        if (random <= 76) {
            i = 0; // normal
        }
        else {
            i = 1; // breach
        }
        globalLight.color = hexToColor(lightColours[i]);
        globalWallLight.color = hexToColor(lightColours[i]);
        return settingNames[i];
    }
    private string SetLayer0Power() {
        int i;
        int random = r.Next(1, 101);
        float[] lightLevels = { 0.875f, 0.375f };
        string[] settingNames = { "full", "low" };
        globalLight = globalLightObj.GetComponent<Light2D>();
        globalWallLight = globalWallLightObj.GetComponent<Light2D>();
        if (random <= 76) {
            i = 0; // full
        }
        else {
            i = 1; // low
        }
        globalLight.intensity = lightLevels[i];
        globalWallLight.intensity = lightLevels[i];
        return settingNames[i];
    }
    // layer 0 setting
    private string SetLayer1Setting() {
        int i;
        int random = r.Next(1, 101);
        string[] lightColours = { "FFFFFF", "4444FF", "4444FF", "FF0000" };
        float[] lightLevels = { 0.25f, 1.25f, 2f, 0.8125f };
        string[] settingNames = { "starlight", "moonlight", "full moon", "blood moon" };
        globalLight = globalLightObj.GetComponent<Light2D>();   
        globalWallLight = globalWallLightObj.GetComponent<Light2D>();
        if (random <= 26) {
            i = 0; // starlight
        }
        else if (random <= 91) {
            i = 1; // moonlight
        }
        else if (random <= 99) {
            i = 2; // full moon
        }
        else {
            i = 3; // blood moon
        }

        globalLight.intensity = lightLevels[i];
        globalWallLight.intensity = lightLevels[i];
        globalLight.color = hexToColor(lightColours[i]);
        globalWallLight.color = hexToColor(lightColours[i]);
        return settingNames[i];
    }
    private string SetLayer1Weather(int layer1Upscale) {
        int random = r.Next(1, 5);
        Vignette vignette;
        string[] weathers = { "clear", "storm" };
        GameObject fog = Instantiate(fogs, new Vector3(0, 0, 1), Quaternion.Euler(0f, 0f, 0f), cameraParent.transform);
        ParticleSystem fogR = fog.transform.GetChild(0).GetComponent<ParticleSystem>();
        ParticleSystem fogL = fog.transform.GetChild(1).GetComponent<ParticleSystem>();
        if (random <= 3) {
            volume.profile.TryGet(out vignette);
            {
                vignette.intensity.value = 0.3f;
            }
#pragma warning disable CS0618 // Type or member is obsolete
            fogR.emissionRate = 1f;
            fogL.emissionRate = 1f;
#pragma warning restore CS0618 // Type or member is obsolete
            return weathers[0];     
        }
        else {
            volume.profile.TryGet(out vignette);
            {
                vignette.intensity.value = 0.6f;
            }
#pragma warning disable CS0618 // Type or member is obsolete
            fogR.emissionRate = 2f;
            fogL.emissionRate = 2f;
#pragma warning restore CS0618 // Type or member is obsolete
            return weathers[1];
        }
    }
    // layer 2 setting
    // layer 3 setting
    // layer 4 setting

    /// <summary>
    /// Method to convert hex RGB string of format "0xFFFFFF" into a UnityEngine.Color value.
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    private UnityEngine.Color hexToColor(string hex) {
        hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
        byte a = 255;//assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8) {
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }
}