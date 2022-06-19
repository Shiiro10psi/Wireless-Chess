using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WirelessChessAIPlayerLevel1", menuName = "ScriptableObjects/GameFlow/Brains/WirelessChessAIPlayerLevel1", order = 2)]
public class AIPlayerLevel1 : ScriptableObject, IPlayerControl
{
    PlayerSwitcher ps;
    UIControl ui;

    [SerializeField] int PlayerTeamID = 1;
    
    int callbacks = 0;
    int expectedCallbacks = 0;


    public void Callback()
    {
        callbacks++;
        if (callbacks >= expectedCallbacks)
            ps.Step(this);
    }

    public void ScheduleCallback()
    {
        expectedCallbacks++;
    }

    public void StartingAction(PlayerSwitcher g)
    {
        ps = g;

        ui = FindObjectOfType<UIControl>();

        ui.ChangeTeamPanel(PlayerTeamID, "Player " + PlayerTeamID + " Turn");

        var board = FindObjectOfType<Board>();
        var pieces = FindObjectOfType<Pieces>();

        var teamPieces = pieces.GetPiecesByTeam(PlayerTeamID);
        var piece = teamPieces[Random.Range(0, teamPieces.Count)];
        var targets = piece.FindMoves(board);
        while (targets.Count == 0) { piece = teamPieces[Random.Range(0, teamPieces.Count)]; targets = piece.FindMoves(board); }
        var target = targets[Random.Range(0, targets.Count)];

        ui.LogMove(piece, piece.GetSpace(), target);
        piece.Move(target, this);
    }
}
