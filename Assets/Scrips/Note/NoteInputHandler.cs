using UnityEngine;

public class NoteInputHandler : MonoBehaviour
{
    [Header("판정 존 X 위치")]
    [SerializeField] private float hitZoneX = -4.3f;
    [Header("일반 히트 범위")]
    [SerializeField] private float hitRange = 0.3f;
    [Header("퍼펙트 히트 범위")]
    [SerializeField] private float perfectRange = 0.1f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) TryJudge(NoteSpawner.NoteType.Red);
        if (Input.GetKeyDown(KeyCode.G)) TryJudge(NoteSpawner.NoteType.Green);
        if (Input.GetKeyDown(KeyCode.B)) TryJudge(NoteSpawner.NoteType.Blue);
    }

    private void TryJudge(NoteSpawner.NoteType type)
    {
        // … (위에 드린 로직 그대로) …
    }
}
