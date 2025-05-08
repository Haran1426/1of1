using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HitBox : MonoBehaviour
{
    private NoteSpawner currentNote;



    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Note"))
        {
            currentNote = other.GetComponent<NoteSpawner>();
        }
        if (other.CompareTag("Note"))
        {
            currentNote = null;
        }

        if (other.CompareTag("Player"))
        {
            if (Score.Instance != null)
            {
                int gain = Random.Range(300, 501);
                Score.Instance.AddScore(gain);
                Debug.Log($"히트박스 히트 +{gain}점");
            }
        }
    }


    void Update()
    {
        if (currentNote != null)
        {
            if (Input.GetKeyDown(KeyCode.R) && currentNote.noteType == NoteSpawner.NoteType.Red)
            {
                Score.Instance.AddScore(100);
                Destroy(currentNote.gameObject);
            }
            else if (Input.GetKeyDown(KeyCode.G) && currentNote.noteType == NoteSpawner.NoteType.Green)
            {
                Score.Instance.AddScore(100);
                Destroy(currentNote.gameObject);
            }
            else if (Input.GetKeyDown(KeyCode.B) && currentNote.noteType == NoteSpawner.NoteType.Blue)
            {
                Score.Instance.AddScore(100);
                Destroy(currentNote.gameObject);
            }
        }
    }
}
