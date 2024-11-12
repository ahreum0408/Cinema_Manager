using System;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour {
    public float[] playerMoveSpeedStat = new float[5];
    public int[] playerVolumeStat = new int[5];
    public float[] playerSellingcostStat = new float[5];

    public float[] employeeMoveSpeedStat = new float[5];
    public int[] employeeVolumeStat = new int[5];
    public int[] employeeAddStat = new int[5];

    private GameData _gameData;

    private PlayerController _player;

    private void Awake() {
        _player = FindObjectOfType<PlayerController>();
    }
    private void OnEnable() {
        UpgradeEvents.GameDataLoadEvent += GameDataLoad;
        UpgradeEvents.ChangePlayerDataEvent += PlayerDataChange;
        UpgradeEvents.ChangeEmployeeDataEvent += EmployeeDataChange;
    }
    private void OnDisable() {
        UpgradeEvents.GameDataLoadEvent -= GameDataLoad;
        UpgradeEvents.ChangePlayerDataEvent -= PlayerDataChange;
        UpgradeEvents.ChangeEmployeeDataEvent -= EmployeeDataChange;
    }
    private void GameDataLoad(GameData data) {
        if (data == null) {
            return;
        }
        _gameData = data;

        SettingPlayerStat();
        SettingEmployeeStat();
    }
    private void PlayerDataChange(GameData data) {
        if (data == null) {
            return;
        }
        _gameData = data;

        SettingPlayerStat();
    }
    private void EmployeeDataChange(GameData data) {
        if (data == null) {
            return;
        }
        _gameData = data;

        SettingEmployeeStat();
    }
    private void SettingPlayerStat() {
        float costWeight = playerSellingcostStat[_gameData.p_sellingcostLevel];
        float speed = playerMoveSpeedStat[_gameData.p_movespeedLevel];
        int stack = playerVolumeStat[_gameData.p_volumeLevel];

        _player.SetPlayerStat(costWeight, speed, stack);
    }

    private void SettingEmployeeStat() {
        float speed = employeeMoveSpeedStat[_gameData.e_movespeedLevel];
        int stack = employeeVolumeStat[_gameData.e_volumeLevel];
        int employeeCount = employeeAddStat[_gameData.e_employmentLevel];

        StaffManager.Instance.SetStaffStat(speed, stack, employeeCount);
    }
}
