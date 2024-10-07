using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerUpgradeController : MonoBehaviour{
    private GameData _gameData;

    private void OnEnable() {
        SaveManager.GameDataLoadedEvent += GameDataLoad;
        PlayerUpgradeEvents.GameDataUpdatEvent += GameDataUpdate;
    }
    private void OnDisable() {
        SaveManager.GameDataLoadedEvent -= GameDataLoad;
        PlayerUpgradeEvents.GameDataUpdatEvent -= GameDataUpdate;
    }
    private void GameDataLoad(GameData data) {
        if (data == null) {
            return;
        }
        _gameData = data;

        PlayerUpgradeEvents.GameDataLoadEvent?.Invoke(_gameData);
    }
    private void GameDataUpdate(GameData data) {
        if (data == null) {
            return;
        }
        _gameData = data;

        PlayerUpgradeEvents.PlayerUpgradeUpdatedEvent?.Invoke(_gameData);
    }
}