using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSwitcher", menuName = "ScriptableObjects/GameFlow/PlayerSwitcher", order = 3)]
public class PlayerSwitcher : ScriptableObject, IGameFlowObject
{
    GameControl gc;

    [SerializeField] List<ScriptableObject> Players;
    List<IPlayerControl> _Players = new List<IPlayerControl>();

    int playerIndex = 0;

    public void Initialize()
    {
        playerIndex = 0;

        Players = FindObjectOfType<GameSettings>().players;
        _Players.Clear();

        foreach (ScriptableObject p in Players)
        {
            if (p is IPlayerControl)
            {
                _Players.Add((IPlayerControl)p);
            }
        }
    }

    public void StartingAction(GameControl g)
    { 
        gc = g;
        _Players[playerIndex].StartingAction(this);
    }

    public void Step(IPlayerControl callingObject)
    {
        if (callingObject == _Players[playerIndex])
        {
            playerIndex++;
            if (playerIndex >= _Players.Count) playerIndex = 0;
            gc.Step(this);
        }
    }
    
    public int GetNumberOfHumanPlayers()
    {
        int num = 0;

        foreach (ScriptableObject p in _Players)
        {
            if (p is PlayerMove)
            {
                num++;
            }
        }

        return num;
    }

    public bool IsPlayerHuman(int index)
    {
        if (_Players[index] is PlayerMove)
        {
            return true;
        }
        return false;
    }
    
}
