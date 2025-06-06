using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AppleInteractable : XRBaseInteractable
{
    private AppleBlock appleBlock;
    private AppleSelector selector;

    protected override void Awake()
    {
        base.Awake();
        appleBlock = GetComponent<AppleBlock>();
        selector = FindObjectOfType<AppleSelector>();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if (selector != null && appleBlock != null)
        {
            selector.TrySelect(appleBlock);
        }
    }
}
