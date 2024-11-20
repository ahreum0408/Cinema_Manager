using System;
using UnityEngine;

public static class LevelEvents  {
    public static Action<GameData> GameDataUpdatEvent;
    public static Action<GameData> GameDataLoadEvent;

    public static Action<BuyChecker, bool> ChangeCheckerActiveEvent;
    public static Action<BuyChecker, int> ChangePriceEvent; // checker의 가격 변경
    public static Action<Transform> PriceChangingEvent; // checker의 가격이 변경 중일 때

    public static Action<DisplayStand, bool> ChangeStandActiveEvent;
    public static Action<DisplayStand, int> ChangeStandItemEvent; // 스텐드에 음식 수 변경

    public static Action<FoodContainer, bool> ChangeFoodTruckActiveEvent;
    public static Action<BoxContainer, bool> ChangeBoxTruckActiveEvent;

    public static Action<ParcelService, bool> ChangeParcelServiceActiveEvent;
    public static Action<Table, bool> ChangeTableActiveEvent;

    public static Action<Room, bool> ChangeRoomActiveEvent;
    public static Action<Counter, bool> ChangeCounterActiveEvent;
    public static Action<TrashBin, bool> ChangeTrashBinActiveEvent;
}
