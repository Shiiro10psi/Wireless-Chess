using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    bool mute = false;

    private void Awake()
    {
        SetUpSingleton();
        audioSource = gameObject.AddComponent<AudioSource>();
        Debug.Log(audioSource);
    }
    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void RebuildAudioSource()
    {
        if (audioSource != null)
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    
    public void MuteToggle()
    {
      mute = !mute;
    }
    
    public bool IsMuted()
    {
      return mute;
    }
    
    public void PlaySound(AudioClip clip)
    {
        Debug.Log(clip);
        Debug.Log(audioSource);
        if (audioSource == null) RebuildAudioSource();
      if (!mute)
        audioSource.PlayOneShot(clip, 1f);
    }
}
