using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Object_Manager : MonoBehaviour
{
    public enum Note_State
    {
        None, Red, Blue
    }

    public Note_State note_State = Note_State.None;

    [SerializeField] List<Sprite> Note_Img_List = new List<Sprite>();
    
    private GameObject Point_Note;
    private SpriteRenderer Point_Render;

    public void Select_Note(int _Type)
    {
        note_State = (Note_State)_Type;
        Point_Render.sprite = Note_Img_List[_Type];
    }



    void Start()
    {
        Transform Content = GameObject.FindWithTag("Select_Note").transform;

        for (int i = 0; i < Content.childCount; i++)
        {
            Button child = Content.GetChild(i).GetComponent<Button>();
            int Temp = i + 1;
            child.onClick.AddListener(() => Select_Note(Temp));
        }

        Point_Note = new GameObject();
        Point_Note.name = "Point_Note";
        Point_Note.transform.localScale = new Vector3(3, 3, 1);
        Point_Render = Point_Note.AddComponent<SpriteRenderer>();
        Point_Render.sortingLayerName = "UI";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && note_State != 0) // 버전 업그레이드 하면 new InputSystem으로 변경
        {
            Select_Note(0);
        }

        Point_Note.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                                                                        Input.mousePosition.z - Camera.main.transform.position.z));
    }
}
