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
    public int roomID;
    public int roomsCleared;
    public int spacesMoved;
    public int specialSpacesMoved;
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
    private int totalSpecialSpacesMoved = 0;
    private int totalDeaths = 0;
    public int totalRoomsCleared = 0;
    public int roomSpacesMoved = 0;
    public int roomSpecialSpacesMoved = 0;
    public int roomDeaths = 0;

    public bool debugSave = false;

    decimal totalTime;

    public SaveLoadMaps saveManager;

    void Start()
    {
        dataPath = System.IO.Directory.GetCurrentDirectory() + "/Assets/SavedData/";
        saveManager = this.GetComponent<SaveLoadMaps>();
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
        roomSavedData.roomID = saveManager.selectedMap;
        roomSavedData.spacesMoved = roomSpacesMoved;
        roomSavedData.specialSpacesMoved = roomSpecialSpacesMoved;
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
        totalSpecialSpacesMoved = totalSpecialSpacesMoved + roomSpecialSpacesMoved;
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

        predictActionAwarenessMerging();
        predictUnambiguousFeedback();
        predictTotalConcentration();
        predictTransformationOfTime();
    }
    private void predictChallengeSkillBalance()
    {
        int decideChallengeSkillBalance = roomSpacesMoved * roomDeaths;
        if (challengeSkillBalance == 0)
        {
            challengeSkillBalance = 1;
        }
        if (decideChallengeSkillBalance > 90 && challengeSkillBalance >= 3)
        {
            challengeSkillBalance = challengeSkillBalance - 2;
        }
        else if (decideChallengeSkillBalance > 90 && challengeSkillBalance >= 2)
        {
            challengeSkillBalance--;
        }
        if (decideChallengeSkillBalance > 60 && decideChallengeSkillBalance < 90 && challengeSkillBalance >= 2)
        {
            challengeSkillBalance--;
        }
        //else if(challengeSkillBalance > 25 && challengeSkillBalance < 40)
        //{
        //    challengeSkillBalance = 3;
        //}
        if (decideChallengeSkillBalance > 20 && decideChallengeSkillBalance < 60 && challengeSkillBalance < 5)
        {
            challengeSkillBalance++;
        }
        if (decideChallengeSkillBalance < 20 && challengeSkillBalance < 4)
        {
            challengeSkillBalance = challengeSkillBalance + 2;
        }
        else if (decideChallengeSkillBalance < 20 && challengeSkillBalance == 4)
        {
            challengeSkillBalance++;
        }
    }
    private void predictClearGoals()
    {
        if (totalRoomsCleared > 1)
        {
            clearGoals = totalRoomsCleared;
            if (totalRoomsCleared < 5)
            {
                clearGoals = 1;
            }
            //if (roomSpacesMoved > clearGoals + (2 * totalRoomsCleared) && roomSpacesMoved < clearGoals + (3 * totalRoomsCleared))
            //{
            //    clearGoals = 2;
            //}
            if (totalRoomsCleared >= 5 && totalRoomsCleared < 10)
            {
                clearGoals = 3;
            }
            //if (roomSpacesMoved < clearGoals + totalRoomsCleared)
            //{
            //    clearGoals = 4;
            //}
            if (totalRoomsCleared >= 10)
            {
                clearGoals = 5;
            }
        }
        else
        {
            clearGoals = 1;
        }
    }
    private void predictLossOfSelfConsciousness()
    {
        decimal convertMinsToInt = (decimal)roomMinutes;
        decimal convertSecsToInt = (decimal)roomSeconds;
        convertMinsToInt = convertMinsToInt * 100;
        convertSecsToInt = (convertSecsToInt * 166) / 100;
        totalTime = convertMinsToInt + convertSecsToInt;
        if (lossOfSelfConsciousness == 0)
        {
            lossOfSelfConsciousness = 1;
        }
        if (totalTime > challengeSkillBalance * 10 && lossOfSelfConsciousness != 1)
        {
            lossOfSelfConsciousness = lossOfSelfConsciousness - 2;
        }
        //if (totalTime < 50 && totalTime > 40)
        //{
        //    lossOfSelfConsciousness = 2;
        //}
        //if (totalTime < 30 && totalTime > 15)
        //{
        //    lossOfSelfConsciousness = 3;
        //}
        //if (totalTime < 30 && totalTime > 20)
        //{
        //    lossOfSelfConsciousness = 4;
        //}
        if (totalTime < challengeSkillBalance * 7 && lossOfSelfConsciousness < 5)
        {
            lossOfSelfConsciousness = lossOfSelfConsciousness + 2;
        }
    }
    private void predictActionAwarenessMerging()
    {
        decimal actionAwarenessMergingPrediction = totalTime * roomDeaths;
        if (actionAwarenessMergingPrediction > 500)
        {
            actionAwarenessMerging = 1;
        }
        if (actionAwarenessMerging > 400 && actionAwarenessMerging < 500)
        {
            actionAwarenessMerging = 2;
        }
        if (actionAwarenessMerging > 300 && actionAwarenessMerging < 400)
        {
            actionAwarenessMerging = 3;
        }
        if (actionAwarenessMerging > 200 && actionAwarenessMerging < 300)
        {
            actionAwarenessMerging = 4;
        }
        if (actionAwarenessMerging < 200)
        {
            actionAwarenessMerging = 5;
        }
    }
    private void predictUnambiguousFeedback()
    {
        if (totalSpecialSpacesMoved < 100)
        {
            unambiguousFeedback = 1;
        }
        if (totalSpecialSpacesMoved > 100 && totalSpecialSpacesMoved < 200)
        {
            unambiguousFeedback = 2;
        }
        if (totalSpecialSpacesMoved > 200 && totalSpecialSpacesMoved < 300)
        {
            unambiguousFeedback = 3;
        }
        if (totalSpecialSpacesMoved > 300 && totalSpecialSpacesMoved < 400)
        {
            unambiguousFeedback = 4;
        }
        if (totalSpecialSpacesMoved > 400)
        {
            unambiguousFeedback = 5;
        }
    }
    private void predictTotalConcentration()
    {
        if (totalRoomsCleared < 5)
        {
            totalConcentration = 1;
        }
        if (totalRoomsCleared > 5 && totalRoomsCleared < 8)
        {
            totalConcentration = 2;
        }
        if (totalRoomsCleared > 8 && totalRoomsCleared < 11)
        {
            totalConcentration = 3;
        }
        if (totalRoomsCleared > 11 && totalRoomsCleared < 14)
        {
            totalConcentration = 4;
        }
        if (totalRoomsCleared > 14)
        {
            totalConcentration = 5;
        }
    }
    private void predictTransformationOfTime()
    {
        if (totalMinutes > 10)
        {
            transformationOfTime = 1;
        }
        if (totalMinutes > 8 && totalMinutes < 10)
        {
            transformationOfTime = 2;
        }
        if (totalMinutes > 6 && totalMinutes < 8)
        {
            transformationOfTime = 3;
        }
        if (totalMinutes > 4 && totalMinutes < 6)
        {
            transformationOfTime = 4;
        }
        if (totalMinutes < 4)
        {
            transformationOfTime = 5;
        }
    }
}
