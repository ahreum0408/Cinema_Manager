using System.Collections.Generic;
using System.Drawing;
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

    // checker 가격 및 stand 개수 보관
    public List<int> allCheckPriceList;
    public List<int> allDisplayStandItemCountList;

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
    //public int mp_storageLevel = 0; // 창고 용량

    public GameData() {
        this.bgm = true;
        this.effect = true;
        this.haptic = true;

        this.coin = 0;
        this.gam = 0;

        this.level = new Level(); 
        this.levelIndex = 0;
        this.exp = 0;

        allCheckPriceList = new List<int> { 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50 };
        allDisplayStandItemCountList = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

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
