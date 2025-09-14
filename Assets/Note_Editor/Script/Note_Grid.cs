using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Note_Grid : MonoBehaviour
{
    public float cellSize = 1f;   // 가로 칸 크기
    public int width = 16;        // 가로 칸 수

    private SpriteRenderer sr;

    // 세로는 위/중앙/아래 3줄 고정
    private float[] allowedY;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        UpdateGrid();

        // 세로 라인 위치 = 선 위치
        allowedY = new float[]
        {
            cellSize,   // 맨 위 선
            0f,         // 중앙 선
            -cellSize   // 맨 아래 선
        };
    }

    void OnValidate()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        UpdateGrid();
    }

    private void UpdateGrid()
    {
        if (sr == null) return;
        // 높이를 2칸으로 고정 (위, 중앙, 아래 선 표현)
        sr.size = new Vector2(width, 2f);
    }

    // 격자 범위 체크
    public bool IsInsideGrid(Vector3 pos)
    {
        float halfW = width * 0.5f * cellSize;
        return (pos.x >= -halfW && pos.x <= halfW);
    }

    public Vector3 GetSnappedPosition(Vector3 worldPos)
    {
        float snappedX = GetSnappedX(worldPos.x); // 가로: 반칸 단위
        float snappedY = GetSnappedY(worldPos.y); // 세로: 3줄 고정
        return new Vector3(snappedX, snappedY, 0f);
    }
    private float GetSnappedX(float x)
    {
        float halfCell = cellSize * 0.5f;
        return Mathf.Round(x / halfCell) * halfCell;
    }
    private float GetSnappedY(float y)
    {
        // allowedY = { +cellSize, 0, -cellSize }
        float snappedY = allowedY[0];
        float minDist = Mathf.Abs(y - allowedY[0]);

        foreach (float line in allowedY)
        {
            float dist = Mathf.Abs(y - line);
            if (dist < minDist)
            {
                minDist = dist;
                snappedY = line;
            }
        }
        return snappedY;
    }


}
