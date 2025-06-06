using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public TMP_Text scoreText;  // ✅ TMP로 변경

    private int score = 0;

    void Awake()
    {
        // 싱글톤 설정
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddScore(int value)
    {
        score += value;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "" + score;
    }

    public int GetScore()
    {
        return score;
    }
    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }
}
