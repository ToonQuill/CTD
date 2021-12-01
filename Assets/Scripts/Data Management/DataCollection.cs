using System.IO;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class savedData
{
    public int roomsCleared;
    public int spacesMoved;
    public int deaths;
    public float seconds;
    public float minutes;
    public float hours;
}
[System.Serializable]
public class predictedStats
{
    public int challengeSkillBalance;
    public int actionAwarenessMerging;
    public int clearGoals;
    public int unambiguousFeedback;
    public int totalConcentration;
    public int lossOfSelfConsciousness;
    public int transformationOfTime;
}
[System.Serializable]
public class allTheStatistics
{
    public savedData savingData;
    public predictedStats predictingStats;
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

    public int challengeSkillBalance = 0;
    public int actionAwarenessMerging = 0;
    public int clearGoals = 0;
    public int unambiguousFeedback = 0;
    public int totalConcentration = 0;
    public int lossOfSelfConsciousness = 0;
    public int transformationOfTime = 0;

    public allTheStatistics totalAllTheStatistics = new allTheStatistics();
    public savedData totalSavedData = new savedData();
    public predictedStats currentPredictedStats = new predictedStats();

    public allTheStatistics roomAllTheStatistics = new allTheStatistics();
    public savedData roomSavedData = new savedData();
    public predictedStats roomPredictedStats = new predictedStats();

    private int totalSpacesMoved = 0;
    private int totalDeaths = 0;
    private int totalRoomsCleared = 0;
    public int roomSpacesMoved = 0;
    public int roomDeaths = 0;

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

        predictTheStats();

        totalAllTheStatistics = new allTheStatistics();

        savedDataLocation = dataPath + "Playthrough " + saveNumber + " Full.json";

        savedData totalSavedData = new savedData();
        totalSavedData.roomsCleared = totalRoomsCleared++;
        totalSavedData.spacesMoved = totalSpacesMoved;
        totalSavedData.deaths = totalDeaths;
        totalSavedData.hours = totalHours;
        totalSavedData.minutes = totalMinutes;
        totalSavedData.seconds = totalSeconds;
        totalAllTheStatistics.savingData = totalSavedData; //adds saved data to all the stats big 

        predictedStats totalPredictedStats = new predictedStats();
        totalPredictedStats.challengeSkillBalance = totalPredictedStats.challengeSkillBalance + challengeSkillBalance;
        totalPredictedStats.actionAwarenessMerging = totalPredictedStats.actionAwarenessMerging + actionAwarenessMerging;
        totalPredictedStats.clearGoals = totalPredictedStats.clearGoals + clearGoals;
        totalPredictedStats.unambiguousFeedback = totalPredictedStats.unambiguousFeedback + unambiguousFeedback;
        totalPredictedStats.totalConcentration = totalPredictedStats.totalConcentration + totalConcentration;
        totalPredictedStats.lossOfSelfConsciousness = totalPredictedStats.lossOfSelfConsciousness + lossOfSelfConsciousness;
        totalPredictedStats.transformationOfTime = totalPredictedStats.transformationOfTime + transformationOfTime;
        totalAllTheStatistics.predictingStats = totalPredictedStats;

        json = JsonUtility.ToJson(totalAllTheStatistics, true);
        File.WriteAllText(savedDataLocation, json);


        roomAllTheStatistics = new allTheStatistics();

        savedDataLocation = dataPath + "Playthrough " + saveNumber + " Room " + roomNumber + ".json";
        savedData roomSavedData = new savedData();
        roomSavedData.spacesMoved = roomSpacesMoved;
        roomSavedData.deaths = roomDeaths;
        roomSavedData.hours = roomHours;
        roomSavedData.minutes = roomMinutes;
        roomSavedData.seconds = roomSeconds;
        roomAllTheStatistics.savingData = roomSavedData; //adds saved data to all the stats big 

        predictedStats roomPredictedStats = new predictedStats();
        roomPredictedStats.challengeSkillBalance = challengeSkillBalance;
        roomPredictedStats.actionAwarenessMerging = actionAwarenessMerging;
        roomPredictedStats.clearGoals = clearGoals;
        roomPredictedStats.unambiguousFeedback = unambiguousFeedback;
        roomPredictedStats.totalConcentration = totalConcentration;
        roomPredictedStats.lossOfSelfConsciousness = lossOfSelfConsciousness;
        roomPredictedStats.transformationOfTime = transformationOfTime;
        roomAllTheStatistics.predictingStats = roomPredictedStats;

        json = JsonUtility.ToJson(roomAllTheStatistics, true);
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
    private void predictTheStats()
    {
        predictChallengeSkillBalance();
        predictClearGoals();
        predictLossOfSelfConsciousness();
    }
    private void predictChallengeSkillBalance()
    {
        challengeSkillBalance = roomSpacesMoved * roomDeaths;
        if (challengeSkillBalance > 400)
        {
            challengeSkillBalance = 1;
        }
        else if (challengeSkillBalance > 300 && challengeSkillBalance < 400)
        {
            challengeSkillBalance = 2;
        }
        else if(challengeSkillBalance > 200 && challengeSkillBalance < 300)
        {
            challengeSkillBalance = 3;
        }
        else if(challengeSkillBalance > 100 && challengeSkillBalance < 200)
        {
            challengeSkillBalance = 4;
        }
        else if(challengeSkillBalance < 100)
        {
            challengeSkillBalance = 5;
        }
    }
    private void predictClearGoals()
    {
        if (totalRoomsCleared > 1)
        {
            clearGoals = (totalSpacesMoved / totalRoomsCleared);
            if (roomSpacesMoved > clearGoals + (3 * totalRoomsCleared))
            {
                clearGoals = 1;
            }
            if (roomSpacesMoved > clearGoals + (2 * totalRoomsCleared) && roomSpacesMoved < clearGoals + (3 * totalRoomsCleared))
            {
                clearGoals = 2;
            }
            if (roomSpacesMoved > clearGoals + totalRoomsCleared && roomSpacesMoved < clearGoals + (2 * totalRoomsCleared))
            {
                clearGoals = 3;
            }
            if (roomSpacesMoved < clearGoals + totalRoomsCleared)
            {
                clearGoals = 4;
            }
            if (roomSpacesMoved < clearGoals)
            {
                clearGoals = 5;
            }
        }
    }
    private void predictLossOfSelfConsciousness()
    {
        decimal convertMinsToInt = (decimal)roomMinutes;
        decimal convertSecsToInt = (decimal)roomSeconds;
        convertMinsToInt = convertMinsToInt * 100;
        convertSecsToInt = (convertSecsToInt * 166) / 100;
        decimal totalTime = convertMinsToInt + convertSecsToInt;
        if (totalTime > 200)
        {
            lossOfSelfConsciousness = 1;
        }
        if (totalTime < 200 && totalTime > 150)
        {
            lossOfSelfConsciousness = 2;
        }
        if (totalTime < 150 && totalTime > 100)
        {
            lossOfSelfConsciousness = 3;
        }
        if (totalTime < 100 && totalTime > 50)
        {
            lossOfSelfConsciousness = 4;
        }
        if (totalTime < 50)
        {
            lossOfSelfConsciousness = 5;
        }
    }
}
