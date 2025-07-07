using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioSource sfxSource;

    public void ToggleMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause(); //∏ÿ√„
        }
        else
        {
            audioSource.Play();  //Ω√¿€
        }
    }

    public void ToggleSFX(bool isOn)
    {
        sfxSource.mute = !isOn; //»ø∞˙¿Ω ø¬
    }
}
