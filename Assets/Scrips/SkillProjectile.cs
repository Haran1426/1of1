using UnityEngine;

public class SkillProjectile : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip skillHitSound;

    // Skill.cs에서 오디오/클립 전달
    public void SetupAudio(AudioSource external, AudioClip hitClip)
    {
        if (external != null) audioSource = external;
        else
        {
            audioSource = GetComponent<AudioSource>();
            if (!audioSource) audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
        skillHitSound = hitClip;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Note")) return;

        InGameUIManager.Instance.OnSkillScoreOnly();

        if (audioSource != null && skillHitSound != null)
            audioSource.PlayOneShot(skillHitSound);

        Destroy(other.gameObject);
    }
}
