using System;
using System.Collections.Generic;

[Serializable]
public class NoteData
{
    public int type;       // 1=Red, 2=Green, ...
    public float posX;
    public float posY;
    public float distance; // 이전 노트와 거리
    public float beat;     // 이전 노트 대비 박자 차이
}

[Serializable]
public class MapData
{
    public string songName;
    public float bpm;
    public List<NoteData> notes;
}
