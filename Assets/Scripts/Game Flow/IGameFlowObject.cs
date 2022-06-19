using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameFlowObject
{
    void StartingAction(GameControl g);

    void Initialize();
}
