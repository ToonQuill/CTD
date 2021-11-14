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

    CreateGrid createdGrid;

    [HideInInspector] public int xCord, zCord;

    [HideInInspector] public bool tileIsMoveable = false;
    public bool tileIsArrow = false;
    [HideInInspector] public bool moveableTile = false;
    [HideInInspector] public bool isOccupied;

    public List<GameObject> neighbours = new List<GameObject>();
    public List<GameObject> tileColliders = new List<GameObject>();

    private bool foundNeighbours = false;

    public Vector2 warpCords = new Vector2(0, 0);

    void Start()
    {
        neighbours = new List<GameObject>();
        tileColliders = new List<GameObject>();
        defaultTile = this.GetComponentInParent<CreateGrid>().defaultTile;
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
            if (currentTileData.Invisible)
            {
                this.GetComponent<Renderer>().enabled = false;
            }
            else
            {
                this.GetComponent<Renderer>().enabled = true;
            }
            tileCanBeChanged = false;
        }
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
