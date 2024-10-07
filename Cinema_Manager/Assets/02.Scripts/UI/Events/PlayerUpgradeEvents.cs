using System;
using UnityEngine;

public static class PlayerUpgradeEvents {
    public static Action ShowEvent;
    public static Action<GameData> PlayerUpgradeUpdatedEvent;

    public static Action<GameData> GameDataUpdatEvent;
    public static Action<GameData> GameDataLoadEvent;
}
