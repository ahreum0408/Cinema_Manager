using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Level {
    public int levelNumder = 1; // 현제 레벨

    // 레벨의 경험치에 대한 최소 최댓값
    public int lowValue = 0;
    public int highValue = 20;

    [SerializeField] private List<GameObject> openNewMapList;

    private List<BuyChecker> _buyCheckersList = new List<BuyChecker>();

    private List<DisplayStand> _standList = new List<DisplayStand>();
    private List<FoodContainer> _foodTruckList = new List<FoodContainer>();
    private List<BoxContainer> _boxTruckList = new List<BoxContainer>();
    private List<Table> _tableList = new List<Table>();
    private List<ParcelService> _parcelServiceList = new List<ParcelService>();
    private List<Room> _roomList = new List<Room>();

    private List<IOpenTarget> _anotherObjList = new List<IOpenTarget>();

    public Dictionary<TargetType, int> _targetDictionary = new Dictionary<TargetType, int>();


    // 내 타겟의 데이터의 종류 별로 분류
    public void Init() {
        foreach (GameObject area in openNewMapList) {
            if (area.TryGetComponent(out BuyChecker checker)) { // checker를 통하여 열 애들
                _buyCheckersList.Add(checker);

                GameObject target = checker.OpenGTarget;

                if (target.TryGetComponent(out DisplayStand stand)) {
                    if (_targetDictionary.TryGetValue(stand.Type, out int value)) {
                        _targetDictionary[stand.Type] = value;
                    }
                    else {
                        _targetDictionary.Add(stand.Type, 1);
                    }
                    _standList.Add(stand);
                }
                else if (target.TryGetComponent(out FoodContainer foodtruck)) {
                    if (_targetDictionary.TryGetValue(foodtruck.Type, out int value)) {
                        _targetDictionary[foodtruck.Type] = value;
                    }
                    else {
                        _targetDictionary.Add(foodtruck.Type, 1);
                    }
                    _foodTruckList.Add(foodtruck);
                }
                else if (target.TryGetComponent(out BoxContainer boxtruck)) {
                    if (_targetDictionary.TryGetValue(boxtruck.Type, out int value)) {
                        _targetDictionary[boxtruck.Type] = value;
                    }
                    else {
                        _targetDictionary.Add(boxtruck.Type, 1);
                    }
                    _boxTruckList.Add(boxtruck);
                }
                else if (target.TryGetComponent(out Table table)) {
                    if (_targetDictionary.TryGetValue(table.Type, out int value)) {
                        _targetDictionary[table.Type] = value;
                    }
                    else {
                        _targetDictionary.Add(table.Type, 1);
                    }
                    _tableList.Add(table);
                }
                else if (target.TryGetComponent(out ParcelService service)) {
                    if (_targetDictionary.TryGetValue(service.Type, out int value)) {
                        _targetDictionary[service.Type] = value;
                    }
                    else {
                        _targetDictionary.Add(service.Type, 1);
                    }
                    _parcelServiceList.Add(service);
                }
                else if (target.TryGetComponent(out Room room)) {
                    if (_targetDictionary.TryGetValue(room.Type, out int value)) {
                        _targetDictionary[room.Type] = value;
                    }
                    else {
                        _targetDictionary.Add(room.Type, 1);
                    }
                    _roomList.Add(room);
                }
                else {
                    Debug.LogWarning("너는 누구신가요..");
                }
            }
        }
    }
    public void SetCheckerActive(bool active, bool isReversal = false) { // checker와 target에 적용 되는 값을 뒤집을 건인가?
        foreach (BuyChecker checker in _buyCheckersList) {
            if (isReversal) {
                checker.ActiveObj(!active, isReversal);
            }
            else {
                checker.ActiveObj(active); // 킬거임
            }
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
    #endregion
    #region GetLength
    public int GetCheckerListLength() {
        return _buyCheckersList.Count;
    }
    public int GetStandListLength() {
        return _standList.Count;
    }
    public int GetFoodTruckListLength() {
        return _foodTruckList.Count;
    }
    public int GetBoxTruckListLength() {
        return _boxTruckList.Count;
    }
    public int GetTableListLength() {
        return _tableList.Count;
    }
    public int GetParcelServiceListLength() {
        return _parcelServiceList.Count;
    }
    public int GetRoomListLength() {
        return _roomList.Count;
    }
    #endregion
}