using System;
using UnityEngine;

public class GameDataManager : MonoBehaviour {
    [SerializeField] private GameData _gameData;
    public GameData GameData { set => _gameData = value; get => _gameData; }

    private void OnEnable() {
        SettingEvents.GameDataUpdatEvent += SettingDataUpdate;
        PlayerUpgradeEvents.GameDataUpdatEvent += PlayerDataUpdate;
        MachineUpgradeEvents.GameDataUpdatEvent += MachineDataUpdate;
    }
    private void OnDisable() {
        SettingEvents.GameDataUpdatEvent -= SettingDataUpdate;
        PlayerUpgradeEvents.GameDataUpdatEvent -= PlayerDataUpdate;
        MachineUpgradeEvents.GameDataUpdatEvent -= MachineDataUpdate;
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
    }
    private void MachineDataUpdate(GameData data) {
        if(data == null) {
            return;
        }

        _gameData.m_productionspeedLevel = data.m_productionspeedLevel;
        _gameData.m_volumeLevel = data.m_volumeLevel;
        _gameData.m_storageLevel = data.m_storageLevel;
    }
}
