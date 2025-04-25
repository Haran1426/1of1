using System.Collections;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public static Score Instance;
    private int score = 0;

    [Header("점수 UI")]
    public TextMeshProUGUI scoreText;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("점수: " + score);
        UpdateScoreUI();
        StartCoroutine(AnimateScoreUI()); // 💥 점수 애니메이션 실행
    }

    public void OnHitCutLine()
    {
        AddScore(-Random.Range(100, 201));
        Debug.Log("깎임");
    }

    public void OnHitHitBox()
    {
        AddScore(Random.Range(300, 501));
        Debug.Log("상승");
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    // 💥 점수 텍스트 크기 애니메이션 효과
    private IEnumerator AnimateScoreUI()
    {
        Vector3 originalScale = scoreText.transform.localScale;
        Vector3 targetScale = originalScale * 1.05f;

        float time = 0f;
        while (time < 0.2f)
        {
            scoreText.transform.localScale = Vector3.Lerp(originalScale, targetScale, time / 0.15f);
            time += Time.deltaTime;
            yield return null;
        }

        time = 0f;
        while (time < 0.2f)
        {
            scoreText.transform.localScale = Vector3.Lerp(targetScale, originalScale, time / 0.2f);
            time += Time.deltaTime;
            yield return null;
        }

        scoreText.transform.localScale = originalScale;
    }
}
