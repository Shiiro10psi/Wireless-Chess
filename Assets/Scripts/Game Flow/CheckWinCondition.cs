using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CheckWinCondition", menuName = "ScriptableObjects/GameFlow/CheckWinCondition", order = 1)]
public class CheckWinCondition : ScriptableObject, IGameFlowObject
{
    GameControl gc;

    [SerializeField] AudioClip winSound, loseSound;

    public void StartingAction(GameControl g)
    {
        gc = g;
        UIControl ui = FindObjectOfType<UIControl>();

        ui.ChangeTeamPanel(0, "Checking Win Conditions");

        Pieces pieces = FindObjectOfType<Pieces>();
        Board board = FindObjectOfType<Board>();

        List<Piece> kings = pieces.GetPiecesByType(5);

        switch (kings.Count)
        {
            case 2:
                break;
            case 1:
                int winningTeam = kings[0].GetTeam();
                string text = "You Found A Bug.";
                switch (winningTeam)
                {
                    case 1:
                        text = "Player 1 Wins";
                        WinningSounds(1);
                        break;
                    case 2:
                        text = "Player 2 Wins";
                        WinningSounds(2);
                        break;
                }
                ui.ChangeTeamPanel(winningTeam, text);
                return;
        }

        for(int i = 1; i <= 2; i++)
        {
            var listPieces = pieces.GetPiecesByTeam(i);
            bool hasMoves = false;

            foreach (Piece p in listPieces)
            {
                var moves = p.FindMoves(board);
                if (moves.Count > 0) { hasMoves = true; break; }
            }
            if (!hasMoves)
            {
                ui.LogAppend("Player " + i + " cannot move.");
                int opp = 0;
                string text = "You Found A Bug.";
                switch (i)
                {
                    case 1:
                        opp = 2;
                        text = "Player 2 Wins";
                        WinningSounds(2);
                        break;
                    case 2:
                        opp = 1;
                        text = "Player 1 Wins";
                        WinningSounds(1);
                        break;
                }
                ui.ChangeTeamPanel(opp, text);
                return;
            }
        }

        g.Step(this);
    }

    public void Initialize() { }

    private void WinningSounds(int winningPlayer)
    {
        PlayerSwitcher players = gc.GetPlayers();

        switch (players.GetNumberOfHumanPlayers())
        {
            case 0:
                FindObjectOfType<SoundPlayer>().PlaySound(winSound);
                break;
            case 2:
                FindObjectOfType<SoundPlayer>().PlaySound(winSound);
                break;
            case 1:
                if (players.IsPlayerHuman(winningPlayer - 1)) {
                    FindObjectOfType<SoundPlayer>().PlaySound(winSound);
                    break;
                }

                FindObjectOfType<SoundPlayer>().PlaySound(loseSound);
                break;

        }
    }
}
