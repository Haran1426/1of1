using UnityEngine;

public class Note_Grid : MonoBehaviour
{
    [Header("Grid Settings (런타임에서도 변경 가능)")]
    public float cellSize = 1f;  // 한 칸 크기
    public int width = 16;       // 가로 칸 수
    public int height = 3;       // 세로 칸 수

    private SpriteRenderer renderer;

    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        ApplyGridSettings();
    }

    /// <summary>
    /// 외부에서 곡 길이/패턴 난이도에 따라 Grid 크기 설정
    /// </summary>
    public void SetupGrid(int newWidth, int newHeight, float newCellSize)
    {
        width = newWidth;
        height = newHeight;
        cellSize = newCellSize;
        ApplyGridSettings();
    }

    private void ApplyGridSettings()
    {
        if (renderer != null)
        {
            renderer.size = new Vector2(width, height);
        }
    }

    // 월드 좌표 → Snap 좌표
    public Vector3 GetSnappedPosition(Vector3 worldPos)
    {
        float snappedX = Mathf.Round(worldPos.x / cellSize) * cellSize;
        float snappedY = Mathf.Round(worldPos.y / cellSize) * cellSize;
        return new Vector3(snappedX, snappedY, 0f);
    }
}
