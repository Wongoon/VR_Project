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
        HandleXRTrigger(leftInteractor, leftController);
        HandleXRTrigger(rightInteractor, rightController);
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
