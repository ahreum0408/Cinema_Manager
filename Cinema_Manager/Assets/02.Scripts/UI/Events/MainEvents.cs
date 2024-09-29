using System;
using UnityEngine;

public class MainEvents {
    // 창 띄우기
    public static Action ShowSettingWindowEvent;
    public static Action ShowStoreWindowEvent;

    // 각 재화 수치 변경 시 호출
    public static Action<int> ChangeCoinEvent;
    public static Action<int> ChangeGamEvent;

    public static Action MainViewShow;
    public static Action SettingViewShow;
}
