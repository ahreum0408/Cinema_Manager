using System;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;


[RequireComponent(typeof(GameDataManager))]
public class SaveManager : MonoBehaviour {
    public static event Action<GameData> GameDataLoadedEvent;

    [SerializeField] private string _saveFilename = "savegame.dat";

    private GameDataManager gameDataManager;

    void Awake() {
        gameDataManager = GetComponent<GameDataManager>();
    }
    public class QuitApplicationUtility {
        public static void MoveAndroidApplicationToBack() {
            AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            activity.Call<bool>("moveTaskToBack", true);
        }
    }
    void OnApplicationQuit() {

        SaveGameData();

    }
    private void OnApplicationFocus(bool focus) {
        if (!focus) {
            SaveGameData();
        }
    }
    private void OnApplicationPause(bool pause) {
        if (pause) {
            SaveGameData();
        }
    }

    void OnEnable() {
        MainEvents.ShowViewEvent += ViewShown;
        MainEvents.UpdateViewEvent += ViewUpdated;
    }

    void OnDisable() {
        MainEvents.ShowViewEvent -= ViewShown;
        MainEvents.UpdateViewEvent -= ViewUpdated;
    }
    public GameData NewData() {
        return new GameData();
    }

    public void LoadGame() {
        if (gameDataManager.GameData == null) {
            gameDataManager.GameData = NewData();
        }
        if (FileManager.LoadFromFile(_saveFilename, out var jsonString)) {
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

    #region handle
    void ViewShown()
    {
        if (gameDataManager.GameData != null)
        {
            //GameDataLoadedEvent?.Invoke(gameDataManager.GameData);
        }
    }
    // 이거 지금 안씀
    void ViewUpdated(GameData gameData)
    {
        gameDataManager.GameData = gameData;
    }
    #endregion
}
