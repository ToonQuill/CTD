using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool playTest = false;
    public int turnCount = 0;
    public bool newTurn = false;

    public MenuManager menuManager;
    void Update()
    {
        //if (playTest)
        //{
        //    this.GetComponentInChildren<GridMovement>().enabled = true;
        //    this.GetComponentInChildren<CharacterInstantiation>().enabled = true;
        //    this.GetComponentInChildren<CharacterSelection>().enabled = true;
        //    this.GetComponentInChildren<CharacterPathfinding>().enabled = true;
        //    this.GetComponentInChildren<CharacterMovement>().enabled = true;
        //}
        //else if (!playTest && this.GetComponentInChildren<GridMovement>().enabled == true)
        //{
        //    this.GetComponentInChildren<GridMovement>().enabled = false;
        //    this.GetComponentInChildren<CharacterInstantiation>().resetCharacters();
        //    this.GetComponentInChildren<CharacterInstantiation>().enabled = false;
        //    this.GetComponentInChildren<CharacterSelection>().resetCharacterSelection();
        //    this.GetComponentInChildren<CharacterSelection>().enabled = false;
        //    this.GetComponentInChildren<CharacterPathfinding>().enabled = false;
        //    this.GetComponentInChildren<CharacterMovement>().enabled = false;
        //}

        //if (newTurn)
        //{
        //    turnCount++;
        //    this.GetComponentInChildren<CharacterDatabase>().charactersActed = 0;
        //    for (int i = 0; i < this.GetComponentInChildren<CharacterDatabase>().characterData.procListCha.Length; i++)
        //    {
        //        this.GetComponentInChildren<CharacterDatabase>().characterData.characterHasMoved[i] = false;
        //    }
        //    newTurn = false;
        //    menuManager.turnCountDisplay.text = "Turn Count: " + turnCount.ToString();
        //}
    }
}
