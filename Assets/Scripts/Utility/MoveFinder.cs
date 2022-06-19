using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFinder
{
    public static List<Space> Pawn(Board b, Piece p, bool FirstMove, int up, int TeamID)
    {
        List<Space> spaces = new List<Space>();

        Vector2 originCoordinates = b.FindSpaceCoordinates(p.GetSpace());

        var space = b.GetSpaceByCoordinates(originCoordinates + new Vector2(0, up));
        if (space != null && space.GetPiece() is null) spaces.Add(space);

        if (FirstMove && space.GetPiece() is null)
        {
            space = b.GetSpaceByCoordinates(originCoordinates + new Vector2(0, up * 2));
            if (space != null && space.GetPiece() is null) spaces.Add(space);
        }

        space = b.GetSpaceByCoordinates(originCoordinates + new Vector2(-1, up));
        if (space != null && space.GetPiece() != null && space.GetPiece().GetTeam() != p.GetTeam()) spaces.Add(space);
        if (space != null && space.GetMarker() != null && space.GetMarker().GetTeam() != p.GetTeam()) spaces.Add(space);

        space = b.GetSpaceByCoordinates(originCoordinates + new Vector2(1, up));
        if (space != null && space.GetPiece() != null  && space.GetPiece().GetTeam() != p.GetTeam()) spaces.Add(space);
        if (space != null && space.GetMarker() != null && space.GetMarker().GetTeam() != p.GetTeam()) spaces.Add(space);

        return spaces;
    }

    public static List<Space> Rook(Board b, Piece p, int TeamID)
    {
        List<Space> spaces = new List<Space>();

        Vector2 originCoordinates = b.FindSpaceCoordinates(p.GetSpace());

        bool exit = false;
        int counter = 1;
        Space space;

        do //Up
        {
            space = b.GetSpaceByCoordinates(originCoordinates + new Vector2(0, 1 * counter));
            exit = SpaceValidationNonPawnFromLoop(p, spaces, exit, space);

            counter++;
        } while (!exit);
        exit = false;
        counter = 1;

        do //Down
        {
            space = b.GetSpaceByCoordinates(originCoordinates + new Vector2(0, -1 * counter));
            exit = SpaceValidationNonPawnFromLoop(p, spaces, exit, space);

            counter++;
        } while (!exit);
        exit = false;
        counter = 1;

        do //Right
        {
            space = b.GetSpaceByCoordinates(originCoordinates + new Vector2(1 * counter, 0));
            exit = SpaceValidationNonPawnFromLoop(p, spaces, exit, space);

            counter++;
        } while (!exit);
        exit = false;
        counter = 1;

        do //Left
        {
            space = b.GetSpaceByCoordinates(originCoordinates + new Vector2(-1 * counter, 0));
            exit = SpaceValidationNonPawnFromLoop(p, spaces, exit, space);

            counter++;
        } while (!exit);

        return spaces;
    }

    public static List<Space> Bishop(Board b, Piece p, int TeamID)
    {
        List<Space> spaces = new List<Space>();

        Vector2 originCoordinates = b.FindSpaceCoordinates(p.GetSpace());

        bool exit = false;
        int counter = 1;
        Space space;

        do //UpRight
        {
            space = b.GetSpaceByCoordinates(originCoordinates + new Vector2(1 * counter, 1 * counter));
            exit = SpaceValidationNonPawnFromLoop(p, spaces, exit, space);

            counter++;
        } while (!exit);
        exit = false;
        counter = 1;

        do //DownRight
        {
            space = b.GetSpaceByCoordinates(originCoordinates + new Vector2(1 * counter, -1 * counter));
            exit = SpaceValidationNonPawnFromLoop(p, spaces, exit, space);

            counter++;
        } while (!exit);
        exit = false;
        counter = 1;

        do //UpLeft
        {
            space = b.GetSpaceByCoordinates(originCoordinates + new Vector2(-1 * counter, 1 * counter));
            exit = SpaceValidationNonPawnFromLoop(p, spaces, exit, space);

            counter++;
        } while (!exit);
        exit = false;
        counter = 1;

        do //DownLeft
        {
            space = b.GetSpaceByCoordinates(originCoordinates + new Vector2(-1 * counter, -1 * counter));
            exit = SpaceValidationNonPawnFromLoop(p, spaces, exit, space);

            counter++;
        } while (!exit);

        return spaces;
    }

    public static List<Space> Queen(Board b, Piece p, int TeamID)
    {
        List<Space> spaces = new List<Space>();

        spaces.AddRange(Rook(b, p, TeamID));
        spaces.AddRange(Bishop(b, p, TeamID));

        return spaces;
    }

    public static List<Space> Knight(Board b, Piece p, int TeamID)
    {
        List<Space> spaces = new List<Space>();

        Vector2 originCoordinates = b.FindSpaceCoordinates(p.GetSpace());

        var space = b.GetSpaceByCoordinates(originCoordinates + new Vector2(1,2));
        SpaceValidationNonLoop(p, spaces, space);
        space = b.GetSpaceByCoordinates(originCoordinates + new Vector2(2, 1));
        SpaceValidationNonLoop(p, spaces, space);
        space = b.GetSpaceByCoordinates(originCoordinates + new Vector2(2, -1));
        SpaceValidationNonLoop(p, spaces, space);
        space = b.GetSpaceByCoordinates(originCoordinates + new Vector2(1, -2));
        SpaceValidationNonLoop(p, spaces, space);
        space = b.GetSpaceByCoordinates(originCoordinates + new Vector2(-1, 2));
        SpaceValidationNonLoop(p, spaces, space);
        space = b.GetSpaceByCoordinates(originCoordinates + new Vector2(-2, 1));
        SpaceValidationNonLoop(p, spaces, space);
        space = b.GetSpaceByCoordinates(originCoordinates + new Vector2(-2, -1));
        SpaceValidationNonLoop(p, spaces, space);
        space = b.GetSpaceByCoordinates(originCoordinates + new Vector2(-1, -2));
        SpaceValidationNonLoop(p, spaces, space);

        return spaces;
    }

    public static List<Space> King(Board b, Piece p, int TeamID)
    {
        List<Space> spaces = new List<Space>();

        Vector2 originCoordinates = b.FindSpaceCoordinates(p.GetSpace());

        var space = b.GetSpaceByCoordinates(originCoordinates + new Vector2(0, 1));
        SpaceValidationNonLoop(p, spaces, space);
        space = b.GetSpaceByCoordinates(originCoordinates + new Vector2(1, 1));
        SpaceValidationNonLoop(p, spaces, space);
        space = b.GetSpaceByCoordinates(originCoordinates + new Vector2(1, 0));
        SpaceValidationNonLoop(p, spaces, space);
        space = b.GetSpaceByCoordinates(originCoordinates + new Vector2(1, -1));
        SpaceValidationNonLoop(p, spaces, space);
        space = b.GetSpaceByCoordinates(originCoordinates + new Vector2(0, -1));
        SpaceValidationNonLoop(p, spaces, space);
        space = b.GetSpaceByCoordinates(originCoordinates + new Vector2(-1, -1));
        SpaceValidationNonLoop(p, spaces, space);
        space = b.GetSpaceByCoordinates(originCoordinates + new Vector2(-1, 0));
        SpaceValidationNonLoop(p, spaces, space);
        space = b.GetSpaceByCoordinates(originCoordinates + new Vector2(-1, 1));
        SpaceValidationNonLoop(p, spaces, space);

        if (p.GetPieceType() == 5 && p.IsFirstMove())
        {
            var rook = b.GetSpaceByCoordinates(0, (int)p.transform.position.y).GetPiece();
            if (rook != null && rook.IsFirstMove() && b.GetSpaceByCoordinates(1, (int)p.transform.position.y).GetPiece() == null && b.GetSpaceByCoordinates(3, (int)p.transform.position.y).GetPiece() == null)
            {
                space = b.GetSpaceByCoordinates(2, (int)p.transform.position.y);
                SpaceValidationNonLoop(p, spaces, space);
            }
            rook = b.GetSpaceByCoordinates(7, (int)p.transform.position.y).GetPiece();
            if (rook != null && rook.IsFirstMove() && b.GetSpaceByCoordinates(5, (int)p.transform.position.y).GetPiece() == null)
            {
                space = b.GetSpaceByCoordinates(6, (int)p.transform.position.y);
                SpaceValidationNonLoop(p, spaces, space);
            }
        }

        return spaces;
    }


    private static bool SpaceValidationNonPawnFromLoop(Piece p, List<Space> spaces, bool exit, Space space)
    {
        if (space != null && space.GetPiece() is null) spaces.Add(space);
        if (space != null && space.GetPiece() != null && space.GetPiece().GetTeam() != p.GetTeam()) { spaces.Add(space); exit = true; }
        if (space != null && space.GetPiece() != null && space.GetPiece().GetTeam() == p.GetTeam()) { exit = true; }
        if (space is null) exit = true;
        return exit;
    }

    private static void SpaceValidationNonLoop(Piece p, List<Space> spaces, Space space)
    {
        if (space != null && space.GetPiece() is null) spaces.Add(space);
        if (space != null && space.GetPiece() != null && space.GetPiece().GetTeam() != p.GetTeam()) spaces.Add(space);
    }

    
}
