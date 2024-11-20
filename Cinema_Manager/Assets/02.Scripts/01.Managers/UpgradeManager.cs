using System;
using TMPro;
using UnityEngine;

public enum UpgradeTarget {
    playerMoveSpeedStat,
    playerVolumeStat,
    playerSellingcostStat,
    employeeMoveSpeedStat, 
    employeeVolumeVolumeStat,
    employeeAddStat
}
public class UpgradeManager : MonoSingleton<UpgradeManager> {
    [Header("Player")]
    public float[] playerMoveSpeedStat = new float[5];
    public int[] playerVolumeStat = new int[5];
    public float[] playerSellingcostStat = new float[5];

    [Header("Player Upgrade Price")]
    public int[] playerUpgradePrice = new int[5];

    [Header("Empolyee")]
    public float[] employeeMoveSpeedStat = new float[5];
    public int[] employeeVolumeStat = new int[5];
    public int[] employeeAddStat = new int[5];

    [Header("Empolyee Upgrade Price")]
    public int[] employeeUpgradePrice = new int[5];

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
    public void FindDataAndCalculate(UpgradeTarget target) {
        int minusCoin = FindMatchData(target, 1);
        CalcualteMinusCoin(minusCoin);
    }
    private void CalcualteMinusCoin(int coin) {
        CoinManager.Instance.Coin -= coin;
    }
    public bool CanUpgrade(UpgradeTarget target) {
        int minusCoin = FindMatchData(target);
        if (CoinManager.Instance.Coin - minusCoin < 0) {
            return false;
        }
        else {
            return true;
        }
    }
    private int FindMatchData(UpgradeTarget target, int minusIndex = 0) {
        int index = 0, minusCoin = 0;
        switch (target) {
            case UpgradeTarget.playerMoveSpeedStat:
                minusCoin = playerUpgradePrice[_gameData.p_movespeedLevel - minusIndex];
                break;
            case UpgradeTarget.playerVolumeStat:
                minusCoin = playerUpgradePrice[_gameData.p_volumeLevel - minusIndex];
                break;
            case UpgradeTarget.playerSellingcostStat:
                minusCoin = playerUpgradePrice[_gameData.p_sellingcostLevel - minusIndex];
                break;
            case UpgradeTarget.employeeMoveSpeedStat:
                minusCoin = employeeUpgradePrice[_gameData.e_movespeedLevel - minusIndex];
                break;
            case UpgradeTarget.employeeVolumeVolumeStat:
                minusCoin = employeeUpgradePrice[_gameData.e_volumeLevel - minusIndex];
                break;
            case UpgradeTarget.employeeAddStat:
                minusCoin = employeeUpgradePrice[_gameData.e_employmentLevel - minusIndex];
                break;
        }
        return minusCoin;
    }
}
