using UnityEngine;
using Unity.XR.CoreUtils;

[RequireComponent(typeof(CharacterController))]
public class VRKeyboardMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform cameraTransform; // XR Rig 안의 Main Camera (HMD)

    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(h, 0, v);
        if (inputDirection.sqrMagnitude < 0.01f)
            return;

        // 헤드셋 방향 기준 이동
        Vector3 headForward = cameraTransform.forward;
        Vector3 headRight = cameraTransform.right;
        headForward.y = 0;
        headRight.y = 0;

        Vector3 move = (headForward.normalized * v + headRight.normalized * h).normalized;

        // ✅ 카메라의 높이에 따라 XR Origin의 위치 보정 (충돌 감지용)
        Vector3 headPos = cameraTransform.position;
        Vector3 bodyPos = transform.position;

        Vector3 offset = new Vector3(headPos.x - bodyPos.x, 0, headPos.z - bodyPos.z);
        transform.position += offset;

        // ✅ 이동
        characterController.Move(move * moveSpeed * Time.deltaTime);
    }

}
