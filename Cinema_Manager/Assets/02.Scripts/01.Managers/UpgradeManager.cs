using System;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour {
    public int[] playerMoveSpeedStat = new int[5];
    public int[] playerVolumeStat = new int[5];
    public float[] playerSellingcostStat = new float[5];

    public int[] employeeMoveSpeedStat = new int[5];
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

        // 직원 전체에 접근해서 각각의 값에 data에 따른 각 스텟 넣어주기
    }
    private void SettingPlayerStat() {
        float costWeight = playerSellingcostStat[_gameData.p_sellingcostLevel];
        int speed = playerMoveSpeedStat[_gameData.p_movespeedLevel];
        int stack = playerVolumeStat[_gameData.p_volumeLevel];
        _player.SetPlayerStat(costWeight, speed, stack);
    }
}
