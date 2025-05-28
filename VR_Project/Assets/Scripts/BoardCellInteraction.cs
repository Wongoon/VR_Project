using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BoardCellInteraction : MonoBehaviour
{
    public GameObject blackStonePrefab;
    public GameObject whiteStonePrefab;
    
    private static bool isBlackTurn = false;
    private static Dictionary<Vector2Int, GameObject> placedStones = new();

    private Vector2Int coordinates;

    private void Awake() {
        string rowName = transform.parent.name;
        string columnName = transform.name;

        int x = columnName[0] - 'A';
        int y = int.Parse(rowName) - 1;

        coordinates = new Vector2Int(x, y);
    }

    public void OnSelectEntered(SelectEnterEventArgs args) {
        if (placedStones.ContainsKey(coordinates)) {
            Debug.Log($"Board Clicked : {coordinates}");
            return;
        }

        GameObject prefab = isBlackTurn ? blackStonePrefab : whiteStonePrefab;
        Vector3 spawnPos = transform.position + Vector3.up * 1.5f;

        GameObject stone = Instantiate(prefab, spawnPos, Quaternion.identity);
        placedStones.Add(coordinates, stone);

        if (CheckWin(coordiantes, isBlackTurn)) {
            Debug.Log(isBlackTurn ? "Black Win" : "White Win");
        }

        isBlackTurn = !isBlackTurn;
    }

    private bool CheckWin(Vector2Int, current, bool black) {
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

            if ((black && isBlackstone) || (!black && isWhiteStone)) {
                count++;
                pos += dir;
            }
            else break;
        }
        return count;
    }
}
