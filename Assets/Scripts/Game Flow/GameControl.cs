using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    Board board;
    Pieces pieces;

    [SerializeField] List<ScriptableObject> gameFlowObjects;
    List<IGameFlowObject> _gameFlowObjects = new List<IGameFlowObject>();
    int gameFlowIndex = 0;

    private void Awake()
    {
        board = FindObjectOfType<Board>();
        pieces = FindObjectOfType<Pieces>();
        gameFlowIndex = 0;

        foreach (ScriptableObject g in gameFlowObjects)
        {
            if (g is IGameFlowObject)
            {
                _gameFlowObjects.Add((IGameFlowObject)g);

            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {

        if (FindObjectOfType<GameSettings>().FlipCamera) { Camera.main.transform.position = new Vector3(7, 4, -10); Camera.main.transform.rotation = Quaternion.Euler(0, 0, 180); }

            foreach (IGameFlowObject g in _gameFlowObjects)
        {
            g.Initialize();
        }

        board.Construct();
        pieces.Spawn();
        _gameFlowObjects[0].StartingAction(this);


    }
    

    public void Step(IGameFlowObject callingObject)
    {
        if (callingObject == _gameFlowObjects[gameFlowIndex])
        {
            gameFlowIndex++;
            if (gameFlowIndex >= _gameFlowObjects.Count) gameFlowIndex = 0;
            _gameFlowObjects[gameFlowIndex].StartingAction(this);
        }
    }

    public PlayerSwitcher GetPlayers()
    {
        foreach (ScriptableObject g in gameFlowObjects)
        {
            if (g is PlayerSwitcher)
            {
                return (PlayerSwitcher)g;
            }
        }

        return null;
    }
}
