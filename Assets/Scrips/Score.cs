using System.Collections;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public static Score Instance;
    private int score = 0;

    [Header("점수 UI")]
    public TextMeshProUGUI scoreText;

    private Coroutine scoreEffectCoroutine;

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
        bool isPositive = amount > 0;
        score += amount;
        UpdateScoreUI();

        Debug.Log($"점수: {score}");

        // 점수 증가일 때만 애니메이션 실행 (중복 방지)
        if (isPositive)
        {
            if (scoreEffectCoroutine != null)
                StopCoroutine(scoreEffectCoroutine);

            scoreEffectCoroutine = StartCoroutine(AnimateScoreUI());
        }
    }

    public void OnHitCutLine()
    {
        AddScore(-Random.Range(70, 151));
        Debug.Log("❌ 점수 감소");
    }

    public void OnHitHitBox()
    {
        AddScore(Random.Range(200, 251));
        Debug.Log("✅ 점수 증가");
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    private IEnumerator AnimateScoreUI()
    {
        Vector3 originalScale = Vector3.one;
        Vector3 targetScale = originalScale * 1.03f;

        scoreText.rectTransform.localScale = originalScale; // 시작 시 스케일 리셋

        float time = 0f;
        float duration = 0.07f;

        while (time < duration)
        {
            scoreText.rectTransform.localScale = Vector3.Lerp(originalScale, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        scoreText.rectTransform.localScale = targetScale;

        time = 0f;
        while (time < duration)
        {
            scoreText.rectTransform.localScale = Vector3.Lerp(targetScale, originalScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        scoreText.rectTransform.localScale = originalScale;
    }



}
