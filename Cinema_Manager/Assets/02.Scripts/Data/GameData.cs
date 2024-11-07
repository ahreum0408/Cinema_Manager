using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameData {
    // setting view
    public bool bgm;
    public bool effect;
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

    public GameData() {
        this.bgm = true;
        this.effect = true;
        this.haptic = true;

        this.coin = 10000;
        this.gam = 0;

        this.level = new Level(); 
        this.levelIndex = 0;
        this.exp = 0;

        allCheckOnOffList = new List<bool> { true, true, true, true, true, false, false , false , false , false, false, false, false, false, false, false, false, false, false, false };
        allCheckPriceList = new List<int> (Enumerable.Repeat(100, 20).ToArray());

        allStandOnOffList = new List<bool>(new bool[20]);
        allStandItemCountList = new List<int>(new int[20]);

        allFoodTruckOnOffList = new List<bool>(new bool[20]);
        allBoxTruckOnOffList = new List<bool>(new bool[1]);
        
        allTableOnOffList = new List<bool>(new bool[20]);

        allParcelServiceOnOffList = new List<bool>(new bool[1]);

        allRoomOnOffList = new List<bool>(new bool[1]);

        p_movespeedLevel = 0;
        p_movespeedLevel = 0;
        p_sellingcostLevel = 0;

        e_movespeedLevel = 0;
        e_volumeLevel = 0;
        e_employmentLevel = 0;

        mt_productionspeedLevel = 0;
        mt_volumeLevel = 0;
        mt_storageLevel = 0;
    }

    public string ToJson() {
        return JsonUtility.ToJson(this); // 쓰기
    }
    public void LoadJson(string jsonFilepath) {
        JsonUtility.FromJsonOverwrite(jsonFilepath, this); // 경로 부분에 덮어 쓰기
    }
}
