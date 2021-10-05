using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile Type", menuName = "NFE/New Tile Type", order = 2)]
public class Tile : ScriptableObject
{
    [Header("Tile Information")]
    public string tileName;
    public Material tileMaterial;
    public bool invisible = false;

    [Header("Movement Type Variables")]
    //movement type variables
    public bool infantryCanWalk = true;
    public bool armoredCanWalk = true;
    public bool cavalryCanWalk = true;
    public bool flyingCanWalk = true;

    [Header("Combat Modifiers")]
    //combat modifiers
    public int avoidModifier = 0;
    public int defModifier = 0;
    public int resModifier = 0;

    [Header("Other Modifiers")]
    //movement modifiers
    public int movementModifier = 0;
    public int healModifier = 0;
}

