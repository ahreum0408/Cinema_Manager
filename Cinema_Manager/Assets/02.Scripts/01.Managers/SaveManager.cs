using System;
using UnityEngine;


[RequireComponent(typeof(GameDataManager))]
public class SaveManager : MonoBehaviour
{
    public static event Action<GameData> GameDataLoadedEvent;

    [SerializeField] private string _saveFilename = "savegame.dat";

    private GameDataManager gameDataManager;

    void Awake()
    {
        gameDataManager = GetComponent<GameDataManager>();
    }
    void OnApplicationQuit()
    {
        SaveGameData();
    }

    void OnEnable()
    {
        MainEvents.ShowViewEvent += ViewShown;
        MainEvents.UpdateViewEvent += ViewUpdated;
    }

    void OnDisable()
    {
        MainEvents.ShowViewEvent -= ViewShown;
        MainEvents.UpdateViewEvent -= ViewUpdated;
    }
    public GameData NewData()
    {
        return new GameData();
    }

    public void LoadGame()
    {
        if (gameDataManager.GameData == null)
        {
            gameDataManager.GameData = NewData();
        }
        if (FileManager.LoadFromFile(_saveFilename, out var jsonString))
        { // jsonString : 데이터 내용
            gameDataManager.GameData.LoadJson(jsonString);
        }
        if (gameDataManager.GameData != null)
        {
            GameDataLoadedEvent?.Invoke(gameDataManager.GameData);
        }
    }

    public void SaveGameData()
    {
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
