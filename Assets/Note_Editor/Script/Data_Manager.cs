using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class NoteData
{
    public int type;
    public float posX;
    public float posY;
}

[System.Serializable]
public class NoteDataList
{
    public List<NoteData> notes = new List<NoteData>();
}

public class Data_Manager : MonoBehaviour
{
    public NoteDataList noteDataList = new NoteDataList();
    public GameObject[] notePrefabList;

    public void AddNote(int type, float x, float y)
    {
        NoteData data = new NoteData { type = type, posX = x, posY = y };
        noteDataList.notes.Add(data);

        if (type > 0 && type <= notePrefabList.Length)
        {
            GameObject obj = Instantiate(notePrefabList[type - 1], new Vector3(x, y, 0), Quaternion.identity);
            obj.tag = "Note";
        }
        else
        {
            Debug.LogError($"잘못된 노트 타입: {type}, 프리팹 리스트 길이: {notePrefabList.Length}");
        }
    }

    public bool ExistsNoteAtPosition(Vector3 pos)
    {
        foreach (var note in noteDataList.notes)
        {
            if (Mathf.Approximately(note.posX, pos.x) &&
                Mathf.Approximately(note.posY, pos.y))
                return true;
        }
        return false;
    }
}
