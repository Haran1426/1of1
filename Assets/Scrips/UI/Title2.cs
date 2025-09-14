using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title2 : MonoBehaviour
{
    public static Title2 Instance;
    private Image fadeImage;          // 런타임에 생성되는 페이드 이미지
    private float fadeDuration = 1f;  // 페이드 시간
    private bool isFading = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        CreateFadeCanvas();
    }

    private void Start()
    {
        // 시작 시 자동 페이드 인
        StartCoroutine(FadeIn());
    }

    private void CreateFadeCanvas()
    {
        // Canvas 생성
        GameObject canvasObj = new GameObject("FadeCanvas");
        canvasObj.layer = LayerMask.NameToLayer("UI");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000; // 항상 제일 위에 오도록

        // CanvasScaler, Raycaster 추가
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        DontDestroyOnLoad(canvasObj);

        // Image 생성
        GameObject imgObj = new GameObject("FadeImage");
        imgObj.transform.SetParent(canvasObj.transform, false);

        fadeImage = imgObj.AddComponent<Image>();
        fadeImage.color = new Color(0, 0, 0, 1); // 기본 검은 화면

        // 전체 화면 덮기
        RectTransform rect = fadeImage.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    public IEnumerator FadeIn()
    {
        isFading = true;
        float t = fadeDuration;
        Color c = fadeImage.color;

        while (t > 0f)
        {
            t -= Time.deltaTime;
            c.a = t / fadeDuration;
            fadeImage.color = c;
            yield return null;
        }

        c.a = 0f;
        fadeImage.color = c;
        isFading = false;
    }

    public IEnumerator FadeOut(string sceneName)
    {
        isFading = true;
        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = t / fadeDuration;
            fadeImage.color = c;
            yield return null;
        }

        c.a = 1f;
        fadeImage.color = c;

        // 씬 로드
        SceneManager.LoadScene(sceneName);

        yield return null;
        StartCoroutine(FadeIn());
    }

    // 버튼 이벤트
    public void LoadChoiceScene()
    {
        if (!isFading) StartCoroutine(FadeOut("Choice"));
    }

    public void LoadEditorScene()
    {
        if (!isFading) StartCoroutine(FadeOut("Editor"));
    }
}
