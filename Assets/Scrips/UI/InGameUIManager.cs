using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager Instance { get; private set; }

    [Header("판정 이미지")]
    [SerializeField] private GameObject perfectImage;
    [SerializeField] private GameObject hitImage;
    [SerializeField] private GameObject missImage;
    [SerializeField] private float judgementDisplayTime = 0.5f;

    [Header("판정 이펙트 Prefab")]
    [SerializeField] private GameObject perfectEffectPrefab;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private GameObject missEffectPrefab;

    [Header("판정별 점수 범위")]
    [SerializeField] private Vector2 perfectScoreRange = new Vector2(300, 400);
    [SerializeField] private Vector2 hitScoreRange = new Vector2(200, 250);
    [SerializeField] private Vector2 missScoreRange = new Vector2(-150, -70);

    [Header("스킬 점수 범위")]
    [SerializeField] private Vector2 skillScoreRange = new Vector2(100, 150);

    [Header("Score UI")]
    [SerializeField] private TextMeshProUGUI scoreText;

    private int currentScore = 0;

    public enum JudgementType { Perfect, Hit, Miss }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
        UpdateScoreText();
    }

    /// <summary>
    /// R/G/B 입력 시 호출: 이미지·이펙트·점수를 한 번에 처리.
    /// </summary>
    public void HandleJudgement(JudgementType type, Vector3 worldPos)
    {
        // 1) 이미지 표시
        GameObject img = null;
        switch (type)
        {
            case JudgementType.Perfect: img = perfectImage; break;
            case JudgementType.Hit: img = hitImage; break;
            case JudgementType.Miss: img = missImage; break;
        }
        if (img != null) StartCoroutine(ShowImage(img));

        // 2) 이펙트 스폰
        GameObject fxPrefab = null;
        switch (type)
        {
            case JudgementType.Perfect: fxPrefab = perfectEffectPrefab; break;
            case JudgementType.Hit: fxPrefab = hitEffectPrefab; break;
            case JudgementType.Miss: fxPrefab = missEffectPrefab; break;
        }
        if (fxPrefab != null)
        {
            var fx = Instantiate(fxPrefab, worldPos, Quaternion.identity);
            Destroy(fx, 1f);
        }

        // 3) 점수 반영
        switch (type)
        {
            case JudgementType.Perfect:
                ModifyScore(Random.Range((int)perfectScoreRange.x, (int)perfectScoreRange.y));
                break;

            case JudgementType.Hit:
                ModifyScore(Random.Range((int)hitScoreRange.x, (int)hitScoreRange.y));
                break;

            case JudgementType.Miss:
                ModifyScore(Random.Range((int)missScoreRange.x, (int)missScoreRange.y));
                break;
        }
    }

    /// <summary> 스킬로 맞췄을 때: 점수만 추가 (이미지 없음) </summary>
    public void OnSkillScoreOnly()
    {
        ModifyScore(Random.Range((int)skillScoreRange.x, (int)skillScoreRange.y));
    }

    /// <summary> 판정선 통과 시: 점수만 차감 (이미지 없음) </summary>
    public void OnMissScoreOnly()
    {
        ModifyScore(Random.Range((int)missScoreRange.x, (int)missScoreRange.y));
    }

    private void ModifyScore(int delta)
    {
        currentScore += delta;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText == null)
        {
            Debug.LogError("[UIManager] scoreText가 할당되지 않았습니다!");
            return;
        }
        scoreText.text = currentScore.ToString();
    }

    private IEnumerator ShowImage(GameObject img)
    {
        img.SetActive(true);
        yield return new WaitForSeconds(judgementDisplayTime);
        img.SetActive(false);
    }
}
