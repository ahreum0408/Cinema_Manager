using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Level {
    public int levelNumder = 0; // 현제 레벨

    // 레벨의 경험치에 대한 최소 최댓값
    public int lowValue = 0;
    public int highValue = 10;

    [SerializeField] private List<GameObject> openNewMapList;

    private List<BuyChecker> _buyCheckersList = new List<BuyChecker>();

    private List<DisplayStand> _standList = new List<DisplayStand>();
    private List<FoodContainer> _foodTruckList = new List<FoodContainer>();
    private List<BoxContainer> _boxTruckList = new List<BoxContainer>();
    private List<Table> _tableList = new List<Table>();
    private List<ParcelService> _parcelServiceList = new List<ParcelService>();
    private List<Room> _roomList = new List<Room>();
    private List<Counter> _counterList = new List<Counter>();
    private List<SignBoard> _signBoardList = new List<SignBoard>();
    private List<CounterStaffController> _counterStaffList = new List<CounterStaffController>();

    private List<IOpenTarget> _anotherObjList = new List<IOpenTarget>();

    public Dictionary<TargetType, int> _targetDictionary = new Dictionary<TargetType, int>();


    // 내 타겟의 데이터의 종류 별로 분류
    public void Init() {
        foreach (GameObject area in openNewMapList) {
            Transform checkerPos = area.gameObject.transform.GetChild(0);
            Transform targetPos = area.gameObject.transform.GetChild(1);
            if (checkerPos.TryGetComponent(out BuyChecker checker)) {
                _buyCheckersList.Add(checker);
            }
            if (targetPos.TryGetComponent(out IOpenTarget target)) { // checker를 통하여 열 애들
                switch (target.Type) {
                    case TargetType.DisplayStand:
                        if (_targetDictionary.TryGetValue(target.Type, out int value)) {
                            _targetDictionary[target.Type] = ++value;
                        }
                        else {
                            _targetDictionary.Add(target.Type, 1);
                        }
                        _standList.Add(target as DisplayStand);
                        break;
                    case TargetType.FoodContainer:
                        if (_targetDictionary.TryGetValue(target.Type, out int value1)) {
                            _targetDictionary[target.Type] = value1;
                        }
                        else {
                            _targetDictionary.Add(target.Type, 1);
                        }
                        _foodTruckList.Add(target as FoodContainer);
                        break;
                    case TargetType.BoxContainer:
                        if (_targetDictionary.TryGetValue(target.Type, out int value2)) {
                            _targetDictionary[target.Type] = value2;
                        }
                        else {
                            _targetDictionary.Add(target.Type, 1);
                        }
                        _boxTruckList.Add(target as BoxContainer);
                        break;
                    case TargetType.Table:
                        if (_targetDictionary.TryGetValue(target.Type, out int value3)) {
                            _targetDictionary[target.Type] = ++value3;
                        }
                        else {
                            _targetDictionary.Add(target.Type, 1);
                        }
                        _tableList.Add(target as Table);
                        break;
                    case TargetType.ParcelService:
                        if (_targetDictionary.TryGetValue(target.Type, out int value4)) {
                            _targetDictionary[target.Type] = value4;
                        }
                        else {
                            _targetDictionary.Add(target.Type, 1);
                        }
                        _parcelServiceList.Add(target as ParcelService);
                        break;
                    case TargetType.Room:
                        if (_targetDictionary.TryGetValue(target.Type, out int value5)) {
                            _targetDictionary[target.Type] = value5;
                        }
                        else {
                            _targetDictionary.Add(target.Type, 1);
                        }
                        _roomList.Add(target as Room);
                        break;
                    case TargetType.Counter:
                        if (_targetDictionary.TryGetValue(target.Type, out int value6)) {
                            _targetDictionary[target.Type] = value6;
                        }
                        else {
                            _targetDictionary.Add(target.Type, 1);
                        }
                        _counterList.Add(target as Counter);
                        break;
                    case TargetType.SignBoard:
                        if (_targetDictionary.TryGetValue(target.Type, out int value7)) {
                            _targetDictionary[target.Type] = value7;
                        }
                        else {
                            _targetDictionary.Add(target.Type, 1);
                        }
                        _signBoardList.Add(target as SignBoard);
                        break;
                    case TargetType.CounterStaff:
                        if (_targetDictionary.TryGetValue(target.Type, out int value8)) {
                            _targetDictionary[target.Type] = value8;
                        }
                        else {
                            _targetDictionary.Add(target.Type, 1);
                        }
                        _counterStaffList.Add(target as CounterStaffController);
                        break;
                    default:
                        Debug.LogWarning("너는 누구신가요..");
                        break;
                }
            }
        }
    }
    public void SetCheckerActive(bool active, bool firstLoad = false) {
        foreach (BuyChecker checker in _buyCheckersList) {
            checker.ActiveObj(active, firstLoad);
        }
    } // 이거 아마 바꿔야할거임 LevelManager 참고
    
    #region GetList
    public Dictionary<TargetType, int> GetTargetDictionary() {
        return _targetDictionary;
    }
    public List<BuyChecker> GetCheckerList() {
        return _buyCheckersList;
    }
    public List<DisplayStand> GetStandList() {
        return _standList;
    }
    public List<FoodContainer> GetFoodTruckList() {
        return _foodTruckList;
    }
    public List<BoxContainer> GetBoxTruckList() {
        return _boxTruckList;
    }
    public List<Table> GetTableList() {
        return _tableList;
    }
    public List<ParcelService> GetParcelServiceList() {
        return _parcelServiceList;
    }
    public List<Room> GetRoomList() {
        return _roomList;
    }
    public List<Counter> GetCounterList() {
        return _counterList;
    }
    public List<SignBoard> GetSignBoardList() {
        return _signBoardList;
    }
    public List<CounterStaffController> GetCounterStaffList() {
        return _counterStaffList;
    }
    #endregion
}