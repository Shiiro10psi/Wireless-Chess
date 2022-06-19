using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoundControllerUI : MonoBehaviour
{
    MusicPlayer mp;
    SoundPlayer sp;

    [SerializeField] TMP_Text mpText, spText;

    [SerializeField] AudioClip buttonSound;

    // Start is called before the first frame update
    void Start()
    {
        FindMusic();
        FindSound();
    }

    // Update is called once per frame
    void Update()
    {
        if (mp == null) FindMusic();
        if (sp == null) FindSound();
    }

    void FindMusic()
    {
        mp = FindObjectOfType<MusicPlayer>();
        if (mp.IsMuted()) mpText.text = "Unmute Music";
        if (!mp.IsMuted()) mpText.text = "Mute Music";
    }

    void FindSound()
    {
        sp = FindObjectOfType<SoundPlayer>();
        if (sp.IsMuted()) spText.text = "Unmute Sounds";
        if (!sp.IsMuted()) spText.text = "Mute Sounds";
    }

    public void MusicButton()
    {
        sp.PlaySound(buttonSound);
        mp.MuteToggle();
        if (mp.IsMuted()) mpText.text = "Unmute Music";
        if (!mp.IsMuted()) mpText.text = "Mute Music";
    }

    public void SoundButton()
    {
        sp.PlaySound(buttonSound);
        sp.MuteToggle();
        if (sp.IsMuted()) spText.text = "Unmute Sounds";
        if (!sp.IsMuted()) spText.text = "Mute Sounds";
    }
}
