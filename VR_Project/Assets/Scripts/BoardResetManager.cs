using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardResetManager : MonoBehaviour
{
    public GameObject boardRoot;

    public GameObject CameraCanvas;

    public void ResetBoard() {
        var field = typeof(BoardCellInteraction).GetField("placedStones", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var dict = (Dictionary<Vector2Int, GameObject>)field.GetValue(null);

        foreach (var stone in dict.Values) {
            if (stone != null) {
                Destroy(stone);
            }
        }

        dict.Clear();

        var turnField = typeof(BoardCellInteraction).GetField("isBlackTurn", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        turnField.SetValue(null, false);

        Debug.Log("Board has been reset.");
        FindObjectOfType<GameUIManager>()?.ClearMessage();
    }
}