using System;
using UnityEngine;

[Serializable]
public class Level {
    public int levelNumder = 1; // 현제 레벨

    // 레벨의 경험치레 대한 최소 최댓값
    public int lowValue = 0;
    public int highValue = 20;

    public GameObject openMapPrefab;

    public void SetActiveMap(bool active) {
        if(openMapPrefab != null) {
            openMapPrefab.SetActive(active);
        }
    }
}