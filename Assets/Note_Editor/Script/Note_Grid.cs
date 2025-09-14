using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Note_Grid : MonoBehaviour
{
    public float cellSize = 1f;
    [SerializeField, HideInInspector] private int width = 0;

    private SpriteRenderer sr;

    // 세로 고정 (위, 중앙, 아래)
    private float[] allowedY;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        UpdateGrid();

        allowedY = new float[]
        {
            cellSize,   // 위
            0f,         // 가운데
            -cellSize   // 아래
        };
    }

    private void UpdateGrid()
    {
        if (sr == null) return;
        sr.size = new Vector2(width, 2f);
    }

    public void SetWidth(int newWidth)
    {
        width = Mathf.Max(0, newWidth);
        UpdateGrid();
    }

    public int GetWidth() => width;

    // ✅ NotePlacer.cs에서 쓰는 함수들 다시 추가
    public bool IsInsideGrid(Vector3 pos)
    {
        float halfW = width * 0.5f * cellSize;
        return (pos.x >= -halfW && pos.x <= halfW);
    }

    public Vector3 GetSnappedPosition(Vector3 worldPos)
    {
        // X축: 반칸 단위로 스냅
        float snappedX = Mathf.Round(worldPos.x / (cellSize * 0.5f)) * (cellSize * 0.5f);

        // Y축: allowedY 중 가장 가까운 값 선택
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
