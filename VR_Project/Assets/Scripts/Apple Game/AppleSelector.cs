using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class AppleSelector : MonoBehaviour
{
    private AppleBlock firstSelected = null;
    private AppleBlock secondSelected = null;
    private float selectionDelay = 0.5f;
    private Coroutine fadeRoutine;

    [Header("시각화")]
    public RectTransform selectionVisual; // ✅ UI에서 드래그 사각형을 표시할 Image

    [Header("사운드")]
    public AudioClip failSound;
    [Range(0f, 1f)] public float failSoundVolume = 1f;

    public void TrySelect(AppleBlock apple)
    {
        if (firstSelected == null)
        {
            firstSelected = apple;
            apple.Highlight(true);
        }
        else if (secondSelected == null && apple != firstSelected)
        {
            secondSelected = apple;
            apple.Highlight(true);
            CheckRectangleSum();
        }
    }

    void CheckRectangleSum()
    {
        Vector3 posA = firstSelected.transform.position;
        Vector3 posB = secondSelected.transform.position;

        // ✅ 사각형 시각화 표시
        ShowSelectionBox(posA, posB);

        float minX = Mathf.Min(posA.x, posB.x);
        float maxX = Mathf.Max(posA.x, posB.x);
        float minZ = Mathf.Min(posA.z, posB.z);
        float maxZ = Mathf.Max(posA.z, posB.z);

        AppleBlock[] allApples = FindObjectsOfType<AppleBlock>();
        List<AppleBlock> applesInRect = new List<AppleBlock>();
        int sum = 0;

        foreach (var apple in allApples)
        {
            Vector3 pos = apple.transform.position;
            if (pos.x >= minX && pos.x <= maxX && pos.z >= minZ && pos.z <= maxZ)
            {
                applesInRect.Add(apple);
                sum += apple.value;
            }
        }

        if (sum == 10)
        {
            int totalScore = 0;
            foreach (var apple in applesInRect)
                totalScore += apple.isSpecial ? 2 : 1;

            AppleGameManager.Instance.AddScore(totalScore);

            foreach (var apple in applesInRect)
                apple.DelayedDestroy(selectionDelay);

            StartCoroutine(ResetSelectionAfterDelay(selectionDelay));
        }
        else
        {
            PlayFailSound();
            StartCoroutine(UnhighlightAndResetAfterDelay(selectionDelay, applesInRect));
        }
    }

    private void ShowSelectionBox(Vector3 worldPosA, Vector3 worldPosB)
    {
        if (selectionVisual == null) return;

        Vector3 min = Vector3.Min(worldPosA, worldPosB);
        Vector3 max = Vector3.Max(worldPosA, worldPosB);
        Vector3 center = (min + max) * 0.5f;

        float yOffset = 0.1f;
        center.y += yOffset;
        selectionVisual.position = center;

        float width = Mathf.Abs(max.x - min.x);
        float height = Mathf.Abs(max.z - min.z);

        float minWorldSize = 111.2f;
        if (width < minWorldSize) width = minWorldSize;
        if (height < minWorldSize) height = minWorldSize;

        float scaleFactor = 1f / selectionVisual.lossyScale.x;
        float sizeMultiplier = 1.5f;

        selectionVisual.sizeDelta = new Vector2(
            width * scaleFactor * sizeMultiplier,
            height * scaleFactor * sizeMultiplier
        );

        selectionVisual.rotation = Quaternion.Euler(90, 0, 0);
        // selectionVisual.gameObject.SetActive(true);

        // ✅ 페이드 인
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeSelectionBox(1f, 0.2f));
    }

    private IEnumerator FadeSelectionBox(float targetAlpha, float duration)
    {
        Image image = selectionVisual.GetComponent<Image>();
        if (image == null) yield break;

        Color color = image.color;
        float startAlpha = color.a;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            image.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        image.color = color;

        if (targetAlpha == 0f)
            selectionVisual.gameObject.SetActive(false);
    }

    private IEnumerator UnhighlightAndResetAfterDelay(float delay, List<AppleBlock> apples)
    {
        yield return new WaitForSeconds(delay);

        foreach (var apple in apples)
            apple.Highlight(false);

        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeSelectionBox(0f, 0.2f));

        firstSelected = null;
        secondSelected = null;
    }

    private IEnumerator ResetSelectionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeSelectionBox(0f, 0.2f));

        firstSelected = null;
        secondSelected = null;
    }
    private void PlayFailSound()
    {
        if (failSound == null) return;

        GameObject soundObj = new GameObject("FailSound");
        soundObj.transform.position = Camera.main.transform.position;

        AudioSource source = soundObj.AddComponent<AudioSource>();
        source.clip = failSound;
        source.volume = failSoundVolume;
        source.spatialBlend = 0f; // 2D 사운드 (UI처럼 들림)
        source.Play();

        Destroy(soundObj, failSound.length);
    }
}
