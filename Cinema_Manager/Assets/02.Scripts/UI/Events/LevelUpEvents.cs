using System;

public static class LevelUpEvents
{
    public static Action<GameData> GameDataUpdatEvent;
    public static Action<GameData> GameDataLoadEvent;

    public static Action<GameData> LevelUpUpdate;
    public static Action CloseView;
}
