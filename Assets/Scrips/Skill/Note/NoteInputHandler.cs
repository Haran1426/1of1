using UnityEngine;

public class NoteInputHandler : MonoBehaviour
{
    [Header("���� �� X ��ġ")]
    [SerializeField] private float hitZoneX = -4.3f;
    [Header("�Ϲ� ��Ʈ ����")]
    [SerializeField] private float hitRange = 0.3f;
    [Header("����Ʈ ��Ʈ ����")]
    [SerializeField] private float perfectRange = 0.1f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) TryJudge(NoteSpawner.NoteType.Red);
        if (Input.GetKeyDown(KeyCode.G)) TryJudge(NoteSpawner.NoteType.Green);
        if (Input.GetKeyDown(KeyCode.B)) TryJudge(NoteSpawner.NoteType.Blue);
    }

    private void TryJudge(NoteSpawner.NoteType type)
    {
        // �� (���� �帰 ���� �״��) ��
    }
}
