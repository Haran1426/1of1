using UnityEngine;
using UnityEngine.UI;

public class BackgroundJitter : MonoBehaviour
{
    [Header("������ �ӵ�")]
    public float speed = 0.9f;
    [Header("������ ���� (�ȼ�)")]
    public float magnitude = 200f;      // (1) �ȼ� ������ ũ�� ��ƾ� UI���� ����

    // 11��° ��: RectTransform�� �ʱ� ��Ŀ�� ������ ������ ����
    private RectTransform rectTransform;
    private Vector2 initialAnchoredPos;

    void Start()
    {
        // 15��° ��: RectTransform ������Ʈ �������� �ʱ� ��Ŀ�� ������ ����
        rectTransform = GetComponent<RectTransform>();
        initialAnchoredPos = rectTransform.anchoredPosition;
    }

    void Update()
    {
        // 21-22��° ��: Perlin Noise�� �ε巯�� X/Y ������ ���
        float offsetX = (Mathf.PerlinNoise(Time.time * speed, 0f) - 0.5f) * magnitude;
        float offsetY = (Mathf.PerlinNoise(0f, Time.time * speed) - 0.5f) * magnitude;

        // 23��° ��: RectTransform.anchoredPosition�� ������ ����
        rectTransform.anchoredPosition = initialAnchoredPos + new Vector2(offsetX, offsetY);
    }
}
