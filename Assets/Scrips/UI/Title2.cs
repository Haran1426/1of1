using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title2 : MonoBehaviour
{
    public static Title2 Instance;
    private Image fadeImage;          // ��Ÿ�ӿ� �����Ǵ� ���̵� �̹���
    private float fadeDuration = 1f;  // ���̵� �ð�
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
        // ���� �� �ڵ� ���̵� ��
        StartCoroutine(FadeIn());
    }

    private void CreateFadeCanvas()
    {
        // Canvas ����
        GameObject canvasObj = new GameObject("FadeCanvas");
        canvasObj.layer = LayerMask.NameToLayer("UI");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000; // �׻� ���� ���� ������

        // CanvasScaler, Raycaster �߰�
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        DontDestroyOnLoad(canvasObj);

        // Image ����
        GameObject imgObj = new GameObject("FadeImage");
        imgObj.transform.SetParent(canvasObj.transform, false);

        fadeImage = imgObj.AddComponent<Image>();
        fadeImage.color = new Color(0, 0, 0, 1); // �⺻ ���� ȭ��

        // ��ü ȭ�� ����
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

        // �� �ε�
        SceneManager.LoadScene(sceneName);

        yield return null;
        StartCoroutine(FadeIn());
    }

    // ��ư �̺�Ʈ
    public void LoadChoiceScene()
    {
        if (!isFading) StartCoroutine(FadeOut("Choice"));
    }

    public void LoadEditorScene()
    {
        if (!isFading) StartCoroutine(FadeOut("Editor"));
    }
}
