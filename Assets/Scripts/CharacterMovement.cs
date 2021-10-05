using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    public GameObject player;
    private GameObject currentLocation, newLocation;
    private Vector3 newLocationVector;

    private bool playerPlaced = false;
    private bool playerIsMoving = false;

    public Vector2 playerLocation;
    public Vector2 playerLocationGoing;

    private float movementBuffer = 0f;

    private CreateGrid grid;
    private IndividualTileManager iTM;
    // Start is called before the first frame update
    void Start()
    {
        grid = this.GetComponent<CreateGrid>();
    }
    private void initPlayer()
    {
        player = Instantiate(player);
        playerLocation = new Vector2(grid.gridWidth / 2, grid.gridDepth / 2);
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
        if (Input.GetKey(KeyCode.UpArrow))
        {
            playerLocationGoing = new Vector2(playerLocationGoing.x, playerLocationGoing.y + 1);
            updatePlayerCoordinates();
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            playerLocationGoing = new Vector2(playerLocationGoing.x, playerLocationGoing.y - 1);
            updatePlayerCoordinates();
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerLocationGoing = new Vector2(playerLocationGoing.x - 1, playerLocationGoing.y);
            updatePlayerCoordinates();
        }
        if (Input.GetKey(KeyCode.RightArrow))
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
            }
        }
    }
    private void characterIsMoving()
    {
        player.transform.position = Vector3.MoveTowards(player.transform.position, newLocationVector, Time.deltaTime * 40f);
        float distance = Vector3.Distance(player.transform.position, newLocation.transform.position);
        if (distance < 0.1f)
        {
            playerIsMoving = false;
        }
    }
}
