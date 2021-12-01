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
    void Update()
    {

    }
    public void checkSpace()
    {
        for (int i = 0; i < grid.gridSize; i++)
        {
            if (grid.tileData.storedCoordinates[i] == characterManager.playerLocation)
            {
                iTM = grid.tileData.storedGameObjects[i].GetComponent<IndividualTileManager>();
                //all space/character intersections here
                checkForUnavailableSpace();
                checkForSwitchSpace();
                if (characterManager.playerAlive) { checkForTransition(); }
            }
        }
    }
    private void checkForTransition()
    {
        if (iTM.tileData.Transition == true)
        {
            currentlyTransistioning = true;
            //has a transition been set yet?
            if (iTM.warpCords == new Vector2(0, 0))
            {
                //time to make a new room!
                //...and use all the accumulated data, too!

                //first, make sure the transitioner knows what room to go from this point onwards
                iTM.warpCords = new Vector2(currentRoom, currentRoom + 1);
                //next, save the current room
                savingManager.SaveLevelData(currentRoom);

                grid.resetGrid();
                grid.initTiles();
                currentRoom = currentRoom + 1;
                //savingManager.SaveLevelData(currentRoom);
            }
            else
            {
                int chosenRoom = (int)iTM.warpCords.y;
                savingManager.LoadLevelData(chosenRoom);
                currentRoom = chosenRoom;
            }
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
}
