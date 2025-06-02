using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
    
{
    public NoteSpawner.NoteType noteType;
    private float hitZoneX = -4.3f; // 판정선 X 위치
    private float hitRange = 0.3f;  // 허용 오차 범위

    private bool isHit = false; // 중복 판정 방지용

    void Update()
    {
        // 이미 맞춘 노트는 무시
        if (isHit) return;

        float distanceToHitZone = Mathf.Abs(transform.position.x - hitZoneX);
        if (distanceToHitZone <= hitRange)
        {
            if (CheckInputForNote(noteType))
            {
                isHit = true;
                Score.Instance.AddScore(100);
                Debug.Log($"Hit {noteType} note!");
                Destroy(gameObject);
            }
        }

        // 너무 지나쳤으면 Miss 처리 (여기선 그냥 삭제)
        if (transform.position.x < hitZoneX - 1f)
        {
            Destroy(gameObject);
        }
    }

    // 각 노트 타입에 맞는 키 입력 체크
    bool CheckInputForNote(NoteSpawner.NoteType type)
    {
        switch (type)
        {
            case NoteSpawner.NoteType.Red:
                return Input.GetKeyDown(KeyCode.R);
            case NoteSpawner.NoteType.Green:
                return Input.GetKeyDown(KeyCode.G);
            case NoteSpawner.NoteType.Blue:
                return Input.GetKeyDown(KeyCode.B);
            case NoteSpawner.NoteType.Yellow:
                return Input.GetKey(KeyCode.R) && Input.GetKeyDown(KeyCode.G) ||
                       Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.R);
            case NoteSpawner.NoteType.Magenta:
                return Input.GetKey(KeyCode.R) && Input.GetKeyDown(KeyCode.B) ||
                       Input.GetKey(KeyCode.B) && Input.GetKeyDown(KeyCode.R);
            case NoteSpawner.NoteType.Cyan:
                return Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.B) ||
                       Input.GetKey(KeyCode.B) && Input.GetKeyDown(KeyCode.G);
            case NoteSpawner.NoteType.White:
                return (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.B)) ||
                       (Input.GetKey(KeyCode.R) && Input.GetKeyDown(KeyCode.G) && Input.GetKey(KeyCode.B)) ||
                       (Input.GetKeyDown(KeyCode.R) && Input.GetKey(KeyCode.G) && Input.GetKey(KeyCode.B));
        }

        return false;
    }
}
