using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WirelessChessPlayer", menuName = "ScriptableObjects/GameFlow/Brains/WirelessChessPlayer", order = 1)]
public class PlayerMove : ScriptableObject, IPlayerControl
{
    GridNavInputController controller;
    PlayerSwitcher ps;
    UIControl ui;

    [SerializeField] int PlayerTeamID = 1;

    Space selectedSpace;

    int callbacks = 0;
    int expectedCallbacks = 0;

    public void StartingAction(PlayerSwitcher g)
    {
        ps = g;

        controller = FindObjectOfType<GridNavInputController>();
        ui = FindObjectOfType<UIControl>();

        ui.ChangeTeamPanel(PlayerTeamID, "Player " + PlayerTeamID + " Turn");

        controller.Wake(this);

        selectedSpace = null;
    }

    public void Select(Space space)
    {
        var board = FindObjectOfType<Board>();
        var interactable = board.GetInteractableSpaces();
        
        if (selectedSpace is null)
        {
            if (space.GetPiece() != null && space.GetPiece().GetTeam() == PlayerTeamID)
            {
                selectedSpace = space;
                var piece = selectedSpace.GetPiece();
                if (piece is WirelessChessPiece)
                {
                    var piece2 = (WirelessChessPiece)piece;
                    piece2.Highlight(1);
                }
                board.DetermineInteractableSpaces(selectedSpace.GetPiece());
            }
            return;
        }
        if (selectedSpace != null  && interactable.Contains(space))
        {
            var piece = selectedSpace.GetPiece();
            if (piece is WirelessChessPiece)
            {
                var piece2 = (WirelessChessPiece)piece;
                piece2.Highlight(0);
            }
            if (selectedSpace == space) { board.ClearInteractableSpaces(); selectedSpace = null; return; }
            ui.LogMove(selectedSpace.GetPiece(), selectedSpace, space);
            selectedSpace.GetPiece().Move(space, this);
            board.ClearInteractableSpaces();
            selectedSpace = null;
            controller.Sleep();
            return;
        }
    }

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
    
    public void Debug(Space space)
    {
        if (space != null)
        {
            var piece = (WirelessChessPiece) space.GetPiece();
            if (piece != null) { piece.DebugLogResolveString(); DebugHighLight(piece); }
        }
    }

    public void DebugHighLight(WirelessChessPiece p)
    {
        p.DebugHighlight();
    }
}
