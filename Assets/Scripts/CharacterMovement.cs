using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    public GameObject player;
    public GameObject currentLocation, newLocation;
    public Vector3 newLocationVector;

    public bool playerAlive = true;
    public bool playerPlaced = false;
    public bool playerIsMoving = false;
    public GameObject playerRespawnPoint;

    public Vector2 playerLocation;
    public Vector2 playerLocationGoing;

    private float movementBuffer = 0f;

    private CreateGrid grid;
    private IndividualTileManager iTM;
    private RoomManager roomManager;
    private TileDatabase tileDatabase;
    private CameraMovement cameraManager;
    private DataCollection dataCollection;
    // Start is called before the first frame update
    void Start()
    {
        grid = this.GetComponent<CreateGrid>();
        roomManager = this.GetComponent<RoomManager>();
        tileDatabase = this.GetComponent<TileDatabase>();
        cameraManager = this.GetComponent<CameraMovement>();
        dataCollection = this.GetComponent<DataCollection>();
    }
    public void initPlayer()
    {
        if (!player.scene.IsValid()) { player = Instantiate(player); }
        if (playerLocation == null) { playerLocation = new Vector2(grid.gridWidth / 2, grid.gridDepth / 2); }
        playerLocationGoing = playerLocation;
        for (int i = 0; i < grid.gridSize; i++)
        {
            if (grid.tileData.storedCoordinates[i] == playerLocation)
            {
                Vector3 spawnLocation = grid.tileData.storedGameObjects[i].transform.position;
                currentLocation = grid.tileData.storedGameObjects[i];
                player.transform.position = new Vector3(spawnLocation.x, spawnLocation.y + 5f, spawnLocation.z);
            }
        }
        newLocationVector = new Vector3(player.transform.position.x, player.transform.position.y,
        player.transform.position.z - 10f);
        playerPlaced = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerPlaced && grid.instantiatedTiles)
        {
            initPlayer();
        }

        if (movementBuffer > 0.25)
        {
            inputManager();
        }
        else
        {
            movementBuffer = movementBuffer + Time.deltaTime;
        }

        if (playerIsMoving)
        {
            characterIsMoving();
        }
    }

    private void inputManager()
    {
        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && playerAlive)
        {
            playerLocationGoing = new Vector2(playerLocationGoing.x, playerLocationGoing.y + 1);
            updatePlayerCoordinates();
        }
        if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && playerAlive)
        {
            playerLocationGoing = new Vector2(playerLocationGoing.x, playerLocationGoing.y - 1);
            updatePlayerCoordinates();
        }
        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && playerAlive)
        {
            playerLocationGoing = new Vector2(playerLocationGoing.x - 1, playerLocationGoing.y);
            updatePlayerCoordinates();
        }
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && playerAlive)
        {
            playerLocationGoing = new Vector2(playerLocationGoing.x + 1, playerLocationGoing.y);
            updatePlayerCoordinates();
        }
    }
    private void updatePlayerCoordinates()
    {
        verifyEligibility();
        movementBuffer = 0;
        if (playerLocationGoing != playerLocation)
        {
            moveCharacter(playerLocationGoing);
        }
    }

    private void verifyEligibility()
    {
        if (playerLocationGoing.y < 0)
        {
            playerLocationGoing = new Vector2(playerLocationGoing.x, 0);
        }
        if (playerLocationGoing.y > (grid.gridDepth - 1))
        {
            playerLocationGoing = new Vector2(playerLocationGoing.x, grid.gridDepth - 1);
        }
        if (playerLocationGoing.x < 0)
        {
            playerLocationGoing = new Vector2(0, playerLocationGoing.y);
        }
        if (playerLocationGoing.x > (grid.gridWidth - 1))
        {
            playerLocationGoing = new Vector2(grid.gridWidth - 1, playerLocationGoing.y);
        }
        for (int i = 0; i < grid.gridSize; i++)
        {
            if (grid.tileData.storedCoordinates[i] == playerLocationGoing
                && !grid.tileData.storedGameObjects[i].GetComponent<IndividualTileManager>().tileData.Passable)
            {
                playerLocationGoing = playerLocation;
            }
        }
    }
    private void moveCharacter(Vector2 playerLocationGoing)
    {
        for (int i = 0; i < grid.gridSize; i++)
        {
            if (grid.tileData.storedCoordinates[i] == playerLocationGoing)
            {
                newLocation = grid.tileData.storedGameObjects[i];
                newLocationVector = new Vector3(newLocation.transform.position.x, newLocation.transform.position.y + 5f,
                    newLocation.transform.position.z);
                playerIsMoving = true;
                playerLocation = playerLocationGoing;
                if (grid.tileData.storedGameObjects[i].GetComponent<IndividualTileManager>().tileData.RespawnPoint)
                {
                    playerRespawnPoint = grid.tileData.storedGameObjects[i];
                }
                cameraManager.movingCamera();
            }
        }
    }
    private void characterIsMoving()
    {
        player.transform.position = Vector3.MoveTowards(player.transform.position, newLocationVector, Time.deltaTime * 50f);
        if (player.transform.position == newLocationVector)
        {
            roomManager.checkSpace();
            if (playerAlive && !roomManager.currentlyTransistioning) { characterMovementInfluencingTiles(); }
            playerIsMoving = false;
            roomManager.checkSpace();
            roomManager.currentlyTransistioning = false;
            dataCollection.roomSpacesMoved++;
            roomManager.checkForTransition();
        }
    }
    private void characterMovementInfluencingTiles()
    {
        //for any tiles influenced by movement
        for (int i = 0; i < grid.gridSize; i++)
        {
            iTM = grid.tileData.storedGameObjects[i].GetComponent<IndividualTileManager>();
            if (iTM.currentTileData.descendingTile)
            {
                iTM.descendingNumber--;
                if (iTM.descendingNumber < 0)
                {
                    iTM.descendingNumber = 5;
                }
                iTM.tileData = tileDatabase.descendingTiles[iTM.descendingNumber];
            }
        }
    }
    public void resetPlayerLocation()
    {
        for (int i = 0; i < grid.gridSize; i++)
        {
            if (grid.tileData.storedGameObjects[i] == playerRespawnPoint)
            {
                playerLocation = grid.tileData.storedCoordinates[i];
                playerLocationGoing = grid.tileData.storedCoordinates[i];
                newLocation = grid.tileData.storedGameObjects[i];
                newLocationVector = new Vector3(newLocation.transform.position.x, newLocation.transform.position.y + 5f,
                newLocation.transform.position.z);
                player.transform.position = newLocationVector;
                cameraManager.movingCamera();
            }
        }
    }
}
