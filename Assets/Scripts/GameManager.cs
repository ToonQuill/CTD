using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool playTest = false;

    public MenuManager menuManager;
    void Update()
    {
        if (playTest)
        {
            this.GetComponent<CreateGrid>().enabled = true;
            this.GetComponent<CharacterMovement>().enabled = true;
            this.GetComponent<SaveLoadMaps>().enabled = true;
            this.GetComponent<RoomManager>().enabled = true;
            this.GetComponent<CameraMovement>().enabled = true;
            this.GetComponent<DataCollection>().enabled = true;
        }
        else
        {
            this.GetComponent<CreateGrid>().enabled = false;
            this.GetComponent<CharacterMovement>().enabled = false;
            this.GetComponent<SaveLoadMaps>().enabled = false;
            this.GetComponent<RoomManager>().enabled = false;
            this.GetComponent<CameraMovement>().enabled = false;
            this.GetComponent<DataCollection>().enabled = false;
        }

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
