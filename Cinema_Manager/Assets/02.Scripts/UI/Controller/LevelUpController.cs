using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LevelUpController : MonoBehaviour {
    private GameData _gameData;
    public List<ParticleSystem> levelUpParticle = new List<ParticleSystem>();

    private void OnEnable() {
        SaveManager.GameDataLoadedEvent += GameDataLoad;
        LevelUpEvents.GameDataUpdatEvent += GameDataUpdate;
        LevelUpEvents.CloseView += CloseView;
    }
    private void OnDisable() {
        SaveManager.GameDataLoadedEvent -= GameDataLoad;
        LevelUpEvents.GameDataUpdatEvent -= GameDataUpdate;
        LevelUpEvents.CloseView -= CloseView;
    }

    private void GameDataLoad(GameData data) {
        if (data == null) {
            return;
        }
        _gameData = data;
        LevelUpEvents.GameDataLoadEvent?.Invoke(_gameData);
    }
    private void GameDataUpdate(GameData data) {
        if (data == null) {
            return;
        }
        _gameData = data;
        LevelUpEvents.LevelUpUpdate?.Invoke(_gameData);
        PlayParticle();
    }
    private void CloseView() {
        StopParticle();
    }

    private void PlayParticle() {
        foreach (var particle in levelUpParticle) {
            particle.Play();
        }
    }
    private void StopParticle() {
        foreach (var particle in levelUpParticle) {
            particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}
