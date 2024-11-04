using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Level {
    public int levelNumder = 1; // 현제 레벨

    // 레벨의 경험치레 대한 최소 최댓값
    public int lowValue = 0;
    public int highValue = 20;

    public List<BuyChecker> openNewMapList;

    public void SetActiveListObj(bool active) {
        foreach(BuyChecker area in openNewMapList) {
            if (area != null) {
                area.gameObject.SetActive(active);
            }
            else {
                Debug.LogWarning("지금 null인게 있음");
            }
        }
    }
}