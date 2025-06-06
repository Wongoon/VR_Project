using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tap : MonoBehaviour
{
    public GameObject button;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            gameObject.SetActive(false);
        }
    }
}
