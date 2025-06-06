using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AppleClickSelector : MonoBehaviour
{
    public Camera mainCamera;

    [Header("XR 컨트롤러 입력")]
    public XRRayInteractor leftInteractor;
    public XRBaseController leftController;
    public XRRayInteractor rightInteractor;
    public XRBaseController rightController;

    private AppleSelector selector;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        selector = FindObjectOfType<AppleSelector>();
    }

    void Update()
    {
        HandleMouseClick();
        HandleXRTrigger(leftInteractor, leftController);
        HandleXRTrigger(rightInteractor, rightController);
    }

    private void HandleMouseClick()
    {
        /*if (Input.GetMouseButtonDown(0)) 주석 지우고 마우스 클릭 켜기
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                AppleBlock apple = hit.collider.GetComponentInParent<AppleBlock>();
                if (apple != null)
                    selector?.TrySelect(apple);
            }
        }*/
    }

    private void HandleXRTrigger(XRRayInteractor interactor, XRBaseController controller)
    {
        if (interactor == null || controller == null)
            return;

        if (controller.activateInteractionState.activatedThisFrame)
        {
            if (interactor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                AppleBlock apple = hit.collider.GetComponentInParent<AppleBlock>();
                if (apple != null)
                {
                    selector?.TrySelect(apple);
                }
            }
        }
    }
}
