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
    private List<FoodContainer> _truckList = new List<FoodContainer>();
    private List<Table> _tableList = new List<Table>();
    private List<Room> _roomList = new List<Room>();
    private List<IOpenTarget> _anotherObjList = new List<IOpenTarget>();


    // 기타 다른 것들도 받아야 함

    // 내 타겟의 데이터의 종류 별로 분류
    public void Init() {
        foreach (GameObject area in openNewMapList) {
            if (area.TryGetComponent(out BuyChecker checker)) { // checker를 통하여 열 애들
                _buyCheckersList.Add(checker);

                GameObject target = checker.OpenGTarget;

                if (target.TryGetComponent(out DisplayStand stand)) {
                    _standList.Add(stand);
                }
                else if (target.TryGetComponent(out FoodContainer truck)) {
                    _truckList.Add(truck);
                }
                else if (target.TryGetComponent(out Table table)) {
                    _tableList.Add(table);
                }
                else if (target.TryGetComponent(out Room room)) {
                    _roomList.Add(room);
                }
                else {
                    Debug.LogWarning("너는 누구신가요..");
                }
            }
            else {
                var obj = area.GetComponent<IOpenTarget>();

                if (area.TryGetComponent(out DisplayStand stand)) {
                    _standList.Add(stand);
                }
                else if (area.TryGetComponent(out FoodContainer truck)) {
                    _truckList.Add(truck);
                }
                else if (area.TryGetComponent(out Table table)) {
                    _tableList.Add(table);
                }

                _anotherObjList.Add(obj);
            }
        }
    }
    public void AfterSetting() {
        foreach (IOpenTarget obj in _anotherObjList) {
            obj.ActiveObj(true);
        }
    }
    public void LoadCheckerData() {

    }
    public void SetActiveChildList(bool active) {
        foreach (BuyChecker checker in _buyCheckersList) {
            checker.ActiveObj(active, true);
        }
    } // 이거 아마 바꿔야할거임 LevelManager 참고

    #region GetList
    public List<BuyChecker> GetCheckerList() {
        return _buyCheckersList;
    }
    public List<DisplayStand> GetStandList() {
        return _standList;
    }
    public List<FoodContainer> GetTruckList() {
        return _truckList;
    }
    public List<Table> GetTableList() {
        return _tableList;
    }
    public List<Room> GetRoomList() {
        return _roomList;
    }
    #endregion
}