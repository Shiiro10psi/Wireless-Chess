using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PawnPromotion", menuName = "ScriptableObjects/GameFlow/PawnPromotion", order = 2)]
public class PawnPromotion : ScriptableObject, IGameFlowObject
{
    public void StartingAction(GameControl g)
    {
        UIControl ui = FindObjectOfType<UIControl>();

        ui.ChangeTeamPanel(0, "Promoting Pawns");

        Pieces pieces = FindObjectOfType<Pieces>();
        Board board = FindObjectOfType<Board>();

        List<Piece> pawns = pieces.GetPiecesByType(0);

        foreach (Piece pawn in pawns)
        {
            Vector2 position = board.FindSpaceCoordinates(pawn.GetSpace());
            if (position.y == 7 && pawn.GetTeam() == 1) pawn.SetPieceType(6);
            if (position.y == 0 && pawn.GetTeam() == 2) pawn.SetPieceType(6);
        }

        g.Step(this);
    }
    
    public void Initialize()
    {

    }
}
