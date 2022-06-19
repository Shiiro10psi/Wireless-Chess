using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieces : MonoBehaviour
{
    [SerializeField] PieceSpawnLayout layout;
    [SerializeField] PieceSprites sprites;

    List<Piece> pieces = new List<Piece>();
    
    public void Spawn()
    {
        Board board = FindObjectOfType<Board>();

        for (int i = 0;  i < layout.Length; i++)
        {
            Piece p = Instantiate(layout.pieces[i], transform);
            p.SetInitialSpace(board.GetSpaceByCoordinates(layout.positions[i]));
            pieces.Add(p);
        }
    }

    public void Remove(Piece p)
    {
        if (pieces.Contains(p)) pieces.Remove(p);
    }

    public List<Piece> GetPiecesByType(params int[] TypeID)
    {
        List<int> _TypeID = new List<int>(TypeID);

        List<Piece> filtered = new List<Piece>();

        foreach (Piece p in pieces)
        {
            if (_TypeID.Contains(p.GetPieceType())) filtered.Add(p);
        }

        return filtered;
    }

    public List<Piece> GetPiecesByTeam(params int[] TeamID)
    {
        List<int> _TeamID = new List<int>(TeamID);

        List<Piece> filtered = new List<Piece>();

        foreach (Piece p in pieces)
        {
            if (_TeamID.Contains(p.GetTeam())) filtered.Add(p);
        }

        return filtered;
    }

    public Sprite GetSprite(int TypeID)
    {
        return sprites.sprites[TypeID];
    }

    public List<Piece> GetAllPieces()
    {
        var list = new List<Piece>();
        list.AddRange(pieces);
        return list;
    }
}
