using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] Vector2 boardSize = new Vector2(8, 8);
    [SerializeField] Space spacePrefab;


    List<List<Space>> boardSpaces;


    List<Space> interactableSpaces = new List<Space>();

    public void Construct()
    {
        boardSpaces = new List<List<Space>>();

        for (int i = 0; i < boardSize.x; i++)
        {
            boardSpaces.Add(new List<Space>());
            for (int j = 0; j < boardSize.y; j++)
            {
                boardSpaces[i].Add(Instantiate(spacePrefab, new Vector2(i, j), Quaternion.identity, transform));
            }
        }
    }

    public Vector2 FindSpaceCoordinates(Space s)
    {
        for (int i = 0; i < boardSize.x; i++)
        {
            if (boardSpaces[i].Contains(s))
            {
                return new Vector2(i, boardSpaces[i].IndexOf(s));
            }
        }

        return -Vector2.one;
    }

    public Space GetSpaceByCoordinates(Vector2 v2)
    {
        if (v2.x >= 0 && v2.x < boardSize.x && v2.y >= 0 && v2.y < boardSize.y)
            return boardSpaces[(int)v2.x][(int)v2.y];
        return null;
    }
    public Space GetSpaceByCoordinates(int x, int y)
    {
        if (x < 0 || y < 0 || x >= boardSize.x || y >= boardSize.y) return null;
        return boardSpaces[x][y];
    }
    public List<Space> GetSpacesInRadius(Space s, int radius)
    {
        List<Space> list = new List<Space>();
        for (int i = (int)s.transform.position.x - radius; i <= (int)s.transform.position.x + radius; i++)
        {
            for (int j = (int)s.transform.position.y - radius; j <= (int)s.transform.position.y + radius; j++)
            {
                var space = GetSpaceByCoordinates(i, j);
                if (space != null && space != s)
                {
                    list.Add(space);
                }
            }
        }
        return list;
    }

    public void DetermineInteractableSpaces(Piece p)
    {
        Space origin = p.GetSpace();
        interactableSpaces.Add(origin);
        interactableSpaces.AddRange(p.FindMoves(this));

        foreach (Space s in interactableSpaces)
        {
            if (s == origin) s.OverlayCancel();
            if ((s.GetPiece() != null || (p.GetPieceType() == 0 && s.GetMarker() != null && s.GetMarker().GetPawn() != p)) && s != origin) s.OverlayCapture();
            if (s.GetPiece() == null && s.GetMarker() == null) s.OverlayMove();
            if (p.GetPieceType() == 0 && s.GetMarker() != null && s.GetMarker().GetPawn() == p) s.OverlayMove();
        }
    }

    public List<Space> GetInteractableSpaces()
    {
        return interactableSpaces;
    }

    public void ClearInteractableSpaces()
    {
        foreach (Space s in interactableSpaces)
        {
            s.ClearOverlays();
        }

        interactableSpaces.Clear();
    }
}
