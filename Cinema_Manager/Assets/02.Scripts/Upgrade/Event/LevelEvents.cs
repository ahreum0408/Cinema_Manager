using System;
using UnityEngine;

public static class LevelEvents  {
    public static Action<GameData> GameDataUpdatEvent;
    public static Action<GameData> GameDataLoadEvent;
    public static Action<BuyChecker, int> ChangePriceEvent;
    public static Action<DisplayStand, int> ChangeDisplayStandEvent;
}
