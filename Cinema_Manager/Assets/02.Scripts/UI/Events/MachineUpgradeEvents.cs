using System;
using UnityEngine;

public static class MachineUpgradeEvents {
    public static Action ShowEvent;
    public static Action<GameData> MachineUpgradeUpdatedEvent;

    public static Action<GameData> GameDataUpdatEvent;
    public static Action<GameData> GameDataLoadEvent;
}
