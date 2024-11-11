using System;
using System.Collections.Generic;
using UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour {
    private UIDocument _uiDocument;

    private UIView _currentView; // 현재뷰
    private UIView _previousView; // 이전뷰

    private List<UIView> _allViews = new List<UIView>();

    // 
    private UIView _mainView;
    private UIView _settingView;
    private UIView _employeeUpgradeView;
    private UIView _playerUpgradeView;
    private UIView _truckMachineUpgradeView;
    private UIView _packageMachineUpgradeView;
    private UIView _levelUpView;


    public const string mainViewName = "MainView";
    public const string settingViewName = "SettingView";
    public const string upgradeEmployeeViewName = "EmployeeUpgradeView";
    public const string upgradePlayerViewName = "PlayerUpgradeView";
    public const string upgradeTruckMachineViewName = "TruckMachineUpgradeView";
    public const string upgradePackageMachineViewName = "PackageMachineUpgradeView";
    public const string levelUpViewName = "LevelUpView";

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
        _truckMachineUpgradeView = new TruckMachineUpgradeView(root.Q<VisualElement>(upgradeTruckMachineViewName)); // Landing modal screen
        _packageMachineUpgradeView = new PackageMachineUpgradeView(root.Q<VisualElement>(upgradePackageMachineViewName)); // Landing modal screen
        //_levelUpView = new LevelUpView(root.Q<VisualElement>(levelUpViewName)); // Landing modal screen

        _allViews.Add(_mainView);
        _allViews.Add(_settingView);
        _allViews.Add(_employeeUpgradeView);
        _allViews.Add(_playerUpgradeView);
        _allViews.Add(_truckMachineUpgradeView);
        _allViews.Add(_packageMachineUpgradeView);
        _allViews.Add(_levelUpView);

        _mainView.Show();
        //_levelUpView.Show();
    }
    private void ChangeShowView(UIView newView) {
        if (_currentView != null && _currentView != _mainView) { // 지금 보고 있는 view가 있으면 꺼
            _currentView.Hide();
        }

        _previousView = _currentView;
        _currentView = newView;

        if (_currentView != null){ // 지금 볼거 있으면 그거 켜주고 지금 보고 있는 view가 변경됬음을 알려줘
            _currentView.Show();
        }
    }
    private void CloseCurrentView() {
        if(_currentView != null && _currentView != _mainView) {
            _currentView.Hide();
        }
    }

    // 이벤트 등록 및 해제
    private void RegisterToEvents() {
        MainEvents.MainViewShow += ShowMainView;
        MainEvents.SettingViewShow += ShowSettingView;
        MainEvents.PlayerUpgradeViewShow += ShowPlayerView;
        MainEvents.EmployeeUpgradeViewShow += ShowEmployeeUpgradeView;
        MainEvents.TruckMachineUpgradeViewShow += ShowTruckMachineUpgradeView;
        MainEvents.PackageMachineUpgradeViewShow += ShowPackageMachineUpgradeView;
        MainEvents.LevelUpViewShow += ShowLevelUpView;

        MainEvents.CloseCurrentEvent += CloseCurrentView;
    }
    private void UnRegisterToEvents() {
        MainEvents.MainViewShow -= ShowMainView;
        MainEvents.SettingViewShow -= ShowSettingView;
        MainEvents.PlayerUpgradeViewShow -= ShowPlayerView;
        MainEvents.EmployeeUpgradeViewShow -= ShowEmployeeUpgradeView;
        MainEvents.TruckMachineUpgradeViewShow -= ShowTruckMachineUpgradeView;
        MainEvents.PackageMachineUpgradeViewShow -= ShowPackageMachineUpgradeView;
        MainEvents.LevelUpViewShow -= ShowLevelUpView;

        MainEvents.CloseCurrentEvent -= CloseCurrentView;
    }


    #region ShowViews
    private void ShowMainView() {
        ChangeShowView(_mainView);
    }
    private void ShowSettingView() {
        ChangeShowView(_settingView);
    }
    private void ShowPlayerView() {
        ChangeShowView(_playerUpgradeView);
    }
    private void ShowEmployeeUpgradeView() {
        ChangeShowView(_employeeUpgradeView);
    }
    private void ShowTruckMachineUpgradeView() {
        ChangeShowView(_truckMachineUpgradeView);
    }
    private void ShowPackageMachineUpgradeView() {
        ChangeShowView(_packageMachineUpgradeView);
    }
    private void ShowLevelUpView() {
        ChangeShowView(_levelUpView);
    }
    #endregion
}
