using System;
using System.Collections.Generic;

[Serializable]
public class NoteData
{
    public int type;       // 1=Red, 2=Green, ...
    public float posX;
    public float posY;
    public float distance; // ���� ��Ʈ�� �Ÿ�
    public float beat;     // ���� ��Ʈ ��� ���� ����
}

[Serializable]
public class MapData
{
    public string songName;
    public float bpm;
    public List<NoteData> notes;
}
