using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSettingsMenu : MonoBehaviour
{
    TMP_Dropdown player1Dropdown, player2Dropdown;
    Image player1Color, player2Color;

    [SerializeField] List<ScriptableObject> Player1ControlOptions;
    [SerializeField] List<ScriptableObject> Player2ControlOptions;

    [SerializeField] AudioClip buttonSound;

    private void Awake()
    {
        //Find UI elements
        var images = FindObjectsOfType<Image>();
        foreach (Image i in images)
        {
            if (i.name == "Player1Color") player1Color = i;
            if (i.name == "Player2Color") player2Color = i;
        }

        var dropdowns = FindObjectsOfType<TMP_Dropdown>();
        foreach (TMP_Dropdown d in dropdowns)
        {
            if (d.name == "Player1Dropdown") player1Dropdown = d;
            if (d.name == "Player2Dropdown") player2Dropdown = d;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnButton()
    {
        FindObjectOfType<SoundPlayer>().PlaySound(buttonSound);
        FindObjectOfType<SceneLoader>().ReturnToMenu();
    }

    public void StartButton()
    {
        FindObjectOfType<SoundPlayer>().PlaySound(buttonSound);
        PackSettings();
        FindObjectOfType<SceneLoader>().LoadScene(2);
    }

    private void PackSettings()
    {
        GameSettings g = FindObjectOfType<GameSettings>();
        if (g is null) g = new GameSettings();

        List<Color> colors = new List<Color>();
        colors.Add(Color.grey);
        colors.Add(player1Color.color);
        colors.Add(player2Color.color);

        bool cam = false;
        if (player1Dropdown.value != 0 && player2Dropdown.value == 0) cam = true;
        
        List<ScriptableObject> objects = new List<ScriptableObject>
        {
            Player1ControlOptions[player1Dropdown.value],
            Player2ControlOptions[player2Dropdown.value]
        };

        g.Pack(colors, objects, cam);
    }

    public void Player1ColorSlider(float value)
    {
        player1Color.color = Color.HSVToRGB(value, 0.9f, 0.9f);
    }

    public void Player1ColorButton()
    {
        player1Color.color = Color.white;
    }

    public void Player2ColorSlider(float value)
    {
        player2Color.color = Color.HSVToRGB(value, 0.9f, 0.9f);
    }

    public void Player2ColorButton()
    {
        player2Color.color = new Color(0.1f, 0.1f, 0.1f);
    }
}
