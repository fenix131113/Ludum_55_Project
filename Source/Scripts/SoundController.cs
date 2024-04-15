using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance;
    private AudioSource audioSource;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            audioSource = new GameObject().AddComponent<AudioSource>();
        }
    }

    public void PlayOneShot(AudioClip clip, float sound = 1f) => audioSource.PlayOneShot(clip, sound);
}
