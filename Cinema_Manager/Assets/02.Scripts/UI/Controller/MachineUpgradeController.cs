using System;
using UnityEngine;

public class MachineUpgradeController : MonoBehaviour {
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

        MachineUpgradeEvents.GameDataLoadEvent?.Invoke(data);
    }
}
