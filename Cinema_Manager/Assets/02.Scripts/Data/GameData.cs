using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameData
{
    public bool isMinimumExecution;

    // setting view
    public bool bgm;
    //public bool effect;
    public bool haptic;

    // main view
    public int coin;
    public int gam;

    // level
    public Level level;
    public int levelIndex;
    public int exp;

    public List<bool> allCheckOnOffList;
    public List<int> allCheckPriceList;

    public List<bool> allStandOnOffList;
    public List<int> allStandItemCountList;

    public List<bool> allFoodTruckOnOffList;
    public List<bool> allBoxTruckOnOffList;

    public List<bool> allTableOnOffList;

    public List<bool> allParcelServiceOnOffList;

    public List<bool> allRoomOnOffList;
    public List<bool> allCounterOnOffList;
    public List<bool> allSignBoardOnOffList;

    public List<bool> allCounterStaffOnOffList;

    // player
    public int p_movespeedLevel = 0; // 플레이어 이속
    public int p_volumeLevel = 0; // 플레이어 용량
    public int p_sellingcostLevel = 0; // 플레이어 판매시 재화 획득량

    // employee
    public int e_movespeedLevel = 0; // 직원 이속
    public int e_volumeLevel = 0; // 직원 용량
    public int e_employmentLevel = 0; // 직원 추가

    // machine-truck(트럭종류)
    public int mt_productionspeedLevel = 0; // 생산속도 증가
    public int mt_volumeLevel = 0; // 보관량
    public int mt_storageLevel = 0; // 들고오는 양

    // machine-package(택배 붙이는 기계)
    public int mp_packingspeedLevel = 0; // 포장 증가
    public int mp_volumeLevel = 0; // 보관량 증가

    public GameData()
    {
        this.isMinimumExecution = false;

        this.bgm = false;
        //this.effect = true;
        this.haptic = true;

        this.coin = 10000; // 475
        this.gam = 0;

        this.level = new Level();
        this.levelIndex = 0;
        this.exp = 0;

        this.allCheckOnOffList = new List<bool>(new bool[50]);
        this.allCheckPriceList = new List<int>(Enumerable.Repeat(100, 50).ToArray());

        this.allStandOnOffList = new List<bool>(new bool[20]);
        this.allStandItemCountList = new List<int>(new int[20]);

        this.allFoodTruckOnOffList = new List<bool>(new bool[20]);
        this.allBoxTruckOnOffList = new List<bool>(new bool[1]);

        this.allTableOnOffList = new List<bool>(new bool[20]);

        this.allParcelServiceOnOffList = new List<bool>(new bool[1]);

        this.allRoomOnOffList = new List<bool>(new bool[3]);
        this.allCounterOnOffList = new List<bool>(new bool[3]);
        this.allSignBoardOnOffList = new List<bool>(new bool[3]);

        this.allCounterStaffOnOffList = new List<bool>(new bool[1]);

        this.p_movespeedLevel = 0;
        this.p_movespeedLevel = 0;
        this.p_sellingcostLevel = 0;

        this.e_movespeedLevel = 0;
        this.e_volumeLevel = 0;
        this.e_employmentLevel = 0;

        this.mt_productionspeedLevel = 0;
        this.mt_volumeLevel = 0;
        this.mt_storageLevel = 0;
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this); // 쓰기
    }
    public void LoadJson(string jsonFilepath)
    {
        JsonUtility.FromJsonOverwrite(jsonFilepath, this); // 경로 부분에 덮어 쓰기
    }
}
