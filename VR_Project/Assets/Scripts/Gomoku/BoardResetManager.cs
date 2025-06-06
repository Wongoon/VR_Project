using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardResetManager : MonoBehaviour
{
    public GameObject boardRoot;

    public GameObject CameraCanvas;

    public void ResetBoard() {
        BoardCellInteraction.ResetBoardState();
        
        Destroy(StoneHolder.heldStone);
        StoneHolder.heldStone = null;

        FindObjectOfType<GameUIManager>()?.ClearMessage();
    }
}