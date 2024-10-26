using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Level {
    public int levelNumder = 1; // 현제 레벨

    // 레벨의 경험치레 대한 최소 최댓값
    public int lowValue = 0;
    public int highValue = 20;

    public List<CheckerArea> openNewMapList;

    public void SetActiveMap(bool active) {
        if(openNewMapList != null) {
            foreach(CheckerArea area in openNewMapList) {
                area.gameObject.SetActive(active);
            }
        }
    }
}