using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile Type", menuName = "CTD/New Tile Type", order = 2)]
public class Tile : ScriptableObject
{
    [Header("Tile Information")]
    public string tileName;
    public Material tileMaterial;

    [Header("Tile Attributes")]
    public bool Passable = true;
    public bool Invisible = false;
    public bool Void = false;

    [Header("Tile Effects")]
    public bool Switch = false;
    public bool Transition = false;

    [Header("Descending Tiles")]
    public bool descendingTile = false;
    public int descendingNumber = 0;

}

