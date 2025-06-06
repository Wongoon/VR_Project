using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FruitSpawner : MonoBehaviour
{
    public GameObject fruitPrefab; // 생성할 과일 프리팹
    public int gridWidth = 17;
    public int gridHeight = 10;
    public float spacing = 1.1f; // 블록 간 간격
    public Vector3 startPosition = new Vector3(0, 0.5f, 0); // 바닥에서 살짝 띄워서 생성

    [Header("공통 연결 오브젝트")]
    public GameObject floatingScoreTextPrefab;
    public Canvas worldSpaceCanvas;

    [Header("BGM 설정")]
    public AudioSource bgmSource;
    public AudioClip bgmClip;

    [Header("타이머 설정")]
    public float timeLimit = 30f; // 제한 시간 (초)
    public TextMeshProUGUI timerText;        // UI 텍스트 (선택 사항)
    private float currentTime;
    private bool isTimerRunning = false;

    public Button startButton; // ✅ 시작 버튼 연결

    private bool hasSpawned = false;

    // ✅ Start에서 제거하고 외부에서 호출할 수 있게 변경
    public void SpawnFruits()
    {
        // 1. 시작 버튼 숨김
        startButton.gameObject.SetActive(false);

        // 2. 기존 사과 삭제 (재시작 대비)
        AppleBlock[] oldApples = FindObjectsOfType<AppleBlock>();
        foreach (var apple in oldApples)
        {
            Destroy(apple.gameObject);
        }

        // 3. 사과 새로 생성
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                Vector3 spawnPos = startPosition + new Vector3(x * spacing, 0, z * spacing);
                GameObject fruit = Instantiate(fruitPrefab, spawnPos, Quaternion.identity, transform);

                AppleBlock block = fruit.GetComponent<AppleBlock>();
                if (block != null)
                {
                    block.floatingScoreTextPrefab = floatingScoreTextPrefab;
                    block.worldSpaceCanvas = worldSpaceCanvas;
                }
                else
                {
                    Debug.LogWarning("🍎 생성된 과일에 AppleBlock 컴포넌트가 없습니다!");
                }
            }
        }

        // 4. BGM 재생
        if (bgmSource != null && bgmClip != null)
        {
            bgmSource.clip = bgmClip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning("🎵 BGM 설정이 누락되었습니다!");
        }

        // 5. 타이머 초기화 및 시작
        currentTime = timeLimit;
        isTimerRunning = true;

        // ✅ 이전 상태에서 다시 시작 가능하게 하기 위해 제거
        // hasSpawned = true; // ❌ 필요 없음
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetScore();
        }
    }

    void Update()
    {
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime;

            if (timerText != null)
                timerText.text = Mathf.CeilToInt(currentTime).ToString();

            if (currentTime <= 0)
            {
                isTimerRunning = false;
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        Debug.Log("⏰ 게임 종료!");

        // 1. BGM 정지
        if (bgmSource != null)
            bgmSource.Stop();

        // 2. 모든 사과 제거
        AppleBlock[] allApples = FindObjectsOfType<AppleBlock>();
        foreach (var apple in allApples)
        {
            Destroy(apple.gameObject);
        }
        startButton.gameObject.SetActive(true);
        // 3. 추가 게임 종료 처리 (선택)
        // 예: UI 활성화, 점수 표시, 재시작 버튼 등
    }


}