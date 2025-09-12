using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    public float speed = 300f;          // px/sec
    public RectTransform[] tiles;       // 화면에 보이는 순서대로(좌→우)

    RectTransform rt0;
    float[] widths;                     // 각 타일 폭
    float[] prefix;                     // 누적 오프셋(prefix sum)
    float totalW;                       // 전체 길이(한 바퀴)
    float baseX;                        // 첫 타일 기준 X
    float offset;                       // 스크롤 오프셋(0~totalW)

    void Start()
    {
        Canvas.ForceUpdateCanvases();

        // (1) 기준/폭 계산
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

        // (2) 초기가딱 스냅(좌→우로 정확히 붙이기)
        for (int i = 0; i < n; i++)
        {
            var p = tiles[i].anchoredPosition;
            p.x = baseX + prefix[i];
            tiles[i].anchoredPosition = p;
        }
    }

    void Update()
    {
        // (3) 모듈러 오프셋
        offset = Mathf.Repeat(offset + speed * Time.deltaTime, totalW);

        // (4) 매 프레임 강제 배치(각 폭을 반영하므로 틈/겹침 없음)
        for (int i = 0; i < tiles.Length; i++)
        {
            var p = tiles[i].anchoredPosition;
            p.x = baseX - offset + prefix[i];
            tiles[i].anchoredPosition = p;
        }
    }
}
