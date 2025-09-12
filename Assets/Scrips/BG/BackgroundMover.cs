using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    public float speed = 300f;          // px/sec
    public RectTransform[] tiles;       // ȭ�鿡 ���̴� �������(�¡��)

    RectTransform rt0;
    float[] widths;                     // �� Ÿ�� ��
    float[] prefix;                     // ���� ������(prefix sum)
    float totalW;                       // ��ü ����(�� ����)
    float baseX;                        // ù Ÿ�� ���� X
    float offset;                       // ��ũ�� ������(0~totalW)

    void Start()
    {
        Canvas.ForceUpdateCanvases();

        // (1) ����/�� ���
        rt0 = tiles[0];
        baseX = rt0.anchoredPosition.x;

        int n = tiles.Length;
        widths = new float[n];
        prefix = new float[n];

        for (int i = 0; i < n; i++)
        {
            float w = tiles[i].rect.width;
            if (w <= 0.01f) w = Mathf.Abs(tiles[i].sizeDelta.x);
            widths[i] = w;
            prefix[i] = (i == 0) ? 0f : prefix[i - 1] + widths[i - 1];
        }

        totalW = 0f;
        for (int i = 0; i < n; i++) totalW += widths[i];

        // (2) �ʱⰡ�� ����(�¡��� ��Ȯ�� ���̱�)
        for (int i = 0; i < n; i++)
        {
            var p = tiles[i].anchoredPosition;
            p.x = baseX + prefix[i];
            tiles[i].anchoredPosition = p;
        }
    }

    void Update()
    {
        // (3) ��ⷯ ������
        offset = Mathf.Repeat(offset + speed * Time.deltaTime, totalW);

        // (4) �� ������ ���� ��ġ(�� ���� �ݿ��ϹǷ� ƴ/��ħ ����)
        for (int i = 0; i < tiles.Length; i++)
        {
            var p = tiles[i].anchoredPosition;
            p.x = baseX - offset + prefix[i];
            tiles[i].anchoredPosition = p;
        }
    }
}
