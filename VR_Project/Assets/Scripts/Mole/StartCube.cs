using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StartCube : XRBaseInteractable
{
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.StartGame();
        }

        // ť�� ������� �����
        Destroy(gameObject);
    }
}
