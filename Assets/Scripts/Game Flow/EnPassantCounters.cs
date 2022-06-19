using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnPassantCounter", menuName = "ScriptableObjects/GameFlow/EnPassantCounter", order = 4)]
public class EnPassantCounters : ScriptableObject, IGameFlowObject
{
    public void Initialize()
    {
    }

    public void StartingAction(GameControl g)
    {
        var list = FindObjectsOfType<EnPassantMarker>();
        foreach(EnPassantMarker e in list)
        {
            e.Step();
        }

        g.Step(this);
    }

    
}
