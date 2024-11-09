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
    private List<BoxContainer> _bosTruckList = new List<BoxContainer>();
    private List<Table> _tableList = new List<Table>();
    private List<ParcelService> _parcelServiceList = new List<ParcelService>();
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
                else if (target.TryGetComponent(out FoodContainer foodtruck)) {
                    _foodTruckList.Add(foodtruck);
                }
                else if (target.TryGetComponent(out BoxContainer boxtruck)) {
                    _bosTruckList.Add(boxtruck);
                }
                else if (target.TryGetComponent(out Table table)) {
                    _tableList.Add(table);
                }
                else if (target.TryGetComponent(out ParcelService service)) {
                    _parcelServiceList.Add(service);
                }
                else if (target.TryGetComponent(out Room room)) {
                    _roomList.Add(room);
                }
                else {
                    Debug.LogWarning("너는 누구신가요..");
                }
            }
            /*else {
                var obj = area.GetComponent<IOpenTarget>();

                if (area.TryGetComponent(out DisplayStand stand)) {
                    _standList.Add(stand);
                }
                else if (area.TryGetComponent(out FoodContainer truck)) {
                    _foodTruckList.Add(truck);
                }
                else if (area.TryGetComponent(out Table table)) {
                    _tableList.Add(table);
                }

                _anotherObjList.Add(obj);
            }*/
        }
    }
 /*   public void AfterSetting() {
        foreach (IOpenTarget obj in _anotherObjList) {
            Debug.Log("after init");
            obj.ActiveObj(true);
        }
    }*/
    public void LoadCheckerData() {

    }
    public void SetCheckerActive(bool active, bool isReversal = false) { // checker와 target에 적용 되는 값을 뒤집을 건인가?
        foreach (BuyChecker checker in _buyCheckersList) {
            if (isReversal) {
                checker.ActiveObj(!active, isReversal);
            }
            else {
                Debug.Log(checker.name);
                checker.ActiveObj(active); // 킬거임
            }
        }
    } // 이거 아마 바꿔야할거임 LevelManager 참고
    
    #region GetList
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
        return _bosTruckList;
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
}