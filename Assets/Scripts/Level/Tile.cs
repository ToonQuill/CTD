using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile Type", menuName = "NFE/New Tile Type", order = 2)]
public class Tile : ScriptableObject
{
    [Header("Tile Information")]
    public string tileName;
    public Material tileMaterial;

    [Header("Tile Attributes")]
    public bool Passable = true;
    public bool Invisible = false;
    public bool Switch = false;


}

