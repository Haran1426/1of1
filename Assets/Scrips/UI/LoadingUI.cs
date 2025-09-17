using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [Header("UI ����")]
    [SerializeField] private TextMeshProUGUI loadingText; // �ۼ�Ʈ ǥ�ÿ�
    [SerializeField] private Image loadingBar;            // �ε��� �̹���

    [Header("����")]
    [SerializeField] private float duration = 1.5f;       // ��ü �ð� (��)

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

            // �ۼ�Ʈ (1% ~ 100%)
            int percent = Mathf.CeilToInt(Mathf.Lerp(1, 100, t));

            if (loadingText != null)
                loadingText.text = percent + "%";

            if (loadingBar != null)
                loadingBar.fillAmount = t;

            yield return null; // �� ������ ����
        }

        // ������ �� ����
        if (loadingText != null)
            loadingText.text = "100%";
        if (loadingBar != null)
            loadingBar.fillAmount = 1f;
    }
}
