using UnityEngine;

public class GameData {
    // setting view
    public float bgmValue;
    public float effectValue;

    public int coin;
    public int gam;

    public GameData() {
        this.bgmValue = 1f;
        this.effectValue = 1f;

        this.coin = 0;
        this.gam = 0;
    }
    public string ToJson() {
        return JsonUtility.ToJson(this); // 쓰기
    }

    public void LoadJson(string jsonFilepath) {
        JsonUtility.FromJsonOverwrite(jsonFilepath, this); // 경로 부분에 덮어 쓰기
    }
}
