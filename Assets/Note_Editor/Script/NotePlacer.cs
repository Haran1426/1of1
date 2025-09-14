using UnityEngine;

public class NotePlacer : MonoBehaviour
{
    public Camera cam;
    public Note_Grid grid;
    public Object_Manager objManager;
    public Data_Manager dataManager;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject prefab = objManager.GetSelectedPrefab();
            if (prefab == null) return;

            Vector3 mouseWorld = cam.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, -cam.transform.position.z));

            Vector3 snapped = grid.GetSnappedPosition(mouseWorld);

            // ✅ 격자 안 & 중복 설치 방지
            if (grid.IsInsideGrid(snapped) && !dataManager.ExistsNoteAtPosition(snapped))
            {
                int type = (int)objManager.note_State;
                dataManager.AddNote(type, snapped.x, snapped.y);
            }
        }
    }
}
