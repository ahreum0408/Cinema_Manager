using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using Debug = UnityEngine.Debug;

public class LevelManager : MonoSingleton<LevelManager> {
    public List<Level> levelDatas = new List<Level>();

    private List<BuyChecker> _allCheckers = new List<BuyChecker>();

    private List<DisplayStand> _allStand = new List<DisplayStand>();
    private List<FoodContainer> _allTruck = new List<FoodContainer>();
    private List<Table> _allTable = new List<Table>();
    private List<Room> _allRoom = new List<Room>();

    private Level _currentLevel;
    private int _exp;
    private int _levelIndex = 0;

    private GameData _gameData;

    private void Start() {
        Init();
    }
    private void Init() {
        // level에 존재하는 모든 데이터 값을 level에서 찾아 넣어줌
        foreach (var data in levelDatas) {
            data.Init();

            List<BuyChecker> buyCheckers = data.GetCheckerList();
            List<DisplayStand> standList = data.GetStandList();
            List<FoodContainer> truckList = data.GetTruckList();
            List<Table> tableList = data.GetTableList();
            List<Room> roomList = data.GetRoomList();

            if (buyCheckers != null) {
                foreach (var checker in buyCheckers) {
                    _allCheckers.Add(checker);
                }
            }
            if (standList != null) {
                foreach (var stand in standList) {
                    _allStand.Add(stand);
                }
            }
            if (truckList != null) {
                foreach (var truck in truckList) {
                    _allTruck.Add(truck);
                }
            }
            if (tableList != null) {
                foreach (var table in tableList) {
                    _allTable.Add(table);
                }
            }
            if (roomList != null) {
                foreach (var table in roomList) {
                    _allRoom.Add(table);
                }
            }
        }
    }

    private void OnEnable() {
        LevelEvents.GameDataLoadEvent += GameDataLoad;

        LevelEvents.ChangePriceEvent += ChangeCheckerPrice;
        LevelEvents.ChangeCheckerActiveEvent += ChangeCheckerActive;

        LevelEvents.ChangeStandActiveEvent += ChangeStandActive;
        LevelEvents.ChangeStandItemEvent += ChangeDisplyStandItem;

        LevelEvents.ChangeTruckActiveEvent += ChangeTruckActive;

        LevelEvents.ChangeRoomActiveEvent += ChangRoomActive;
    }
    private void OnDisable() {
        LevelEvents.GameDataLoadEvent -= GameDataLoad;

        LevelEvents.ChangePriceEvent -= ChangeCheckerPrice;
        LevelEvents.ChangeCheckerActiveEvent -= ChangeCheckerActive;

        LevelEvents.ChangeStandActiveEvent -= ChangeStandActive;
        LevelEvents.ChangeStandItemEvent -= ChangeDisplyStandItem;

        LevelEvents.ChangeTruckActiveEvent -= ChangeTruckActive;

        LevelEvents.ChangeRoomActiveEvent -= ChangRoomActive;
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

        foreach (var levelData in levelDatas) {
            levelData.AfterSetting();
        }

        SetLevelData();

        // 체커의 가격도 맞춰주고 가격에 따라서 stand도 켜줌
        SettingCheckerPrice();
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
        for (int i = 0; i < _allRoom.Count; i++) {
            if (_allRoom[i] != null) {
                _allRoom[i].ActiveObj(_gameData.allRoomOnOffList[i]);
            }
        }
    }
    private void SettingCheckerPrice() {
        for (int i = 0; i < _allCheckers.Count; i++) {
            var checker = _allCheckers[i];
            if (checker.OpenITarget != null) {
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

    private void ChangeCheckerActive(BuyChecker checker, bool active) {
        int index = 0;

        index = _allCheckers.IndexOf(checker); // 내 체커 끄고
        _gameData.allCheckOnOffList[index] = active;

        if (checker.OpenGTarget.TryGetComponent(out DisplayStand stand)) {
            index = _allStand.IndexOf(stand); // 내가 누구인지 index뽑고
            _gameData.allStandOnOffList[index] = !active;
        }
        else if (checker.OpenGTarget.TryGetComponent(out FoodContainer truck)) {
            index = _allTruck.IndexOf(truck); // 내가 누구인지 index뽑고
            _gameData.allTruckOnOffList[index] = !active;
        }
        else if (checker.OpenGTarget.TryGetComponent(out Table table)) {
            index = _allTable.IndexOf(table); // 내가 누구인지 index뽑고
            _gameData.allTableOnOffList[index] = !active;
        }
        LevelEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
    private void ChangeStandActive(DisplayStand activeObj, bool active) {
        int index = _allStand.IndexOf(activeObj); // 내가 누구인지 index뽑고
        _gameData.allStandOnOffList[index] = active;
        LevelEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
    private void ChangeTruckActive(FoodContainer activeObj, bool active) {
        int index = _allTruck.IndexOf(activeObj); // 내가 누구인지 index뽑고
        _gameData.allTruckOnOffList[index] = active;
        LevelEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
    private void ChangRoomActive(Room activeObj, bool active) {
        int index = _allRoom.IndexOf(activeObj); // 내가 누구인지 index뽑고
        _gameData.allRoomOnOffList[index] = active;
        LevelEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
    private void ChangeCheckerPrice(BuyChecker checker, int price) {
        int index = _allCheckers.IndexOf(checker); // 내가 누구인지 index뽑고
        _gameData.allCheckPriceList[index] = price;
        LevelEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
    private void ChangeDisplyStandItem(DisplayStand stand, int count = 0) {
        int index = _allStand.IndexOf(stand); // 내가 누구인지 index뽑고
        _gameData.allStandItemCountList[index] = count;
        LevelEvents.GameDataUpdatEvent?.Invoke(_gameData);
    }
}
