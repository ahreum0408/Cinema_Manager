using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerUpgradeController : MonoBehaviour{

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

        PlayerUpgradeEvents.GameDataLoadEvent?.Invoke(data);
    }
}