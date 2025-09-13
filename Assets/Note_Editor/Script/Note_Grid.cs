using UnityEngine;

public class Note_Grid : MonoBehaviour
{
    public float CellSize = 1f; // 한 칸 크기
    public int Width = 16;      // 가로 칸 수 (Inspector에서 조절 가능)

    private const int FixedHeight = 3; // 세로는 항상 3줄
    private SpriteRenderer renderer;

    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        UpdateGrid();
    }

    // ✅ Inspector 값 바꿀 때 자동 반영
    void OnValidate()
    {
        if (renderer == null) renderer = GetComponent<SpriteRenderer>();
        UpdateGrid();
    }

    private void UpdateGrid()
    {
        if (renderer == null) return;
        // 세로는 고정 3줄
        renderer.size = new Vector2(Width, FixedHeight);
    }

    // ✅ 격자 범위 안인지 체크
    public bool IsInsideGrid(Vector3 pos)
    {
        float halfW = Width * 0.5f * CellSize;
        float halfH = FixedHeight * 0.5f * CellSize;

        return (pos.x >= -halfW && pos.x <= halfW &&
                pos.y >= -halfH && pos.y <= halfH);
    }
}
