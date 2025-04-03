using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSource;
    public void ToggleMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.Play();  
        }
    }
}
