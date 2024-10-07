using System;
using UnityEngine;

public static class SettingEvents{
    public static Action ShowEvent;
    public static Action<GameData> SettingUpdatedEvent;

    public static Action<GameData> GameDataUpdatEvent;
    public static Action<GameData> GameDataLoadEvent;
}
