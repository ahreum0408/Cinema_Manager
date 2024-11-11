using UnityEngine;
using UnityEngine.Playables;

public class LevelUpController : MonoBehaviour {
    private GameData _gameData;

    private void OnEnable() {
        SaveManager.GameDataLoadedEvent += GameDataLoad;
        LevelUpEvents.GameDataUpdatEvent += GameDataUpdate;
    }
    private void OnDisable() {
        SaveManager.GameDataLoadedEvent -= GameDataLoad;
        LevelUpEvents.GameDataUpdatEvent -= GameDataUpdate;
    }

    private void GameDataLoad(GameData data) {
        if (data == null) {
            return;
        }
        _gameData = data;
        LevelUpEvents.GameDataLoadEvent?.Invoke(_gameData);
    }
    private void GameDataUpdate(GameData data) {
        if (data == null) {
            return;
        }
        _gameData = data;
        LevelUpEvents.LevelUpUpdate?.Invoke(_gameData);
    }
}
