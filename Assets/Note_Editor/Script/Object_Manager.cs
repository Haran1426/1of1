using UnityEngine;
using UnityEngine.UI;   


public enum Note_State
{
    None,
    Red,
    Green,
    Blue,
    Yellow,
    Magenta,
    Cyan,
    White
}

public class Object_Manager : MonoBehaviour
{
    [Header("현재 선택된 노트 상태")]
    public Note_State note_State = Note_State.None;

    [Header("노트 프리팹 (빨강~화이트 순서대로)")]
    [SerializeField] private GameObject[] notePrefabList;

    /// <summary> 현재 선택된 Prefab 반환 </summary>
    public GameObject GetSelectedPrefab()
    {
        int index = (int)note_State;
        if (index >= 0 && index < notePrefabList.Length)
            return notePrefabList[index];
        return null;
    }


    /// <summary> UI 버튼에서 호출 (Inspector에서 OnClick 연결) </summary>
    public void Select_Note(int type)
    {
        note_State = (Note_State)type;
    }
}

