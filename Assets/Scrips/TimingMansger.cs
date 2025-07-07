//using System.Collections.Generic;
//using UnityEngine;

//public class TimingManager : MonoBehaviour
//{

//    public Transform judgeLine; // 판정 기준선 위치
//    public float[] timingWindows = { 0.1f, 0.3f, 0.5f }; // Perfect, Good, Bad 거리 차이
//    public string[] timingResults = { "Perfect", "Good", "Bad", "Miss" };

//    private List<GameObject> noteList = new List<GameObject>();

//    public void AddNote(GameObject note)
//    {
//        if (!noteList.Contains(note))
//        {
//            noteList.Add(note);
//        }
//    }

//    public void RemoveNote(GameObject note)
//    {
//        if (noteList.Contains(note))
//        {
//            noteList.Remove(note);
//        }
//    }

//    public string CheckTiming(GameObject note)
//    {
//        if (note == null || judgeLine == null) return "Miss";

//        float distance = Mathf.Abs(note.transform.position.x - judgeLine.position.x);

//        for (int i = 0; i < timingWindows.Length; i++)
//        {
//            if (distance <= timingWindows[i])
//            {
//                return timingResults[i];
//            }
//        }

//        return "Miss";
//    }
//}
