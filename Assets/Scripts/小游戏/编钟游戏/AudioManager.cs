using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("UI音效")]
    [SerializeField] private AudioClip panelSwitchSound;
    [SerializeField] private float panelSwitchVolume = 0.5f;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayPanelSwitchSound()
    {
        if (panelSwitchSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(panelSwitchSound, panelSwitchVolume);
        }
    }
} 