using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [Header("UI 참조")]
    [SerializeField] private TextMeshProUGUI loadingText; // 퍼센트 표시용
    [SerializeField] private Image loadingBar;            // 로딩바 이미지

    [Header("설정")]
    [SerializeField] private float duration = 1.5f;       // 전체 시간 (초)

    private void OnEnable()
    {
        StartCoroutine(PlayLoading());
    }

    private IEnumerator PlayLoading()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // 퍼센트 (1% ~ 100%)
            int percent = Mathf.CeilToInt(Mathf.Lerp(1, 100, t));

            if (loadingText != null)
                loadingText.text = percent + "%";

            if (loadingBar != null)
                loadingBar.fillAmount = t;

            yield return null; // 매 프레임 갱신
        }

        // 마지막 값 보정
        if (loadingText != null)
            loadingText.text = "100%";
        if (loadingBar != null)
            loadingBar.fillAmount = 1f;
    }
}
