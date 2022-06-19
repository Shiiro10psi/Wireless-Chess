using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WirelessChessPiece : Piece
{
    WirelessChessPiece connection;
    WirelessChessPiece lastConnection;
    int connectionLevel;
    List<WirelessChessPiece> possibleConnections;

    [SerializeField] int signalStrength = 1;

    String resolveString = "";

    override protected void Awake()
    {
        base.Awake();
        possibleConnections = new List<WirelessChessPiece>();

        switch (TypeID)
        {
            case 0:
                signalStrength = 1;
                break;
            case 1:
                signalStrength = 2;
                break;
            case 2:
                signalStrength = 2;
                break;
            case 3:
                signalStrength = 2;
                break;
            case 4:
                signalStrength = 2;
                break;
            case 5:
                signalStrength = 2;
                break;
            case 6:
                signalStrength = 1;
                break;
            default:
                signalStrength = 1;
                break;
        }
    }

    public override void SetPieceType(int ID)
    {
        base.SetPieceType(ID);
        switch(TypeID)
        {
            case 0:
                signalStrength = 1;
                break;
            case 1:
                signalStrength = 2;
                break;
            case 2:
                signalStrength = 2;
                break;
            case 3:
                signalStrength = 2;
                break;
            case 4:
                signalStrength = 2;
                break;
            case 5:
                signalStrength = 3;
                break;
            case 6:
                signalStrength = 1;
                break;
            default:
                signalStrength = 1;
                break;
        }
    }

    public int GetSignalStrength() { return signalStrength; }

    private void SetConnection(WirelessChessPiece p)
    {
        connection = p;

        if (connection is null) { SetTeam(0); return; }
        SetTeam(connection.GetTeam());
    }

    public bool AddConnection(WirelessChessPiece p)
    {
        if (TypeID == 5 || TypeID == 6) return false;
        if (p == lastConnection && connection != lastConnection) { possibleConnections.Add(lastConnection);  return true; }
        if (connection != null) return false;
        possibleConnections.Add(p);
        return true;
    }

    public void JamConnections(int team)
    {
        possibleConnections.RemoveAll(p => p.GetTeam() == team);
    }

    public void ResolveConnections()
    {
        if (TypeID == 5 || TypeID == 6) { resolveString = "Resolved at Layer 1: Non-changing piece.";  possibleConnections.Clear(); return; }
        if (possibleConnections.Contains(lastConnection)) { resolveString = "Resolved at Layer 1: Last Connection Maintained."; SetConnection(lastConnection); possibleConnections.Clear(); return; }
        if (possibleConnections.Count > 1) ResolveLayer2();
        if (possibleConnections.Count == 1) { resolveString = "Resolved at Layer 1: Only Available Connection."; SetConnection(possibleConnections[0]); }
        possibleConnections.Clear();
    }

    private void ResolveLayer2()
    {
        for (int i = 1; i <= 3; i++)
        {
            List<WirelessChessPiece> sublist = possibleConnections.FindAll(p =>
                Mathf.Max(Mathf.Abs(transform.position.x - p.transform.position.x), Mathf.Abs(transform.position.y - p.transform.position.y)) == i);
            if (sublist.Count == 1) { resolveString = "Resolved at Layer 2 (Proximity): Nearest Piece Determined as " + sublist[0].name + " at " +sublist[0].transform.position.x + "," + sublist[0].transform.position.y;
                SetConnection(sublist[0]); return; }
            if (sublist.Count > 1) { ResolveLayer3(sublist); return; }
            if (sublist.Count < 1) continue;
        }
    }

    private void ResolveLayer3(List<WirelessChessPiece> list)
    {
        for (int i = 5; i > 0; i--)
        {
            List<WirelessChessPiece> sublist = list.FindAll(p => GetPieceType() == i);
            if (sublist.Count == 1) {
                resolveString = "Resolved at Layer 3 (Piece Type): Highest Rank Piece Determined as" + sublist[0].name;
                SetConnection(sublist[0]); return; }
            if (sublist.Count > 1) { ResolveLayer4(sublist); return; }
            if (sublist.Count < 1) continue;
        }

        List<WirelessChessPiece> sublist2 = list.FindAll(p => GetPieceType() == 0 || GetPieceType() == 6);
        if (sublist2.Count == 1) { resolveString = "Resolved at Layer 3 (Piece Type): Highest Rank Piece Determined as" + sublist2[0].name; SetConnection(sublist2[0]); return; }
        if (sublist2.Count > 1) { ResolveLayer4(sublist2); return; }
        if (sublist2.Count < 1) return;
    }

    private void ResolveLayer4(List<WirelessChessPiece> list)
    {
        for (int i = 0; i < 8; i++)
        {
            List<WirelessChessPiece> sublist = list.FindAll(p => (int)p.transform.position.x == i);
            if (sublist.Count == 1) { resolveString = "Resolved at Layer 4 (Leftmost): Leftmost Piece Determined as " + sublist[0].name + " at " + sublist[0].transform.position.x + "," + sublist[0].transform.position.y;
                SetConnection(sublist[0]); return; }
            if (sublist.Count > 1) { ResolveLayer5(sublist); return; }
            if (sublist.Count < 1) continue;
        }
    }

    private void ResolveLayer5(List<WirelessChessPiece> list)
    {
        for (int i = 0; i < 8; i++)
        {
            List<WirelessChessPiece> sublist = list.FindAll(p => (int)p.transform.position.y == i);
            if (sublist.Count == 1) { resolveString = "Resolved at Layer 5 (Bottommost): Bottommost Piece Determined as " + sublist[0].name + " at " + sublist[0].transform.position.x + "," + sublist[0].transform.position.y;
                SetConnection(sublist[0]); return; }
            if (sublist.Count < 1) continue;
        }
    }

    public void ClearConnection()
    {
        lastConnection = connection;
        SetConnection(null);
    }

    public void Highlight()
    {
        Highlight(0);
    }

    public void Highlight(int state)
    {
        m.SetInteger("_ShowConnectionGlow", state);
        if (connection != null) connection.Highlight(state, 0);
    }

    public void Highlight(int state, int count)
    {
        if (count >= 20) return;
        m.SetInteger("_ShowConnectionGlow", state);
        if (connection != null) connection.Highlight(state, count+ 1);
    }

    public void DebugHighlight()
    {
        Highlight(1);
        Invoke("Highlight", 1f);
    }


    public void DebugLogResolveString()
    {
        FindObjectOfType<UIControl>().LogAppend(resolveString);
    }
}
