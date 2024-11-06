using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Debug = UnityEngine.Debug;

public class LevelManager : MonoSingleton<LevelManager> {
    public List<Level> levelDatas = new List<Level>();

    private List<BuyChecker> _allCheckers = new List<BuyChecker>();
    private List<DisplayStand> _allStand = new List<DisplayStand>();
    private List<FoodContainer> _allTruck = new List<FoodContainer>();
    private List<Table> _allTable = new List<Table>();

    private Level _currentLevel;
    private int _exp;
    private int _levelIndex = 0;

    private GameData _gameData;

    protected override void Awake() {
        base.Awake();
        Init();
    }
    private void Init() {
        foreach (var levelData in levelDatas) {
            foreach (var openMap in levelData.openNewMapList) {
                _allCheckers.Add(openMap);
            }
        }
        foreach (var checker in _allCheckers) {
            if (checker != null) {
                var stand = checker.OpenITarget as DisplayStand;
                if (stand != null) {
                    _allStand.Add(stand);
                }
            }
            else {
                Debug.Log("stand가 없음");
            }
        }
        foreach (var checker in _allCheckers) {
            if (checker != null) {
                var truck = checker.OpenITarget as FoodContainer;
                if (truck != null) {
                    _allTruck.Add(truck);
                }
            }
            else {
                Debug.Log("stand가 없음");
            }
        }
        foreach (var checker in _allCheckers) {
            if (checker != null) {
                var table = checker.OpenITarget as Table;
                if (table != null) {
                    _allTable.Add(table);
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
        LevelEvents.ChangeDisplayStandEvent += ChangeDisplyStandItem;
        LevelEvents.ChangeCheckerActiveEvent += ChangeCheckerActive;
    }
    private void OnDisable() {
        LevelEvents.GameDataLoadEvent -= GameDataLoad;
        LevelEvents.ChangePriceEvent -= ChangeCheckerPrice;
        LevelEvents.ChangeDisplayStandEvent -= ChangeDisplyStandItem;
        LevelEvents.ChangeCheckerActiveEvent -= ChangeCheckerActive;
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
        _currentLevel.SetActiveChildList(true); // 다음 스테이지 켜주고
    }
    private void GameDataLoad(GameData data) {
        if (data == null) {
            return;
        }

        _gameData = data;
        _currentLevel = _gameData.level;
        _levelIndex = _gameData.levelIndex;
        _exp = _gameData.exp;

        SetLevelData();

        // 체커의 가격도 맞춰주고 가격에 따라서 stand도 켜줌
        SettingCheckerPrice();
        // truck켜주기
        //SettingTruck();
    }

    private void SetLevelData() {
        // 각각의 checker에 값 적용
        for (int i = 0; i < _allCheckers.Count; i++) {
            if (_allCheckers[i] != null) {
                _allCheckers[i].ActiveObj(_gameData.allCheckOnOffList[i]);
            }
        }
        for (int i = 0; i < _allStand.Count; i++) {
            if (_allStand[i] != null) {
                _allStand[i].ActiveObj(_gameData.allStandOnOffList[i]);
            }
        }
        for (int i = 0; i < _allTruck.Count; i++) {
            if (_allTruck[i] != null) {
                _allTruck[i].ActiveObj(_gameData.allTruckOnOffList[i]);
            }
        }
        for (int i = 0; i < _allTable.Count; i++) {
            if (_allTable[i] != null) {
                _allTable[i].ActiveObj(_gameData.allTableOnOffList[i]);
            }
        }
    }
    private void SettingTruck() {
        for (int i = 0; i < _allTruck.Count; i++) {
            if (_allTruck[i] is IOpenTarget openTarget) {
                if (openTarget != null && openTarget.IsOpen) { // 오픈되었다

                }
            }
        }
    }
    private void SettingCheckerPrice() {
        for (int i = 0; i < _allCheckers.Count; i++) {
            var checker = _allCheckers[i];
            if (checker != null) {
                if(checker.OpenITarget.IsOpen) { // 오픈되었다
                    OnStandItem(checker);
                }
                else {
                    checker.Price = _gameData.allCheckPriceList[i];
                }

            }
        }
    }
    private void OnStandItem(BuyChecker checker) {
        if(checker.OpenGTarget.TryGetComponent(out DisplayStand stand)){

            int index = _allStand.IndexOf(stand);
            int itemCount = _gameData.allStandItemCountList[index];
            _allStand[index].AddItemToStand(itemCount);
        }
    }

    private void ChangeCheckerActive(BuyChecker activeObj, bool active) {
        int index = 0;

        index = _allCheckers.IndexOf(activeObj); // 내가 누구인지 index뽑고
        _gameData.allCheckOnOffList[index] = !active; // true

        if (activeObj.OpenGTarget.TryGetComponent(out DisplayStand stand)) {
            index = _allStand.IndexOf(stand); // 내가 누구인지 index뽑고
            _gameData.allStandOnOffList[index] = active;
        }
        else if (activeObj.OpenGTarget.TryGetComponent(out FoodContainer truck)) {
            index = _allTruck.IndexOf(truck); // 내가 누구인지 index뽑고
            _gameData.allTruckOnOffList[index] = active;
        }
        else if (activeObj.OpenGTarget.TryGetComponent(out Table table)) {
            index = _allTable.IndexOf(table); // 내가 누구인지 index뽑고
            _gameData.allTableOnOffList[index] = active;
        }
        LevelEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
    private void ChangeCheckerPrice(BuyChecker checker, int price) {
        int index = _allCheckers.IndexOf(checker); // 내가 누구인지 index뽑고
        _gameData.allCheckPriceList[index] = price;
        LevelEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
    private void ChangeDisplyStandItem(DisplayStand stand, int count) {
        int index = _allStand.IndexOf(stand); // 내가 누구인지 index뽑고
        _gameData.allStandItemCountList[index] = count;
        LevelEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
}
