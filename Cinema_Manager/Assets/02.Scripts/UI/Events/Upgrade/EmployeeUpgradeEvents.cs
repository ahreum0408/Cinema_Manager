using System;

public static class EmployeeUpgradeEvents
{
    public static Action<GameData> GameDataUpdatEvent;
    public static Action<GameData> GameDataLoadEvent;
}
