using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WirelessChessAIPlayerLevel3", menuName = "ScriptableObjects/GameFlow/Brains/WirelessChessAIPlayerLevel3", order = 4)]
public class AIPlayerLevel3 : ScriptableObject, IPlayerControl
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
        foreach (Piece p in teamPieces)
        {
            
            var targets = p.FindMoves(board);

            foreach (Space s in targets)
            {
                if (s.GetPiece() is null) { moves.Add(new Move(p, s, 1)); continue; }
                switch (s.GetPiece().GetPieceType())
                {
                    case 0:
                        moves.Add(new Move(p, s, 5)); continue;
                    case 1:
                        moves.Add(new Move(p, s, 10)); continue;
                    case 2:
                        moves.Add(new Move(p, s, 10)); continue;
                    case 3:
                        moves.Add(new Move(p, s, 10)); continue;
                    case 4:
                        moves.Add(new Move(p, s, 50)); continue;
                    case 5:
                        moves.Add(new Move(p, s, 500)); continue;
                    case 6:
                        moves.Add(new Move(p, s, 50)); continue;
                }

            }
        }
        List<Move> weightedMoves = new List<Move>();
        foreach (Move m in moves)
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
