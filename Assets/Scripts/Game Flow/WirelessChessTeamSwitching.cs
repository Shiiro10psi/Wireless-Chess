using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "WirelessChessTeamSwitching" , menuName = "ScriptableObjects/GameFlow/WirelessChessTeamSwitching", order = 4)]
public class WirelessChessTeamSwitching : ScriptableObject, IGameFlowObject
{
    GameControl gc;
    Recursor r;

    public void Initialize()
    {
    }

    public void StartingAction(GameControl g)
    {
        gc = g;
        UIControl ui = FindObjectOfType<UIControl>();
        ui.ChangeTeamPanel(0, "Checking Connections");

        Pieces pieces = FindObjectOfType<Pieces>();
        Board board = FindObjectOfType<Board>();

        List<Piece> allPieces = pieces.GetAllPieces();
        List<WirelessChessPiece> _allPieces = new List<WirelessChessPiece>();
        foreach (Piece p in allPieces)
        {
            if (p is WirelessChessPiece) _allPieces.Add((WirelessChessPiece)p);
        }

        List<WirelessChessPiece> kings = _allPieces.FindAll(p => p.GetPieceType() == 5);
        List<WirelessChessPiece> jammers = _allPieces.FindAll(p => p.GetPieceType() == 6);
        foreach (WirelessChessPiece p in _allPieces)
        {
            if (!kings.Contains(p) && !jammers.Contains(p))
                p.ClearConnection();
        }
        r = new GameObject().AddComponent<Recursor>();
        r.Starter(this, kings,jammers, 0);

        
    }

    public void Callback()
    {
        gc.Step(this);
        if (r != null) Destroy(r.gameObject);
    }

    

    

    private class Recursor : MonoBehaviour
    {
        WirelessChessTeamSwitching parent;

        public void Starter(WirelessChessTeamSwitching p, List<WirelessChessPiece> layer, List<WirelessChessPiece> jammers, int times)
        {
            parent = p;
            StartCoroutine(RecursionWait(layer, jammers, 0));
        }

        public void RecursiveConnection(List<WirelessChessPiece> layer, List<WirelessChessPiece> jammers, int times)
        {
            times++; if (times > 20) { parent.Callback(); return; }

            Board board = FindObjectOfType<Board>();
            List<WirelessChessPiece> newLayer = new List<WirelessChessPiece>();

            foreach (WirelessChessPiece p in layer)
            {
                List<Space> spaces = board.GetSpacesInRadius(p.GetSpace(), p.GetSignalStrength());
                foreach (Space s in spaces)
                {
                    WirelessChessPiece p2 = (WirelessChessPiece)s.GetPiece();
                    if (p2 != null)
                    {
                        bool flag = p2.AddConnection(p);
                        if (flag) newLayer.Add(p2);
                    }
                }
            }

            foreach (WirelessChessPiece j in jammers)
            {
                List<Space> spaces = board.GetSpacesInRadius(j.GetSpace(), j.GetSignalStrength());
                foreach (Space s in spaces)
                {
                    WirelessChessPiece p2 = (WirelessChessPiece)s.GetPiece();
                    if (p2 != null)
                    {
                        switch (j.GetTeam())
                        {
                            case 1:
                                p2.JamConnections(2);
                                break;
                            case 2:
                                p2.JamConnections(1);
                                break;
                        }
                    }
                }
            }

            newLayer = newLayer.Distinct().ToList();

            foreach (WirelessChessPiece p in newLayer)
            {
                p.ResolveConnections();
            }

            StartCoroutine(RecursionWait(newLayer, jammers, times));
        }

        private IEnumerator RecursionWait(List<WirelessChessPiece> layer, List<WirelessChessPiece> jammers, int times)
        {
            yield return new WaitForSeconds(0.5f);

            if (layer.Count > 0) RecursiveConnection(layer, jammers, times);
            if (layer.Count == 0) parent.Callback();
        }
    }
}
