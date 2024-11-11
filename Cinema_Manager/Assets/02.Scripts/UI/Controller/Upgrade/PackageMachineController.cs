using UnityEngine;

public class PackageMachineController : MonoBehaviour {
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

        PackageMachineUpgradeEvents.GameDataLoadEvent?.Invoke(data);
    }
}
