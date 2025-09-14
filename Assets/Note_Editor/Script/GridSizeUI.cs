using UnityEngine;
using TMPro;

public class GridSizeUI : MonoBehaviour
{
    [SerializeField] private Note_Grid noteGrid;
    [SerializeField] private TMP_InputField widthInput;
    [SerializeField] private TMP_InputField mapNameInput;
    [SerializeField] private Data_Manager dataManager;

    public void OnApplySettings()
    {
        // Width 입력 처리
        if (int.TryParse(widthInput.text, out int newWidth))
        {
            noteGrid.SetWidth(newWidth);  // ✅ Inspector 말고 UI 입력값만 반영
            Debug.Log("가로 칸 수 변경됨: " + newWidth);
        }
        else
        {
            Debug.LogWarning("숫자를 입력해야 합니다!");
        }

        // MapName 입력 처리
        if (!string.IsNullOrEmpty(mapNameInput.text))
        {
            dataManager.saveFileName = mapNameInput.text + ".json";
            Debug.Log("맵 이름 변경됨: " + dataManager.saveFileName);
        }
        else
        {
            Debug.LogWarning("맵 이름이 비어 있습니다.");
        }
    }
}
