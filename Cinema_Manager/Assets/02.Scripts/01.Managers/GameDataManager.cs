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
        MachineUpgradeEvents.GameDataUpdatEvent += MachineDataUpdate;
        EmployeeUpgradeEvents.GameDataUpdatEvent += EmpolyeeDataUpdate;
    }
    private void OnDisable() {
        LevelEvents.GameDataUpdatEvent -= LevelDataUpdate;
        MainEvents.GameDataUpdatEvent -= MainDataUpdate;
        SettingEvents.GameDataUpdatEvent -= SettingDataUpdate;
        PlayerUpgradeEvents.GameDataUpdatEvent -= PlayerDataUpdate;
        MachineUpgradeEvents.GameDataUpdatEvent -= MachineDataUpdate;
        EmployeeUpgradeEvents.GameDataUpdatEvent -= EmpolyeeDataUpdate;
    }
    private void LevelDataUpdate(GameData data) {
        if (data == null) {
            return;
        }

        _gameData.allCheckPriceList = data.allCheckPriceList;
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

        _gameData.bgmValue = data.bgmValue;
        _gameData.effectValue = data.effectValue;
    }
    private void PlayerDataUpdate(GameData data) {
        if (data == null) {
            return;
        }

        _gameData.p_movespeedLevel = data.p_movespeedLevel;
        _gameData.p_volumeLevel = data.p_volumeLevel;
        _gameData.p_sellingcostLevel = data.p_sellingcostLevel;

        PlayerUpgradeEvents.GameDataLoadEvent?.Invoke(data);
    }
    private void MachineDataUpdate(GameData data) {
        if(data == null) {
            return;
        }

        _gameData.m_productionspeedLevel = data.m_productionspeedLevel;
        _gameData.m_volumeLevel = data.m_volumeLevel;
        _gameData.m_storageLevel = data.m_storageLevel;
    }
    private void EmpolyeeDataUpdate(GameData data) {
        if (data == null) {
            return;
        }

        _gameData.e_movespeedLevel = data.e_movespeedLevel;
        _gameData.e_volumeLevel = data.e_volumeLevel;
        _gameData.e_employmentLevel = data.e_employmentLevel;
    }
}
