using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    private CreateGrid grid;
    private IndividualTileManager iTM;
    //private CharacterMovement chaMov;
    public GameObject assignedCamera;

    //set start position
    //temp, can later be manually selected
    [HideInInspector] public int chosenXStart = 0;
    [HideInInspector] public int chosenZStart = 0;

    [HideInInspector] public int newX = 0;
    [HideInInspector] public int newZ = 0;

    [HideInInspector] public Vector2 currentGridCords;

    [HideInInspector] public bool movedSpace = false;

    [HideInInspector] public int bufferAmount;

    public bool freezeMovement = false;
    private int buffer = 10;
    void Awake()
    {
        newX = chosenXStart;
        newZ = chosenZStart;
        currentGridCords = new Vector2(newX, newZ);
    }
    void Start()
    {
        grid = this.GetComponent<CreateGrid>();
        //chaMov = this.GetComponentInParent<ScriptReferences>().chaMov;
    }
    void Update()
    {
        inputManager();
        updateCoordinates();
        moveCamera();
    }

    private void inputManager()
    {
        if (buffer == 0 && !freezeMovement)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                newZ++;
                updateCordsTracking();
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                newZ--;
                updateCordsTracking();
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                newX--;
                updateCordsTracking();
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                newX++;
                updateCordsTracking();
            }
        }
        buffer--;
        if (buffer < 0)
        {
            buffer = 0;
        }
    }
    private void updateCoordinates()
    {
        if (newZ < 0)
        {
            newZ = 0;
        }

        if (newZ > (grid.gridDepth - 1))
        {
            newZ = grid.gridDepth - 1;
        }

        if (newX < 0)
        {
            newX = 0;
        }

        if (newX > (grid.gridWidth - 1))
        {
            newX = grid.gridWidth - 1;
        }
    }
    private void updateCordsTracking()
    {
        buffer = bufferAmount;
        currentGridCords = new Vector2(newX, newZ);
        movedSpace = true;
    }

    private void moveCamera()
    {
        for (int i = 0; i < grid.tileData.storedGameObjects.Length; i++)
        {
            iTM = grid.tileData.storedGameObjects[i].GetComponent<IndividualTileManager>();
            if (iTM.xCord == newX & iTM.zCord == newZ)
            {
                Vector3 newLocation = new Vector3(iTM.transform.position.x, iTM.transform.position.y, iTM.transform.position.z);
                assignedCamera.transform.position = Vector3.Lerp(assignedCamera.transform.position, 
                    new Vector3(newLocation.x, newLocation.y + 50f, newLocation.z - 15f), Time.deltaTime * 5f);
                grid.selectedInitialTile.transform.position = new Vector3(newLocation.x, newLocation.y + 
                    (grid.gridDistance / 2), newLocation.z);
            }
        }
    }
}
