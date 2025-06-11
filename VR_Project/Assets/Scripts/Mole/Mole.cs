using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Mole : XRGrabInteractable
{
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.AddScore();
        }

        Destroy(gameObject);
    }
}
