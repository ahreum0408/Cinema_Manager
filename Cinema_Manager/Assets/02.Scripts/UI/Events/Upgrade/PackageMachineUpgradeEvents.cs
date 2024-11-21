using System;

public static class PackageMachineUpgradeEvents
{
    public static Action<GameData> GameDataUpdatEvent;
    public static Action<GameData> GameDataLoadEvent;
}
