using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CreateGrid : MonoBehaviour
{
    //data from prefab
    [Header("Assignable GameObjects")]
    public GameObject defaultTile;
    public GameObject wallTile;
    public GameObject switchTile;

    [Header("Grid Configuration")]
    //grid data
    [Range(5, 30)]
    public int gridWidth = 0;
    [Range(5, 30)]
    public int gridDepth = 0;
    [HideInInspector] public int gridSize = 0;

    [HideInInspector] public float gridDistance= 5f;

    //for init the creation of the grid
    private int cycle = 0;
    [HideInInspector] public bool instantiatedTiles = false;

    //getting info from individual tiles and other scripts
    [HideInInspector] public GameObject selectedInitialTile;
    [HideInInspector] public Tile defaultTileData;
    private IndividualTileManager iTM;
    //private GridMovement gm;

    //for storing positions
    [HideInInspector] public tileList tileData = new tileList();
    [System.Serializable]
    public class tileList
    {
        public Vector3[] tilePosition;
        public Vector2[] storedCoordinates;
        public GameObject[] storedGameObjects;
    }
    void Start()
    {
        //gm = this.GetComponent<GridMovement>();
        initTiles();
    }
    public void initTiles()
    {
        gridSize = gridWidth * gridDepth;
        tileData = new tileList();
        tileData.tilePosition = new Vector3[gridSize];
        tileData.storedCoordinates = new Vector2[gridSize];
        tileData.storedGameObjects = new GameObject[gridSize];
        //create grid
        GridClass grid = new GridClass(gridWidth, gridDepth, gridDistance);
        for (int x = 0; x < grid.gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < grid.gridArray.GetLength(1); z++)
            {
                createEmptyTiles(x, z);
            }
        }
        instantiatedTiles = true;
    }

    private void createEmptyTiles(int x, int z)
    {
        GameObject createdTile = Instantiate(defaultTile);
        createdTile.name = x + "," + z;
        createdTile.transform.parent = transform;
        createdTile.transform.position = new Vector3(x * gridDistance, 0f, z * gridDistance);
        createdTile.GetComponent<Renderer>().enabled = true;
        //createdTile.GetComponent<Renderer>().sharedMaterial = defaultTileData.tileMaterial;

        tileData.tilePosition[cycle] = createdTile.transform.position;

        tileData.storedGameObjects[cycle] = createdTile.gameObject;

        iTM = createdTile.GetComponent<IndividualTileManager>();
        iTM.xCord = x;
        iTM.zCord = z;

        tileData.storedCoordinates[cycle] = new Vector2(x, z);

        cycle++;
    }
    public void resetGrid()
    {
        for (int i = 0; i < tileData.storedGameObjects.Length; i++)
        {
            Destroy(tileData.storedGameObjects[i]);
        }
        Destroy(selectedInitialTile);
        instantiatedTiles = false;
        cycle = 0;
    }
}
