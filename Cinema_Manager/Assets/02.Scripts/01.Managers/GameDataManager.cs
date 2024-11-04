using System;
using UnityEngine;

public class GameDataManager : MonoBehaviour {
    [SerializeField] private GameData _gameData;
    public GameData GameData { set => _gameData = value; get => _gameData; }

    private SaveManager _saveManager;

    private void Awake() {
        _saveManager = GetComponent<SaveManager>();
    }
    private void Start() {
        _saveManager.LoadGame();
    }

    private void OnEnable() {
        LevelEvents.GameDataUpdatEvent += LevelDataUpdate;
        MainEvents.GameDataUpdatEvent += MainDataUpdate;
        SettingEvents.GameDataUpdatEvent += SettingDataUpdate;
        PlayerUpgradeEvents.GameDataUpdatEvent += PlayerDataUpdate;
        TruckMachineUpgradeEvents.GameDataUpdatEvent += TruckMachineDataUpdate;
        PackageMachineUpgradeEvents.GameDataUpdatEvent += PackageMachineDataUpdate;
        EmployeeUpgradeEvents.GameDataUpdatEvent += EmpolyeeDataUpdate;
    }
    private void OnDisable() {
        LevelEvents.GameDataUpdatEvent -= LevelDataUpdate;
        MainEvents.GameDataUpdatEvent -= MainDataUpdate;
        SettingEvents.GameDataUpdatEvent -= SettingDataUpdate;
        PlayerUpgradeEvents.GameDataUpdatEvent -= PlayerDataUpdate;
        TruckMachineUpgradeEvents.GameDataUpdatEvent -= TruckMachineDataUpdate;
        PackageMachineUpgradeEvents.GameDataUpdatEvent -= PackageMachineDataUpdate;
        EmployeeUpgradeEvents.GameDataUpdatEvent -= EmpolyeeDataUpdate;
    }
    private void LevelDataUpdate(GameData data) {
        if (data == null) {
            return;
        }

        _gameData.allCheckOnOffList = data.allCheckOnOffList;
        _gameData.allCheckPriceList = data.allCheckPriceList;

        _gameData.allStandOnOffList = data.allStandOnOffList;
        _gameData.allStandItemCountList = data.allStandItemCountList;

        _gameData.allTruckOnOffList = data.allTruckOnOffList;

        _gameData.allTableOnOffList = data.allTableOnOffList;
    }
    private void MainDataUpdate(GameData data) {
        if (data == null) {
            return;
        }

        _gameData.coin = data.coin;
        _gameData.gam = data.gam;
        _gameData.exp = data.exp;
        _gameData.level = data.level;

    }
    private void SettingDataUpdate(GameData data) {
        if (data == null) { 
            return;
        }

        _gameData.bgm = data.bgm;
        _gameData.effect = data.effect;
        _gameData.haptic = data.haptic;
    }
    private void PlayerDataUpdate(GameData data) {
        if (data == null) {
            return;
        }

        _gameData.p_movespeedLevel = data.p_movespeedLevel;
        _gameData.p_volumeLevel = data.p_volumeLevel;
        _gameData.p_sellingcostLevel = data.p_sellingcostLevel;
        UpgradeEvents.ChangePlayerDataEvent?.Invoke(_gameData);
    }
    private void TruckMachineDataUpdate(GameData data) {
        if(data == null) {
            return;
        }

        _gameData.mt_productionspeedLevel = data.mt_productionspeedLevel;
        _gameData.mt_volumeLevel = data.mt_volumeLevel;
        _gameData.mt_storageLevel = data.mt_storageLevel;
    }
    private void PackageMachineDataUpdate(GameData data) {
        if (data == null) {
            return;
        }

        _gameData.mp_packingspeedLevel = data.mp_packingspeedLevel;
        _gameData.mp_volumeLevel = data.mp_volumeLevel;
    }
    private void EmpolyeeDataUpdate(GameData data) {
        if (data == null) {
            return;
        }

        _gameData.e_movespeedLevel = data.e_movespeedLevel;
        _gameData.e_volumeLevel = data.e_volumeLevel;
        _gameData.e_employmentLevel = data.e_employmentLevel;

        UpgradeEvents.ChangeEmployeeDataEvent?.Invoke(_gameData);
    }
}
