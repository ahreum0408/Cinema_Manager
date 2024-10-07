using System;
using UnityEngine;

public static class SettingEvents{
    // 저장할 값들이 변경
    public static Action UIGameDataChange; // 아직 gamedata가 없음 이거 넘겨야함

    public static Action ShowEvent;
    public static Action<GameData> SettingUpdatedEvent;

    public static Action<GameData> GameDataUpdatEvent;
    public static Action<GameData> GameDataLoadEvent;
}
