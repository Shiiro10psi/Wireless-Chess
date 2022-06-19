using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] RectTransform panelSlider;
    [SerializeField] AudioClip buttonSound;

    public void NextButton()
    {
        FindObjectOfType<SoundPlayer>().PlaySound(buttonSound);
        panelSlider.anchoredPosition = new Vector2(panelSlider.anchoredPosition.x - 1900, panelSlider.anchoredPosition.y);
    }

    public void PrevButton()
    {

        panelSlider.anchoredPosition = new Vector2(panelSlider.anchoredPosition.x + 1900, panelSlider.anchoredPosition.y);
        FindObjectOfType<SoundPlayer>().PlaySound(buttonSound);
    }

    public void ReturnButton()
    {
        FindObjectOfType<SoundPlayer>().PlaySound(buttonSound);
        FindObjectOfType<SceneLoader>().ReturnToMenu();
    }
}
