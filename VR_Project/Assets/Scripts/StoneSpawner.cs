using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StoneSpawner : MonoBehaviour
{
    public GameObject stonePrefab;

    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        var interactorTransform = args.interactorObject.transform;

        GameObject newStone = Instantiate(stonePrefab, Vector3.forward * 5, Quaternion.identity);

        newStone.transform.SetParent(interactorTransform);

        Debug.Log($"{interactorTransform.name}");
    }
}
