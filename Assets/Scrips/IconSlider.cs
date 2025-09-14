using UnityEngine;
using DG.Tweening;

public class IconSlider : MonoBehaviour
{
    [Header("아이콘들 (왼쪽 → 오른쪽 순서)")]
    public RectTransform[] icons;

    [Header("이동 애니메이션 시간")]
    public float duration = 0.5f;

    private int currentIndex = 0;            // 현재 선택된 아이콘 인덱스
    private Vector2[] initialPositions;      // 아이콘 초기 위치 저장

    void Start()
    {
        // 초기 위치 저장
        initialPositions = new Vector2[icons.Length];
        for (int i = 0; i < icons.Length; i++)
        {
            initialPositions[i] = icons[i].anchoredPosition;
        }
    }

    // < 버튼 클릭
    public void OnClickLeft()
    {
        if (currentIndex <= 0) return;
        currentIndex--;
        SlideIcons();
    }

    // > 버튼 클릭
    public void OnClickRight()
    {
        if (currentIndex >= icons.Length - 1) return;
        currentIndex++;
        SlideIcons();
    }

    // 아이콘 이동 처리
    private void SlideIcons()
    {
        float distance = initialPositions[1].x - initialPositions[0].x; // 아이콘 간 간격

        for (int i = 0; i < icons.Length; i++)
        {
            Vector2 targetPos = initialPositions[i] - new Vector2(currentIndex * distance, 0);

            icons[i].DOKill(); // 이전 트윈 중단
            icons[i].DOAnchorPos(targetPos, duration).SetEase(Ease.OutQuad);
        }
    }

    // Start 버튼 클릭
    public void OnClickStart()
    {
        switch (currentIndex)
        {
            case 0:
                SceneFader.FadeToScene("Game");        // ✅ 아이콘 1 → Game 씬
                break;
            case 1:
                SceneFader.FadeToScene("Scene_Hood");  // ✅ 아이콘 2 → Scene_Hood 씬
                break;
            case 2:
                SceneFader.FadeToScene("Scene_Girl");  // ✅ 아이콘 3 → Scene_Girl 씬
                break;
            default:
                Debug.LogWarning("씬 매핑 안됨: " + currentIndex);
                break;
        }
    }
}
