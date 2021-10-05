using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    public GameObject player;
    private GameObject currentTileLocation;
    private int newXCoordinate;
    private int newYCoordinate;

    private bool playerPlaced = false;
    private Vector2 playerLocation;

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
        newXCoordinate = grid.gridWidth / 2;
        newYCoordinate = grid.gridDepth / 2;
        for (int i = 0; i < grid.gridSize; i++)
        {
            if (grid.tileData.storedCoordinates[i] == playerLocation)
            {
                Vector3 spawnLocation = grid.tileData.storedGameObjects[i].transform.position;
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
        inputManager();
    }

    private void inputManager()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            newYCoordinate++;
            updatePlayerCoordinates();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            newYCoordinate--;
            updatePlayerCoordinates();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            newXCoordinate--;
            updatePlayerCoordinates();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            newXCoordinate++;
            updatePlayerCoordinates();
        }
    }
    private void updatePlayerCoordinates()
    {
        verifyEligibility();
        Vector2 tempNewLocation = new Vector2(newXCoordinate, newYCoordinate);
        if (tempNewLocation != playerLocation)
        {
            playerLocation = tempNewLocation;
            moveCharacter();
        }
    }

    private void verifyEligibility()
    {
        if (newYCoordinate < 0)
        {
            newYCoordinate = 0;
        }
        if (newYCoordinate > (grid.gridDepth - 1))
        {
            newYCoordinate = grid.gridDepth - 1;
        }
        if (newXCoordinate < 0)
        {
            newXCoordinate = 0;
        }
        if (newXCoordinate > (grid.gridWidth - 1))
        {
            newXCoordinate = grid.gridWidth - 1;
        }
    }
    private void moveCharacter()
    {
        for (int i = 0; i < grid.gridSize; i++)
        {
            if (grid.tileData.storedCoordinates[i] == playerLocation)
            {
                Vector3 spawnLocation = grid.tileData.storedGameObjects[i].transform.position;
                player.transform.position = new Vector3(spawnLocation.x, spawnLocation.y + 5f, spawnLocation.z);
            }
        }
    }
}
