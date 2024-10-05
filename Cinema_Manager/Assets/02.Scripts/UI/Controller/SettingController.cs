using System;
using UnityEngine;

public class SettingController : MonoBehaviour {
    private GameData _gameData;

    private void OnEnable() {
        SaveManager.GameDataLoadedEvent += GameDataLoad;
        SettingEvents.GameDataUpdatEvent += SettingUpdate;
    }
    private void OnDisable() {
        SaveManager.GameDataLoadedEvent -= GameDataLoad;
        SettingEvents.GameDataUpdatEvent -= SettingUpdate;
    }

    private void GameDataLoad(GameData data) {
        if (data == null) {
            return;
        }
        _gameData = data;

        SettingEvents.GameDataLoadEvent?.Invoke(_gameData);
    }
    private void SettingUpdate(GameData data) {
        if (data == null) {
            return;
        }
        _gameData = data;
        SettingEvents.SettingUpdatedEvent?.Invoke(_gameData);
    }
}
