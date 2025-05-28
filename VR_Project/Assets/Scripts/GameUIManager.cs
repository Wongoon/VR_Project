using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    public TMP_Text turnText;
    public TMP_Text winText;

    private void Start() {
        UpdateTurnUI(false);
        winText.text = "";
    }

    public void UpdateTurnUI(bool isBlackTurn) {
        turnText.text = isBlackTurn ? "Black's Turn" : "White's Turn";
    }

    public void ShowWinner(bool blackWon) {
        winText.text = blackWon ? "Black Win" : "White Win";
    }

    public void ClearMessage() {
        winText.text = "";
    }
}
