using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class RestartCube : XRBaseInteractable
{
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // 씬 재시작
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // 큐브 제거
        Destroy(gameObject);
    }
}
