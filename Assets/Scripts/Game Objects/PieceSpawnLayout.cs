using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PieceLayout", menuName = "ScriptableObjects/PieceSpawnLayout", order = 1)]
public class PieceSpawnLayout : ScriptableObject
{
    public int Length { get { return pieces.Count; }  }

    [SerializeField] public List<Piece> pieces;
    [SerializeField] public List<Vector2> positions;
}