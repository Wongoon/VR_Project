using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    public TMP_Text turnText;

    private void Start() {
        turnText.text = "";
        UpdateTurnUI(false);
    }

    public void UpdateTurnUI(bool isBlackTurn) {
        turnText.text = isBlackTurn ? "Black\nTurn" : "White\nTurn";
    }

    public void ShowWinner(bool blackWon) {
        turnText.text = blackWon ? "Black\nWin" : "White\nWin";
    }

    public void ClearMessage()
    {
        turnText.text = "";
        UpdateTurnUI(false);
    }
}
