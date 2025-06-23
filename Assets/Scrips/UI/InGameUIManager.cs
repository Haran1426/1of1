using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    private Slider HPbar;
    private float maxHP = 100;
    private float curHP = 100;
    float imsi;

    void Start()
    {
        imsi = curHP / maxHP;
        HPbar.value = imsi;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (curHP > 0)
            {
                curHP -= 10;
                if (curHP < 0) curHP = 0;
            }

            imsi = curHP / maxHP; 
        }

        HandleHP();
    }

    private void HandleHP()
    {
        HPbar.value = Mathf.Lerp(HPbar.value, imsi, Time.deltaTime * 10);
    }
}



public class Pausemenu : MonoBehaviour
{
    public GameObject pausePannel;
    public GameObject Target;
    public void Menu_Btn()
    {

        Time.timeScale = 0f;


        pausePannel.SetActive(true);
    }

    public void Continue()
    {

        Time.timeScale = 1f;
        pausePannel.SetActive(false);
        Target.gameObject.SetActive(true);
    }
}


public class Score : MonoBehaviour
{
    public static Score Instance;
    private int score;

    [Header("점수 UI")]
    public TextMeshProUGUI scoreText;

    private Coroutine scoreEffectCoroutine;

    void Awake()
    {
        score = 0;
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
        Debug.Log("점수 감소");
    }
 


    public void OnSkill()
    {
        AddScore(Random.Range(100, 151));
        Debug.Log("스킬로 점수 증가");
    }
    public void OnHitHitBox()
    {
        AddScore(Random.Range(200, 251));
        Debug.Log("점수 증가");
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
