using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StoneSpawner : MonoBehaviour
{
    public GameObject stonePrefab;
    public bool isBlackBasket;

    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (BoardCellInteraction.IsGameOver || isBlackBasket != BoardCellInteraction.IsBlackTurn() || StoneHolder.heldStone != null) {
            return;
        }
        var interactorTransform = args.interactorObject.transform;
        Vector3 spawnPos = interactorTransform.position + interactorTransform.forward * 5f;

        GameObject newStone = Instantiate(stonePrefab, spawnPos, interactorTransform.rotation, interactorTransform);
        newStone.transform.SetParent(interactorTransform);

        StoneHolder.heldStone = newStone;

        Debug.Log($"{interactorTransform.name}");
    }
}
