using System.Collections.Generic;
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
    cyan,
    White
}
public class Object_Manager : MonoBehaviour
{
    [Header("선택된 노트 상태")]
    public Note_State note_State = Note_State.None;
    [SerializeField] private List<Sprite> noteImgList;
    [SerializeField] private List<GameObject> notePrefabList;

    private GameObject pointNote;
    private SpriteRenderer pointRender;
    public GameObject GetSelectedPrefab()
    {
        if (note_State == Note_State.None) return null;
        int index = (int)note_State - 1;
        if (index >= 0 && index < notePrefabList.Count)
            return notePrefabList[index];
        return null;
    }

    public void Select_Note(int type)
    {
        note_State = (Note_State)type;

        if (type > 0 && type <= noteImgList.Count)
            pointRender.sprite = noteImgList[type - 1]; // None 제외
        else
            pointRender.sprite = null;
        Debug.LogError("s");
    }

    void Start()
    {
        Transform content = GameObject.FindWithTag("Select_Note").transform;

        for (int i = 0; i < content.childCount; i++)
        {
            int temp = i + 1;
            Button child = content.GetChild(i).GetComponent<Button>();
            child.onClick.AddListener(() => Select_Note(temp));
        }

        // 미리보기 오브젝트
        pointNote = new GameObject("Point_Note");
        pointNote.transform.localScale = new Vector3(3, 3, 1);
        pointRender = pointNote.AddComponent<SpriteRenderer>();
        pointRender.sortingLayerName = "Preview";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Select_Note(0); // 선택 해제
        }
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        mouseWorld.z = 0f;
        pointNote.transform.position = mouseWorld;
    }

}
