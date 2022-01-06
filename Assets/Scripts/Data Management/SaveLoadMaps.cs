using System.IO;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadMaps : MonoBehaviour
{

    [System.Serializable]
    public class individualTileData
    {
        public string tileID;
        public Vector2 warpCords;
        public int switchNumber;
        public int switchNumberEffect;
        public int switchTransformInto;
        //more data, like starting positions, enemy positions, enemy data, etc, can be saved here... but that's for later!
    }
    public class savedLevelData
    {
        public int gridDepth;
        public int gridWidth;
        public Vector2 playerStartCoords;
        public int assignedChallengeSkillBalance;
        public int assignedClearGoals;
        public int assignedLossOfSelfConsciousness;
        public List<individualTileData> iTD = new List<individualTileData>(550);
    }

    private string json;
    public string dataPath;
    public int selectedMap = 0;

    public bool mapReadyToLoad = false;
    public bool mapReadyToSave = false;

    [HideInInspector]
    public individualTileData chosenMapData = new individualTileData();

    private TileDatabase tileDatabase;
    private IndividualTileManager iTM;
    private CreateGrid createdGrid;
    private DataCollection dataCollection;
    private CharacterMovement characterManager;
    private CameraMovement cameraManager;
    private RoomManager roomManager;

    private int gridDepth, gridWidth;

    private string inputtedLevel;

    public savedLevelData savedLevel = new savedLevelData();

    private string searchingForLevel;
    private string searchingForRoomPredictedStats;
    private string searchingForTotalPredictedStats;
    private int searchingForLevelValue = 2;
    public savedLevelData searchingForLevelData = new savedLevelData();
    public allTheStatistics searchingForRoomPredictedStatsData = new allTheStatistics();
    public allTheStatistics searchingForTotalPredictedStatsData = new allTheStatistics();

    public Vector3 spawnLocation;

    public int levelToBeLoaded;

    void Start()
    {
        //for builds
        //dataPath = Application.persistentDataPath + "/";
        //for editor

        //can add a folder here to distinguish playthroughs for testing
        dataPath = System.IO.Directory.GetCurrentDirectory() + "/Assets/SavedRooms/";

        tileDatabase = this.GetComponent<TileDatabase>();
        createdGrid = this.GetComponent<CreateGrid>();
        dataCollection = this.GetComponent<DataCollection>();
        characterManager = this.GetComponent<CharacterMovement>();
        cameraManager = this.GetComponent<CameraMovement>();
        roomManager = this.GetComponent<RoomManager>();
        gridDepth = createdGrid.gridDepth;
        gridWidth = createdGrid.gridWidth;
    }
    void Update()
    {
        if (mapReadyToLoad)
        {
            createdGrid.resetGrid();
            iTM = null;
            LoadLevelData(selectedMap);
        }
        if (mapReadyToSave)
        {
            SaveLevelData(selectedMap);
        }
    }
    public bool LoadLevelData(int selectedLevel)
    {
        createdGrid.resetGrid();
        savedLevel = new savedLevelData();
        inputtedLevel = dataPath + "Room " + selectedLevel + ".json";
        if (File.Exists(inputtedLevel))
        {
            json = File.ReadAllText(inputtedLevel);
            savedLevel = JsonUtility.FromJson<savedLevelData>(json);

            createdGrid.gridDepth = savedLevel.gridDepth;
            createdGrid.gridWidth = savedLevel.gridWidth;
            createdGrid.initTiles();

            characterManager.playerLocation = savedLevel.playerStartCoords;
            characterManager.playerLocationGoing = savedLevel.playerStartCoords;
            characterManager.initPlayer();

            cameraManager.newCameraLocation = characterManager.newLocationVector;
            cameraManager.assignedCamera.transform.position = new Vector3(cameraManager.newCameraLocation.x, 
                cameraManager.newCameraLocation.y + 50f,cameraManager.newCameraLocation.z);

            for (int i = 0; i < createdGrid.tileData.storedGameObjects.Length; i++)
            {
                iTM = createdGrid.tileData.storedGameObjects[i].GetComponent<IndividualTileManager>();
                for (int j = 0; j < tileDatabase.allTileTypes.Length; j++)
                {
                    if (savedLevel.iTD[i].tileID == tileDatabase.allTileTypes[j].tileName)
                    {
                        iTM.tileData = tileDatabase.allTileTypes[j];
                    }
                }
                if (savedLevel.iTD[i].warpCords != new Vector2(0, 0))
                {
                    // change this to warp cords of level chosen 
                    iTM.warpCords = savedLevel.iTD[i].warpCords;
                }
                if (savedLevel.iTD[i].switchNumber != 0)
                {
                    iTM.switchNumber = savedLevel.iTD[i].switchNumber;
                }
                if (savedLevel.iTD[i].switchNumberEffect != 0)
                {
                    iTM.switchNumberEffect = savedLevel.iTD[i].switchNumberEffect;
                }
                if (savedLevel.iTD[i].switchTransformInto != iTM.switchTransformInto)
                {
                    iTM.switchTransformInto = savedLevel.iTD[i].switchTransformInto;
                }
            }
        }
        mapReadyToLoad = false;
        return true;
    }

    public void SaveLevelData(int selectedLevel)
    {
        inputtedLevel = dataPath + "Room " + selectedLevel + ".json";
        savedLevel = new savedLevelData();
        savedLevel.gridDepth = createdGrid.gridDepth;
        savedLevel.gridWidth = createdGrid.gridWidth;
        savedLevel.playerStartCoords = createdGrid.playerStartCoords;
        savedLevel.assignedChallengeSkillBalance = createdGrid.assignedChallengeSkillBalance;
        savedLevel.assignedClearGoals = createdGrid.assignedClearGoals;
        savedLevel.assignedLossOfSelfConsciousness = createdGrid.assignedLossOfSelfConsciousness;
        foreach (Transform child in transform)
        {
            chosenMapData = new individualTileData();
            iTM = child.GetComponent<IndividualTileManager>();
            chosenMapData.tileID = iTM.tileData.tileName;
            chosenMapData.warpCords = iTM.warpCords;
            chosenMapData.switchNumber = iTM.switchNumber;
            chosenMapData.switchNumberEffect = iTM.switchNumberEffect;
            if (iTM.switchTransformInto != 0)
            {
                chosenMapData.switchTransformInto = iTM.switchTransformInto;
            }
            else
            {
                chosenMapData.switchTransformInto = 0;
            }
            savedLevel.iTD.Add(chosenMapData);
        }
        json = JsonUtility.ToJson(savedLevel, true);
        File.WriteAllText(inputtedLevel, json);
        Debug.Log(inputtedLevel);
        mapReadyToSave = false;
    }
    public bool VerifyLevelExists(int selectedLevel)
    {
        inputtedLevel = dataPath + "Room " + selectedLevel + ".json";
        if (File.Exists(inputtedLevel))
        {
            return true;
        }
        return false;
    }
    public void DecideWhatLevelToLoad()
    {
        dataPath = System.IO.Directory.GetCurrentDirectory() + "/Assets/SavedData/";

        ////retrieve total stats data
        //searchingForTotalPredictedStats = dataPath + "Playthrough " + dataCollection.saveNumber + " Full.json";
        //searchingForTotalPredictedStatsData = new predictedStats();
        //json = File.ReadAllText(searchingForTotalPredictedStats);
        //searchingForTotalPredictedStatsData = JsonUtility.FromJson<predictedStats>(json);

        //retrive room stats data
        searchingForRoomPredictedStats = dataPath + "Playthrough " + dataCollection.saveNumber + " Room " + dataCollection.roomNumber + ".json";
        searchingForRoomPredictedStatsData = new allTheStatistics();
        json = File.ReadAllText(searchingForRoomPredictedStats);
        searchingForRoomPredictedStatsData = JsonUtility.FromJson<allTheStatistics>(json);
        Debug.Log(dataPath + "Playthrough " + dataCollection.saveNumber + " Room " + dataCollection.roomNumber + ".json");

        //use statistics to decide what level to load
        dataPath = System.IO.Directory.GetCurrentDirectory() + "/Assets/SavedRooms/";
        for (int l = 1; l < 47; l++)
        {
            searchingForLevel = dataPath + "Room " + l + ".json";
            if (File.Exists(searchingForLevel))
            {
                searchingForLevelData = new savedLevelData();
                json = File.ReadAllText(searchingForLevel);
                searchingForLevelData = JsonUtility.FromJson<savedLevelData>(json);
                Debug.Log("Checking Level " + l);
                Debug.Log("Expected Level: " + searchingForRoomPredictedStatsData.predictingStats.challengeSkillBalance
                    + " " + searchingForRoomPredictedStatsData.predictingStats.clearGoals
                    + " " + searchingForRoomPredictedStatsData.predictingStats.lossOfSelfConsciousness);
                for (int a = 0; a < 6; a++)
                {
                    //Debug.Log(a + " " + searchingForRoomPredictedStatsData.predictingStats.challengeSkillBalance 
                    //    + " " + searchingForLevelData.assignedChallengeSkillBalance
                    //    + " " + searchingForLevelData.assignedClearGoals
                    //    + " " + searchingForLevelData.assignedLossOfSelfConsciousness);
                    if (a == searchingForRoomPredictedStatsData.predictingStats.challengeSkillBalance && 
                        a == searchingForLevelData.assignedChallengeSkillBalance)
                    {
                        for (int b = 0; b < 6; b++)
                        {
                            if (b == searchingForRoomPredictedStatsData.predictingStats.clearGoals && 
                                b == searchingForLevelData.assignedClearGoals)
                            {
                                for (int c = 0; c < 6; c++)
                                {
                                    if (c == searchingForRoomPredictedStatsData.predictingStats.lossOfSelfConsciousness && 
                                        c == searchingForLevelData.assignedLossOfSelfConsciousness)
                                    {
                                        //this level is good
                                        levelToBeLoaded = l;
                                        roomManager.currentlyTransistioning = false;
                                        dataCollection.roomNumber++;
                                        Debug.Log("Loading Level " + levelToBeLoaded);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
       
    }
}
