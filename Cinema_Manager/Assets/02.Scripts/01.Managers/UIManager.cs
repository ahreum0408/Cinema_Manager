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
    UIView _employeeUpgradeView;
    UIView _playerUpgradeView;
    UIView _machineUpgradeView;


    public const string mainViewName = "MainView";
    public const string settingViewName = "SettingView";
    public const string upgradeEmployeeViewName = "EmployeeUpgradeView";
    public const string upgradePlayerViewName = "PlayerUpgradeView";
    public const string upgradeMachineViewName = "MachineUpgradeView";

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
        _employeeUpgradeView = new EmployeeUpgradeView(root.Q<VisualElement>(upgradeEmployeeViewName)); // Landing modal screen
        _playerUpgradeView = new PlayerUpgradeView(root.Q<VisualElement>(upgradePlayerViewName)); // Landing modal screen
        _machineUpgradeView = new MachinepgradeView(root.Q<VisualElement>(upgradeMachineViewName)); // Landing modal screen

        _allViews.Add(_mainView);
        _allViews.Add(_settingView);
        _allViews.Add(_employeeUpgradeView);
        _allViews.Add(_playerUpgradeView);
        _allViews.Add(_machineUpgradeView);

        //_mainView.Show();
        _machineUpgradeView.Show();
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
        MainEvents.MainViewShow += ShowMainView;
        MainEvents.SettingViewShow += ShowSettingView;
        MainEvents.EmployeeUpgradeViewShow += ShowEmployeeUpgradeView;
        MainEvents.MachineUpgradeViewShow += ShowMachineUpgradeView;
    }
    private void UnRegisterToEvents() {
        MainEvents.MainViewShow -= ShowMainView;
        MainEvents.SettingViewShow -= ShowSettingView;
        MainEvents.EmployeeUpgradeViewShow -= ShowEmployeeUpgradeView;
        MainEvents.MachineUpgradeViewShow -= ShowMachineUpgradeView;
    }


    #region ShowViews
    private void ShowMainView() {
        ChangeShowView(_mainView);
    }
    private void ShowSettingView() {
        ChangeShowView(_settingView);
    }
    private void ShowEmployeeUpgradeView() {
        ChangeShowView(_employeeUpgradeView);
    }
    private void ShowMachineUpgradeView() {
        ChangeShowView(_machineUpgradeView);
    }
    #endregion
}
