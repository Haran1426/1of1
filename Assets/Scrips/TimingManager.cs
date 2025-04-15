using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    public List<GameObject> noteList = new List<GameObject>(); // ���� ������ ��Ʈ ����Ʈ

    [Header("Ÿ�̹� ���� ����")]
    [SerializeField] private Transform goodRect;
    [SerializeField] private Transform coolRect;
    [SerializeField] private Transform badRect;

    // Start is called before the first frame update
    void Start()
    {
        // �ʼ� ������Ʈ�� ����� ������� �ʾҴ��� üũ
        if (goodRect == null || coolRect == null || badRect == null)
        {
            Debug.LogWarning("Ÿ�̹� ���� ������ ������� �ʾҽ��ϴ�.");
        }
    }


    // ��Ʈ�� Ÿ�̹� ������ ���Դ��� Ȯ��
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

    // �߽��� �������� �ش� ���� ������ �ִ��� Ȯ��
    private bool IsWithin(float noteX, Transform rect)
    {
        float centerX = rect.position.x;
        float width = rect.localScale.x / 2f; // �������� ���� �ʺ�� ����

        return (noteX >= centerX - width && noteX <= centerX + width);
    }

    // �ܺο��� ��Ʈ�� ����Ʈ�� �߰�
    public void AddNote(GameObject note)
    {
        if (!noteList.Contains(note))
            noteList.Add(note);
    }

    // �ܺο��� ��Ʈ�� ����Ʈ���� ����
    public void RemoveNote(GameObject note)
    {
        if (noteList.Contains(note))
            noteList.Remove(note);
    }
}
