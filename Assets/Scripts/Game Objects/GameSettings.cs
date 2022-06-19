using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [SerializeField] List<Color> _TeamColors;
    public List<Color> TeamColors { get { return _TeamColors; } private set { _TeamColors = value; } }
    [SerializeField] public List<ScriptableObject> players;
    public bool FlipCamera = false;

    private void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void Pack(List<Color> colors, List<ScriptableObject> playerObjects, bool cam)
    {
        _TeamColors = colors;
        players = playerObjects;
        FlipCamera = cam;
    }
}
