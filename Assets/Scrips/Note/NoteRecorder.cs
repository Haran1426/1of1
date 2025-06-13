//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class NoteRecorder : MonoBehaviour //수동 노트 입력 스크리브트
//{
//    public AudioSource music;
//    private List<float> noteTimings = new();
//    void Update()
//    {
//        if (music.isPlaying && Input.GetKeyDown(KeyCode.Space))
//        {
//            float t = Time.time;
//            noteTimings.Add(t);
//            Debug.Log($"노트 기록: {t}");
//        }
//        if (Input.GetKeyDown(KeyCode.S)) {
//            string json = JsonUtility.ToJson(new Wrapper { timings = noteTimings.ToArray() });
//            System.IO.File.WriteAllText(Application.dataPath + "/noteData.json", json);
//            Debug.Log("저장됨");

//        }
//    }

//    [System.Serializable]
//    public class Wrapper { public float[] timings;}
//}
