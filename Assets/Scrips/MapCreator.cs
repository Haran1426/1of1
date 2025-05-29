using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class NoteData
{
    public float time;       // ��Ʈ�� ������ �ð� (�� ����)
    public string type;      // R, G, B, RG ��
}

[System.Serializable]
public class GimmickData
{
    public float time;       // ��� �߻� �ð�
    public string type;      // SpeedUp, FadeOut ��
    public float value;      // ��� ���� �Ǵ� ��� ��
    public float duration;   // ���� �ð� (�ʿ��� ��츸)
}

[System.Serializable]
public class MapData
{
    public string songName;          // �� �̸�
    public float bpm;                // BPM
    public List<NoteData> notes;     // ��Ʈ ����Ʈ
    public List<GimmickData> gimmicks; // ��� ����Ʈ
}

public class MapCreator : MonoBehaviour
{
    void Start()
    {
        // ���� �� ������ ����
        MapData map = new MapData
        {
            songName = "test_track",
            bpm = 120f,
            notes = new List<NoteData>
            {
                new NoteData { time = 1.2f, type = "R" },
                new NoteData { time = 2.3f, type = "G" },
                new NoteData { time = 3.5f, type = "B" },
                new NoteData { time = 4.0f, type = "RG" }
            },
            gimmicks = new List<GimmickData>
            {
                new GimmickData { time = 5.0f, type = "SpeedUp", value = 1.5f, duration = 0f },
                new GimmickData { time = 7.2f, type = "FadeOut", value = 0f, duration = 2.0f }
            }
        };

        // JSON ���ڿ� ����
        string json = JsonUtility.ToJson(map, true);

        // ���� ���: Assets/Resources/Maps/
        string directory = Application.dataPath + "/Resources/Maps";
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        string path = Path.Combine(directory, "test_track.json");

        File.WriteAllText(path, json);

        Debug.Log("�� JSON ���� �Ϸ�: " + path);
    }
}
