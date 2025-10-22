using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
  
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += PlayDeathSound;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= PlayDeathSound;
    }

    void PlayDeathSound()
    {
        audioSource.Play();
    }
}
