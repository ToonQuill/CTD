using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private CreateGrid grid;
    private CharacterMovement characterManager;
    public IndividualTileManager iTM;
    private TileDatabase tileDatabase;
    private SaveLoadMaps savingManager;
    private DataCollection dataCollection;
    public MenuManager menuManager;

    private int currentRoom = 1;
    public bool currentlyTransistioning = false;

    //0 = nothing, 1 = new grid, 2 = load existing
    public int newGridTime = 0;

    public string dataPath;
    void Start()
    {
        grid = this.GetComponent<CreateGrid>();
        characterManager = this.GetComponent<CharacterMovement>();
        savingManager = this.GetComponent<SaveLoadMaps>();
        tileDatabase = this.GetComponent<TileDatabase>();
        dataCollection = this.GetComponent<DataCollection>();
        dataPath = System.IO.Directory.GetCurrentDirectory() + "/Assets/SavedRooms/";
    }
    public void checkSpace()
    {
        for (int i = 0; i < grid.gridSize; i++)
        {
            if (grid.tileData.storedCoordinates[i] == characterManager.playerLocation)
            {
                iTM = grid.tileData.storedGameObjects[i].GetComponent<IndividualTileManager>();
                //all space/character intersections here
                if (!currentlyTransistioning) { checkForUnavailableSpace(); }
                if (!currentlyTransistioning) { checkForSwitchSpace(); }
                if (!currentlyTransistioning) { checkForSpecialSpace(); }
            }
        }
    }
    public void checkForTransition()
    {
        if (iTM.tileData.Transition == true)
        {
            currentlyTransistioning = true;
            //has a transition been set yet?
            //old stuff

            //time to make a new room!
            //...and use all the accumulated data, too!

            //first, make sure the transitioner knows what room to go from this point onwards
            //iTM.warpCords = new Vector2(currentRoom, currentRoom + 1);
            //next, save the current room
            //savingManager.SaveLevelData(currentRoom);

            //grid.resetGrid();
            //grid.initTiles();
            //currentRoom = currentRoom + 1;
            //savingManager.SaveLevelData(currentRoom);

            dataCollection.SaveData(currentRoom);
            if (currentRoom == 36 && dataCollection.challengeSkillBalance == 5 && dataCollection.clearGoals == 5 && dataCollection.lossOfSelfConsciousness == 5)
            {
                savingManager.LoadLevelData(0);
                menuManager.menus[1].SetActive(true);
            }
            else if (dataCollection.totalRoomsCleared < 15 || dataCollection.totalMinutes > 10)
            {
                savingManager.DecideWhatLevelToLoad();
                int chosenRoom = savingManager.levelToBeLoaded;
                savingManager.LoadLevelData(chosenRoom);
                currentRoom++;
            }
            else
            {
                //show ui saying they can quit game now with quit button App.Quit
                savingManager.LoadLevelData(0);
                menuManager.menus[1].SetActive(true);
            }
            return;
        }
    }
    private void checkForUnavailableSpace()
    {
        if (iTM.tileData.Void == true)
        {
            //characterManager.playerAlive = false;
            Debug.Log("Get Duuunnnkkkeddd On");
            characterManager.resetPlayerLocation();
            dataCollection.roomDeaths++;
        }
    }

    private void checkForSwitchSpace()
    {
        if (iTM.currentTileData.Switch == true)
        {
            iTM.switchTile.GetComponent<Renderer>().enabled = false;
            int switchNumber = iTM.switchNumber;
            for (int v = 0; v < grid.gridSize; v++)
            {
                iTM = grid.tileData.storedGameObjects[v].GetComponent<IndividualTileManager>();
                if (switchNumber == iTM.switchNumberEffect)
                {
                    iTM.tileData = tileDatabase.allTileTypes[iTM.switchTransformInto];
                }
            }
        }
    }
    private void checkForSpecialSpace()
    {
        if (iTM.currentTileData.Switch == true || iTM.currentTileData.descendingTile == true)
        {
            dataCollection.roomSpecialSpacesMoved++;
        }
    }
}
