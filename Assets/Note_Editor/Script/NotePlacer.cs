using UnityEngine;

public class NotePlacer : MonoBehaviour
{
    [SerializeField] private Note_Grid grid;               // 격자 참조
    [SerializeField] private Object_Manager objectManager; // 현재 선택된 노트 상태 가져오기

    void Update()
    {
        // 좌클릭 시 동작
        if (Input.GetMouseButtonDown(0))
        {
            // 선택된 프리팹이 없으면 리턴
            GameObject prefab = objectManager.GetSelectedPrefab();
            if (prefab == null) return;

            // 마우스 좌표 → 월드 좌표
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            mouseWorld.z = 0f;

            // ✅ Snap 처리
            Vector3 snappedPos = GetSnappedPosition(mouseWorld);

            // ✅ 격자 범위 안에만 배치
            if (grid.IsInsideGrid(snappedPos))
            {
                Instantiate(prefab, snappedPos, Quaternion.identity);
            }
        }
    }

    /// <summary>
    /// 마우스 월드 좌표를 격자에 맞춰 스냅핑
    /// </summary>
    Vector3 GetSnappedPosition(Vector3 rawPos)
    {
        float cell = grid.CellSize;

        // ✅ X축: 반칸 단위 스냅
        float snappedX = Mathf.Round(rawPos.x / (cell * 0.5f)) * (cell * 0.5f);

        // ✅ Y축: 1칸 단위 스냅 (위/가운데/아래 3줄만)
        float snappedY = Mathf.Round(rawPos.y / cell) * cell;

        // Y축을 -1, 0, +1 줄로 제한
        if (snappedY > cell) snappedY = cell;
        else if (snappedY < -cell) snappedY = -cell;

        return new Vector3(snappedX, snappedY, 0f);
    }
}
