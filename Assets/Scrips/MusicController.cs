using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioSource sfxSource;

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

    public void ToggleSFX(bool isOn)
    {
        sfxSource.mute = !isOn;
    }
}
