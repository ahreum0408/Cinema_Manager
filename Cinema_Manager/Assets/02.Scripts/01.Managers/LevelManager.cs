using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LevelManager : MonoBehaviour {
    public List<Level> levelDatas = new List<Level>();

    private int _exp;
    private Level _currentLevel;
    private int _levelIndex = 0;

    private GameData _gameData;

    private void Start() {
        MainEvents.GameDataLoadEvent += GameDataLoad;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.A)) {
            GetExp(10);
        }
    }
    public void GetExp(int exp) {
        if(_levelIndex > levelDatas.Count) {
            Debug.LogWarning("현제 최고 레벨에 도달함");
            return;
        }
        _exp += exp;
        MainEvents.GetExpEvent?.Invoke(_exp);

        if (_exp >= _currentLevel.highValue) {
            // 경험치 계산
            int remainingValue = _exp - _currentLevel.highValue;
            _exp = remainingValue;
            MainEvents.GetExpEvent?.Invoke(_exp);
            LevelUp();
        }
    }
    private void LevelUp() {
        _currentLevel = levelDatas[++_levelIndex];
        MainEvents.UpgradeLevelEvent?.Invoke(levelDatas[_levelIndex]);
    }
    private void GameDataLoad(GameData data) {
        if (data == null) {
            return;
        }
        _gameData = data;

        _currentLevel = _gameData.level;
        _exp = _gameData.exp;
    }
}
