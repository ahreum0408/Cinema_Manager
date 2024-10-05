using System;
using UnityEngine;

public class GameDataManager : MonoBehaviour {
    [SerializeField] private GameData _gameData;
    public GameData GameData { set => _gameData = value; get => _gameData; }

    private void OnEnable() {
        SettingEvents.GameDataUpdatEvent += SettingDataUpdate;
    }
    private void OnDisable() {
        SettingEvents.GameDataUpdatEvent -= SettingDataUpdate;
    }

    private void SettingDataUpdate(GameData data) {
        if (data == null) { 
            return;
        }

        _gameData.bgmValue = data.bgmValue;
        _gameData.effectValue = data.effectValue;
    }
}
