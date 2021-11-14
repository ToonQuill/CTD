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
        //more data, like starting positions, enemy positions, enemy data, etc, can be saved here... but that's for later!
    }
    public class savedLevelData
    {
        public int gridDepth;
        public int gridWidth;
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

    private int gridDepth, gridWidth;

    private string inputtedLevel;

    public savedLevelData savedLevel = new savedLevelData();

    void Start()
    {
        //for builds
        //dataPath = Application.persistentDataPath + "/";
        //for editor

        //can add a folder here to distinguish playthroughs for testing
        dataPath = System.IO.Directory.GetCurrentDirectory() + "/Assets/SavedRooms/";

        tileDatabase = this.GetComponent<TileDatabase>();
        createdGrid = this.GetComponent<CreateGrid>();
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
                    iTM.warpCords = savedLevel.iTD[i].warpCords;
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
        foreach (Transform child in transform)
        {
            chosenMapData = new individualTileData();
            iTM = child.GetComponent<IndividualTileManager>();
            chosenMapData.tileID = iTM.tileData.tileName;
            chosenMapData.warpCords = iTM.warpCords;
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
}
