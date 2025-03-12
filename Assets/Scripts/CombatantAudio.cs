using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CombatantAudio : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip swordSwingSFX;
    [SerializeField] private AudioClip hurtSFX;

    private AudioSource audioSource;

    private void Awake()
    {
        TryGetComponent(out audioSource);
    }

    public void Hurt()
    {
        audioSource.PlayOneShot(hurtSFX);
    }

    public void SwordSwing()
    {
        audioSource.PlayOneShot(swordSwingSFX);
    }
}
