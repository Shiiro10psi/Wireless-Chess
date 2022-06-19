using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIControl : MonoBehaviour
{
    Image TeamPanel;
    TMP_Text TeamText;
    TMP_Text MoveLog;
    ScrollRect LogPanel;

    List<Color> TeamColors;

    [SerializeField] AudioClip buttonSound;
    
    private void Awake()
    {
        var Images = GetComponentsInChildren<Image>();
        foreach (Image i in Images)
        {
            if (i.gameObject.name == "TeamPanel") TeamPanel = i;
        }

        var Text = GetComponentsInChildren<TMP_Text>();
        foreach (TMP_Text t in Text)
        {
            if (t.gameObject.name == "TeamText") TeamText = t;
            if (t.gameObject.name == "MoveLog") MoveLog = t;
        }

        var Scrolls = GetComponentsInChildren<ScrollRect>();
        foreach (ScrollRect s in Scrolls)
        {
            if (s.gameObject.name == "LogPanel") LogPanel = s;
        }

        LogAppend("Welcome to Wireless Chess.");

        TeamColors = FindObjectOfType<GameSettings>().TeamColors;
    }

    public void ChangeTeamPanel(int teamId, string text)
    {
        TeamPanel.color = TeamColors[teamId];
        TeamText.text = text;
        Color.RGBToHSV(TeamColors[teamId], out float H, out float S, out float V);
        if (V > 0.5f)
        {
            TeamText.color = Color.black;
        }
        if (V < 0.5f)
        {
            TeamText.color = Color.white;
        }
    }

    public void ButtonReload()
    {
        FindObjectOfType<SoundPlayer>().PlaySound(buttonSound);
        FindObjectOfType<SceneLoader>().ReloadScene();
    }

    public void ButtonReturn()
    {
        FindObjectOfType<SoundPlayer>().PlaySound(buttonSound);
        FindObjectOfType<SceneLoader>().ReturnToMenu();
    }


    public void LogAppend(string s)
    {
        MoveLog.text = MoveLog.text + s + "\n";
        Invoke("ScrollToBottom",0.05f);
    }

    private void ScrollToBottom()
    {
        LogPanel.normalizedPosition = new Vector2(0, 0f);
    }

    public void LogMove(Piece piece, Space from, Space to)
    {
        string s = "";
        switch (piece.GetPieceType())
        {
            case 0:
                s += "Pawn";
                break;
            case 1:
                s += "Rook";
                break;
            case 2:
                s += "Knight";
                break;
            case 3:
                s += "Bishop";
                break;
            case 4:
                s += "Queen";
                break;
            case 5:
                s += "King";
                break;
            case 6:
                s += "Jammer Pawn";
                break;
        }

        s += " from ";

        switch (from.transform.position.x)
        {
            case 0:
                s += "A";
                break;
            case 1:
                s += "B";
                break;
            case 2:
                s += "C";
                break;
            case 3:
                s += "D";
                break;
            case 4:
                s += "E";
                break;
            case 5:
                s += "F";
                break;
            case 6:
                s += "G";
                break;
            case 7:
                s += "H";
                break;
        }

        s += (int)(from.transform.position.y + 1);

        s += " to ";

        switch (to.transform.position.x)
        {
            case 0:
                s += "A";
                break;
            case 1:
                s += "B";
                break;
            case 2:
                s += "C";
                break;
            case 3:
                s += "D";
                break;
            case 4:
                s += "E";
                break;
            case 5:
                s += "F";
                break;
            case 6:
                s += "G";
                break;
            case 7:
                s += "H";
                break;
        }
        s += (int)(to.transform.position.y + 1);

        LogAppend(s);
    }

    public void LogStale(int player)
    {
        if (player == 0)
        {
            LogAppend("White is unable to move.");
        }
        else if (player == 1)
        {
            LogAppend("Black is unable to move.");
        }
    }
}
