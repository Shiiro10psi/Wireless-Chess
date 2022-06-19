using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] AudioClip buttonSound;

    public void StartButton()
    {
        FindObjectOfType<SoundPlayer>().PlaySound(buttonSound);
        FindObjectOfType<SceneLoader>().LoadScene(1);
    }

    public void TutorialButton()
    {
        FindObjectOfType<SoundPlayer>().PlaySound(buttonSound);
        FindObjectOfType<SceneLoader>().LoadScene(4);
    }

    public void CreditsButton()
    {
        FindObjectOfType<SoundPlayer>().PlaySound(buttonSound);
        FindObjectOfType<SceneLoader>().LoadScene(3);
    }
}
