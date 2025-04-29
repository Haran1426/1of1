using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HitBox : MonoBehaviour
{
    private NoteColor currentNote;



    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Note"))
        {
            currentNote = other.GetComponent<NoteColor>();
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
                Debug.Log("��Ʈ�ڽ� ��Ʈ +{gain}��");
            }
        }
    }


    void Update()
    {
        if (currentNote != null)
        {
            if (Input.GetKeyDown(KeyCode.R) && currentNote.noteType == NoteColor.NoteType.Red)
            {

                Score.Instance.AddScore(100);
                Destroy(currentNote.gameObject);
            }
            else if (Input.GetKeyDown(KeyCode.G) && currentNote.noteType == NoteColor.NoteType.Green)
            {
                Score.Instance.AddScore(100);
                Destroy(currentNote.gameObject);
            }
            else if (Input.GetKeyDown(KeyCode.B) && currentNote.noteType == NoteColor.NoteType.Blue)
            {


                Score.Instance.AddScore(100);
                Destroy(currentNote.gameObject);
            }
        }
    }
}
