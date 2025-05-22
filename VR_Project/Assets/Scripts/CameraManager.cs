using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] float turnSpeed;
    private float xRotation;
    private float yRotation;

    public Transform cameraPosition;
    public float moveSpeed = 1.0f;
    public float rotationSpeed = 1.0f;

    void Update()
    {
        MouseRotation();
    }

    void MouseRotation()
    {
        xRotation = Input.GetAxisRaw("Mouse X") * turnSpeed * Time.fixedDeltaTime * Time.timeScale;
        yRotation = Input.GetAxisRaw("Mouse Y") * turnSpeed * Time.fixedDeltaTime * Time.timeScale;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    public void CameraRotation() {
        StartCoroutine(AdjustCamera());
    }

    IEnumerator AdjustCamera() {
        Quaternion secondRotation = Quaternion.Euler(0, 0, 0);

        yield return StartCoroutine(TargetRotation(cameraPosition, secondRotation));
    }

    IEnumerator TargetRotation(Transform obj, Quaternion targetRotation) {
        Quaternion startRotation = obj.rotation;

        float timeElapsed = 0;
        while (timeElapsed < 1f)
        {
            timeElapsed += Time.deltaTime * rotationSpeed;
            obj.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed);
            yield return null;
        }

        obj.localRotation = targetRotation;
    }
}
