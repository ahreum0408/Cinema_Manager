using System;
using UnityEngine;

public static class MainEvents {
    // 창 띄우기
    public static Action ShowSettingWindowEvent;
    public static Action ShowStoreWindowEvent;

    // 각 재화 수치 변경 시 호출
    public static Action<int> ChangeCoinEvent;
    public static Action<int> ChangeGamEvent;

    public static Action MainViewShow;
    public static Action SettingViewShow;
    public static Action EmployeeUpgradeViewShow;
    public static Action MachineUpgradeViewShow;

    // 각 view에서 사용
    public static Action ShowViewEvent;
    public static Action<GameData> UpdateViewEvent;
}
