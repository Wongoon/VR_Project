using TMPro;
using UnityEngine;

public class FloatingScore : MonoBehaviour
{
    public float floatUpSpeed = 1f;     // 위로 떠오르는 속도
    public float fadeOutTime = 1f;      // 페이드 시간

    private TextMeshProUGUI tmp;
    private Color originalColor;
    private float timer = 0f;

    void Start()
    {
        // TMP 컴포넌트 찾기 (명시적 캐스팅 사용)
        tmp = GetComponentInChildren<TextMeshProUGUI>();

        if (tmp == null)
        {
            return;
        }

        originalColor = tmp.color;
    }

    void Update()
    {
        if (tmp == null) return;

        // 위로 부드럽게 이동
        transform.position += Vector3.up * floatUpSpeed * Time.deltaTime;

        // 점점 투명하게 만들기
        timer += Time.deltaTime;
        float alpha = Mathf.Lerp(originalColor.a, 0f, timer / fadeOutTime);
        tmp.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

        // 완전히 사라지면 제거
        if (timer >= fadeOutTime)
        {
            Destroy(gameObject);
        }
    }

    public void SetText(string text)
    {
        if (tmp == null)
        {
            tmp = GetComponentInChildren<TMP_Text>() as TextMeshProUGUI;
        }

        if (tmp != null)
        {
            tmp.text = text;
        }
    }
}
