using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public List<Level> levelDatas = new List<Level>();

    private List<CheckerArea> allAreas = new List<CheckerArea>();   
    private List<DisplayStand> allStand = new List<DisplayStand>();   

    private Level _currentLevel;
    private int _exp;
    private int _levelIndex = 0;

    private GameData _gameData;

    private void Awake() {
        foreach (var levelData in levelDatas) {
            levelData.SetActiveMap(false);
        }
        foreach (var levelData in levelDatas) {
            foreach (var openMap in levelData.openNewMapList) {
                allAreas.Add(openMap);
            }
        }
        foreach (var levelData in allAreas) {
            var furniture = levelData as BuyChecker;
            allStand.Add(furniture.OpenTarget);
        }
        LevelEvents.GameDataLoadEvent += GameDataLoad;
        LevelEvents.ChangePriceEvent += ChangeCheckerPrice;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.A)) {
            GetExp(10);
        }
    }
    public void GetExp(int exp) {
        if(_levelIndex >= levelDatas.Count - 1 && _exp >= _currentLevel.highValue) {
            Debug.LogWarning("현제 최고 레벨에 도달함");
            return;
        }

        _exp += exp;
        MainEvents.GetExpEvent?.Invoke(_exp);
        if (_levelIndex < levelDatas.Count - 1 && _exp >= _currentLevel.highValue) {
            // 경험치 계산
            int remainingValue = _exp - _currentLevel.highValue;
            _exp = remainingValue;
            MainEvents.GetExpEvent?.Invoke(_exp);
            LevelUp();
        }
    }
    private void LevelUp() {
        _currentLevel = levelDatas[++_levelIndex];
        MainEvents.UpgradeLevelEvent?.Invoke(levelDatas[_levelIndex], _levelIndex);
        _currentLevel.SetActiveMap(true); // 다음 스테이지 켜주고
    }
    private void GameDataLoad(GameData data) {
        if (data == null) {
            return;
        }

        _gameData = data;
        _currentLevel = _gameData.level;
        _levelIndex = _gameData.levelIndex;
        _exp = _gameData.exp;

        // 켜져야 하는 거는 켜주고
        for (int i = 0; i <= _levelIndex; i++) {
            levelDatas[i].SetActiveMap(true);
        }
        // 체커의 가격도 맞춰주고 가격에 따라서 stand도 켜줌
        for(int i = 0; i < allAreas.Count; i++){
            var checker = allAreas[i] as BuyChecker;
            if(checker != null){
                checker.Price = _gameData.allCheckPriceList[i];
                if(checker.Price == 0) {
                    if (allStand[i] != null) {
                        checker.gameObject.SetActive(false);
                        allStand[i].SetAvticeGameObject(true);
                    }
                }
            }
        }
    }
    private void ChangeCheckerPrice(BuyChecker checker, int price) {
        for(int i = 0; i < allAreas.Count; i++) {
            if (allAreas[i] == checker) {
                _gameData.allCheckPriceList[i] = price;
                LevelEvents.GameDataUpdatEvent?.Invoke(_gameData);
            }
        }
    }
}
