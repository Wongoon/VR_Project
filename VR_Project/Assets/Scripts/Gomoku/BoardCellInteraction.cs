using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using System;

public class BoardCellInteraction : MonoBehaviour
{
    public GameObject blackStonePrefab;
    public GameObject whiteStonePrefab;
    
    private static bool isBlackTurn = false;
    private static Dictionary<Vector2Int, GameObject> placedStones = new();

    private Vector2Int coordinates;

    private static bool isGameOver = false;

    private static GameUIManager uiManager;
    public static bool IsGameOver => isGameOver;

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
        if (isGameOver || placedStones.ContainsKey(coordinates))
        {
            return;
        }

        if (StoneHolder.heldStone == null) {
            return;
        }

        GameObject held = StoneHolder.heldStone;
        Vector3 spawnPos = transform.position + Vector3.up * 1.5f;

        GameObject stone = Instantiate(held, spawnPos, Quaternion.identity);
        stone.tag = held.tag;
        placedStones.Add(coordinates, stone);

        Destroy(StoneHolder.heldStone);
        StoneHolder.heldStone = null;

        if (CheckWin(coordinates, isBlackTurn))
        {
            isGameOver = true;
            uiManager?.ShowWinner(isBlackTurn);
            FindObjectOfType<BoardResetManager>()?.CameraCanvas.SetActive(true);
            return;
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

    public static void ResetBoardState() {
        foreach (var stone in placedStones.Values) {
            if (stone != null) {
                GameObject.Destroy(stone);
            }
        }

        placedStones.Clear();
        isBlackTurn = false;
        isGameOver = false;
    }

    public static bool IsBlackTurn()
    {
        return isBlackTurn;
    }
}
