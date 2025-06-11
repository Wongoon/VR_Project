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

        // 큐브 사라지게 만들기
        Destroy(gameObject);
    }
}
