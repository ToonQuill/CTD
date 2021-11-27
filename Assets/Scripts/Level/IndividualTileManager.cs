using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndividualTileManager : MonoBehaviour
{
    //data for the plot tiles
    public Tile tileData;
    [HideInInspector] public Tile currentTileData;

    private bool tileCanBeChanged = false;

    private GameObject defaultTile;
    private GameObject wallTile;
    [HideInInspector] public GameObject switchTile;

    CreateGrid createdGrid;

    [HideInInspector] public int xCord, zCord;

    [HideInInspector] public bool tileIsMoveable = false;
    [HideInInspector] public bool moveableTile = false;
    [HideInInspector] public bool isOccupied;

    public List<GameObject> neighbours = new List<GameObject>();
    public List<GameObject> tileColliders = new List<GameObject>();

    private bool foundNeighbours = false;

    [Header("Special Tile Effects")]
    //special tile properties
    public Vector2 warpCords = new Vector2(0, 0);
    public int descendingNumber = -1;

    //what switches and corresponding effects link together
    public int switchNumber = 0;
    public int switchNumberEffect = 0;
    public Tile switchTransformInto;
    //public string switchEffect;
    void Start()
    {
        neighbours = new List<GameObject>();
        tileColliders = new List<GameObject>();
        defaultTile = this.GetComponentInParent<CreateGrid>().defaultTile;
        wallTile = this.GetComponentInParent<CreateGrid>().wallTile;
        switchTile = this.GetComponentInParent<CreateGrid>().switchTile;
        currentTileData = this.GetComponentInParent<CreateGrid>().defaultTileData;
        isOccupied = false;
        for (int i = 0; i < this.transform.childCount; i++)
        {
            tileColliders.Add(this.transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!tileCanBeChanged)
        {
            if (tileData == null)
            {
                tileData = currentTileData;
                tileCanBeChanged = true;
            }
        }
        changeTile();
        if (!foundNeighbours)
        {
            FindNeighbours();
        }
    }
    private void changeTile()
    {
        if (currentTileData != tileData)
        {
            currentTileData = tileData;
            this.GetComponent<Renderer>().enabled = true;
            this.GetComponent<Renderer>().sharedMaterial = currentTileData.tileMaterial;
            if (currentTileData.descendingTile)
            {
                descendingNumber = currentTileData.descendingNumber;
            }
            if (currentTileData.Invisible || (currentTileData.descendingTile && descendingNumber == 0))
            {
                this.GetComponent<Renderer>().enabled = false;
            }
            else
            {
                this.GetComponent<Renderer>().enabled = true;
            }
            if (currentTileData.Wall)
            {
                if (!wallTile.scene.IsValid())
                {
                    wallTile = Instantiate(wallTile);
                    wallTile.transform.parent = transform;
                    wallTile.transform.position = transform.position;
                    wallTile.GetComponent<Renderer>().enabled = true;
                }
                wallTile.GetComponent<Renderer>().enabled = true;
            }
            else if (!currentTileData.Wall && wallTile.scene.IsValid())
            {
                wallTile.GetComponent<Renderer>().enabled = false;
            }
            if (currentTileData.Switch)
            {
                if (!switchTile.scene.IsValid())
                {
                    switchTile = Instantiate(switchTile);
                    switchTile.transform.parent = transform;
                    switchTile.transform.position = transform.position;
                    switchTile.GetComponent<Renderer>().enabled = true;
                }
                switchTile.GetComponent<Renderer>().enabled = true;
            }
            else if (!currentTileData.Switch && switchTile.scene.IsValid())
            {
                switchTile.GetComponent<Renderer>().enabled = false;
            }
        }
        tileCanBeChanged = false;
    }
    private void FindNeighbours()
    {
        for (int i = 0; i < tileColliders.Count; i++)
        {
            for (int j = 0; j < this.GetComponentInParent<CreateGrid>().tileData.storedGameObjects.Length; j++)
            {
                createdGrid = this.GetComponentInParent<CreateGrid>();
                if (tileColliders[i].GetComponent<BoxCollider>().bounds.Intersects
                (createdGrid.tileData.storedGameObjects[j].GetComponent<BoxCollider>().bounds)
                && createdGrid.tileData.storedGameObjects[j] != this.gameObject)
                {
                    neighbours.Add(createdGrid.tileData.storedGameObjects[j].gameObject);
                }
            }
        }
        foundNeighbours = true;
    }
}
