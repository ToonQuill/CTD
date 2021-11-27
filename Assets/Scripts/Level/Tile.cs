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
    public bool RespawnPoint = true;
    public bool Void = false;
    public bool Wall = false;

    [Header("Switch Effects")]
    public bool Switch = false;
    public bool SwitchEffect = false;

    [Header("Descending Effects")]
    public bool descendingTile = false;
    public int descendingNumber = 0;

    [Header("Misc Effects")]
    public bool Transition = false;
}

