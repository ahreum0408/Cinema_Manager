using UnityEngine;

public class MainController : MonoBehaviour {

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

        MainEvents.GameDataLoadEvent?.Invoke(data);
    }
}
