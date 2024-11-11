using System;
using UnityEngine;

public class TruckMachineUpgradeController : MonoBehaviour {
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

        TruckMachineUpgradeEvents.GameDataLoadEvent?.Invoke(data);
    }
}
