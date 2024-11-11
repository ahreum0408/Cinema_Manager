using UnityEngine;

public class LevelUpController : MonoBehaviour {
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

        LevelUpEvents.GameDataLoadEvent?.Invoke(data);
    }
}
