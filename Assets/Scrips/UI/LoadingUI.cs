using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingTitle;
    [SerializeField] private TextMeshProUGUI loadingPercent;
    [SerializeField] private Image loadingBar;
    [SerializeField] private float duration = 1.5f;

    private Coroutine barRoutine;
    private Coroutine dotRoutine;

    private void OnEnable()
    {
        InitializeUI();

        barRoutine = StartCoroutine(PlayLoading());
        dotRoutine = StartCoroutine(PlayDots());
    }

    private void OnDisable()
    {
        if (barRoutine != null)
        {
            StopCoroutine(barRoutine);
            barRoutine = null;
        }

        if (dotRoutine != null)
        {
            StopCoroutine(dotRoutine);
            dotRoutine = null;
        }
    }

    private void InitializeUI()
    {
        if (loadingBar != null)
            loadingBar.fillAmount = 0f;

        if (loadingPercent != null)
            loadingPercent.text = "1%";

        if (loadingTitle != null)
            loadingTitle.text = "Loading";
    }

    private IEnumerator PlayDots()
    {
        string baseText = "Loading";
        int dot = 0;

        while (true)
        {
            dot = (dot + 1) % 4;

            if (loadingTitle != null)
                loadingTitle.text = baseText + new string('.', dot);

            yield return new WaitForSeconds(0.3f);
        }
    }

    private IEnumerator PlayLoading()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            int percent = Mathf.CeilToInt(Mathf.Lerp(1, 100, t));

            if (loadingPercent != null)
                loadingPercent.text = percent + "%";

            if (loadingBar != null)
                loadingBar.fillAmount = t;

            yield return null;
        }

        if (loadingPercent != null)
            loadingPercent.text = "100%";

        if (loadingBar != null)
            loadingBar.fillAmount = 1f;
    }
}
