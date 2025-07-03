using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 반드시 한 파일엔 public class 하나만!
public class GameUI : MonoBehaviour
{
    [SerializeField] private Slider HPbar;
    private float maxHP = 100;
    private float curHP = 100;
    private float imsi;

    public GameObject perfectImage;
    public GameObject hitImage;
    public GameObject missImage;

    void Start()
    {
        imsi = curHP / maxHP;
        HPbar.value = imsi;

        if (perfectImage) perfectImage.SetActive(false);
        if (hitImage) hitImage.SetActive(false);
        if (missImage) missImage.SetActive(false);
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

    public void ShowJudgement(string type)
    {
        StartCoroutine(JudgementCoroutine(type));
    }

    private IEnumerator JudgementCoroutine(string type)
    {
        GameObject target = null;
        if (type == "Perfect") target = perfectImage;
        else if (type == "Hit") target = hitImage;
        else if (type == "Miss") target = missImage;

        if (target != null)
        {
            target.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            target.SetActive(false);
        }
    }
}

// 아래는 public 제거, 그냥 class로!
class Pausemenu : MonoBehaviour
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

class Score : MonoBehaviour
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

    public void OnPerfectHit()
    {
        AddScore(Random.Range(300, 401));
        Debug.Log("Perfect로 점수 증가");
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

        scoreText.rectTransform.localScale = originalScale;

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
