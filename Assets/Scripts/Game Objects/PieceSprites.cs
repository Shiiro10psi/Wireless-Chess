using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PieceLayout", menuName = "ScriptableObjects/PieceSprites", order = 2)]
public class PieceSprites : ScriptableObject
{
    public List<Sprite> sprites;
}
