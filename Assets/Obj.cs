using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class Obj : MonoBehaviour
{
    public enum Note_State
    {
        None, red, green, blue, yellow, magenta, cyan, white
    }

    public Note_State note_State = Note_State.None;
    [SerializeField] List<Sprite> Note_img_List = new List<Sprite>();
    private SpriteRenderer Point_Renderer;
    private GameObject Point_Note;
    void Start()
    {
        Transform content = GameObject.FindWithTag("Select_Note").transform;

        for ( int i = 0; i < content.childCount; i++)
        {
            Button child = content.GetChild(i).GetComponent<Button>();
            int Temp = i + 1;
            child.onClick.AddListener(() => Select_Note(Temp));
            
        }
        Point_Note = new GameObject();
        Point_Note.name = "Point_Enemy";
        Point_Renderer = Point_Note.AddComponent<SpriteRenderer>();
        
    }
    public void Select_Note(int _Type)
    {
        note_State = (Note_State)_Type;
        Point_Renderer.sprite = Note_img_List[_Type];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && note_State != 0) // 버전 업그레이드 하면 뉴인풋시스템으로 바꾸기
        {
            Select_Note(0);
        }
        Point_Note.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            Input.mousePosition.y - Camera.main.transform.position.z));


    }
}
