using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    
    [SerializeField] bool mute = false;

    private void Awake()
    {
        SetUpSingleton();
    }
    // Start is called before the first frame update
    void Start()
    {
        FindAudioSource();
        
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

        // Update is called once per frame
        void Update()
    {
        if (audioSource == null)
        FindAudioSource();

        if (!audioSource.isPlaying && !mute)
            audioSource.Play();
    }
    
    
    void FindAudioSource()
    {
      var sources = FindObjectsOfType<AudioSource>();
      
      foreach (AudioSource a in sources)
      {
        if (a.CompareTag("MusicPlayer"))
          {
            audioSource = a;
            break;
          }
      }
      
      SetPlay();
    }
    
    public void MuteToggle()
    {
      mute = !mute;
      SetPlay();
    }
    
    public bool IsMuted()
    {return mute;}
    
    void SetPlay()
    {
      if (!mute) audioSource.Play();
      if (mute) audioSource.Stop();
    }
    
}
