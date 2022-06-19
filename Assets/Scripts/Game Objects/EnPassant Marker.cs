using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnPassantMarker : MonoBehaviour
{
    Piece pawn;
    Space space;
    int team = 0;
    int counter = 0;

    public void SetPawn(Piece p)
    {
        pawn = p;
    }

    public Piece GetPawn() { return pawn; }

    public void Step()
    {
        counter++;
        if (counter >= 2) { space.SetMarker(null); Destroy(gameObject); }
    }

    public void Capture()
    {
        pawn.EnPassantCapture();
        space.SetMarker(null);
        Destroy(gameObject);
    }

    public void SetSpace(Space s)
    {
        space = s;
        s.SetMarker(this);
    }

    public void SetTeam(int t)
    {
        team = t;
    }

    public int GetTeam() { return team; }
}
