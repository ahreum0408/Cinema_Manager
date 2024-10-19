using System;
using UnityEngine;

public class SettingController : MonoBehaviour {
    private GameData _gameData;

    private void OnEnable() {
        SaveManager.GameDataLoadedEvent += GameDataLoad;
        SettingEvents.GameDataUpdatEvent += GameDataUpgdate;
    }
    private void OnDisable() {
        SaveManager.GameDataLoadedEvent -= GameDataLoad;
        SettingEvents.GameDataUpdatEvent -= GameDataUpgdate;
    }

    private void GameDataLoad(GameData data) {
        if (data == null) {
            return;
        }
        _gameData = data;

        SettingEvents.GameDataLoadEvent?.Invoke(_gameData);
    }
    private void GameDataUpgdate(GameData data) {
        if (data == null) {
            return;
        }
        _gameData = data;
    }
}
