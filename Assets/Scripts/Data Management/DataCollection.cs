using System.IO;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
public class savedData
{
    public int spacesMoved;
    public int deaths;
    public float seconds;
    public float minutes;
    public float hours;
}
public class DataCollection : MonoBehaviour
{
    private string json;
    private string dataPath;
    public int saveNumber;
    public int roomNumber;
    private string savedDataLocation;
    private bool madeNewSaveData = false;

    public float totalHours = 0;
    public float totalMinutes = 0;
    public float totalSeconds = 0;
    public float roomHours = 0;
    public float roomMinutes = 0;
    public float roomSeconds = 0;

    public savedData totalSavedData = new savedData();
    public savedData roomSavedData = new savedData();

    private int totalSpacesMoved;
    private int totalDeaths;
    private int roomSpacesMoved;
    private int roomDeaths;

    public bool debugSave = false;

    void Start()
    {
        dataPath = System.IO.Directory.GetCurrentDirectory() + "/Assets/SavedData/";
        saveNumber = 1;
        roomNumber = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!madeNewSaveData)
        {
            MakeNewSavedData();
        }
        else
        {
            roomTimer();
        }

        if (debugSave)
        {
            SaveData(roomNumber);
            debugSave = false;
        }
    }
    void MakeNewSavedData()
    {
        savedDataLocation = dataPath + "Playthrough " + saveNumber + " Full.json";
        if (!File.Exists(savedDataLocation))
        {
            madeNewSaveData = true;
            File.WriteAllText(savedDataLocation, "This saved data exists");
        }
        else
        {
            saveNumber++;
        }
    }
    public void SaveData(int roomNumber)
    {
        addRoomStatsToTotal();

        savedDataLocation = dataPath + "Playthrough " + saveNumber + " Full.json";
        totalSavedData = new savedData();
        totalSavedData.deaths = totalDeaths;
        totalSavedData.spacesMoved = totalSpacesMoved;
        totalSavedData.hours = totalHours;
        totalSavedData.minutes = totalMinutes;
        totalSavedData.seconds = totalSeconds;
        json = JsonUtility.ToJson(totalSavedData, true);
        File.WriteAllText(savedDataLocation, json);

        savedDataLocation = dataPath + "Playthrough " + saveNumber + " Room " + roomNumber + ".json";
        roomSavedData = new savedData();
        roomSavedData.deaths = roomDeaths;
        roomSavedData.spacesMoved = roomSpacesMoved;
        roomSavedData.hours = roomHours;
        roomSavedData.minutes = roomMinutes;
        roomSavedData.seconds = roomSeconds;
        json = JsonUtility.ToJson(roomSavedData, true);
        File.WriteAllText(savedDataLocation, json);

        resetRoomStats();
    }
    private void roomTimer()
    {
        roomSeconds = roomSeconds + Time.deltaTime;
        if (roomSeconds > 60)
        {
            roomSeconds = 0;
            roomMinutes++;
        }
        if (roomMinutes > 60)
        {
            roomMinutes = 0;
            roomHours++;
        }
    }
    private void addRoomStatsToTotal()
    {
        totalDeaths = totalDeaths + roomDeaths;
        totalSpacesMoved = totalSpacesMoved + roomSpacesMoved;
        totalSeconds = totalSeconds + roomSeconds;
        totalMinutes = totalMinutes + roomMinutes;
        totalHours = totalHours = roomHours;
        if (totalSeconds > 60)
        {
            totalMinutes++;
            totalSeconds = totalSeconds - 60;
        }
        if (totalMinutes > 60)
        {
            totalHours++;
            totalMinutes = totalMinutes - 60;
        }
    }
    private void resetRoomStats()
    {
        roomDeaths = 0;
        roomSpacesMoved = 0;
        roomSeconds = 0;
        roomMinutes = 0;
        roomHours = 0;
    }
}
