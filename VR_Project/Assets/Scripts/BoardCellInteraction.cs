using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class BoardCellInteraction : MonoBehaviour
{
    public GameObject blackStonePrefab;
    public GameObject whiteStonePrefab;
    
    private static bool isBlackTurn = false;
    private static Dictionary<Vector2Int, GameObject> placedStones = new();

    private Vector2Int coordinates;

    private static bool isGameOver = false;

    private static GameUIManager uiManager;

    private void Awake() {
        string rowName = transform.parent.name;
        string columnName = transform.name;

        int x = columnName[0] - 'A';
        int y = int.Parse(rowName) - 1;

        coordinates = new Vector2Int(x, y);
    }

    private void Start() {
        if (uiManager == null) {
            uiManager = FindObjectOfType<GameUIManager>();
        }
    }

    public void OnSelectEntered(SelectEnterEventArgs args) {
        Debug.Log($"Board Clicked : {coordinates}");
        if (isGameOver || placedStones.ContainsKey(coordinates)) {
            return;
        }

        GameObject prefab = isBlackTurn ? blackStonePrefab : whiteStonePrefab;
        Vector3 spawnPos = transform.position + Vector3.up * 1.5f;

        GameObject stone = Instantiate(prefab, spawnPos, Quaternion.identity);
        placedStones.Add(coordinates, stone);

        if (CheckWin(coordinates, isBlackTurn)) {
            Debug.Log(isBlackTurn ? "Black Win" : "White Win");
            isGameOver = true;
            uiManager?.ShowWinner(isBlackTurn);
            FindObjectOfType<BoardResetManager>()?.CameraCanvas.SetActive(true);
        }

        isBlackTurn = !isBlackTurn;
        uiManager?.UpdateTurnUI(isBlackTurn);
    }

    private bool CheckWin(Vector2Int current, bool black) {
        Vector2Int[] directions = new Vector2Int[] {
            Vector2Int.right, Vector2Int.up, new Vector2Int(1, 1), new Vector2Int(1, -1)
        };

        foreach (Vector2Int dir in directions) {
            int count = 1;
            count += CountInDirection(current, dir, black);
            count += CountInDirection(current, -dir, black);

            if (count >= 5)
                return true;
        }
        return false;

    }

    private int CountInDirection(Vector2Int start, Vector2Int dir, bool black) {
        int count = 0;
        Vector2Int pos = start + dir;

        while (placedStones.TryGetValue(pos, out GameObject stone)) {
            bool isBlackStone = stone.CompareTag("BlackStone");
            bool isWhiteStone = stone.CompareTag("WhiteStone");

            if ((black && isBlackStone) || (!black && isWhiteStone)) {
                count++;
                pos += dir;
            }
            else break;
        }
        return count;
    }
}
