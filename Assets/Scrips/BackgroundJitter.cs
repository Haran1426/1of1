using UnityEngine;
using UnityEngine.UI;

public class BackgroundJitter : MonoBehaviour
{
    [Header("움직임 속도")]
    public float speed = 0.9f;
    [Header("움직임 범위 (픽셀)")]
    public float magnitude = 200f;      // (1) 픽셀 단위로 크게 잡아야 UI에서 보임

    // 11번째 줄: RectTransform과 초기 앵커드 포지션 변수로 수정
    private RectTransform rectTransform;
    private Vector2 initialAnchoredPos;

    void Start()
    {
        // 15번째 줄: RectTransform 컴포넌트 가져오고 초기 앵커드 포지션 저장
        rectTransform = GetComponent<RectTransform>();
        initialAnchoredPos = rectTransform.anchoredPosition;
    }

    void Update()
    {
        // 21-22번째 줄: Perlin Noise로 부드러운 X/Y 오프셋 계산
        float offsetX = (Mathf.PerlinNoise(Time.time * speed, 0f) - 0.5f) * magnitude;
        float offsetY = (Mathf.PerlinNoise(0f, Time.time * speed) - 0.5f) * magnitude;

        // 23번째 줄: RectTransform.anchoredPosition에 오프셋 적용
        rectTransform.anchoredPosition = initialAnchoredPos + new Vector2(offsetX, offsetY);
    }
}
