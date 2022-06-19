using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WirelessChessAIPlayerLevel2", menuName = "ScriptableObjects/GameFlow/Brains/WirelessChessAIPlayerLevel2", order = 3)]
public class AIPlayerLevel2 : ScriptableObject, IPlayerControl
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

        List<Move> moves = new List<Move>();

        var teamPieces = pieces.GetPiecesByTeam(PlayerTeamID);
        var piece = teamPieces[Random.Range(0, teamPieces.Count)];
        var targets = piece.FindMoves(board);
        while (targets.Count == 0) { piece = teamPieces[Random.Range(0, teamPieces.Count)]; targets = piece.FindMoves(board); }

        foreach (Space s in targets)
        {
            if (s.GetPiece() is null) { moves.Add(new Move(piece, s, 1)); continue; }
            switch (s.GetPiece().GetPieceType())
            {
                case 0:
                    moves.Add(new Move(piece, s, 5)); continue;
                case 1:
                    moves.Add(new Move(piece, s, 10)); continue;
                case 2:
                    moves.Add(new Move(piece, s, 10)); continue;
                case 3:
                    moves.Add(new Move(piece, s, 10)); continue;
                case 4:
                    moves.Add(new Move(piece, s, 50)); continue;
                case 5:
                    moves.Add(new Move(piece, s, 500)); continue;
                case 6:
                    moves.Add(new Move(piece, s, 50)); continue;
            }

        }

        List<Move> weightedMoves = new List<Move>();
        foreach(Move m in moves)
        {
            for (int i = 0; i < m.weight; i++)
            {
                weightedMoves.Add(m);
            }
        }

        var target = weightedMoves[Random.Range(0, weightedMoves.Count)];


        ui.LogMove(target.piece, target.piece.GetSpace(), target.target);
        target.piece.Move(target.target, this);
    }

    private struct Move
    {
        public Piece piece;
        public Space target;
        public int weight;

        public Move(Piece p, Space t, int w)
        {
            piece = p;
            target = t;
            weight = w;
        }
    }
    
}
