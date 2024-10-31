using System;
using UnityEngine;

public static class UpgradeEvents {
    public static Action<GameData> GameDataUpdatEvent;
    public static Action<GameData> GameDataLoadEvent;

    public static Action<GameData> ChangePlayerDataEvent;
    public static Action<GameData> ChangeEmployeeDataEvent;
}
