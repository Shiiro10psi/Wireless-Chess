using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerControl
{
    void StartingAction(PlayerSwitcher g);
    void Callback();
    void ScheduleCallback();
}
