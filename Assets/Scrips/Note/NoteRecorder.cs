//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class NoteRecorder : MonoBehaviour //���� ��Ʈ �Է� ��ũ����Ʈ
//{
//    public AudioSource music;
//    private List<float> noteTimings = new();
//    void Update()
//    {
//        if (music.isPlaying && Input.GetKeyDown(KeyCode.Space))
//        {
//            float t = Time.time;
//            noteTimings.Add(t);
//            Debug.Log($"��Ʈ ���: {t}");
//        }
//        if (Input.GetKeyDown(KeyCode.S)) {
//            string json = JsonUtility.ToJson(new Wrapper { timings = noteTimings.ToArray() });
//            System.IO.File.WriteAllText(Application.dataPath + "/noteData.json", json);
//            Debug.Log("�����");

//        }
//    }

//    [System.Serializable]
//    public class Wrapper { public float[] timings;}
//}
