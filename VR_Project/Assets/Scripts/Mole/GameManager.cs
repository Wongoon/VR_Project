using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    public float timeLeft = 60f;
    public int score = 0;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    public GameObject molePrefab;
    public List<Transform> spawnPoints;
    public TextMeshProUGUI gameOverText;
    public GameObject stopButton;
    public GameObject startButton;

    private bool isGameActive = false;

    void Start()
    {
        gameOverText.gameObject.SetActive(false);
        stopButton.SetActive(false);
        timeLeft = 60f;
        timeText.text = "Time\n60S";
        scoreText.text = "Score\n0P";
    }

    void Update()
    {
        if (!isGameActive) return;

        timeLeft -= Time.deltaTime;
        timeText.text = "Time\n" + Mathf.CeilToInt(timeLeft) + "S";
        scoreText.text = "Score\n" + score + "P";

        if (timeLeft <= 0f)
        {
            isGameActive = false;
            StopAllCoroutines();
            timeLeft = 0f;
            timeText.text = "Time\n0S";
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = "GAME OVER";
            startButton.SetActive(true);
            stopButton.SetActive(false);
        }
    }

    public void StartGame()
    {
        score = 0;
        timeLeft = 60f;
        isGameActive = true;
        startButton.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        stopButton.SetActive(true);
        Mole[] moles = FindObjectsOfType<Mole>();
        foreach (Mole mole in moles)
        {
            Destroy(mole.gameObject);
        }
        StartCoroutine(SpawnMoles());
    }

    public void StopGame()
    {
        isGameActive = false;
        startButton.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        stopButton.SetActive(false);
        StopAllCoroutines();
        gameOverText.text = "Stop";
        Mole[] moles = FindObjectsOfType<Mole>();
        foreach (Mole mole in moles)
        {
            Destroy(mole.gameObject);
        }
    }

    public void AddScore()
    {
        if (!isGameActive) return;
        score++;
    }

    IEnumerator SpawnMoles()
    {
        while (isGameActive)
        {
            int idx = Random.Range(0, spawnPoints.Count);
            Transform spawnPos = spawnPoints[idx];

            Vector3 molePosition = spawnPos.position + new Vector3(0f, 1f, 0f);
            GameObject mole = Instantiate(molePrefab, molePosition, Quaternion.identity);

            yield return new WaitForSeconds(1.0f);
            if (mole != null)
            {
                Destroy(mole);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}