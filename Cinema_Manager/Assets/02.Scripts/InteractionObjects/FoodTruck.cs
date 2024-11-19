using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using static AyunDefine;

public class FoodTruck : MonoBehaviour
{
    public Action OnBringFood;

    [Header("Truck Move Setting")]
    [SerializeField] private Transform _visualTrm;
    [SerializeField] private Transform _startTrm;
    [SerializeField] private Transform _endTrm;
    [SerializeField] private float _moveTime = 3;

    private bool _isBringFood = false;

    private FoodContainer _container;

    #region 나중에 업그레이드로 빼야할 것들
    private float _truckBringTime = 8f; // 음식 가져오는데 걸리는 시간
    // 이 시간은 _moveTime * 2보다 커야함
    #endregion
    private float _currentBringTime = 0;

    private void Awake()
    {
        _container = GetComponentInParent<FoodContainer>();
    }

    private void Start()
    {
        _container.OnScaleSettingEndEvent += HandleOnScaleSettingEndEvent;
    }

    private void OnEnable()
    {
        _visualTrm.gameObject.SetActive(false);
        _isBringFood = false;
    }

    private void OnDestroy()
    {
        _container.OnScaleSettingEndEvent -= HandleOnScaleSettingEndEvent;
    }

    private void HandleOnScaleSettingEndEvent()
    {
        _currentBringTime = _truckBringTime;
        _isBringFood = true;

        _visualTrm.position = _startTrm.position;
        _visualTrm.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (_isBringFood)
        {
            _currentBringTime += Time.deltaTime;
            if (_currentBringTime >= _truckBringTime - _moveTime) // 이거 시간 재는거 확인해보기
            {
                _currentBringTime = 0;
                _isBringFood = false;
                BringBackFood();
            }
        }
    }

    // 음식 가지러 가기
    public void GoTakeFood()
    {
        // Sound
        SoundManager.Instance.Play(AudioClips.TruckStart, true, 1, _visualTrm);
        SoundManager.Instance.Play(AudioClips.TruckHorn, true, 1, _visualTrm);

        _isBringFood = true;
        TruckMove(_startTrm.position, false);
    }

    // 음식 가져옴
    public void BringBackFood()
    {
        TruckMove(_endTrm.position, true);
    }

    private void TruckMove(Vector3 targetPos, bool isBringFood)
    {
        _visualTrm.DOMove(targetPos, _moveTime)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
        {
            if (isBringFood)
            {
                // Sound
                SoundManager.Instance.Play(AudioClips.TruckStop, true, 1, _visualTrm);

                OnBringFood?.Invoke();
            }
        });
    }
}
