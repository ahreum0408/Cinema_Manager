using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoSingleton<LevelManager> {
    public List<Level> levelDatas = new List<Level>();

    private List<CheckerArea> _allAreas = new List<CheckerArea>();   
    private List<DisplayStand> _allStand = new List<DisplayStand>();   

    private Level _currentLevel;
    private int _exp;
    private int _levelIndex = 0;

    private GameData _gameData;

    protected override void Awake() {
        base.Awake();
        foreach (var levelData in levelDatas) {
            levelData.SetActiveListObj(false);
        }
        foreach (var levelData in levelDatas) {
            foreach (var openMap in levelData.openNewMapList) {
                _allAreas.Add(openMap);
            }
        }
        foreach (var levelData in _allAreas) {
            var furniture = levelData as BuyChecker;
            if(furniture != null) {
                if (furniture.OpenTarget.TryGetComponent<DisplayStand>(out DisplayStand stand)) {
                    _allStand.Add(stand);
                }
            }
            else {
                Debug.Log("stand가 없음");
            }
        }
    }
    private void OnEnable() {
        LevelEvents.GameDataLoadEvent += GameDataLoad;
        LevelEvents.ChangePriceEvent += ChangeCheckerPrice;
        LevelEvents.ChangeDisplayStandEvent += ChangeDisplyStandPrice;
    }
    private void OnDisable() {
        LevelEvents.GameDataLoadEvent -= GameDataLoad;
        LevelEvents.ChangePriceEvent -= ChangeCheckerPrice;
        LevelEvents.ChangeDisplayStandEvent -= ChangeDisplyStandPrice;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.E)) {
            GetExp(10);
            Debug.LogWarning("지금 exp 얻는 곳이 존재하니 주의 할 것");
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
        _currentLevel.SetActiveListObj(true); // 다음 스테이지 켜주고
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
        OnListObj();
        // 체커의 가격도 맞춰주고 가격에 따라서 stand도 켜줌
        SettingCheckerPrice();
    }

    private void OnListObj() {
        for (int i = 0; i <= _levelIndex; i++) {
            levelDatas[i].SetActiveListObj(true);
        }
    }
    private void SettingCheckerPrice() {
        for (int i = 0; i < _allAreas.Count; i++) {
            var checker = _allAreas[i] as BuyChecker;
            if (checker != null) {
                checker.Price = _gameData.allCheckPriceList[i];
                if (checker.Price == 0) { // 이미 해금을 했다 => stand를 켜야한다
                    SettingStandItem(i, checker);
                }
            }
        }
    }
    private void SettingStandItem(int i, BuyChecker checker) {
        if (_allStand[i] != null) {
            checker.gameObject.SetActive(false); // 체커 끄고
            _allStand[i].gameObject.SetActive(true); // 스텐드 키고
            int itemCount = _gameData.allDisplayStandItemCountList[i];
            _allStand[i].AddItemToStand(itemCount);
        }
    }
    private void ChangeCheckerPrice(BuyChecker checker, int price) {
        int index = _allAreas.IndexOf(checker);
        _gameData.allCheckPriceList[index] = price;
        LevelEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
    private void ChangeDisplyStandPrice(DisplayStand stand, int count) {
        int index = _allStand.IndexOf(stand);
        _gameData.allDisplayStandItemCountList[index] = count;
        LevelEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
}
