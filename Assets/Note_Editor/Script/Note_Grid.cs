using UnityEngine;

public class Note_Grid : MonoBehaviour
{
    [Header("Grid Settings (��Ÿ�ӿ����� ���� ����)")]
    public float cellSize = 1f;  // �� ĭ ũ��
    public int width = 16;       // ���� ĭ ��
    public int height = 3;       // ���� ĭ ��

    private SpriteRenderer renderer;

    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        ApplyGridSettings();
    }

    /// <summary>
    /// �ܺο��� �� ����/���� ���̵��� ���� Grid ũ�� ����
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

    // ���� ��ǥ �� Snap ��ǥ
    public Vector3 GetSnappedPosition(Vector3 worldPos)
    {
        float snappedX = Mathf.Round(worldPos.x / cellSize) * cellSize;
        float snappedY = Mathf.Round(worldPos.y / cellSize) * cellSize;
        return new Vector3(snappedX, snappedY, 0f);
    }
}
