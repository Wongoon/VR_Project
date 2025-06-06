using TMPro;
using UnityEngine;
using System.Collections;

public class AppleBlock : MonoBehaviour
{
    public int value;
    public TextMeshPro textMesh;
    public Renderer appleRenderer;
    public GameObject destroyEffectPrefab;

    [Header("점수 텍스트")]
    public GameObject floatingScoreTextPrefab; // ✅ TMP 프리팹 연결
    public Canvas worldSpaceCanvas; // ✅ 월드 캔버스 연결

    [Header("외곽선 설정")]
    public GameObject outlineObject;

    [Header("특수 사과 설정")]
    public bool isSpecial = false;                      // ✅ 특별 사과 여부
    public Color specialColor = Color.yellow;           // ✅ 강조 색상
    public int specialBonusMultiplier = 2;              // ✅ 점수 배수

    [Header("사운드 설정")]
    public AudioClip destroySound;
    [Range(0f, 1f)] public float destroySoundVolume = 1f;

    void Start()
    {
        value = GetWeightedRandomValue();

        // ✅ 특별 사과 여부 결정
        if (Random.value < 0.1f)
        {
            isSpecial = true;

            // 색상 변경
            if (appleRenderer != null)
                appleRenderer.material.color = specialColor;

            if (textMesh != null)
            {
                textMesh.color = specialColor;
                textMesh.text = value.ToString(); // 값 그대로 표시
            }
        }
        else
        {
            if (textMesh != null)
                textMesh.text = value.ToString();
        }

    }

    public void Highlight(bool on)
    {
        if (outlineObject != null)
            outlineObject.SetActive(on);
    }

    public void DelayedDestroy(float delay)
    {
        StartCoroutine(DelayedDestroyCoroutine(delay));
    }

    private IEnumerator DelayedDestroyCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        PlayDestroySound();


        // ✅ 이펙트 생성
        if (destroyEffectPrefab != null)
        {
            Vector3 effectPosition = transform.position + new Vector3(0, 1f, 0);
            Quaternion effectRotation = Quaternion.Euler(-90, 0, 0);

            GameObject effect = Instantiate(destroyEffectPrefab, effectPosition, effectRotation);
            var ps = effect.GetComponent<ParticleSystem>();
            if (ps != null) ps.Play();
        }

        // ✅ 점수 텍스트 생성
        if (floatingScoreTextPrefab != null && worldSpaceCanvas != null)
        {
            // 1. 사과보다 위쪽 월드 좌표
            Vector3 worldAbove = transform.position + new Vector3(0, 2f, 0);

            // 2. 프리팹을 월드 공간 캔버스 자식으로 생성
            GameObject scoreObj = Instantiate(floatingScoreTextPrefab, worldSpaceCanvas.transform);
            scoreObj.transform.position = worldAbove;

            // 3. 텍스트 설정
            FloatingScore floating = scoreObj.GetComponent<FloatingScore>();
            if (floating != null)
            {
                string scoreText = isSpecial ? "+2" : "+1";
                floating.SetText(scoreText);
            }
            else
            {
                Debug.LogError("❌ FloatingScore 스크립트를 찾지 못했습니다!");
            }
        }
        else
        {
            Debug.LogError("❌ 프리팹 또는 Canvas가 비어 있음!");
        }

        // ✅ 디버깅 출력
        Debug.Log($"[DEBUG] floatingScoreTextPrefab: {floatingScoreTextPrefab}");
        Debug.Log($"[DEBUG] worldSpaceCanvas: {worldSpaceCanvas}");

        // ✅ 사과 제거
        Destroy(gameObject);
    }


    int GetWeightedRandomValue()
    {
        int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        int[] weights = { 25, 20, 15, 10, 8, 7, 6, 5, 4 };
        int totalWeight = 0;
        foreach (int w in weights) totalWeight += w;

        int rand = Random.Range(0, totalWeight);
        int cumulative = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            cumulative += weights[i];
            if (rand < cumulative)
                return numbers[i];
        }

        return 1;
    }
    private void PlayDestroySound()
    {
        if (destroySound == null) return;

        GameObject soundObj = new GameObject("DestroySound");
        soundObj.transform.position = transform.position;

        AudioSource source = soundObj.AddComponent<AudioSource>();
        source.clip = destroySound;
        source.volume = destroySoundVolume;
        source.spatialBlend = 1f;
        source.minDistance = 1f;
        source.maxDistance = 20f;

        source.Play();
        Destroy(soundObj, destroySound.length);
    }
}

