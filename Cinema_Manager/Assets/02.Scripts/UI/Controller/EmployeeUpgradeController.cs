using System;
using UnityEngine;

public class EmployeeUpgradeController : MonoBehaviour {
    private GameData _gameData;

    private void OnEnable() {
        SaveManager.GameDataLoadedEvent += GameDataLoad;
        EmployeeUpgradeEvents.GameDataUpdatEvent += GameDataUpdate;
    }
    private void OnDisable() {
        SaveManager.GameDataLoadedEvent -= GameDataLoad;
        EmployeeUpgradeEvents.GameDataUpdatEvent -= GameDataUpdate;
    }
    private void GameDataLoad(GameData data) {
        if (data == null) {
            return;
        }
        _gameData = data;

        EmployeeUpgradeEvents.GameDataLoadEvent?.Invoke(_gameData);
    }
    private void GameDataUpdate(GameData data) {
        if (data == null) {
            return;
        }
        _gameData = data;
    }
}
