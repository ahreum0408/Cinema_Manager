using UnityEngine;

public class UpgradeController : MonoBehaviour {
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

        UpgradeEvents.GameDataLoadEvent?.Invoke(data);
    }
}
