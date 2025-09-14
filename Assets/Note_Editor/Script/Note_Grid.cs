using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Note_Grid : MonoBehaviour
{
    public float cellSize = 1f;   // 칸 크기
    private int width = 0;        // ✅ Inspector에 노출 안 함 (UI 전용)

    private SpriteRenderer sr;

    // 세로는 3줄 고정 (위/중앙/아래)
    private float[] allowedY;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        UpdateGrid();

        allowedY = new float[]
        {
            cellSize,   // 위
            0f,         // 중앙
            -cellSize   // 아래
        };
    }

    private void UpdateGrid()
    {
        if (sr == null) return;
        sr.size = new Vector2(width, 2f); // 세로는 2 고정
    }

    // ✅ UI에서만 호출
    public void SetWidth(int newWidth)
    {
        width = Mathf.Max(0, newWidth);
        UpdateGrid();
    }

    public bool IsInsideGrid(Vector3 pos)
    {
        float halfW = width * 0.5f * cellSize;
        return (pos.x >= -halfW && pos.x <= halfW);
    }

    public Vector3 GetSnappedPosition(Vector3 worldPos)
    {
        float snappedX = Mathf.Round(worldPos.x / (cellSize * 0.5f)) * (cellSize * 0.5f);

        float snappedY = allowedY[0];
        float minDist = Mathf.Abs(worldPos.y - allowedY[0]);

        foreach (float y in allowedY)
        {
            float dist = Mathf.Abs(worldPos.y - y);
            if (dist < minDist)
            {
                minDist = dist;
                snappedY = y;
            }
        }

        return new Vector3(snappedX, snappedY, 0f);
    }
}
