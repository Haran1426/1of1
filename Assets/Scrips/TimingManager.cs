using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    public List<GameObject> noteList = new List<GameObject>(); // 현재 생성된 노트 리스트

    [Header("타이밍 판정 영역")]
    [SerializeField] private Transform goodRect;
    [SerializeField] private Transform coolRect;
    [SerializeField] private Transform badRect;

    // Start is called before the first frame update
    void Start()
    {
        // 필수 오브젝트가 제대로 연결되지 않았는지 체크
        if (goodRect == null || coolRect == null || badRect == null)
        {
            Debug.LogWarning("타이밍 판정 영역이 연결되지 않았습니다.");
        }
    }


    // 노트가 타이밍 영역에 들어왔는지 확인
    public string CheckTiming(GameObject note)
    {
        float noteX = note.transform.position.x;

        if (IsWithin(noteX, goodRect))
            return "Good";
        else if (IsWithin(noteX, coolRect))
            return "Cool";
        else if (IsWithin(noteX, badRect))
            return "Bad";

        return "Miss";
    }

    // 중심을 기준으로 해당 영역 범위에 있는지 확인
    private bool IsWithin(float noteX, Transform rect)
    {
        float centerX = rect.position.x;
        float width = rect.localScale.x / 2f; // 스케일을 영역 너비로 가정

        return (noteX >= centerX - width && noteX <= centerX + width);
    }

    // 외부에서 노트를 리스트에 추가
    public void AddNote(GameObject note)
    {
        if (!noteList.Contains(note))
            noteList.Add(note);
    }

    // 외부에서 노트를 리스트에서 제거
    public void RemoveNote(GameObject note)
    {
        if (noteList.Contains(note))
            noteList.Remove(note);
    }
}
