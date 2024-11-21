using System;

public static class MainEvents
{
    // 기본적으로 가지고 있을거
    public static Action<GameData> GameDataUpdatEvent;
    public static Action<GameData> GameDataLoadEvent;

    // 각 재화 수치 변경 시 호출
    public static Action<string> ChangeCoinEvent;
    public static Action<string> ChangeGamEvent;

    public static Action<Level, int> UpgradeLevelEvent;
    public static Action<int> GetExpEvent;

    // 각 view 관리
    public static Action MainViewShow;
    public static Action SettingViewShow;
    public static Action PlayerUpgradeViewShow;
    public static Action EmployeeUpgradeViewShow;
    public static Action TruckMachineUpgradeViewShow;
    public static Action PackageMachineUpgradeViewShow;
    public static Action LevelUpViewShow;


    // 각 view에서 사용
    public static Action ShowViewEvent;
    public static Action CloseCurrentEvent;
    public static Action<GameData> UpdateViewEvent;
}
