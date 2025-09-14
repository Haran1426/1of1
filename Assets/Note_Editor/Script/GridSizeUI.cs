using UnityEngine;
using TMPro;

public class GridSizeUI : MonoBehaviour
{
    [SerializeField] private Note_Grid noteGrid;          // Grid (Note_Grid 붙은 오브젝트)
    [SerializeField] private TMP_InputField widthInput;   // Width 입력칸
    [SerializeField] private TMP_InputField mapNameInput; // MapName 입력칸
    [SerializeField] private Data_Manager dataManager;    // 맵 데이터 저장용

    // 버튼에서 호출될 함수
    public void OnApplySettings()
    {
        // Width 입력 처리
        if (int.TryParse(widthInput.text, out int newWidth))
        {
            noteGrid.SetWidth(newWidth); // ✅ 직접 메서드 호출
            Debug.Log("가로 칸 수 변경됨: " + newWidth);
        }
        else
        {
            Debug.LogWarning("숫자를 입력해야 합니다!");
        }

        // MapName 입력 처리
        if (!string.IsNullOrEmpty(mapNameInput.text))
        {
            dataManager.saveFileName = mapNameInput.text + ".json"; // ✅ 인스턴스 참조
            Debug.Log("맵 이름 변경됨: " + dataManager.saveFileName);
        }
        else
        {
            Debug.LogWarning("맵 이름이 비어 있습니다.");
        }
    }
}
