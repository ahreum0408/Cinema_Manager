using System;
using UnityEngine;

public class MachineUpgradeController : MonoBehaviour {
    private GameData _gameData;
    private void OnEnable() {
        SaveManager.GameDataLoadedEvent += GameDataLoad;
        MachineUpgradeEvents.GameDataUpdatEvent += GameDataUpdate;
    }

    private void OnDisable() {
        SaveManager.GameDataLoadedEvent -= GameDataLoad;
        MachineUpgradeEvents.GameDataUpdatEvent -= GameDataUpdate;
    }
    private void GameDataLoad(GameData data) {
        if (data == null) {
            return;
        }
        _gameData = data;

        MachineUpgradeEvents.GameDataLoadEvent?.Invoke(_gameData);
    }
    private void GameDataUpdate(GameData data) {
        if (data == null) {
            return;
        }
        _gameData = data;

        MachineUpgradeEvents.GameDataLoadEvent?.Invoke(_gameData);
    }
}
