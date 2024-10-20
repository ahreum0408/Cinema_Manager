using UnityEngine;

public class GameData {
    // setting view
    public float bgmValue;
    public float effectValue;
    public bool haptic = true;

    // main view
    public int coin;
    public int gam;

    public Level level;
    public int levelIndex;
    public int exp;

    // player
    public int p_movespeedLevel; // 플레이어 이속
    public int p_volumeLevel; // 플레이어 용량
    public int p_sellingcostLevel; // 플레이어 판매시 재화 획득량

    // employee
    public int e_movespeedLevel; // 직원 이속
    public int e_volumeLevel; // 직원 용량
    public int e_employmentLevel; // 직원 추가

    // machine
    public int m_productionspeedLevel; //  생산속도 증가
    public int m_volumeLevel; // 기계 용량 증가
    public int m_storageLevel; // 창고 용량

    public GameData() {
        this.bgmValue = 1f;
        this.effectValue = 1f;

        this.coin = 0;
        this.gam = 0;

        this.level = new Level(); 
        this.levelIndex = 0;
        this.exp = 0;
    }
    public string ToJson() {
        return JsonUtility.ToJson(this); // 쓰기
    }

    public void LoadJson(string jsonFilepath) {
        JsonUtility.FromJsonOverwrite(jsonFilepath, this); // 경로 부분에 덮어 쓰기
    }
}
