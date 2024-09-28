using System;
using System.Collections.Generic;
using UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour {
    UIDocument _uiDocument;

    UIView _currentView; // 현재뷰
    UIView _previousView; // 이전뷰

    List<UIView> _allViews = new List<UIView>();

    // 
    UIView _mainView;
    UIView _settingView;


    public const string mainViewName = "MainView";
    public const string settingViewName = "SettingView";

    void OnEnable() {
        _uiDocument = GetComponent<UIDocument>();

        SetupViews();

        RegisterToEvents();

        // Start with the home screen
        //ChangeShowView(_mainView);
    }
    void OnDisable() {
        UnRegisterToEvents();

        foreach (UIView view in _allViews) {
            view.Dispose();
        }
    }

    private void SetupViews() {
        VisualElement root = _uiDocument.rootVisualElement;

        _mainView = new MainView(root.Q<VisualElement>(mainViewName)); // Landing modal screen
        _settingView = new SettingView(root.Q<VisualElement>(settingViewName)); // Landing modal screen

        _allViews.Add(_mainView);
        _allViews.Add(_settingView);

        //_mainView.Show();
        _settingView.Show();
    }
    private void ChangeShowView(UIView newView) {
        if (_currentView != null) { // 지금 보고 있는 view가 있으면 꺼
            _currentView.Hide();
        }

        _previousView = _currentView;
        _currentView = newView;

        if (_currentView != null){ // 지금 볼거 있으면 그거 켜주고 지금 보고 있는 view가 변경됬음을 알려줘
            _currentView.Show();
            //MainMenuUIEvents.CurrentViewChanged?.Invoke(_currentView.GetType().Name);
        }
    }

    // 이벤트 등록 및 해제
    private void RegisterToEvents() {
        MainEvents.MainViewShow += MainSettingView;
        MainEvents.SettingViewShow += ShowSettingView;
    }

    private void UnRegisterToEvents() {

    }

    #region ShowViews
    private void MainSettingView() {
        ChangeShowView(_mainView);
    }
    private void ShowSettingView() {
        ChangeShowView(_settingView);
    }
    #endregion
}
