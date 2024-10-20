using UnityEngine;

public class MainController : MonoBehaviour {
    private GameData _gameData;

    private void OnEnable() {
        SaveManager.GameDataLoadedEvent += GameDataLoad;
        MainEvents.GameDataUpdatEvent += GameDataUpdate;
    }
    private void OnDisable() {
        SaveManager.GameDataLoadedEvent -= GameDataLoad;
        MainEvents.GameDataUpdatEvent -= GameDataUpdate;
    }

    private void GameDataLoad(GameData data) {
        if (data == null) {
            return;
        }
        _gameData = data;
        MainEvents.GameDataLoadEvent?.Invoke(_gameData);
    }
    private void GameDataUpdate(GameData data) {
        if (data == null) {
            return;
        }
        _gameData = data;
    }
}
