using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;


[RequireComponent(typeof(GameDataManager))]
public class SaveManager : MonoBehaviour {
    public static event Action<GameData> GameDataLoadedEvent;

    [SerializeField] private string _saveFilename = "savegame.dat";

    private GameDataManager gameDataManager;

    void Awake() {
        gameDataManager = GetComponent<GameDataManager>();
    }
    void OnApplicationQuit() {
        SaveGameData();
    }

    void OnEnable() {
        SettingEvents.ShowEvent += SettingViewShown;
        SettingEvents.SettingUpdatedEvent += SettingViewUpdated;
    }

    void OnDisable() {
        SettingEvents.ShowEvent -= SettingViewShown;
        SettingEvents.SettingUpdatedEvent -= SettingViewUpdated;
    }
    public GameData NewData() {
        return new GameData();
    }

    public void LoadGame() {
        if (gameDataManager.GameData == null) {
            gameDataManager.GameData = NewData();
        }
        else if (FileManager.LoadFromFile(_saveFilename, out var jsonString)){ // jsonString : 데이터 내용
            gameDataManager.GameData.LoadJson(jsonString);
        }

        if (gameDataManager.GameData != null) {
            GameDataLoadedEvent?.Invoke(gameDataManager.GameData);
        }
    }

    public void SaveGameData() {
        string jsonFile = gameDataManager.GameData.ToJson();
        FileManager.WriteToFile(_saveFilename, jsonFile);
    }
    void SettingViewShown() {
        if (gameDataManager.GameData != null) {
            GameDataLoadedEvent?.Invoke(gameDataManager.GameData);
        }
    }
    void SettingViewUpdated(GameData gameData) {
        gameDataManager.GameData = gameData;
        SaveGameData();
    }
}
