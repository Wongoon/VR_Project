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
            Debug.Log("Return");
            return;
        }

        GameObject prefab = isBlackTurn ? blackStonePrefab : whiteStonePrefab;
        Vector3 spawnPos = transform.position + Vector3.up * 1.5f;

        GameObject stone = Instantiate(prefab, spawnPos, Quaternion.identity);
        placedStones.Add(coordinates, stone);

        isBlackTurn = !isBlackTurn;
    }
}