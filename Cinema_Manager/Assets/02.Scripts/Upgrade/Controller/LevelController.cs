using UnityEngine;

public class LevelController : MonoBehaviour {

    private void OnEnable() {
        SaveManager.GameDataLoadedEvent += GameDataLoad;
    }
    private void OnDisable() {
        SaveManager.GameDataLoadedEvent -= GameDataLoad;
    }
    private void GameDataLoad(GameData data) {
        if (data == null) {
            return;
        }

        LevelEvents.GameDataLoadEvent?.Invoke(data);
    }
}